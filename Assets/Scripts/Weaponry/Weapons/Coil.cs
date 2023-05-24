using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coil : WeaponBase
{
    [SerializeField] private GameObject zone;


    public override void LevelUpEffect()
    {
        level++;

        switch(level)
        {
            case 2:
                damageRange.y += 1;
            break;
            case 3:
                var main = MainShooter.main;
                main.startSpeed = new ParticleSystem.MinMaxCurve(5, 35);
            break;
            case 4:
                damageRange += Vector2.one;
            break;
            case 5:
                var emission = MainShooter.emission;
                var burst = emission.GetBurst(0);
                burst.count  = new ParticleSystem.MinMaxCurve(25);
                emission.SetBurst(0, burst);
            break;
        }
    }

    public override string WeaponDescription()
    {
        return "While active, releases periodic bursts of electricity around itself, dealing " + damageRange.x + " - " + damageRange.y + " damage and applying <color=yellow>static</color> to enemies it hits.";
    }

    protected override void ApplyPerk()
    {
        zone.SetActive(true);
    }

    protected override void RemovePerk()
    {
        zone.SetActive(false);
    }
}
