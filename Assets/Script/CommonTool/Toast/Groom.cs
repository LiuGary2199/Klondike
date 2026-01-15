using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Groom : SnowUIPlace
{
[UnityEngine.Serialization.FormerlySerializedAs("ToastText")]    public Text GroomMoss;

    

    public override void Display(object uiFormParams)
    {
        base.Display(uiFormParams);

        GroomMoss.text = uiFormParams.ToString();
        StartCoroutine(nameof(SeepFirstGroom));
    }

    private IEnumerator SeepFirstGroom()
    {
        yield return new WaitForSeconds(2);
        FirstUIMode(GetType().Name);
    }

}
