using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carbine : WeaponBase
{
    [SerializeField] private GameObject explosion;
    private string knockbackAmount = "slightly";

    public override void LevelUp()
    {
        level++;

        switch(level)
        {
            case 1:
                damageRange.y += 5;
            break;
            case 2:
                IncreaseKnockback(10);
                knockbackAmount = "moderately";
            break;
            case 3:
                UnlockAltTile();
            break;
            case 4:
                IncreaseKnockback(15);
                knockbackAmount = "Significantly";
            break;
            case 5:
                damageRange += Vector2.one * 5; 
            break;
        }
    }

    private void IncreaseKnockback(float amount)
    {
        var coll = MainShooter.collision;
        coll.colliderForce += amount;
    }

    public override string WeaponDescription()
    {
        return "Shoots a projectile straight forward that causes " + damageRange.x + " - " + damageRange.y + " damage to the enemy hit and knocks it back " + knockbackAmount +".";
    }

    protected override void ApplyPerk()
    {
        explosion.SetActive(true);
    }

    protected override void RemovePerk()
    {
        explosion.SetActive(false);
    }
}
