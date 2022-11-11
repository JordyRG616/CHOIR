using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Spawner Upgrades/Frequency", fileName ="Frequency Upgrade")]
public class UpgradeSpawnerFrequency : BaseSpawnerUpgrade
{
    public override void ApplyUpgrade(EnemySpawner spawner)
    {
        spawner.LowerFrequency(increment);
        spawner.infoUI.ReceiveIncrement(SpawnerStat.Frequency, -increment);
    }
}
