using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : WeaponBase
{
    [SerializeField] private GameObject zone;
    [SerializeField] private ParticleSystem smoke;
    [SerializeField] private WeaponDamageDealer damageDealer;

    public override void LevelUp()
    {
        level++;

        switch(level)
        {
            case 2:
                damageRange.x += 3;
            break;
            case 3:
                damageRange.x += 4;
            break;
            case 4:
                damageDealer.statuses.Add(StatusType.Burn);
            break;
            case 5:
                var main = MainShooter.main;
                main.startSpeed = new ParticleSystem.MinMaxCurve(2);

                var _main = smoke.main;
                _main.startSpeed = new ParticleSystem.MinMaxCurve(2);
            break;
        }
    }

    public override string WeaponDescription()
    {
        return "Deploys an orb of fire that explodes at the end of activation, dealing " + damageRange.x + " - " + damageRange.y + " damage and applying <color=red>burn</color> to enemies in range.";
    }

    protected override void ApplyPerk()
    {
        zone.SetActive(true);
    }

    protected override void RemovePerk()
    {
        zone.SetActive(false);
    }
}
