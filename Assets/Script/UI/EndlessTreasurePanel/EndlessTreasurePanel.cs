using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using zeta_framework;

public class EndlessTreasurePanel : BaseUIForms
{
    public static EndlessTreasurePanel Instance;

    public Button CloseBtn;
    public Text ClockText;
    public Transform Container;
    public GameObject EndlessItemPrefab;

    private List<GameObject> endlessItemList = new();
    private int uiIndex;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        CloseBtn.onClick.AddListener(() => {
            CloseUIForm(GetType().Name);
        });
    }

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);

        uiIndex = ActivityEndlessTreasureCtrl.Instance.CurrentIndex;

        if (endlessItemList.Count == 0 )
        {
            InitList();
        }
        else
        {
            RefreshList();
        }

        ClockText.text = DateUtil.SecondsFormat2(ActivityEndlessTreasureCtrl.Instance.EndTime - DateUtil.Current());
    }

    private void InitList()
    {
        for(int i = Container.childCount - 1; i >= 0; i--)
        {
            Destroy(Container.GetChild(i).gameObject);
        }

        int currentIndex = ActivityEndlessTreasureCtrl.Instance.CurrentIndex;
        for (int i = 0; i < ActivityEndlessTreasureCtrl.Instance.endlessTreasures.Count; i++)
        {
            ActivityEndlessTreasureDB item = ActivityEndlessTreasureCtrl.Instance.endlessTreasures[i];
            GameObject itemUI = Instantiate(EndlessItemPrefab, Container);
            itemUI.GetComponent<EndlessItemUI>().RenderItem(item, i > currentIndex);
            endlessItemList.Add(itemUI);
        }

        RefreshList();
    }

    private void RefreshList()
    {
        int currentIndex = ActivityEndlessTreasureCtrl.Instance.CurrentIndex;
        for (int i = 0; i < endlessItemList.Count; i++)
        {
            endlessItemList[i].SetActive(i >= currentIndex);
        }
        endlessItemList[currentIndex].GetComponent<EndlessItemUI>().UnlockItem();
    }

    public void MoveAnimation() {
        int currentIndex = ActivityEndlessTreasureCtrl.Instance.CurrentIndex;
        if (uiIndex < currentIndex)
        {
            // 领奖后移动动画
            endlessItemList[uiIndex].GetComponent<CanvasGroup>().DOFade(0, 0.5f).OnComplete(() =>
            {
                endlessItemList[uiIndex].SetActive(false);
                endlessItemList[currentIndex].GetComponent<EndlessItemUI>().UnlockItem();
                uiIndex = currentIndex;
            });
        }
    }
}
