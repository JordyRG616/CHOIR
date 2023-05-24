using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Railgun : WeaponBase
{
    public override void LevelUpEffect()
    {
        level++;

        switch(level)
        {
            case 2:
                damageRange.x += 5;
            break;
            case 3:
                criticalChance += .2f;
            break;
            case 4:
                damageRange.y += 10;
            break;
            case 5:
                criticalChance += .3f;
            break;
        }
    }

    public override string WeaponDescription()
    {
        return "Shoots a high speed projectile that ricochet off surfaces, dealing " + damageRange.x + " - " + damageRange.y + " damage to the enemy hit.";
    }

    protected override void ApplyPerk()
    {
        var trigger = MainShooter.trigger;
        trigger.enabled = true;

        var coll = MainShooter.collision;
        coll.collidesWith = LayerMask.GetMask("Ground");
    }

    protected override void RemovePerk()
    {
        var trigger = MainShooter.trigger;
        trigger.enabled = false;
        
        var coll = MainShooter.collision;
        coll.collidesWith = LayerMask.GetMask("Ground", "Enemies");
    }
}
