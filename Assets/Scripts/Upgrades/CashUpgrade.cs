using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Set Upgrade/Cash Upgrade", fileName = "New Cash Upgrade")]
public class CashUpgrade : UpgradeBase
{
    [SerializeField] private int cash;

    public override void Apply()
    {
        SpawnerManager.Main.OnEndOfWave += GiveCash;
    }

    private void GiveCash(int waveNumber)
    {
        CrystalManager.Main.ExpendBuildPoints(-cash);
    }

    public override void Remove()
    {
        SpawnerManager.Main.OnEndOfWave -= GiveCash;
    }
}
