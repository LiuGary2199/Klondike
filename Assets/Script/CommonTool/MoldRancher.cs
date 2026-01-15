using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>  </summary>
public class MoldRancher : TiltDigestion<MoldRancher>
{
    public Coroutine Award(float delay, System.Action action)
    {
        return StartCoroutine(AwardIE(delay, action));
    }
    IEnumerator AwardIE(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    public void FloeAward(Coroutine coroutine)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
    }
}
