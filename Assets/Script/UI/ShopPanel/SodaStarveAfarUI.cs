using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using zeta_framework;

public class SodaStarveAfarUI : MonoBehaviour
{
[UnityEngine.Serialization.FormerlySerializedAs("ItemIcon")]    public Image AfarPart;
[UnityEngine.Serialization.FormerlySerializedAs("ItemNum")]    public Text AfarLad;

    public void Deaf(AfarPerry itemGroup)
    {
        AfarPart.sprite = itemGroup.Afar.Part;
        AfarLad.text = "X" + itemGroup.Seal_Toy.ToString();
    }
}
