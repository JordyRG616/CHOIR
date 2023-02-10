using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarab : FlameBase
{
    [SerializeField] GameObject extraThrower;

    protected override void Awake()
    {
        base.Awake();

        upgrades.Add(new WeaponUpgrade(UpgradeTag.Multishot,
            () =>
            {
                var shape = MainShooter.shape;
                shape.randomDirectionAmount = 0.12f;

                extraThrower.SetActive(true);
            }
            ));
    }

    public override void Shoot(WeaponKey key)
    {
        WeaponMasterController.Main.heatLevel += 1;

        base.Shoot(key);
    }

    public override void Stop()
    {
        WeaponMasterController.Main.heatLevel -= 1;

        base.Stop();
    }
}
