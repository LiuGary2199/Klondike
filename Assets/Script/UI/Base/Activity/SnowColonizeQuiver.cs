using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zeta_framework;

public class SnowColonizeQuiver : MonoBehaviour
{
[UnityEngine.Serialization.FormerlySerializedAs("activity_id")]    public string Northern_To;
[UnityEngine.Serialization.FormerlySerializedAs("CountdownText")]    public Text FoodstuffMoss;

    private Colonize Northern;

    // Start is called before the first frame update
    void Start()
    {
        Northern = ColonizePeak.Instance.FarColonizeSoAt<Colonize>(Northern_To);
        UranusBindWidow();
        
        if (!string.IsNullOrEmpty(Northern.Hilly) && GetComponent<Button>() != null)
        {
            GetComponent<Button>().onClick.AddListener(() => {
                UIRancher.FarBefriend().BindUIPlace(ColonizePeak.Instance.FarColonizeSoAt<Colonize>(Northern_To).Hilly);
            });
        }

        // 监听活动状态变化，显示/隐藏图标
        AfricanExtendHorse.FarBefriend().Iroquois(CAdjoin.We_ColonizeWidowUranus_ + Northern_To, (md) => {
            UranusBindWidow();
        });
    }

    /// <summary>
    /// 根据活动状态，确定是否显示当前活动prefab
    /// </summary>
    private void UranusBindWidow()
    {
        gameObject.SetActive(Northern.TeleBind());
        if (transform.parent.GetComponent<TautPotatoDuctless>() != null)
        {
            transform.parent.GetComponent<TautPotatoDuctless>().BalconyPotato();
        }

        if (FoodstuffMoss != null)
        {
            if(Northern.Widow == ActivityState.Attending)
            {
                FoodstuffMoss.transform.parent.gameObject.SetActive(true);
                FoodstuffMoss.text = WardGate.CarvingExceed2(Northern.RidMold - WardGate.Voyager());
            } 
            else if(Northern.Widow == ActivityState.NeedSettlement)
            {
                FoodstuffMoss.transform.parent.gameObject.SetActive(true);
                FoodstuffMoss.text = "Claim";
            }
            else
            {
                FoodstuffMoss.transform.parent.gameObject.SetActive(false);
            }
        }
    }
}
