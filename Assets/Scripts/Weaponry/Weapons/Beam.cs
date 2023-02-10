using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : LaserBase
{
    [SerializeField] private float baseSize;
    [SerializeField] private float maxSize;


    protected override void Awake()
    {
        base.Awake();

        upgrades.Add(new WeaponUpgrade(UpgradeTag.Multishot,
            () =>
            {
                var velocity = MainShooter.velocityOverLifetime;
                velocity.enabled = true;
            }
            ));
    }

    public override void ApplyPassiveEffect()
    {
        weaponMasterController.currentPotency += 0.15f;
    }

    protected override void ApplyPotency()
    {
        var main = MainShooter.main;
        var value = Mathf.Lerp(baseSize, maxSize, weaponMasterController.potencyPercentage);
        main.startSize = new ParticleSystem.MinMaxCurve(value);
    }

    protected override void ApplyOverloadEffect()
    {
        var main = MainShooter.main;
        var value = baseSize / 2;
        main.startSize = new ParticleSystem.MinMaxCurve(value);
    }
}
