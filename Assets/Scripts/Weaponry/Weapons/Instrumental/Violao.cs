using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Violao : WeaponBase
{
    private string knockbackAmount = "slightly";

    public override void LevelUpEffect()
    {
        
    }

    public override string WeaponDescription()
    {
        return "Shoots a projectile straight forward that causes " + damageRange.x + " - " + damageRange.y + " damage to the enemy hit and knocks it back " + knockbackAmount + ".";
    }

    protected override void ApplyPerk()
    {
        throw new System.NotImplementedException();
    }

    protected override void RemovePerk()
    {
        throw new System.NotImplementedException();
    }
}
