using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : WeaponBase
{
    private string knockbackAmount = "slightly";
    private int firerate
    {
        get
        {
            if(level == 5 && perkApplied) return 8;
            if(level == 5 && !perkApplied) return 4;
            return 2;
        }
    }

    public override void LevelUp()
    {
        level++;

        switch(level)
        {
            case 2:
                damageRange += Vector2.one * 2;
            break;
            case 3:
                damageRange.y += 3;
            break;
            case 4:
                IncreaseKnockback(10);
                knockbackAmount = "moderately";
            break;
        }
    }

    private void IncreaseKnockback(float amount)
    {
        var coll = MainShooter.collision;
        coll.colliderForce += amount;
    }

    private void SetFirerate()
    {
        var emission = MainShooter.emission;
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(firerate);
    }

    public override void Shoot(WeaponKey key)
    {
        SetFirerate();
        base.Shoot(key);
    }

    public override string WeaponDescription()
    {
        return "While active, shoots " + firerate + " bullets per second that deals " + damageRange.x + " - " + damageRange.y + " damage each, knocking back the target " + knockbackAmount + ".";
    }

    protected override void ApplyPerk()
    {

    }

    protected override void RemovePerk()
    {
        
    }
}
