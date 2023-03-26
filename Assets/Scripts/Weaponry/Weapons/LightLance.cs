using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightLance : WeaponBase
{
    [SerializeField] private Material fireMat;
    [SerializeField] private Material laserMat;
    [SerializeField] private ParticleSystemRenderer lance;
    [SerializeField] private ParticleSystemRenderer coating;
    [SerializeField] private WeaponDamageDealer damageDealer;
    private int firerate = 1;
    private string lances  = " lance";

    public override void LevelUp()
    {
        level++;

        switch(level)
        {
            case 2:
                RaiseLaserDuration(.5f);
            break;
            case 3:
                damageRange += Vector2.one * 3;
            break;
            case 4:
                firerate++;
                lances = " lances";
                var emission = MainShooter.emission;
                emission.rateOverTime = new ParticleSystem.MinMaxCurve(firerate);
            break;
            case 5:
                RaiseLaserDuration(1);
            break;
        }
    }

    private void RaiseLaserDuration(float value)
    {
        var main = MainShooter.main;
        var lifetime = main.startLifetime.constant;
        main.startLifetime = new ParticleSystem.MinMaxCurve(lifetime + value);
    }

    public override string WeaponDescription()
    {
        return "While active, shoots " + firerate + lances + " per second that pierces enemies, dealing " + damageRange.x + " - " + damageRange.y + " damage to enemies.";
    }

    protected override void ApplyPerk()
    {
        lance.trailMaterial = fireMat;
        coating.trailMaterial = fireMat;

        damageDealer.statuses.Add(StatusType.Burn);
    }

    protected override void RemovePerk()
    {
        lance.trailMaterial = laserMat;
        coating.trailMaterial = laserMat;

        damageDealer.statuses.Remove(StatusType.Burn);
    }
}
