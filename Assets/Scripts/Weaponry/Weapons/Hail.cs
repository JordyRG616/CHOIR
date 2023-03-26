using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hail : WeaponBase
{
    [SerializeField] private ParticleSystem secondShooter;
    [SerializeField] private GameObject salvo;
    private int missiles = 6;

    public override void LevelUp()
    {
        level++;

        switch(level)
        {
            case 2:
                RaiseAmmo(MainShooter, 1);
                RaiseAmmo(secondShooter, 1);
                missiles += 2;
            break;
            case 3:
                criticalChance += 0.15f;
            break;
            case 4:
                damageRange += Vector2.one * 4;
            break;
            case 5:
                RaiseAmmo(MainShooter, 3);
                RaiseAmmo(secondShooter, 3);
                missiles += 6;
            break;
        }
    }

    public override string WeaponDescription()
    {
        return "Shoots a volley of " + missiles + " small missiles that travels in a random direction and explodes in a small radioactive zone, causing " + damageRange.x + " - " + damageRange.y + " damage to enemies.";
    }

    protected override void ApplyPerk()
    {
        salvo.SetActive(true);
    }

    protected override void RemovePerk()
    {
        salvo.SetActive(false);
    }

    private void RaiseAmmo(ParticleSystem system, int value)
    {
        var emission = system.emission;
        var burst = emission.GetBurst(0);
        burst.cycleCount += value;
        emission.SetBurst(0, burst);
    }
}
