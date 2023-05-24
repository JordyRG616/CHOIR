using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : WeaponBase
{
    [SerializeField] private ParticleSystem trail;
    [SerializeField] private GameObject multishot;

    public override void LevelUpEffect()
    {
        level++;

        switch(level)
        {
            case 2:
                RaiseLaserDuration(1);
            break;
            case 3:
                damageRange += Vector2.one * 2;
            break;
            case 4:
                RaiseLaserDuration(2);
            break;
            case 5:
                var main = trail.main;
                main.startLifetime = new ParticleSystem.MinMaxCurve(.4f);
            break;
        }
    }

    private void RaiseLaserDuration(float value)
    {
        var main = MainShooter.main;
        var lifetime = main.startLifetime.constant;
        main.startLifetime = new ParticleSystem.MinMaxCurve(lifetime + value);
    }

    public override string WeaponDescription()
    {
        return "Shoots a laser ray that ricochet off surfaces and pierces enemies, dealing " + damageRange.x + " - " + damageRange.y + " damage.";
    }

    protected override void ApplyPerk()
    {
        multishot.SetActive(true);
    }

    protected override void RemovePerk()
    {
        multishot.SetActive(false);
    }
}
