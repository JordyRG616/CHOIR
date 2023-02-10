using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEffect : WeaponEffect
{
    [SerializeField] private float multiplier;
    [SerializeField] private float effectDuration;

    private WaitForSeconds effectTime;

    protected override void Awake()
    {
        base.Awake();
        effectTime = new WaitForSeconds(effectDuration);
    }

    protected override void Effect(GameObject enemy)
    {
        var healthController = enemy.GetComponent<EnemyHealthModule>();
        if (healthController.exposedMultiplier > 0) return;


        StartCoroutine(FreezeEnemy(healthController));
    }

    private IEnumerator FreezeEnemy(EnemyHealthModule healthController)
    {
        healthController.exposedMultiplier = multiplier + WeaponMasterController.Main.exposedMultiplier;

        yield return effectTime;

        healthController.exposedMultiplier = 0;
    }

    public float RaiseDuration(float value)
    {
        effectDuration += value;
        return effectDuration;
    }
}
