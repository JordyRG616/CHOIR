using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helix : WeaponBase
{
    [SerializeField] private ParticleSystem zone;
    [SerializeField] private GameObject waste;

    public override void LevelUp()
    {
        level++;

        switch(level)
        {
            case 2:
                damageRange.x += 3;
            break;
            case 3:
                damageRange.y += 4;
            break;
            case 4:
                var main = MainShooter.main;
                main.startSpeed = new ParticleSystem.MinMaxCurve(1.8f);
            break;
            case 5:
                var zoneMain = zone.main;
                zoneMain.startSize = new ParticleSystem.MinMaxCurve(6f);
            break;
        }
    }

    public override string WeaponDescription()
    {
        return "Shoots two bullets that moves in a helical pattern and explode in a radioactive zone, dealing " + damageRange.x + " - " + damageRange.y + " damage to enemies caught in the explosion.";
    }

    protected override void ApplyPerk()
    {
        waste.SetActive(true);
    }

    protected override void RemovePerk()
    {
        waste.SetActive(false);
    }
}
