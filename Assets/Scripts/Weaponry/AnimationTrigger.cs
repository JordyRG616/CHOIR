using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationTrigger : MonoBehaviour
{
    public UnityEvent trigger;
    public float delay;
    private float timeCounter;
    public UnityEvent delayedTrigger;

    public void Trigger()
    {
        trigger?.Invoke();
    }

    public void StartDelayCount()
    {
        StartCoroutine(DelayCount());
    }

    private IEnumerator DelayCount()
    {
        var wait = new WaitForEndOfFrame();

        while (timeCounter < delay)
        {
            timeCounter += Time.deltaTime;

            yield return wait;
        }

        DelayTrigger();
        timeCounter = 0;
    }

    private void DelayTrigger()
    {
        delayedTrigger?.Invoke();
    }
}
