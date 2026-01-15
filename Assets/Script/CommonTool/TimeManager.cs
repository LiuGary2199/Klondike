using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>  </summary>
public class TimeManager : MonoSingleton<TimeManager>
{
    public Coroutine Delay(float delay, System.Action action)
    {
        return StartCoroutine(DelayIE(delay, action));
    }
    IEnumerator DelayIE(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    public void StopDelay(Coroutine coroutine)
    {
        if (coroutine != null)
            StopCoroutine(coroutine);
    }
}
