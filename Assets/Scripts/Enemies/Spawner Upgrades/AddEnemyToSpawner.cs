using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Spawner Upgrades/Add Enemy", fileName = "New Add Enemy Upgrade")]
public class AddEnemyToSpawner : BaseSpawnerUpgrade
{
    public GameObject enemy;

    public override void ApplyUpgrade(EnemySpawner spawner)
    {
        spawner.ReceiveEnemy(enemy);
    }
}
