using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Spawner Upgrades/Damage", fileName = "Enemy Damage Upgrade")]
public class UpgradeSpawnerDamage : BaseSpawnerUpgrade
{
    public override void ApplyUpgrade(EnemySpawner spawner)
    {
        spawner.EnhanceSpawnedEnemy += EnhanceDamage;
        spawner.infoUI.ReceiveIncrement(SpawnerStat.Damage, increment);
    }

    private void EnhanceDamage(GameObject enemy)
    {
        enemy.GetComponent<EnemyDamageController>().damage += Mathf.RoundToInt(increment);
    }
}
