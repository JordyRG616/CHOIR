using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : ElectricBase
{
    [SerializeField] private List<ParticleSystem> shooters;

    protected override void Awake()
    {
        base.Awake();

        upgrades.Add(new WeaponUpgrade(UpgradeTag.Multishot, () =>
        {
            foreach (var shooter in shooters)
            {
                var emission = shooter.emission;
                var burst = emission.GetBurst(0);
                burst.cycleCount += 1;
                emission.SetBurst(0, burst);
            }
        }
        ));
    }
}
