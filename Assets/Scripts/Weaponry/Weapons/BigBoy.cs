using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBoy : WeaponBase
{
    [SerializeField] private ParticleSystem secondShooter;

    public override void LevelUp()
    {
        level++;

        switch(level)
        {
            case 2:
                damageRange += Vector2.one * 2;
            break;
            case 3:
                criticalChance += 0.1f;
            break;
            case 4:
                RaiseSpeed(MainShooter);
                RaiseSpeed(secondShooter);
            break;
            case 5:
                criticalChance += .25f;
            break;
        }
    }

    public override string WeaponDescription()
    {
        return "Launches two nukes that explode in a radioactive zone, dealing " + damageRange.x + " - " + damageRange.y + " damage to enemies caught in the explosion.";
    }

    protected override void ApplyPerk()
    {
        ChangeShooter(MainShooter);
        ChangeShooter(secondShooter);
    }

    protected override void RemovePerk()
    {
        ChangeShooter(MainShooter);
        ChangeShooter(secondShooter);
    }

    private void ChangeShooter(ParticleSystem system)
    {
        var main = system.main;
        var emission = system.emission;
        var burst = emission.GetBurst(0);
        
        main.startSize = perkApplied ? (.84f / 1.5f) : .84f;
        var count = perkApplied ? 2 : 1;
        burst.count = new ParticleSystem.MinMaxCurve(count);

        emission.SetBurst(0, burst);
    }

    private void RaiseSpeed(ParticleSystem system)
    {
        var main = system.main;
        main.startSpeed = new ParticleSystem.MinMaxCurve(-20, -25);
    }
}
