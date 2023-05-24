using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Web : WeaponBase
{
    [SerializeField] private GameObject extraWires;

    public override void LevelUpEffect()
    {
        level++;

        switch(level)
        {
            case 2:
                damageRange.y += 3;
            break;
            case 3:
                damageRange += Vector2.one * 3;
            break;
            case 4:
                var main = MainShooter.main;
                main.startSpeed = new ParticleSystem.MinMaxCurve(15);
            break;
            case 5:
                var coll = MainShooter.collision;
                coll.lifetimeLoss = new ParticleSystem.MinMaxCurve(.33f);
            break;
        }
    }

    public override string WeaponDescription()
    {
        return "Shoots a web of laser wires that spreads around the weapon, dealing " + damageRange.x + " - " + damageRange.y + " damage and applying <color=purple>frailty</color>. to enemies inside.";
    }

    protected override void ApplyPerk()
    {
        extraWires.SetActive(true);
    }

    protected override void RemovePerk()
    {
        extraWires.SetActive(false);
    }
}
