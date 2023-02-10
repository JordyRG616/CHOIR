using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : BallisticBase
{
    [SerializeField] private int initialBulletCount;

    protected override void Awake()
    {
        base.Awake();

        upgrades.Add(new WeaponUpgrade(UpgradeTag.Multishot, () => initialBulletCount += 3));
    }

    protected override void ApplyChargeEffect()
    {
        int ammo = 0;

        for (int i = 0; i < 4; i++)
        {
            if (WeaponMasterController.Main.ammo == 0) break;

            WeaponMasterController.Main.ammo--;
            ammo++;
        }

        var emission = MainShooter.emission;
        var burst = emission.GetBurst(0);
        burst.cycleCount = initialBulletCount + ammo;
        emission.SetBurst(0, burst);
    }

    protected override void RemoveChargeEffect()
    {
        var emission = MainShooter.emission;
        var burst = emission.GetBurst(0);
        burst.cycleCount = initialBulletCount;
        emission.SetBurst(0, burst);
    }
}
