using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary> 刮刮卡 </summary>
public class ThriftySwear : SnowUIPlace, IPointerDownHandler, IDragHandler, IPointerUpHandler, IBeginDragHandler
{
[UnityEngine.Serialization.FormerlySerializedAs("BrushRadius")]    public int DecayWonder= 50;
[UnityEngine.Serialization.FormerlySerializedAs("FinishPercent")]    public float JobberEmotive= 0.9f;
[UnityEngine.Serialization.FormerlySerializedAs("ScratchShowList")]    public List<GameObject> ThriftyBindTrip;
[UnityEngine.Serialization.FormerlySerializedAs("ShowSpine")]    public GameObject BindDepot;
    bool UpJobber;
[UnityEngine.Serialization.FormerlySerializedAs("MaskTexture")]    public Texture2D VerbRoadway;
[UnityEngine.Serialization.FormerlySerializedAs("MaskImage")]    public RawImage VerbFifth;
[UnityEngine.Serialization.FormerlySerializedAs("Effect")]    public Transform Guinea;
    Texture2D _Roadway;
    int RoadwayAgree;
    int RoadwayMilieu;
    float RoadwayCarbon;
    float MouthCarbon;
    Camera _Decade;
    Vector2 _BulkCitation; // 记录上一次鼠标位置

    int[] LondonMill= new int[2];
[UnityEngine.Serialization.FormerlySerializedAs("TargetNumTexts")]    public Text[] LondonLadVogue;
    ScratchCardItemData[] AfarModem= new ScratchCardItemData[9];
[UnityEngine.Serialization.FormerlySerializedAs("Items")]    public Transform[] Rural;
    private List<int> AfarVestige= new List<int>();


    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);
        Deaf();
        ThriftyBindSteamship();
    }

    public override void Hidding()
    {
        base.Hidding();
    }

   
    // 辅助方法：判断PointerEventData是否命中当前卡牌
    private bool IsPointerOverSelf(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        foreach (var result in results)
        {
            if (result.gameObject == this.gameObject)
            {
                return true;
            }
        }
        return false;
    }



    void Deaf()
    {
        //刮刮卡Ui部分初始化
        if (_Decade == null)
            _Decade = Camera.main;
        VerbFifth.enabled = true;
        UpJobber = false;
        MouthCarbon = 0;
        Texture2D originalTexture = VerbRoadway;
        _Roadway = new Texture2D(originalTexture.width, originalTexture.height, TextureFormat.ARGB32, false);
        RoadwayAgree = _Roadway.width;
        RoadwayMilieu = _Roadway.height;
        _Roadway.SetPixels(originalTexture.GetPixels());
        _Roadway.Apply();
        VerbFifth.texture = _Roadway;
        RoadwayCarbon = _Roadway.GetPixels().Length;

        //数据初始化
        for (int i = 0; i < 2; i++)
        {
            LondonMill[i] = Random.Range(1, 100);
            LondonLadVogue[i].text = LondonMill[i].ToString();
        }

        // 生成中奖数据
        AptlyAfarVestige();
        for (int i = 0; i < 9; i++)
        {
            RewardData Mark= KickLineRancher.FarBefriend().FarStarveLineSoRevertBusDelay(ToeBoldLeg.instance._KickLine.scratch_data_list);
            ScratchCardItemData Line= new ScratchCardItemData()
            {
                Lad = AfarVestige[i],
                StarveDodge = Mark.num,
                _StarveJoke = Mark.type
            };
            AfarModem[i] = Line;
        }
        CudAfarUI();
        LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_Scratch);
    }
    void CudAfarUI()
    {
        for (int i = 0; i < 9; i++)
        {
            Rural[i].Find("中奖数字Text").GetComponent<Text>().text = AfarModem[i].Lad.ToString();
            Rural[i].Find("金币").gameObject.SetActive(AfarModem[i]._StarveJoke == RewardType.Gold);
            Rural[i].Find("钻石").gameObject.SetActive(AfarModem[i]._StarveJoke == RewardType.Cash);
            Rural[i].Find("中奖金额Text").GetComponent<Text>().text = AfarModem[i].StarveDodge.ToString();
            Rural[i].Find("中奖框").gameObject.SetActive(false);
        }
    }

    void AptlyAfarVestige()
    {
        AfarVestige.Clear();
        // 9个随机数字（非中奖数字）
        for (int i = 0; i < 9; i++)
        {
            int randomNum;
            do
            {
                randomNum = Random.Range(1, 100);
            } while (randomNum == LondonMill[0] || randomNum == LondonMill[1]);
            AfarVestige.Add(randomNum);
        }
        // 确定中奖数量
        int winCount = Random.Range(1, ToeBoldLeg.instance._KickLine.scratch_win_max_count);
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
            AfarVestige[positions[i]] = LondonMill[targetIndex];
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (UpJobber) return;
        Vector2 currentPos = eventData.position;
        _BulkCitation = currentPos;
        SeizePerry(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (UpJobber) return;

        // 在两点之间插值刮擦
        Vector2 currentPos = eventData.position;
        float distance = Vector2.Distance(_BulkCitation, currentPos);
        int steps = Mathf.CeilToInt(distance / (DecayWonder * 0.5f));

        for (int i = 0; i <= steps; i++)
        {
            float t = steps > 0 ? (float)i / steps : 0;
            Vector2 lerpPos = Vector2.Lerp(_BulkCitation, currentPos, t);
            SeizePerry(lerpPos);
        }

        _BulkCitation = currentPos;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (UpJobber) return;
        SeizePerry(eventData.position);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // AdSceneManager 需要此接口才能正确触发 OnDrag
        // 实际刮擦逻辑在 OnDrag 中处理
    }

    void SeizePerry(Vector2 screenPos)
    {
        Vector3 tempWorldPoint = _Decade.ScreenToWorldPoint(screenPos);
        Vector3 tempLocalPoint = VerbFifth.transform.InverseTransformPoint(tempWorldPoint);
        tempLocalPoint.z = 0;
        //Effect.localPosition = tempLocalPoint;
        Vector2Int pixelPos = new Vector2Int((int)tempLocalPoint.x + RoadwayAgree / 2, (int)tempLocalPoint.y + RoadwayMilieu / 2);

        if (pixelPos.x < 0 || pixelPos.x >= RoadwayAgree || pixelPos.y < 0 || pixelPos.y >= RoadwayMilieu) return;

        for (int i = -DecayWonder; i <= DecayWonder; i++)
        {
            if (pixelPos.x + i < 0 || pixelPos.x + i >= RoadwayAgree) continue;
            for (int j = -DecayWonder; j <= DecayWonder; j++)
            {
                if (pixelPos.y + j < 0 || pixelPos.y + j >= RoadwayMilieu) continue;
                if (Mathf.Pow(i, 2) + Mathf.Pow(j, 2) > Mathf.Pow(DecayWonder, 2)) continue;

                Color color = _Roadway.GetPixel(pixelPos.x + i, pixelPos.y + j);
                if (Mathf.Approximately(color.a, 0)) continue;

                color.a = 0;
                _Roadway.SetPixel(pixelPos.x + i, pixelPos.y + j, color);
                MouthCarbon++;
            }
        }
        _Roadway.Apply();
        BalconySeizeEmotive();
    }

    void BalconySeizeEmotive()
    {
        if (UpJobber)
            return;
        float tempPercent = MouthCarbon / RoadwayCarbon;
        if (tempPercent >= JobberEmotive)
        {
            UpJobber = true;
            VerbFifth.enabled = false;
            //统计奖励 多奖励合并
            RewardData ColdDate = new RewardData();
            RewardData CashDate = new RewardData();
            int WinIndex = 0;
            for (int i = 0; i < 9; i++)
            {
                if (AfarModem[i].Lad == LondonMill[0] || AfarModem[i].Lad == LondonMill[1])
                {
                    if (AfarModem[i]._StarveJoke == RewardType.Gold)
                        ColdDate.num += AfarModem[i].StarveDodge;
                    else
                        CashDate.num += AfarModem[i].StarveDodge;

                    WinIndex++;
                    int ItemIndex = i;
                    MoldRancher.FarBefriend().Award(WinIndex * 0.2f, () =>
                    {
                        Rural[ItemIndex].Find("中奖框").gameObject.SetActive(true);
                        LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.SFX_ScratchGetReward);
                    });
                }
            }
            MoldRancher.FarBefriend().Award(1.5f, () =>
            {
                FirstUIMode(nameof(ThriftySwear));
                PlusUIMode(nameof(StarveSwear)).GetComponent<StarveSwear>().Deaf(ColdDate, CashDate, null, "1012");
            });
        }
    }

    public void ThriftyBindSteamship()
    {
        // 重置骨骼到初始姿态
        SkeletonGraphic ItemSpine = BindDepot.GetComponent<SkeletonGraphic>();
        ItemSpine.Skeleton.SetToSetupPose();
        ItemSpine.AnimationState.ClearTracks();
        // 强制立即更新状态（重要！）
        ItemSpine.Update(0);
        ItemSpine.AnimationState.SetAnimation(0, "ShowAni", false);
        SteamshipRevolution.UrnSwearBind(ThriftyBindTrip);
    }
}


[System.Serializable]
public class ScratchCardItemData //刮刮卡数据
{
    public int Lad;
    public RewardType _StarveJoke;
    public float StarveDodge;
}