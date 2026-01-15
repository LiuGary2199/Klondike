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
    public string ZooStew;
    [SerializeField]
    private GameObject Hilly= null;
    public GameObject Swear{ get { return Hilly; } }

    [SerializeField]
    private Button BeeGibbon= null;
    public Button ZooGibbon{ get { return BeeGibbon; } }

    public Sprite RefutePart;
    public Sprite InventorPart;
}

public class ZooRevolution : MonoBehaviour
{
    [SerializeField]
[UnityEngine.Serialization.FormerlySerializedAs("items")]    public List<TabItem> Peace= null;
[UnityEngine.Serialization.FormerlySerializedAs("Content")]
    public GameObject Totally;
[UnityEngine.Serialization.FormerlySerializedAs("ActiveAnimationObj")]    public GameObject RefuteSteamshipBur;
[UnityEngine.Serialization.FormerlySerializedAs("ActiveBG")]    public Sprite RefuteBG;
[UnityEngine.Serialization.FormerlySerializedAs("InactiveBG")]    public Sprite InventorBG;
[UnityEngine.Serialization.FormerlySerializedAs("ActiveColor")]    public Color RefuteEager;
[UnityEngine.Serialization.FormerlySerializedAs("InactiveColor")]    public Color InventorEager;
    [Header("初始选中Tab名称")]
[UnityEngine.Serialization.FormerlySerializedAs("ActiveTab")]    public GameObject RefuteZoo;

    private string CavernZooStew;

    private Dictionary<string, GameObject> BeeDiffer;

    private Action<string, GameObject> UrgeVariance;    // 打开tab回调

    // Start is called before the first frame update
    void Start()
    {
        BeeDiffer = new Dictionary<string, GameObject>();

        // Tab按钮绑定点击事件
        foreach (TabItem tabItem in Peace)
        {
            tabItem.ZooGibbon.onClick.AddListener(() =>
            {
                PlusZoo(tabItem.ZooStew);
            });
        }

        if (RefuteZoo != null)
        {
            foreach(TabItem tab in Peace)
            {
                if (tab.ZooGibbon.gameObject == RefuteZoo)
                {
                    CavernZooStew = tab.ZooStew;
                    break;
                }
            }
            if (!string.IsNullOrEmpty(CavernZooStew))
            {
                PlusZoo(CavernZooStew);
            }
        }
    }

    /// <summary>
    /// 打开tab页面
    /// </summary>
    /// <param name="_tabName"></param>
    public GameObject PlusZoo(string _tabName)
    {
        if (!string.IsNullOrEmpty(CavernZooStew) && BeeDiffer.ContainsKey(CavernZooStew))
        {
            if (BeeDiffer[CavernZooStew].GetComponent<SnowUIPlace>() != null)
            {
                BeeDiffer[CavernZooStew].GetComponent<SnowUIPlace>().Hidding();
            }
            else
            {
                BeeDiffer[CavernZooStew].SetActive(false);
            }
        }

        GameObject activeTabItem = null;
        foreach (TabItem tabItem in Peace)
        {
            tabItem.ZooGibbon.GetComponent<ZooAfarRevolution>().CudRefuteUI(tabItem.ZooStew.Equals(_tabName), this, tabItem);
            if (tabItem.ZooStew.Equals(_tabName))
            {
                activeTabItem = tabItem.ZooGibbon.gameObject;
                if (!BeeDiffer.ContainsKey(_tabName) && tabItem.Swear != null)
                {
                    GameObject tabItemPanel = Totally.transform.Find(tabItem.Swear.name) == null ? Instantiate(tabItem.Swear, Totally.transform) : tabItem.Swear;
                    BeeDiffer.Add(_tabName, tabItemPanel);
                }
            }
        }
        if (BeeDiffer.ContainsKey(_tabName))
        {
            if (BeeDiffer[_tabName].GetComponent<SnowUIPlace>() != null)
            {
                BeeDiffer[_tabName].GetComponent<SnowUIPlace>().Display(null);
            }
            else
            {
                BeeDiffer[_tabName]?.SetActive(true);
            }
        }

        CavernZooStew = _tabName;

        StartCoroutine(RefuteHeSteamship(activeTabItem));

        UrgeVariance?.Invoke(_tabName, BeeDiffer.ContainsKey(_tabName) ? BeeDiffer[_tabName] : null);

        return BeeDiffer.ContainsKey(_tabName) ? BeeDiffer[_tabName] : null;
    }

    // tab背景动画
    private IEnumerator RefuteHeSteamship(GameObject activeTabItem)
    {
        yield return new WaitForEndOfFrame();
        if (activeTabItem != null && RefuteSteamshipBur != null)
        {
            RefuteSteamshipBur.transform.SetParent(activeTabItem.transform);
            RefuteSteamshipBur.transform.SetSiblingIndex(0);
            RefuteSteamshipBur.GetComponent<RectTransform>().DOMoveX(activeTabItem.GetComponent<RectTransform>().position.x, 0.3f).SetEase(Ease.OutBack);
        }
    }

    /// <summary>
    /// 注册打开tab回调事件
    /// </summary>
    /// <param name="_callback"></param>
    public void IroquoisVariance(Action<string, GameObject> _callback)
    {
        UrgeVariance = _callback;
    }
}
