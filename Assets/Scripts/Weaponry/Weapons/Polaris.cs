using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polaris : WeaponBase
{
    [SerializeField] private ParticleSystem SecondShooter;


    public override void LevelUpEffect()
    {
        level++;

        switch(level)
        {
            case 2:
                damageRange.y += 3;
            break;
            case 3:
                RaiseWires(MainShooter, 10);
                RaiseWires(SecondShooter, 10);
            break;
            case 4:
                damageRange += Vector2.one * 3;
            break;
            case 5:
                RaiseWires(MainShooter, 15);
                RaiseWires(SecondShooter, 15);
            break;
        }
    }

    public override string WeaponDescription()
    {
        return "Creates wires of radioactive material around itself. Enemies take " + damageRange.x + " - " + damageRange.y + " continuous damage while touching a wire.";
    }

    protected override void ApplyPerk()
    {
        ChangeCollision(MainShooter);
        ChangeCollision(SecondShooter);
    }

    protected override void RemovePerk()
    {
        ChangeCollision(MainShooter);
        ChangeCollision(SecondShooter);
    }

    private void ChangeCollision(ParticleSystem system)
    {
        var coll = system.collision;
        coll.enabled = !perkApplied;
    }

    private void RaiseWires(ParticleSystem system, int value)
    {
        var emission = system.emission;
        var burst = emission.GetBurst(0);
        
        burst.count = new ParticleSystem.MinMaxCurve(value);

        emission.SetBurst(0, burst);
    }
}
