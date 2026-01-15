using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarveAfarUI : MonoBehaviour
{
[UnityEngine.Serialization.FormerlySerializedAs("Icon")]    public Image Part;
[UnityEngine.Serialization.FormerlySerializedAs("NumText")]    public Text LadMoss;

    public void Pearly(Sprite icon, int num = 0)
    {
        Part.sprite = icon;
        if (num == 0) {
            LadMoss.gameObject.SetActive(false);
        }
        else
        {
            LadMoss.text = "+" + num.ToString();
            LadMoss.gameObject.SetActive(true);
        }
    }
}
