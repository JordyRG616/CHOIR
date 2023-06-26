using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pandeiro : WeaponBase
{
    public override void LevelUpEffect()
    {
        level++;

        switch(level)
        {
            case 2:
                RaiseLifetime(.5f);
            break;
            case 3:
                damageRange.x += 3;                
            break;
            case 4:
                damageRange.y += 5;
            break;
            case 5:
                RaiseLifetime(1);
            break;
        }
    }

    private void RaiseLifetime(float extraLifetime)
    {
        var main = MainShooter.main;
        main.startLifetime = new ParticleSystem.MinMaxCurve(main.startLifetime.constant + extraLifetime);
    }

    public override string WeaponDescription()
    {
        return "Lauches 5 energy beans that orbit the weapon, dealing " + damageRange.x + " - " + damageRange.y + " damage to enemies that passes through them.";
    }

    protected override void ApplyPerk()
    {
    }

    protected override void RemovePerk()
    {
    }
}
