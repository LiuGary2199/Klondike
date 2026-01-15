using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VerbFortune : MonoBehaviour
{
[UnityEngine.Serialization.FormerlySerializedAs("mask")]    public RectTransform Wait;
[UnityEngine.Serialization.FormerlySerializedAs("mypageview")]    public TaleOval Harmonious;
    private void Awake()
    {
        Harmonious.DyTaleUranus = Woodpecker;
    }

    void Woodpecker(int index)
    {
        if (index >= this.transform.childCount) return;
        Vector3 pos= this.transform.GetChild(index).GetComponent<RectTransform>().position;
        Wait.GetComponent<RectTransform>().position = pos;
    }
}
