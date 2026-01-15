using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DishPerry : MonoBehaviour
{
[UnityEngine.Serialization.FormerlySerializedAs("InitGroup")]    public GameObject DeafPerry;
[UnityEngine.Serialization.FormerlySerializedAs("ItemSprites")]    public Sprite[] AfarOutflow;

    private GameObject EthologyMedalPepper;
    private float SealAgree= 147f; // 两个item的position.x之差

    // Start is called before the first frame update
    void Start()
    {
        EthologyMedalPepper = DeafPerry.transform.Find("SlotCard_1").gameObject;
        float x = SealAgree * 3;
        int multiCount = ToeBoldLeg.instance.DeafLine.slot_group.Count;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < multiCount; j++)
            {
                GameObject fangkuai = Instantiate(EthologyMedalPepper, DeafPerry.transform);
                fangkuai.transform.localPosition = new Vector3(x + SealAgree * multiCount * i + SealAgree * j, EthologyMedalPepper.transform.localPosition.y, 0);
                fangkuai.transform.Find("Text").GetComponent<Text>().text = "×" + ToeBoldLeg.instance.DeafLine.slot_group[j].multi;
                fangkuai.transform.GetComponent<Image>().sprite = AfarOutflow[j % AfarOutflow.Length];
            }
        }
    }

    public void RateMedal()
    {
        DeafPerry.GetComponent<RectTransform>().localPosition = new Vector3(0, -10, 0);
    }

    public void None(int index, Action<float> finish)
    {
        LeafyLeg.FarBefriend().HaulGuinea(LeafyJoke.UIMusic.Sound_OneArmBandit);
        SteamshipRevolution.SuccessiveScroll(DeafPerry, -(SealAgree * 2 + SealAgree * ToeBoldLeg.instance.DeafLine.slot_group.Count * 3 + SealAgree * (index + 1)), () =>
        {
            finish?.Invoke((float)ToeBoldLeg.instance.DeafLine.slot_group[index].multi);
        });
    }
}
