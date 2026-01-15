using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 全局触摸事件管理类（兼容 Keel + 滑动列表，含详细调试日志）
/// </summary>
public class AdSceneManager : MonoBehaviour
{
    #region 单例模式（全局访问入口）
    private static AdSceneManager _instance;
    public static AdSceneManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<AdSceneManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("AdSceneManager");
                    _instance = go.AddComponent<AdSceneManager>();
                }
            }
            return _instance;
        }
    }
    #endregion

    #region 核心字段
    public bool IsTest;
    bool SdkIsInit;
    bool isdrag;
    Vector2 TouchPos;
    private static bool isSimulatingMouseDown = false;
    private GameObject currentPressedObject;
    private const float AntiShakeDelay = 0.05f;
    public TouchData tempData;
    private bool isDragStarted = false;
    private Vector2 pressStartPos;
    private Vector2 lastTouchPos; // 上一帧触摸位置（计算增量偏移）
    private EventSystem _eventSystem;
    private int dragThreshold;
    private bool scrollRectBeginDragCalled = false; // ScrollRect 是否已调用 OnBeginDrag
    
    // 手动滚动方案：保存拖拽开始时的状态（参考论坛方案，避免依赖pressEventCamera）
    private Vector2 scrollRectContentStartPos = Vector2.zero; // 拖拽开始时content的位置
    private Vector2 scrollRectPressLocalPos = Vector2.zero; // 按下时的本地坐标
    
    // 用于分析事件频率和坐标变化
    private float lastMoveEventTime = 0f;
    private Vector2 lastMoveEventPos = Vector2.zero;
    #endregion

    #region 外部 SDK 交互方法
    [DllImport("__Internal")]
    private static extern void AdSceneSetupWithChannel(string channel, string packageName);
    [DllImport("__Internal")]
    private static extern void AdSceneStop();
    [DllImport("__Internal")]
    private static extern void AdScenePlay();

    public void StopSDK()
    {
        if (Application.platform != RuntimePlatform.IPhonePlayer)
            return;
        AdSceneStop();
        Debug.Log("AdSceneManager：停止 SDK");
    }

    public void PlaySDK()
    {
        if (Application.platform != RuntimePlatform.IPhonePlayer)
            return;
        AdScenePlay();
        Debug.Log("AdSceneManager：播放 SDK");
    }

    public void SetupSDK()
    {
        if (Application.platform != RuntimePlatform.IPhonePlayer)
            return;

        if (!IsTest && (string.IsNullOrEmpty(CashOutManager.FarBefriend().TrackerSource) || CashOutManager.FarBefriend().TrackerSource == "Null"))
        {
            Debug.Log("AdSceneManager：正式模式归因渠道为空，不初始化 SDK");
            return;
        }

        string channel = IsTest ? "Applovin" : CashOutManager.FarBefriend().TrackerSource;
        string packageName = IsTest ? "com.pokercreative.solitaireacres" : Application.identifier;
        AdSceneSetupWithChannel(channel, packageName);
        Debug.Log($"AdSceneManager：初始化 SDK - 渠道：{channel}，包名：{packageName}");
    }
    #endregion

    #region 生命周期
    private void Awake()
    {
        _instance = this;
        _eventSystem = EventSystem.current;

        // 初始化拖拽阈值
        if (_eventSystem != null)
        {
            dragThreshold = _eventSystem.pixelDragThreshold;
            Debug.Log($"AdSceneManager：从 EventSystem 读取拖拽阈值：{dragThreshold}px");
        }
        else
        {
            dragThreshold = 10;
            Debug.LogWarning($"AdSceneManager：未找到 EventSystem，使用默认拖拽阈值：10px");
        }
    }

    private void Update()
    {
        // 检测原生鼠标点击，重置 SDK 状态
        if (SdkIsInit && Input.GetMouseButtonDown(0))
        {
            SdkIsInit = false;
            Debug.Log("AdSceneManager：检测到原生鼠标点击，重置 SDK 状态");
        }
    }
    #endregion

    #region 核心触摸数据处理
    /// <summary>
    /// 接收外部触摸数据入口
    /// </summary>
    public void OnAdSceneEvent(string eventData)
    {
        //Debug.Log($"=== AdSceneManager：接收触摸数据 === 原始数据：{eventData}");
        try
        {
            TouchData touchData = JsonUtility.FromJson<TouchData>(eventData);
            if (touchData == null)
            {
                Debug.LogError("AdSceneManager：触摸数据解析失败！");
                return;
            }

            // 坐标转换 + 详细日志
            Vector2 originalPos = new Vector2(touchData.x, touchData.y);
            ConvertTouchCoordinate(touchData);
            //Debug.Log($"AdSceneManager：坐标转换 - " +
            //          $"iOS原始坐标(x:{originalPos.x:F1}, y:{originalPos.y:F1}) | " +
            //          $"Unity屏幕坐标(x:{TouchPos.x:F1}, y:{TouchPos.y:F1}) | " +
            //          $"屏幕高度：{Screen.height} | " +
            //          $"验证：iOS.y({originalPos.y:F1}) + Unity.y({TouchPos.y:F1}) = {originalPos.y + TouchPos.y:F1} (应该等于 {Screen.height})");
            SdkIsInit = true;
            HandleTouchEvent(touchData);
        }
        catch (Exception e)
        {
            Debug.LogError($"AdSceneManager：处理触摸事件异常 - {e.Message}");
        }
    }

    /// <summary>
    /// 坐标转换（外部左上角 → Unity 左下角）
    /// </summary>
    private void ConvertTouchCoordinate(TouchData touchData)
    {
        if (Screen.height <= 0)
        {
            Debug.LogError("AdSceneManager：屏幕高度为0，坐标转换失败");
            return;
        }
        TouchPos = new Vector2(touchData.x, Screen.height - touchData.y);
    }

    /// <summary>
    /// 根据事件类型分发处理（按下/移动/抬起）
    /// </summary>
    private void HandleTouchEvent(TouchData touchData)
    {
        //Debug.Log($"AdSceneManager：处理事件 - eventType：{touchData.eventType} | gestureType：{touchData.gestureType}");
        switch (touchData.eventType)
        {
            case 0: // 按下
                isSimulatingMouseDown = true;
                SimulatePointerDown();
                break;
            case 1: // 移动
                if (isSimulatingMouseDown)
                {
                    SimulatePointerMove();
                }
                else
                {
                    Debug.LogWarning("AdSceneManager：未处于模拟按下状态，跳过移动事件");
                }
                break;
            case 2: // 抬起
                ExecuteUpLogic(touchData);
                break;
            default:
                Debug.LogWarning($"AdSceneManager：未知事件类型 - {touchData.eventType}");
                break;
        }
    }
    #endregion

    #region 模拟 UGUI 事件（核心修复+日志）
    /// <summary>
    /// 模拟按下（触发 OnPointerDown）
    /// </summary>
    private void SimulatePointerDown()
    {
       // Debug.Log($"=== AdSceneManager：模拟按下 ===");
        GameObject target = GetRaycastTarget(TouchPos);
        if (target != null)
        {
            currentPressedObject = target;
            pressStartPos = TouchPos;
            lastTouchPos = TouchPos;
            isDragStarted = false;
            
            // 重置事件频率统计
            lastMoveEventTime = 0f;
            lastMoveEventPos = Vector2.zero;

            // 1. 仅创建事件数据，处理拖拽类 UI（ScrollRect）
            PointerEventData eventData1 = new PointerEventData(EventSystem.current);
            eventData1.position = TouchPos;
            eventData1.pressPosition = pressStartPos;
            eventData1.pointerPress = target;

            // 2. 优先处理滑动列表（拖拽类），无其他多余逻辑
            if (HandleScrollDrag(target, eventData1))
            {
                Debug.Log($"AdSceneManager：按下已处理滑动列表 - {target.name}");
                return;
            }

            Debug.Log($"AdSceneManager：按下命中对象 - {target.name} | 初始位置：{pressStartPos}");

            PointerEventData eventData = CreateBasePointerEventData();
            if (eventData == null)
            {
                Debug.LogError("AdSceneManager：创建 PointerEventData 失败，跳过按下事件");
                return;
            }

            // 触发 OnPointerDown
            IPointerDownHandler downHandler = target.GetComponent<IPointerDownHandler>();
            if (downHandler != null)
            {
                downHandler.OnPointerDown(eventData);
                Debug.Log($"AdSceneManager：触发 IPointerDownHandler - {target.name}");
            }
            else
            {
                Debug.LogWarning($"AdSceneManager：对象 {target.name} 未实现 IPointerDownHandler");
            }
        }
        else
        {
            Debug.LogWarning("AdSceneManager：按下未命中任何有效对象");
        }
    }

    /// <summary>
    /// 模拟移动（触发 OnBeginDrag + OnDrag）
    /// </summary>
    private void SimulatePointerMove()
    {
      //  Debug.Log($"=== AdSceneManager：模拟移动 ===");
        if (currentPressedObject == null)
        {
            Debug.LogWarning("AdSceneManager：当前无按压对象，跳过移动");
            return;
        }

        // 射线检测当前目标
        GameObject currentTarget = GetRaycastTarget(TouchPos);
        HandleHoverStateChange(currentTarget);

        // 1. 仅创建事件数据，处理拖拽类 UI（ScrollRect）
        PointerEventData eventData1 = new PointerEventData(EventSystem.current);
        eventData1.position = TouchPos;
        eventData1.delta = TouchPos - lastTouchPos;
        eventData1.pointerPress = currentPressedObject; // 重要：设置按下对象
        eventData1.pressPosition = pressStartPos; // 重要：设置按下位置
        float distance = Vector2.Distance(TouchPos, pressStartPos);
        bool isDragging = distance >= dragThreshold;
        eventData1.dragging = isDragging;

        // 计算屏幕坐标的移动距离
        Vector2 screenDelta = TouchPos - lastTouchPos;
        float screenDeltaMagnitude = screenDelta.magnitude;
        
        // 计算事件频率（两次移动事件之间的时间间隔）
        float currentTime = Time.time;
        float timeSinceLastMove = lastMoveEventTime > 0 ? currentTime - lastMoveEventTime : 0f;
        float moveSpeed = timeSinceLastMove > 0 ? screenDeltaMagnitude / timeSinceLastMove : 0f;
        lastMoveEventTime = currentTime;
        lastMoveEventPos = TouchPos;
        
        //Debug.Log($"AdSceneManager：移动事件准备 - " +
        //          $"当前对象：{currentPressedObject.name} | " +
        //          $"屏幕坐标：当前={TouchPos}, 上次={lastTouchPos}, 按下={pressStartPos} | " +
        //          $"屏幕delta：{screenDelta} (大小：{screenDeltaMagnitude:F2}px) | " +
        //          $"事件间隔：{timeSinceLastMove:F3}秒 | " +
        //          $"移动速度：{moveSpeed:F1}px/秒 | " +
        //          $"累计距离：{distance:F2}px | " +
        //          $"阈值：{dragThreshold}px | " +
        //          $"状态：isDragging={isDragging}, isDragStarted={isDragStarted} | " +
        //          $"⚠️ 如果屏幕delta>{100}px或速度>{500}px/秒，可能是事件频率问题！");

        // 2. 优先处理滑动列表（拖拽类），无其他多余逻辑
        if (HandleScrollDrag(currentPressedObject, eventData1))
        {
            if (isDragging && !isDragStarted)
            {
                isDragStarted = true;
               // Debug.Log($"AdSceneManager：移动事件 - 拖拽状态已开启");
            }
            lastTouchPos = TouchPos;
            return;
        }
        // 计算偏移量
        Vector2 delta = TouchPos - lastTouchPos;
        lastTouchPos = TouchPos;
        float totalOffset = Vector2.Distance(TouchPos, pressStartPos);
        //Debug.Log($"AdSceneManager：移动详情 - " +
        //          $"当前对象：{currentPressedObject.name} | " +
        //          $"当前位置：{TouchPos} | " +
        //          $"增量偏移：{delta} | " +
        //          $"累计偏移：{totalOffset:F2}px | " +
        //          $"阈值：{dragThreshold}px | " +
        //          $"已开启拖拽：{isDragStarted}");

        // 创建事件数据
        PointerEventData eventData = CreateBasePointerEventData();
        if (eventData == null)
        {
            Debug.LogError("AdSceneManager：创建 PointerEventData 失败，跳过移动事件");
            return;
        }
        eventData.delta = delta;

        // 未开启拖拽：判断是否达标
        if (!isDragStarted)
        {
            if (totalOffset >= dragThreshold)
            {
               // Debug.Log($"AdSceneManager：累计偏移达标（{totalOffset:F2}px ≥ {dragThreshold}px），开启拖拽");
                IBeginDragHandler beginDragHandler = currentPressedObject.GetComponent<IBeginDragHandler>();
                if (beginDragHandler != null)
                {
                    beginDragHandler.OnBeginDrag(eventData);
                   // Debug.Log($"AdSceneManager：触发 IBeginDragHandler - {currentPressedObject.name}");
                }
                else
                {
                    Debug.LogWarning($"AdSceneManager：对象 {currentPressedObject.name} 未实现 IBeginDragHandler，无法开启拖拽");
                }
                isDragStarted = true;
            }
            else
            {
                Debug.Log($"AdSceneManager：累计偏移未达标（{totalOffset:F2}px < {dragThreshold}px），不开启拖拽");
            }
        }

        // 已开启拖拽：触发 OnDrag
        if (isDragStarted)
        {
            IDragHandler dragHandler = currentPressedObject.GetComponent<IDragHandler>();
            if (dragHandler != null)
            {
                dragHandler.OnDrag(eventData);
               // Debug.Log($"AdSceneManager：触发 IDragHandler - {currentPressedObject.name} | 增量偏移：{delta}");
            }
            else
            {
                Debug.LogWarning($"AdSceneManager：对象 {currentPressedObject.name} 未实现 IDragHandler，无法响应拖拽");
            }
        }
    }

    /// <summary>
    /// 执行抬起逻辑（防抖）
    /// </summary>
    private void ExecuteUpLogic(TouchData touchData)
    {
       // Debug.Log($"=== AdSceneManager：执行抬起逻辑 ===");
        Invoke(nameof(SimulatePointerUp), AntiShakeDelay / 1000f);
    }

    /// <summary>
    /// 模拟抬起（触发 OnEndDrag + OnPointerUp + 点击）
    /// </summary>
    private void SimulatePointerUp()
    {
     //   Debug.Log($"=== AdSceneManager：模拟抬起 ===");
        if (currentPressedObject == null)
        {
            Debug.LogWarning("AdSceneManager：当前无按压对象，跳过抬起");
            return;
        }

        PointerEventData eventData1 = new PointerEventData(EventSystem.current);
        eventData1.position = TouchPos;
        eventData1.pressPosition = pressStartPos;
        eventData1.pointerPress = null; // 抬起时设置为 null
        eventData1.dragging = false; // 抬起时设置为 false
        eventData1.delta = TouchPos - lastTouchPos;

        // 1. 处理拖拽类 UI（ScrollRect）的抬起
        if (HandleScrollDrag(currentPressedObject, eventData1))
        {
            ResetTouchState();
            return;
        }

        PointerEventData eventData = CreateBasePointerEventData();
        if (eventData == null)
        {
            Debug.LogError("AdSceneManager：创建 PointerEventData 失败，跳过抬起事件");
            ResetTouchState();
            return;
        }

        // 1. 触发 OnEndDrag（已拖拽时）
        if (isDragStarted)
        {
            IEndDragHandler endDragHandler = currentPressedObject.GetComponent<IEndDragHandler>();
            if (endDragHandler != null)
            {
                endDragHandler.OnEndDrag(eventData);
                //Debug.Log($"AdSceneManager：触发 IEndDragHandler - {currentPressedObject.name}");
            }
            else
            {
                Debug.LogWarning($"AdSceneManager：对象 {currentPressedObject.name} 未实现 IEndDragHandler");
            }
        }

        // 2. 触发 OnPointerUp
        IPointerUpHandler upHandler = currentPressedObject.GetComponent<IPointerUpHandler>();
        if (upHandler != null)
        {
            upHandler.OnPointerUp(eventData);
            //Debug.Log($"AdSceneManager：触发 IPointerUpHandler - {currentPressedObject.name}");
        }
        else
        {
            Debug.LogWarning($"AdSceneManager：对象 {currentPressedObject.name} 未实现 IPointerUpHandler");
        }

        // 3. 触发点击（未拖拽时）
        if (!isDragStarted)
        {
          //  Debug.Log($"AdSceneManager：未开启拖拽，触发点击事件");
            TriggerPointerClick(eventData);
        }
        else
        {
            Debug.Log($"AdSceneManager：已开启拖拽，跳过点击事件");
        }

        // 重置所有状态
        ResetTouchState();
    }
    #endregion

    #region 辅助方法（含日志）
    /// <summary>
    /// 射线检测（获取命中的有效对象）
    /// </summary>
    private GameObject GetRaycastTarget(Vector2 pos)
    {
        //Debug.Log($"=== AdSceneManager：射线检测 ===");
        if (EventSystem.current == null)
        {
            Debug.LogError("AdSceneManager：EventSystem 为空，射线检测失败");
            return null;
        }

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = pos;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

       // Debug.Log($"AdSceneManager：射线检测到 {results.Count} 个候选对象 | 检测坐标：{pos}");
        foreach (var result in results)
        {
            bool isActive = result.gameObject.activeInHierarchy;
            bool isValid = result.isValid;
          //  Debug.Log($"  - 候选对象：{result.gameObject.name} | 激活状态：{isActive} | 有效：{isValid} | 层级：");
            if (isActive && isValid)
            {
               // Debug.Log($"AdSceneManager：射线检测最终命中 - {result.gameObject.name}");
                return result.gameObject;
            }
        }
        Debug.LogWarning("AdSceneManager：射线检测未命中任何有效对象");
        return null;
    }

    /// <summary>
    /// 处理 Hover 状态切换（进入/离开）
    /// </summary>
    private void HandleHoverStateChange(GameObject currentTarget)
    {
        //Debug.Log($"=== AdSceneManager：处理 Hover 状态 ===");
        PointerEventData eventData = CreateBasePointerEventData();
        if (eventData == null)
        {
            Debug.LogError("AdSceneManager：创建 PointerEventData 失败，跳过 Hover 处理");
            return;
        }

        // 离开上一个对象
        if (currentPressedObject != currentTarget && currentPressedObject != null)
        {
            IPointerExitHandler exitHandler = currentPressedObject.GetComponent<IPointerExitHandler>();
            if (exitHandler != null)
            {
                exitHandler.OnPointerExit(eventData);
              //  Debug.Log($"AdSceneManager：触发 IPointerExitHandler - {currentPressedObject.name}");
            }
        }

        // 进入当前对象
        if (currentTarget != null && currentPressedObject != currentTarget)
        {
            IPointerEnterHandler enterHandler = currentTarget.GetComponent<IPointerEnterHandler>();
            if (enterHandler != null)
            {
                enterHandler.OnPointerEnter(eventData);
              //  Debug.Log($"AdSceneManager：触发 IPointerEnterHandler - {currentTarget.name}");
            }
        }
    }

    /// <summary>
    /// 触发点击事件
    /// </summary>
    private void TriggerPointerClick(PointerEventData eventData)
    {
      //  Debug.Log($"=== AdSceneManager：触发点击事件 ===");
        if (_eventSystem == null || eventData == null)
        {
          //  Debug.LogError("AdSceneManager：EventSystem 或 PointerEventData 为空，点击事件触发失败");
            return;
        }

        List<RaycastResult> results = new List<RaycastResult>();
        _eventSystem.RaycastAll(eventData, results);

        if (results.Count > 0)
        {
            GameObject targetObject = results[0].gameObject;
         //   Debug.Log($"AdSceneManager：点击命中对象 - {targetObject.name}");

            if (HandleUnityNativeUI(targetObject, eventData))
                return;
            IPointerClickHandler clickHandler = targetObject.GetComponent<IPointerClickHandler>();
            if (clickHandler != null)
            {
                clickHandler.OnPointerClick(eventData);
            //   Debug.Log($"AdSceneManager：触发 IPointerClickHandler - {targetObject.name}");
            }
            else
            {
                Debug.LogWarning($"AdSceneManager：对象 {targetObject.name} 未实现 IPointerClickHandler");
            }
        }
        else
        {
            Debug.LogWarning("AdSceneManager：点击未命中任何对象");
        }
    }

    /// <summary>
    /// 处理 Unity 原生 UI 组件
    /// </summary>
    private bool HandleUnityNativeUI(GameObject targetObject, PointerEventData eventData)
    {
        // 处理 Button
        Button button = targetObject.GetComponent<Button>();
        if (button != null && button.interactable)
        {
            button.onClick.Invoke();
           // Debug.Log($"AdSceneManager：触发原生 Button 点击 - {targetObject.name}");
            return true;
        }

        button = targetObject.GetComponentInParent<Button>();
        if (button != null && button.interactable)
        {
            button.onClick.Invoke();
           // Debug.Log($"AdSceneManager：触发父对象 Button 点击 - {button.gameObject.name}");
            return true;
        }

        // 处理 Toggle
        Toggle toggle = targetObject.GetComponent<Toggle>();
        if (toggle != null && toggle.interactable)
        {
            toggle.isOn = !toggle.isOn;
            toggle.onValueChanged.Invoke(toggle.isOn);
            //Debug.Log($"AdSceneManager：触发原生 Toggle 切换 - {targetObject.name} | 状态：{toggle.isOn}");
            return true;
        }

        // 处理 InputField
        InputField inputField = targetObject.GetComponent<InputField>();
        if (inputField != null && inputField.interactable)
        {
            inputField.ActivateInputField();
            //Debug.Log($"AdSceneManager：激活原生 InputField - {targetObject.name}");
            return true;
        }

        return false;
    }

    /// <summary>
    /// 修正版：处理滑动列表（ScrollRect）的拖拽逻辑（无 eventData.phase）
    /// </summary>
    private bool HandleScrollDrag(GameObject hitObject, PointerEventData eventData)
    {
        // 1. 找到父对象上的 ScrollRect（支持命中 Viewport/Content/列表项）
        ScrollRect scrollRect = hitObject.GetComponent<ScrollRect>() ?? hitObject.GetComponentInParent<ScrollRect>();
        if (scrollRect == null)
        {
            Debug.Log($"AdSceneManager：HandleScrollDrag - 未找到 ScrollRect | 命中对象：{hitObject.name}");
            return false; // 不是滑动列表，返回false走其他逻辑
        }

        // 坐标验证：检查坐标是否正确转换到 ScrollRect 的本地坐标系
        Camera canvasCamera = GetCanvasCamera();
        RectTransform viewRect = scrollRect.viewport != null ? scrollRect.viewport : scrollRect.GetComponent<RectTransform>();
        Vector2 localPos;
        bool coordinateValid = RectTransformUtility.ScreenPointToLocalPointInRectangle(
            viewRect, eventData.position, canvasCamera, out localPos);

        // 计算本地坐标差值（ScrollRect实际使用的）
        Vector2 localPressPos;
        bool pressCoordinateValid = RectTransformUtility.ScreenPointToLocalPointInRectangle(
            viewRect, eventData.pressPosition, canvasCamera, out localPressPos);
        Vector2 localDelta = localPos - localPressPos;
        float localDeltaMagnitude = localDelta.magnitude;
        
        // 计算屏幕坐标到本地坐标的转换比例（用于分析）
        Vector2 screenDelta = eventData.position - eventData.pressPosition;
        float screenDeltaMagnitude = screenDelta.magnitude;
        float conversionRatio = screenDeltaMagnitude > 0.01f ? localDeltaMagnitude / screenDeltaMagnitude : 1f;

        //Debug.Log($"AdSceneManager：HandleScrollDrag 详细分析 - " +
        //          $"ScrollRect：{scrollRect.gameObject.name} | " +
        //          $"状态：dragging={eventData.dragging}, BeginDragCalled={scrollRectBeginDragCalled} | " +
        //          $"屏幕坐标：当前={eventData.position}, 按下={eventData.pressPosition}, 屏幕delta={eventData.delta} (大小：{screenDeltaMagnitude:F2}px) | " +
        //          $"本地坐标：当前={localPos}, 按下={localPressPos}, 本地delta={localDelta} (大小：{localDeltaMagnitude:F2}) | " +
        //          $"坐标转换：当前={coordinateValid}, 按下={pressCoordinateValid}, 转换比例={conversionRatio:F3} | " +
        //          $"相机：{(canvasCamera != null ? canvasCamera.name : "null (Overlay)")} | " +
        //          $"⚠️ ScrollRect使用本地delta移动内容！如果本地delta>{50}，列表会移动很多！");

        // 2. 通过 UGUI 原生属性判断触摸阶段，触发对应 ScrollRect 接口
        // 按下阶段：pointerPress 不为空（表示有对象被按下），且未开始拖拽
        if (eventData.pointerPress != null && !eventData.dragging)
        {
            // 重置拖拽状态标志（使用手动滚动方案，不依赖ScrollRect的OnBeginDrag）
            scrollRectBeginDragCalled = false;
            scrollRectContentStartPos = Vector2.zero;
            scrollRectPressLocalPos = Vector2.zero;
           // Debug.Log($"AdSceneManager：ScrollRect 按下阶段 - 重置拖拽状态（将使用手动滚动方案）");
            
            // 触发 ScrollRect 按下逻辑（通过 IPointerDownHandler 接口）
            IPointerDownHandler downHandler = scrollRect as IPointerDownHandler;
            if (downHandler != null)
            {
                downHandler.OnPointerDown(eventData);
             //   Debug.Log($"AdSceneManager：ScrollRect 按下 - {scrollRect.gameObject.name} | 起点：{eventData.position} | pressPosition：{eventData.pressPosition}");
            }
            else
            {
                Debug.LogWarning($"AdSceneManager：ScrollRect 未实现 IPointerDownHandler - {scrollRect.gameObject.name}");
            }
        }
        // 拖拽阶段：dragging 为 true（表示已开启拖拽）
        else if (eventData.dragging)
        {
            // 参考论坛方案：手动计算并设置滚动位置，避免依赖ScrollRect的相机转换
            // 因为pressEventCamera可能无法获取（ScreenSpaceOverlay模式或跨Canvas场景）
            // 方案：不调用ScrollRect的OnDrag，而是手动计算本地坐标差值并设置content位置
            
            // 保存拖拽开始时的内容位置（只在第一次拖拽时保存）
            if (!scrollRectBeginDragCalled)
            {
                // 第一次拖拽，保存初始状态
                scrollRectContentStartPos = scrollRect.content != null ? scrollRect.content.anchoredPosition : Vector2.zero;
                scrollRectPressLocalPos = localPressPos; // 保存按下时的本地坐标
                scrollRectBeginDragCalled = true;
                
                //Debug.Log($"AdSceneManager：ScrollRect 手动拖拽开始 - {scrollRect.gameObject.name} | " +
                //          $"按下本地坐标：{localPressPos} | " +
                //          $"内容初始位置：{scrollRectContentStartPos} | " +
                //          $"✅ 使用手动滚动方案，避免相机转换问题！");
            }
            
            // 手动计算滚动位置：新位置 = 初始位置 + (当前本地坐标 - 按下本地坐标)
            Vector2 pointerDelta = localPos - scrollRectPressLocalPos;
            Vector2 newContentPos = scrollRectContentStartPos + pointerDelta;
            
            // 记录拖拽前的内容位置
            Vector2 contentPosBefore = scrollRect.content != null ? scrollRect.content.anchoredPosition : Vector2.zero;
            
            // 手动设置content位置（参考ScrollRect的SetContentAnchoredPosition逻辑）
            if (scrollRect.content != null)
            {
                scrollRect.content.anchoredPosition = newContentPos;
            }
            
            // 记录拖拽后的内容位置
            Vector2 contentPosAfter = scrollRect.content != null ? scrollRect.content.anchoredPosition : Vector2.zero;
            Vector2 actualContentDelta = contentPosAfter - contentPosBefore;
            float actualMoveDistance = actualContentDelta.magnitude;
            
            //Debug.Log($"AdSceneManager：ScrollRect 手动拖拽执行 - {scrollRect.gameObject.name} | " +
            //          $"屏幕delta：{eventData.delta} | " +
            //          $"本地delta：{localDelta} (大小：{localDeltaMagnitude:F2}) | " +
            //          $"pointerDelta：{pointerDelta} (大小：{pointerDelta.magnitude:F2}) | " +
            //          $"内容位置：{contentPosBefore} → {contentPosAfter} | " +
            //          $"实际移动：{actualContentDelta} (大小：{actualMoveDistance:F2}) | " +
            //          $"✅ 手动计算滚动位置，避免相机转换问题！");
        }
        // 抬起阶段：pointerPress 为空（按下对象已释放），且曾处于拖拽中
        else if (eventData.pointerPress == null && eventData.dragging == false && scrollRect.isActiveAndEnabled)
        {
            //Debug.Log($"AdSceneManager：ScrollRect 抬起阶段 - 已调用BeginDrag：{scrollRectBeginDragCalled}");
            
            // 触发 ScrollRect 抬起+结束拖拽逻辑
            IPointerUpHandler upHandler = scrollRect as IPointerUpHandler;
            IEndDragHandler endDragHandler = scrollRect as IEndDragHandler;

            if (endDragHandler != null)
            {
                endDragHandler.OnEndDrag(eventData);
                Debug.Log($"AdSceneManager：ScrollRect 结束拖拽 - {scrollRect.gameObject.name} | 最终位置：{eventData.position}");
            }
            if (upHandler != null)
            {
                upHandler.OnPointerUp(eventData);
                Debug.Log($"AdSceneManager：ScrollRect 抬起 - {scrollRect.gameObject.name}");
            }
        }
        else
        {
            //Debug.LogWarning($"AdSceneManager：HandleScrollDrag - 未匹配任何阶段 | " +
            //                $"pointerPress：{(eventData.pointerPress != null ? eventData.pointerPress.name : "null")} | " +
            //                $"dragging：{eventData.dragging} | " +
            //                $"isActiveAndEnabled：{scrollRect.isActiveAndEnabled}");
        }

        return true; // 已处理滑动，阻止事件继续传递
    }


    /// <summary>
    /// 获取 Canvas 的相机（用于坐标转换）
    /// </summary>
    private Camera GetCanvasCamera()
    {
        if (_eventSystem == null)
        {
            _eventSystem = EventSystem.current;
        }
        
        if (_eventSystem != null)
        {
            Canvas canvas = _eventSystem.GetComponent<Canvas>();
            if (canvas != null)
            {
                return canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
            }
        }
        
        // 尝试从场景中查找 Canvas
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (Canvas canvas in canvases)
        {
            if (canvas.isActiveAndEnabled)
            {
                return canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
            }
        }
        
        return null; // Screen Space - Overlay 模式返回 null
    }


    /// <summary>
    /// 创建标准化 PointerEventData
    /// </summary>
    private PointerEventData CreateBasePointerEventData()
    {
        if (_eventSystem == null)
        {
            _eventSystem = EventSystem.current;
            if (_eventSystem == null)
            {
                Debug.LogError("AdSceneManager：创建 PointerEventData 失败 - 未找到 EventSystem");
                return null;
            }
        }

        PointerEventData eventData = new PointerEventData(_eventSystem);
        eventData.position = TouchPos;
        eventData.pressPosition = pressStartPos;
        eventData.button = PointerEventData.InputButton.Left;
        eventData.pointerPress = currentPressedObject;
        eventData.pointerDrag = isDragStarted ? currentPressedObject : null;
        return eventData;
    }

    /// <summary>
    /// 重置触摸状态
    /// </summary>
    private void ResetTouchState()
    {
        //Debug.Log($"AdSceneManager：重置触摸状态 - " +
        //          $"当前对象：{(currentPressedObject != null ? currentPressedObject.name : "null")} | " +
        //          $"isDragStarted：{isDragStarted} | " +
        //          $"scrollRectBeginDragCalled：{scrollRectBeginDragCalled}");
        currentPressedObject = null;
        isDragStarted = false;
        isSimulatingMouseDown = false;
        lastTouchPos = Vector2.zero;
        pressStartPos = Vector2.zero;
        scrollRectBeginDragCalled = false; // 重置 ScrollRect 拖拽标志
        scrollRectContentStartPos = Vector2.zero; // 重置手动滚动状态
        scrollRectPressLocalPos = Vector2.zero; // 重置手动滚动状态
    }
    #endregion

    #region 兼容原有鼠标接口
    public Vector2 GetMousePos()
    {
        return SdkIsInit ? TouchPos : Input.mousePosition;
    }

    public bool GetMouseButton(int button)
    {
        if (!SdkIsInit)
            return Input.GetMouseButton(button);
        return button == 0 && isSimulatingMouseDown;
    }

    public bool GetMouseButtonDown(int button)
    {
        if (!SdkIsInit)
            return Input.GetMouseButtonDown(button);
        return button == 0 && isSimulatingMouseDown && !isDragStarted;
    }

    public bool GetMouseButtonUp(int button)
    {
        if (!SdkIsInit)
            return Input.GetMouseButtonUp(button);
        return button == 0 && !isSimulatingMouseDown;
    }
    #endregion
}

#region 数据结构定义
[System.Serializable]
public class AdSceneEventWrapper
{
    public string eventName;
    public int eventType;
    public TouchData eventData;
}

[System.Serializable]
public class TouchData
{
    public float y;
    public float x;
    public int eventType;
    public int gestureType;
}
#endregion
