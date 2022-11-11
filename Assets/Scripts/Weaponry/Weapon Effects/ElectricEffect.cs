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
        var enemyMarch = enemy.GetComponent<EnemyMarch>();
        if (enemyMarch.frozen) return;

        var globalIncrement = WeaponMasterController.Main.globalElectricBuff;
        effectTime = new WaitForSeconds(effectDuration + globalIncrement);

        StartCoroutine(FreezeEnemy(enemyMarch));
    }

    private IEnumerator FreezeEnemy(EnemyMarch enemyMarch)
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
