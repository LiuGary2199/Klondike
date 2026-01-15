using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using Spine;
using Spine.Unity;

/// <summary> 刮刮卡 </summary>
public class ScratchPanel : BaseUIForms, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public int BrushRadius = 50;
    public float FinishPercent = 0.9f;
    public List<GameObject> ScratchShowList;
    public GameObject ShowSpine;
    bool IsFinish;
    public Texture2D MaskTexture;
    public RawImage MaskImage;
    public Transform Effect;
    Texture2D _Texture;
    int TextureWidth;
    int TextureHeight;
    float TextureLength;
    float TraseLength;
    Camera _Camera;
    Vector2 _LastPosition; // 记录上一次鼠标位置

    int[] TargetNums = new int[2];
    public Text[] TargetNumTexts;
    ScratchCardItemData[] ItemDatas = new ScratchCardItemData[9];
    public Transform[] Items;
    private List<int> ItemNumbers = new List<int>();


    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);
        Init();
        ScratchShowAnimation();
    }

    void Init()
    {
        //刮刮卡Ui部分初始化
        if (_Camera == null)
            _Camera = Camera.main;
        MaskImage.enabled = true;
        IsFinish = false;
        TraseLength = 0;
        Texture2D originalTexture = MaskTexture;
        _Texture = new Texture2D(originalTexture.width, originalTexture.height, TextureFormat.ARGB32, false);
        TextureWidth = _Texture.width;
        TextureHeight = _Texture.height;
        _Texture.SetPixels(originalTexture.GetPixels());
        _Texture.Apply();
        MaskImage.texture = _Texture;
        TextureLength = _Texture.GetPixels().Length;

        //数据初始化
        for (int i = 0; i < 2; i++)
        {
            TargetNums[i] = Random.Range(1, 100);
            TargetNumTexts[i].text = TargetNums[i].ToString();
        }

        // 生成中奖数据
        CreatItemNumbers();
        for (int i = 0; i < 9; i++)
        {
            RewardData data = GameDataManager.GetInstance().GetRewardDataByWeightAndRange(NetInfoMgr.instance._GameData.scratch_data_list);
            ScratchCardItemData Data = new ScratchCardItemData()
            {
                Num = ItemNumbers[i],
                RewardValue = data.num,
                _RewardType = data.type
            };
            ItemDatas[i] = Data;
        }
        SetItemUI();
        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.SFX_Scratch);
    }
    void SetItemUI()
    {
        for (int i = 0; i < 9; i++)
        {
            Items[i].Find("中奖数字Text").GetComponent<Text>().text = ItemDatas[i].Num.ToString();
            Items[i].Find("金币").gameObject.SetActive(ItemDatas[i]._RewardType == RewardType.Gold);
            Items[i].Find("钻石").gameObject.SetActive(ItemDatas[i]._RewardType == RewardType.Cash);
            Items[i].Find("中奖金额Text").GetComponent<Text>().text = "x " + ItemDatas[i].RewardValue.ToString();
            Items[i].Find("中奖框").gameObject.SetActive(false);
        }
    }

    void CreatItemNumbers()
    {
        ItemNumbers.Clear();
        // 9个随机数字（非中奖数字）
        for (int i = 0; i < 9; i++)
        {
            int randomNum;
            do
            {
                randomNum = Random.Range(1, 100);
            } while (randomNum == TargetNums[0] || randomNum == TargetNums[1]);
            ItemNumbers.Add(randomNum);
        }
        // 确定中奖数量
        int winCount = Random.Range(1, NetInfoMgr.instance._GameData.scratch_win_max_count);
        // 随机选择winCount个位置，将数字替换为中奖数字
        List<int> positions = new List<int>();
        for (int i = 0; i < 9; i++)
            positions.Add(i);
        // 打乱位置顺序
        for (int i = positions.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = positions[i];
            positions[i] = positions[j];
            positions[j] = temp;
        }
        // 将前winCount个位置替换为中奖数字
        for (int i = 0; i < winCount; i++)
        {
            int targetIndex = Random.Range(0, 2); // 随机选择TargetNums[0]或TargetNums[1]
            ItemNumbers[positions[i]] = TargetNums[targetIndex];
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsFinish) return;
        Vector2 currentPos = eventData.position;
        _LastPosition = currentPos;
        ErasePoint(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsFinish) return;

        // 在两点之间插值刮擦
        Vector2 currentPos = eventData.position;
        float distance = Vector2.Distance(_LastPosition, currentPos);
        int steps = Mathf.CeilToInt(distance / (BrushRadius * 0.5f));

        for (int i = 0; i <= steps; i++)
        {
            float t = steps > 0 ? (float)i / steps : 0;
            Vector2 lerpPos = Vector2.Lerp(_LastPosition, currentPos, t);
            ErasePoint(lerpPos);
        }

        _LastPosition = currentPos;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (IsFinish) return;
        ErasePoint(eventData.position);
    }

    void ErasePoint(Vector2 screenPos)
    {
        Vector3 tempWorldPoint = _Camera.ScreenToWorldPoint(screenPos);
        Vector3 tempLocalPoint = MaskImage.transform.InverseTransformPoint(tempWorldPoint);
        tempLocalPoint.z = 0;
        //Effect.localPosition = tempLocalPoint;
        Vector2Int pixelPos = new Vector2Int((int)tempLocalPoint.x + TextureWidth / 2, (int)tempLocalPoint.y + TextureHeight / 2);

        if (pixelPos.x < 0 || pixelPos.x >= TextureWidth || pixelPos.y < 0 || pixelPos.y >= TextureHeight) return;

        for (int i = -BrushRadius; i <= BrushRadius; i++)
        {
            if (pixelPos.x + i < 0 || pixelPos.x + i >= TextureWidth) continue;
            for (int j = -BrushRadius; j <= BrushRadius; j++)
            {
                if (pixelPos.y + j < 0 || pixelPos.y + j >= TextureHeight) continue;
                if (Mathf.Pow(i, 2) + Mathf.Pow(j, 2) > Mathf.Pow(BrushRadius, 2)) continue;

                Color color = _Texture.GetPixel(pixelPos.x + i, pixelPos.y + j);
                if (Mathf.Approximately(color.a, 0)) continue;

                color.a = 0;
                _Texture.SetPixel(pixelPos.x + i, pixelPos.y + j, color);
                TraseLength++;
            }
        }
        _Texture.Apply();
        RefreshErasePercent();
    }

    void RefreshErasePercent()
    {
        if (IsFinish)
            return;
        float tempPercent = TraseLength / TextureLength;
        if (tempPercent >= FinishPercent)
        {
            IsFinish = true;
            MaskImage.enabled = false;
            //统计奖励 多奖励合并
            RewardData ColdDate = new RewardData();
            RewardData CashDate = new RewardData();
            int WinIndex = 0;
            for (int i = 0; i < 9; i++)
            {
                if (ItemDatas[i].Num == TargetNums[0] || ItemDatas[i].Num == TargetNums[1])
                {
                    if (ItemDatas[i]._RewardType == RewardType.Gold)
                        ColdDate.num += ItemDatas[i].RewardValue;
                    else
                        CashDate.num += ItemDatas[i].RewardValue;

                    WinIndex++;
                    int ItemIndex = i;
                    TimeManager.GetInstance().Delay(WinIndex * 0.2f, () =>
                    {
                        Items[ItemIndex].Find("中奖框").gameObject.SetActive(true);
                        MusicMgr.GetInstance().PlayEffect(MusicType.UIMusic.SFX_ScratchGetReward);
                    });
                }
            }
            TimeManager.GetInstance().Delay(1.5f, () =>
            {
                CloseUIForm(nameof(ScratchPanel));
                OpenUIForm(nameof(RewardPanel)).GetComponent<RewardPanel>().Init(ColdDate, CashDate, null, "1012");
            });
        }
    }

    public void ScratchShowAnimation() 
    {
        // 重置骨骼到初始姿态
        SkeletonGraphic ItemSpine = ShowSpine.GetComponent<SkeletonGraphic>();
        ItemSpine.Skeleton.SetToSetupPose();
        ItemSpine.AnimationState.ClearTracks();
        // 强制立即更新状态（重要！）
        ItemSpine.Update(0);
        ItemSpine.AnimationState.SetAnimation(0, "ShowAni", false);
        AnimationController.WinPanelShow(ScratchShowList);
    }
}


[System.Serializable]
public class ScratchCardItemData //刮刮卡数据
{
    public int Num;
    public RewardType _RewardType;
    public float RewardValue;
}