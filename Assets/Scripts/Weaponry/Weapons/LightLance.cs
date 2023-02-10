using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightLance : LaserBase
{
    [SerializeField] private float baseFirerate;
    [SerializeField] private float maxFirerateIncrement;

    protected override void ApplyPotency()
    {
        var emission = MainShooter.emission;
        var rate = baseFirerate + (maxFirerateIncrement * weaponMasterController.potencyPercentage);
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(rate);
    }

    protected override void ApplyOverloadEffect()
    {
        var emission = MainShooter.emission;
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(0);
    }

    protected override void RemoveEffects()
    {
        var emission = MainShooter.emission;
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(baseFirerate);
    }
}
