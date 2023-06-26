using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : WeaponBase
{
    [SerializeField] private ParticleSystem subShooter;
    [SerializeField] private List<GameObject> flares;
    [SerializeField] private List<WeaponDamageDealer> damageDealers;

    public override void LevelUpEffect()
    {
        level++;

        switch(level)
        {
            case 2:
                damageRange.x += 3;
            break;
            case 3:
                criticalChance += .15f;
            break;
            case 4:
                damageRange.y += 5;
            break;
            case 5:
                var main = MainShooter.main;
                main.startSpeed = new ParticleSystem.MinMaxCurve(15, 25);

                var sub = subShooter.main;
                sub.startSpeed = new ParticleSystem.MinMaxCurve(15, 25);
            break;
        }
    }

    public override string WeaponDescription()
    {
        return "While active, shoots a burst of short electric rays that deals " + damageRange.x + " - " + damageRange.y + " damage on contact and applies <color=yellow>static</color>.";
    }
}
