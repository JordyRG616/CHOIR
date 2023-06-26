using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Banjo : WeaponBase
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
                criticalChance += .1f;
            break;
            case 4:
                damageRange += Vector2.one * 2;
            break;
            case 5:
                criticalChance += .15f;
            break;
        }
    }

    public override string WeaponDescription()
    {
        return "Shoots an energy beam that pierces enemies and walls, dealing " + damageRange.x + " - " + damageRange.y + " damage to enemies in range.";
    }

    protected override void ApplyPerk()
    {
    }

    protected override void RemovePerk()
    {
    }
}
