using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt : WeaponBase
{
    public override void LevelUpEffect()
    {
        level++;

        switch(level)
        {
            case 2:
                damageRange.y += 3;
            break;
            case 3:
                damageRange += Vector2.one * 3;
            break;
            case 4:
                criticalChance += .3f;
            break;
            case 5:
                var main = MainShooter.main;
                main.startLifetime = new ParticleSystem.MinMaxCurve(.35f);
            break;
        }
    }

    public override string WeaponDescription()
    {
        return "Discharges an electric bolt that hits one enemy, dealing " + damageRange.x + " - " + damageRange.y + " damage and applying <color=yellow>static</color>.";
    }
}
