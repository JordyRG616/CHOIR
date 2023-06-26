using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanFlute : WeaponBase
{
    [SerializeField] private List<ParticleSystem> shooters;
    [SerializeField] private FMODUnity.StudioEventEmitter eventEmitter;
    private int counter;
    private int initialKey;

    public override void LevelUpEffect()
    {
        level++;

        switch(level)
        {
            case 2:
                damageRange.y +=  2;
            break;
            case 3:
                RaiseLaserDuration(.5f);
            break;
            case 4:
                damageRange += Vector2.one * 2;
            break;
            case 5:
                RaiseLaserDuration(1.5f);
            break;
        }
    }

    private void RaiseLaserDuration(float value)
    {
        foreach(var shooter in shooters)
        {
            var main = shooter.main;
            var lifetime = main.startLifetime.constant;
            main.startLifetime = new ParticleSystem.MinMaxCurve(lifetime + value);
        }
    }

    public override void Shoot(WeaponKey key)
    {
        base.Shoot(key);
        counter = 0;
        initialKey = (int)key;
    }
    public void A_Shoot()
    {
        shooters[counter].Play();
        eventEmitter.Play();
        initialKey++;
        audioController.ChangeKey((WeaponKey)initialKey);
        counter++;
    }

    public override string WeaponDescription()
    {
        return "Shoots 5 piercing energy rays in succession that bounces off walls, dealing " + damageRange.x + " - " + damageRange.y + " damage to enemies.";
    }
}
