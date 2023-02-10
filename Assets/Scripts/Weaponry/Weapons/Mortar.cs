using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mortar : FlameBase
{
    [SerializeField] GameObject explosion;

    protected override void Awake()
    {
        base.Awake();

        upgrades.Add(new WeaponUpgrade(UpgradeTag.Multishot,
            () =>
            {
                var main = MainShooter.main;
                main.startSize = new ParticleSystem.MinMaxCurve(main.startSize.constant * .6f);

                var emission = MainShooter.emission;
                var burst = emission.GetBurst(0);
                burst.cycleCount += 2;
                emission.SetBurst(0, burst);
            }
            )) ;

        var upg = upgrades.Find(x => x.tag == UpgradeTag.Explosive);

        upg.onUpgradedApplied += () =>
        {
            var main = MainShooter.main;
            main.startLifetime = new ParticleSystem.MinMaxCurve(.5f, .85f);
        };
    }

    protected override void ApplyHeatEffect()
    {
        explosion.SetActive(true);
    }

    protected override void RemoveHeatEffect()
    {
        explosion.SetActive(false);
    }
}
