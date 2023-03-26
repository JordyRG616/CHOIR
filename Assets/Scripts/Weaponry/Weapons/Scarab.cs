using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarab : WeaponBase
{
    public override void LevelUp()
    {
        level++;

        switch(level)
        {
            case 2:
                damageRange.y += 5;
            break;
            case 3:
                RaiseRange(1.5f);
            break;
            case 4:
                damageRange += Vector2.one * 3;
            break;
            case 5:
                RaiseRange(3);
            break;
        }
    }

    private void RaiseRange(float value)
    {
        var main = MainShooter.main;
        var lifetime = main.startLifetime.constant;
        main.startLifetime = new ParticleSystem.MinMaxCurve(lifetime + value);
    }

    public override string WeaponDescription()
    {
        return "While active, shoots a stream of flame that deals " + damageRange.x + " - " + damageRange.y + " continuous damage and apply <color=red>burn</color> to all the enemies inside.";
    }

    protected override void ApplyPerk()
    {
        var vel = MainShooter.velocityOverLifetime;
        vel.enabled = true;
    }

    protected override void RemovePerk()
    {
        var vel = MainShooter.velocityOverLifetime;
        vel.enabled = false;
    }
}
