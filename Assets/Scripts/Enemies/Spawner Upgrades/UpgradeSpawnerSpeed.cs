using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Spawner Upgrades/Speed", fileName = "Enemy speed Upgrade")]
public class UpgradeSpawnerSpeed : BaseSpawnerUpgrade
{
    public override void ApplyUpgrade(EnemySpawner spawner)
    {
        spawner.EnhanceSpawnedEnemy += EnhanceSpeed;
        spawner.infoUI.ReceiveIncrement(SpawnerStat.Speed, increment);
    }

    private void EnhanceSpeed(GameObject enemy)
    {
        enemy.GetComponent<EnemyMarch>().RaiseSpeed(increment);
    }
}