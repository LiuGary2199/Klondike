using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 导航插件
/// </summary>

[Serializable]
public class TabItem
{
    public string TabName;
    [SerializeField]
    private GameObject panel = null;
    public GameObject Panel { get { return panel; } }

    [SerializeField]
    private Button tabButton = null;
    public Button TabButton { get { return tabButton; } }

    public Sprite ActiveIcon;
    public Sprite InactiveIcon;
}

public class TabController : MonoBehaviour
{
    [SerializeField]
    public List<TabItem> items = null;

    public GameObject Content;
    public GameObject ActiveAnimationObj;
    public Sprite ActiveBG;
    public Sprite InactiveBG;
    public Color ActiveColor;
    public Color InactiveColor;
    [Header("初始选中Tab名称")]
    public GameObject ActiveTab;

    private string activeTabName;

    private Dictionary<string, GameObject> tabPanels;

    private Action<string, GameObject> openCallback;    // 打开tab回调

    // Start is called before the first frame update
    void Start()
    {
        tabPanels = new Dictionary<string, GameObject>();

        // Tab按钮绑定点击事件
        foreach (TabItem tabItem in items)
        {
            tabItem.TabButton.onClick.AddListener(() =>
            {
                OpenTab(tabItem.TabName);
            });
        }

        if (ActiveTab != null)
        {
            foreach(TabItem tab in items)
            {
                if (tab.TabButton.gameObject == ActiveTab)
                {
                    activeTabName = tab.TabName;
                    break;
                }
            }
            if (!string.IsNullOrEmpty(activeTabName))
            {
                OpenTab(activeTabName);
            }
        }
    }

    /// <summary>
    /// 打开tab页面
    /// </summary>
    /// <param name="_tabName"></param>
    public GameObject OpenTab(string _tabName)
    {
        if (!string.IsNullOrEmpty(activeTabName) && tabPanels.ContainsKey(activeTabName))
        {
            if (tabPanels[activeTabName].GetComponent<BaseUIForms>() != null)
            {
                tabPanels[activeTabName].GetComponent<BaseUIForms>().Hidding();
            }
            else
            {
                tabPanels[activeTabName].SetActive(false);
            }
        }

        GameObject activeTabItem = null;
        foreach (TabItem tabItem in items)
        {
            tabItem.TabButton.GetComponent<TabItemController>().SetActiveUI(tabItem.TabName.Equals(_tabName), this, tabItem);
            if (tabItem.TabName.Equals(_tabName))
            {
                activeTabItem = tabItem.TabButton.gameObject;
                if (!tabPanels.ContainsKey(_tabName) && tabItem.Panel != null)
                {
                    GameObject tabItemPanel = Content.transform.Find(tabItem.Panel.name) == null ? Instantiate(tabItem.Panel, Content.transform) : tabItem.Panel;
                    tabPanels.Add(_tabName, tabItemPanel);
                }
            }
        }
        if (tabPanels.ContainsKey(_tabName))
        {
            if (tabPanels[_tabName].GetComponent<BaseUIForms>() != null)
            {
                tabPanels[_tabName].GetComponent<BaseUIForms>().Display(null);
            }
            else
            {
                tabPanels[_tabName]?.SetActive(true);
            }
        }

        activeTabName = _tabName;

        StartCoroutine(ActiveBgAnimation(activeTabItem));

        openCallback?.Invoke(_tabName, tabPanels.ContainsKey(_tabName) ? tabPanels[_tabName] : null);

        return tabPanels.ContainsKey(_tabName) ? tabPanels[_tabName] : null;
    }

    // tab背景动画
    private IEnumerator ActiveBgAnimation(GameObject activeTabItem)
    {
        yield return new WaitForEndOfFrame();
        if (activeTabItem != null && ActiveAnimationObj != null)
        {
            ActiveAnimationObj.transform.SetParent(activeTabItem.transform);
            ActiveAnimationObj.transform.SetSiblingIndex(0);
            ActiveAnimationObj.GetComponent<RectTransform>().DOMoveX(activeTabItem.GetComponent<RectTransform>().position.x, 0.3f).SetEase(Ease.OutBack);
        }
    }

    /// <summary>
    /// 注册打开tab回调事件
    /// </summary>
    /// <param name="_callback"></param>
    public void RegisterCallback(Action<string, GameObject> _callback)
    {
        openCallback = _callback;
    }
}
