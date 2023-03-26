using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twingun : WeaponBase
{
    [SerializeField] private ParticleSystem subShooter;
    [SerializeField] private List<GameObject> sparks;
    private string knockback = " moderately";

    public override void LevelUp()
    {
      level++;

        switch(level)
        {
            case 2:
                damageRange += Vector2.one * 2;
            break;
            case 3:
                var main = MainShooter.main;
                main.startSpeed = new ParticleSystem.MinMaxCurve(25, 30);

                var _main = subShooter.main;
                _main.startSpeed = new ParticleSystem.MinMaxCurve(25, 30);
            break;
            case 4:
                IncreaseKnockback(15);
                knockback = " significantly";
            break;
            case 5:
                var emission = MainShooter.emission;
                emission.rateOverTime = new ParticleSystem.MinMaxCurve(9);

                var _emission = subShooter.emission;
                _emission.rateOverTime = new ParticleSystem.MinMaxCurve(9);
            break;
        }
    }

    private void IncreaseKnockback(float amount)
    {
        var coll = MainShooter.collision;
        coll.colliderForce += amount;

        var _coll = subShooter.collision;
        _coll.colliderForce += amount;
    }

    public override string WeaponDescription()
    {
        return "Shoots 6 pellets in both directions, each dealing " + damageRange.x + " - " + damageRange.y + " damage to the enemy hit and knocking it back" + knockback + ".";
    }

    protected override void ApplyPerk()
    {
        sparks.ForEach(x => x.SetActive(true));
    }

    protected override void RemovePerk()
    {
        sparks.ForEach(x => x.SetActive(false));
    }
}
