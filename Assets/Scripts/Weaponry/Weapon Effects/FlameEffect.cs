using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameEffect : WeaponEffect
{
    [SerializeField] private float effectDuration;
    [SerializeField] private float damagePerTick;
    [SerializeField] private float tickInterval;

    private WaitForSeconds tickTime;

    protected override void Awake()
    {
        base.Awake();

        tickTime = new WaitForSeconds(tickInterval);
    }

    protected override void Effect(GameObject enemy)
    {
        var healthController = enemy.GetComponent<EnemyHealthController>();
        if (healthController.isBurning) return;

        StartCoroutine(DamagePerTick(healthController));
    }

    private IEnumerator DamagePerTick(EnemyHealthController healthController)
    {
        healthController.isBurning = true;

        float step = 0;

        var globalIncrement = WeaponMasterController.Main.globalBurnBuff;

        while (step <= effectDuration)
        {
            healthController.TakeDamage(damagePerTick + globalIncrement);

            step += tickInterval;
            yield return tickTime;
        }
     
        healthController.isBurning = false;
    }

    public float RaiseDamage(float value)
    {
        damagePerTick += value;
        return damagePerTick;
    }

    public float GetDPS()
    {
        var dps = damagePerTick * (effectDuration / tickInterval);
        return dps;
    }
}
