using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beholder : MonoBehaviour
{
    [SerializeField] private Vector2 triggerTimeLimits, coneDurationLimits;
    [SerializeField] private GameObject visionCone;
    private float timer;
    private float counter;
    private bool activeCone;

    private void Start()
    {
        timer = Random.Range(triggerTimeLimits.x, triggerTimeLimits.y);
    }

    private void Update()
    {
        counter += Time.deltaTime;

        if (!activeCone && counter >= timer)
        {
            visionCone.SetActive(true);
            GetComponent<EnemyHealthModule>().immune = true;
            activeCone = true;
            timer = Random.Range(coneDurationLimits.x, coneDurationLimits.y);
            counter = 0;
        }

        if (activeCone && counter >= timer)
        {
            visionCone.SetActive(false);
            GetComponent<EnemyHealthModule>().immune = false;
            activeCone = false;
            timer = Random.Range(triggerTimeLimits.x, triggerTimeLimits.y);
            counter = 0;
        }
    }
}