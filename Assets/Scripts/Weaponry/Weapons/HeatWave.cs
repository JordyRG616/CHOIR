using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatWave : WeaponBase
{
    [SerializeField] private WeaponDamageDealer damageDealer;
    private string burnStr = "";

    public override void LevelUp()
    {
        level++;

        var main = MainShooter.main;
        switch(level)
        {
            case 2:
                damageDealer.statuses.Add(StatusType.Burn);
                burnStr = " major";
            break;
            case 3:
                main.startSize = new ParticleSystem.MinMaxCurve(17);
            break;
            case 4:
                damageDealer.statuses.Add(StatusType.Burn);
                burnStr = " severe";
            break;
            case 5:
                main.startSize = new ParticleSystem.MinMaxCurve(17);
            break;
        }
    }

    public override string WeaponDescription()
    {
        return "While active, creates a heat zone that applies" + burnStr + " <color=red>burn</color> to enemies inside.";
    }

    protected override void ApplyPerk()
    {
        damageRange = new Vector2(2, 4);
    }

    protected override void RemovePerk()
    {
        damageRange = new Vector2(0, 0);
    }
}
