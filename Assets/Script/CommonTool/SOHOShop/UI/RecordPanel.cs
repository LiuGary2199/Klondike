using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordPanel : BaseUIForms
{
    public Button BackBtn;
    public GameObject EmptyObj;
    public Transform Container;

    // Start is called before the first frame update
    void Start()
    {
        BackBtn.onClick.AddListener(() =>
        {
            CloseUIForm(SOHOShopUtil.PanelName(GetType().Name));
        });

    }


    public void InitData(string type)
    {
        int childCount = Container.childCount;
        if (childCount > 0)
        {
            for (int i = 0; i < childCount; i++)
            {
                DestroyImmediate(Container.GetChild(0).gameObject);
            }
        }

        int recordNum = 0;
        GameObject recordItemPrefab = Resources.Load<GameObject>("SOHOShop/UIPanel/" + (CommonUtil.IsPortrait() ? "Portrait" : "Landscape") + "/RecordItem");
        if (type == "cash")
        {
            // 加载现金提现列表
            CashRedeemManager.instance.CashRedeemList.ForEach((item) =>
            {
                if (item.state == Redeem.RedeemState.Complete)
                {
                    GameObject recordItem = Instantiate(recordItemPrefab, Container);
                    recordItem.GetComponent<RecordItem>().InitData(item);
                    recordNum++;
                }
            });
        }
        else
        {
            // 加载金币提现列表
            foreach(GoldRedeem item in GoldRedeemManager.instance.waitingGoldRedeems)
            {
                if (item.state == Redeem.RedeemState.Waiting)
                {
                    GameObject recordItem = Instantiate(recordItemPrefab, Container);
                    recordItem.GetComponent<RecordItem>().InitData(item);
                    recordNum++;
                }
            }
        }
        
        if (recordNum == 0)
        {
            EmptyObj.SetActive(true);
            Container.parent.parent.gameObject.SetActive(false);
        }
        else
        {
            EmptyObj.SetActive(false);
            Container.parent.parent.gameObject.SetActive(true);
            Container.GetComponent<RectTransform>().sizeDelta = new Vector2(Container.GetComponent<RectTransform>().sizeDelta.x, 238 * recordNum);
        }
    }

}
