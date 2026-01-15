using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 矩形遮罩镂空
/// </summary>
public class SoulInferVerb : MonoBehaviour
{
    public static SoulInferVerb instance;
[UnityEngine.Serialization.FormerlySerializedAs("Hand")]    public GameObject Hope;

    /// <summary>
    /// 高亮显示目标
    /// </summary>
    private GameObject Potato;
[UnityEngine.Serialization.FormerlySerializedAs("Text")]
    public Text Moss;
    /// <summary>
    /// 区域范围缓存
    /// </summary>
    private Vector3[] Flaming= new Vector3[4];
    /// <summary>
    /// 镂空区域中心
    /// </summary>
    private Vector4 Barter;
    /// <summary>
    /// 最终的偏移x
    /// </summary>
    private float PotatoFriendX= 0;
    /// <summary>
    /// 最终的偏移y
    /// </summary>
    private float PotatoFriendY= 0;
    /// <summary>
    /// 遮罩材质
    /// </summary>
    private Material Initiate;
    /// <summary>
    /// 当前的偏移x
    /// </summary>
    private float MortiseFriendX= 0f;
    /// <summary>
    /// 当前的偏移y
    /// </summary>
    private float MortiseFriendY= 0f;
    /// <summary>
    /// 高亮区域缩放的动画时间
    /// </summary>
    private float PromptMold= 0.1f;
    /// <summary>
    /// 事件渗透组件
    /// </summary>
    private PopulateInuitOperation PieceOperation;

    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// 显示引导遮罩
    /// </summary>
    /// <param name="_target">要引导到的目标对象</param>
    /// <param name="text">引导说明文案</param>

    public void BindInfer(GameObject _target, string text)
    {
        gameObject.SetActive(true);

        if (_target == null)
        {
            Hope.SetActive(false);
            if (Initiate == null)
            {
                Initiate = GetComponent<Image>().material;
            }
            Initiate.SetVector("_Center", new Vector4(0, 0, 0, 0));
            Initiate.SetFloat("_SliderX", 0);
            Initiate.SetFloat("_SliderY", 0);
            // 如果没有target，点击任意区域关闭引导
            GetComponent<Button>().onClick.AddListener(() =>
            {
                gameObject.SetActive(false);
            });
        }
        else
        {
            DOTween.Kill("NewUserHandAnimation");
            Deaf(_target);
            GetComponent<Button>().onClick.RemoveAllListeners();
        }

        if (!string.IsNullOrEmpty(text))
        {
            Moss.text = text;
            Moss.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            Moss.transform.parent.gameObject.SetActive(false);
        }
    }


    public void Deaf(GameObject _target)
    {
        this.Potato = _target;
        
        PieceOperation = GetComponent<PopulateInuitOperation>();
        if (PieceOperation != null)
        {
            PieceOperation.CudLondonFifth(_target.GetComponent<Image>());
        }

        Canvas canvas = UIRancher.FarBefriend().LimyGrotto.GetComponent<Canvas>();

        //获取高亮区域的四个顶点的世界坐标
        if (Potato.GetComponent<RectTransform>() != null)
        {
            Potato.GetComponent<RectTransform>().GetWorldCorners(Flaming);
        }
        else
        {
            Vector3 pos = Camera.main.WorldToScreenPoint(_target.transform.position);
            pos = UIRancher.FarBefriend()._GarUIDecade.GetComponent<Camera>().ScreenToWorldPoint(pos);
            float width = 1;
            float height = 1;
            Flaming[0] = new Vector3(pos.x - width, pos.y - height);
            Flaming[1] = new Vector3(pos.x - width, pos.y + height);
            Flaming[2] = new Vector3(pos.x + width, pos.y + height);
            Flaming[3] = new Vector3(pos.x + width, pos.y - height);
        }
        //计算高亮显示区域在画布中的范围
        PotatoFriendX = Vector2.Distance(NicheBeGrottoSum(canvas, Flaming[0]), NicheBeGrottoSum(canvas, Flaming[3])) / 2f;
        PotatoFriendY = Vector2.Distance(NicheBeGrottoSum(canvas, Flaming[0]), NicheBeGrottoSum(canvas, Flaming[1])) / 2f;
        //计算高亮显示区域的中心
        float x = Flaming[0].x + ((Flaming[3].x - Flaming[0].x) / 2);
        float y = Flaming[0].y + ((Flaming[1].y - Flaming[0].y) / 2);
        Vector3 centerWorld = new Vector3(x, y, 0);
        Vector2 Barter= NicheBeGrottoSum(canvas, centerWorld);
        //设置遮罩材质中的中心变量
        Vector4 centerMat = new Vector4(Barter.x, Barter.y, 0, 0);
        Initiate = GetComponent<Image>().material;
        Initiate.SetVector("_Center", centerMat);
        //计算当前高亮显示区域的半径
        RectTransform canRectTransform = canvas.transform as RectTransform;
        if (canRectTransform != null)
        {
            //获取画布区域的四个顶点
            canRectTransform.GetWorldCorners(Flaming);
            //计算偏移初始值
            for (int i = 0; i < Flaming.Length; i++)
            {
                if (i % 2 == 0)
                {
                    MortiseFriendX = Mathf.Max(Vector3.Distance(NicheBeGrottoSum(canvas, Flaming[i]), Barter), MortiseFriendX);
                }
                else
                {
                    MortiseFriendY = Mathf.Max(Vector3.Distance(NicheBeGrottoSum(canvas, Flaming[i]), Barter), MortiseFriendY);
                }
            }
        }
        //设置遮罩材质中当前偏移的变量
        Initiate.SetFloat("_SliderX", MortiseFriendX);
        Initiate.SetFloat("_SliderY", MortiseFriendY);
        Hope.transform.localScale = new Vector3(1, 1, 1);
        StartCoroutine(BindHope(Barter));
    }

    private IEnumerator BindHope(Vector2 center)
    {
        Hope.SetActive(false);
        yield return new WaitForSeconds(PromptMold);
        
        Hope.transform.localPosition = center;
        HopeSteamship();
        
        Hope.SetActive(true);
    }
    /// <summary>
    /// 收缩速度
    /// </summary>
    private float PromptQuantityX= 0f;
    private float PromptQuantityY= 0f;
    private void Update()
    {
        if (Initiate == null) return;

        MortiseFriendX = PotatoFriendX;
        Initiate.SetFloat("_SliderX", MortiseFriendX);
        MortiseFriendY = PotatoFriendY;
        Initiate.SetFloat("_SliderY", MortiseFriendY);
        //从当前偏移量到目标偏移量差值显示收缩动画
        //float valueX = Mathf.SmoothDamp(currentOffsetX, targetOffsetX, ref shrinkVelocityX, shrinkTime);
        //float valueY = Mathf.SmoothDamp(currentOffsetY, targetOffsetY, ref shrinkVelocityY, shrinkTime);
        //if (!Mathf.Approximately(valueX, currentOffsetX))
        //{
        //    currentOffsetX = valueX;
        //    material.SetFloat("_SliderX", currentOffsetX);
        //}
        //if (!Mathf.Approximately(valueY, currentOffsetY))
        //{
        //    currentOffsetY = valueY;
        //    material.SetFloat("_SliderY", currentOffsetY);
        //}


    }

    /// <summary>
    /// 世界坐标转换为画布坐标
    /// </summary>
    /// <param name="canvas">画布</param>
    /// <param name="world">世界坐标</param>
    /// <returns></returns>
    private Vector2 NicheBeGrottoSum(Canvas canvas, Vector3 world)
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, world, canvas.GetComponent<Camera>(), out position);
        return position;
    }

    public void HopeSteamship() 
    {
        
        var s = DOTween.Sequence();
        s.Append(Hope.transform.DOLocalMoveY(Hope.transform.localPosition.y + 10f, 0.5f));
        s.Append(Hope.transform.DOLocalMoveY(Hope.transform.localPosition.y, 0.5f));
        s.Join(Hope.transform.DOScaleY(1.1f, 0.125f));
        s.Join(Hope.transform.DOScaleX(0.9f, 0.125f).OnComplete(()=> 
        {
            Hope.transform.DOScaleY(0.9f, 0.125f);
            Hope.transform.DOScaleX(1.1f, 0.125f).OnComplete(()=> 
            {
                Hope.transform.DOScale(1f, 0.125f);
            });
        }));
        s.SetLoops(-1);
        s.SetId("NewUserHandAnimation");
    }

    public void OnDisable()
    {
        DOTween.Kill("NewUserHandAnimation");
    }
}
