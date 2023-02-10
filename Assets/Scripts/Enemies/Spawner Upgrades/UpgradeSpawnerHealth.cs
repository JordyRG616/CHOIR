using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Spawner Upgrades/Health", fileName = "Enemy Health Upgrade")]
public class UpgradeSpawnerHealth : BaseSpawnerUpgrade
{
    public override void ApplyUpgrade(EnemySpawner spawner)
    {
        //spawner.EnhanceSpawnedEnemy += EnhanceHealth;
        spawner.infoUI.ReceiveIncrement(SpawnerStat.Health, increment);
    }

    private void EnhanceHealth(GameObject enemy)
    {
        enemy.GetComponent<EnemyHealthModule>().RaiseMaxHealth(increment);
        EnlargeEnemy(enemy);
    }

    private void EnlargeEnemy(GameObject enemy)
    {
        enemy.transform.localScale += new Vector3(0.02f, 0.02f);
        if (enemy.TryGetComponent<SpriteRenderer>(out var sprite))
        {
            sprite.sortingOrder -= 1;
        }
    }
}
