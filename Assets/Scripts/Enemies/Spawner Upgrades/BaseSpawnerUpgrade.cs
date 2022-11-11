using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSpawnerUpgrade : ScriptableObject
{
    public enum UpgradeType { Common, AddEnemy}

    public UpgradeType type;
    public float increment;
    public SpawnerStat stat;

    public abstract void ApplyUpgrade(EnemySpawner spawner);
}
