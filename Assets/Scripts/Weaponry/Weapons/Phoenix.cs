using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phoenix : WeaponBase
{
    [SerializeField] private ActionTile altTile;
    [SerializeField] private GameObject trail;

    public override void LevelUp()
    {
        level++;

        switch(level)
        {
            case 2:
                criticalChance += .1f;
            break;
            case 3:
                damageRange += Vector2.one * 4;
            break;
            case 4:
                criticalChance += .3f;
            break;
            case 5:
                tile = altTile;
            break;
        }
    }

    public override void Shoot(WeaponKey key)
    {
        base.Shoot(key);
    }

    public override string WeaponDescription()
    {
        return "Shoots a burning arrow that deals " + damageRange.x + " - " + damageRange.y + " damage and apply <color=red>burn</color> to the first enemy hit.";
    }

    protected override void ApplyPerk()
    {
        trail.SetActive(true);
    }

    protected override void RemovePerk()
    {
        trail.SetActive(false);
    }
}
