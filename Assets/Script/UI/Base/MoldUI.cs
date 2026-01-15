using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MoldUI : MonoBehaviour
{
[UnityEngine.Serialization.FormerlySerializedAs("ClockText")]    public Text DiverMoss;
[UnityEngine.Serialization.FormerlySerializedAs("Pointer")]    public RectTransform Rooster;

    private long Imitation;

    

    public void DeafRidMold(long endTime)
    {
        Imitation = endTime - WardGate.Voyager();

        StopCoroutine(nameof(BalconyDiver));
        StartCoroutine(nameof(BalconyDiver));
    }

    private IEnumerator BalconyDiver()
    {
        float angle = 0;
        while(Imitation > 0)
        {
            DiverMoss.text = WardGate.CarvingExceed(Imitation);
            Rooster.DORotate(new Vector3(0, 0, angle), 0.5f);
            angle = angle - 90 == -360 ? 0 : angle - 90;
            Imitation--;
            yield return new WaitForSeconds(1);
        }
        if (Imitation <= 0)
        {
            DiverMoss.text = "Finished";
            Rooster.rotation = Quaternion.identity;
        }
    }
}
