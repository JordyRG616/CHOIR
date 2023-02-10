using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatWave : FlameBase
{
    [SerializeField] private GameObject trail;

    public override void ApplyPassiveEffect()
    {
        WeaponMasterController.Main.heatLevel += 1;
    }

    protected override void ApplyHeatEffect()
    {
        trail.SetActive(true);
    }

    protected override void RemoveHeatEffect()
    {
        trail.SetActive(false);
    }
}
