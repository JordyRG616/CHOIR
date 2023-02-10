using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : ElectricBase
{
    [SerializeField] private ParticleSystem trail;
    private float appliedValue;

    protected override void Awake()
    {
        base.Awake();

        upgrades.Add(new WeaponUpgrade(UpgradeTag.Multishot,
            () =>
            {
                var main = MainShooter.main;
                var emission = MainShooter.emission;
                var burst = emission.GetBurst(0);
                burst.cycleCount += 3;
                emission.SetBurst(0, burst);

                var shape = trail.shape;
                shape.randomPositionAmount = 0.75f;
            }
            ));

        ActionMarker.Main.OnReset += RemoveAppliedValues;
    }

    private void RemoveAppliedValues()
    {
        WeaponMasterController.Main.currentPotency -= appliedValue;
        appliedValue = 0;
    }

    public override void Shoot(WeaponKey key)
    {
        base.Shoot(key);

        WeaponMasterController.Main.currentPotency += 0.05f;
        appliedValue += 0.05f;
    }

    
}
