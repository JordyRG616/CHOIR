using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricEffect : WeaponEffect
{
    [SerializeField] private float effectDuration;

    private WaitForSeconds effectTime;

    protected override void Awake()
    {
        base.Awake();

    }

    protected override void Effect(GameObject enemy)
    {
        var enemyMarch = enemy.GetComponent<EnemyMarchModule>();
        if (enemyMarch.frozen) return;

        var globalIncrement = WeaponMasterController.Main.shockDuration;
        effectTime = new WaitForSeconds(effectDuration + globalIncrement);

        StartCoroutine(FreezeEnemy(enemyMarch));
    }

    private IEnumerator FreezeEnemy(EnemyMarchModule enemyMarch)
    {
        enemyMarch.SetFrozen(true);

        yield return effectTime;

        enemyMarch.SetFrozen(false);
    }

    public float RaiseDuration(float value)
    {
        effectDuration += value;
        return effectDuration;
    }
}
