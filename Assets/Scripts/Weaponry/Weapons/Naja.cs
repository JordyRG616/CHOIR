using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Naja : WeaponBase
{
    [SerializeField] private ParticleSystem secondShooter;
    [SerializeField] private GameObject gas;

    public override void LevelUp()
    {
        level++;

        switch(level)
        {
            case 2:
                damageRange += Vector2.one * 2;
            break;
            case 3:
                RaiseSpeed(MainShooter, .5f);
                RaiseSpeed(secondShooter, .5f);
            break;
            case 4:
                damageRange += Vector2.one * 3;
            break;
            case 5:
                RaiseSpeed(MainShooter, 1f);
                RaiseSpeed(secondShooter, 1f);
            break;
        }
    }

    public override string WeaponDescription()
    {
        return "Shoots a stream of toxic material in both directions. Enemies take " + damageRange.x + " - " + damageRange.y + " damage while touching the material.";
    }

    protected override void ApplyPerk()
    {
        gas.SetActive(true);
    }

    protected override void RemovePerk()
    {
        gas.SetActive(false);
    }

    private void RaiseSpeed(ParticleSystem system, float value)
    {
        var main = system.main;
        main.simulationSpeed += value;
    }
}
