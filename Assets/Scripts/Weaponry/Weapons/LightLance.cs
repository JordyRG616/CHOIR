using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class LightLance : WeaponBase
{
    [SerializeField] private StudioEventEmitter emitter;
    private int emitAmount = 1;
    private float firerate = 1f;
    private string textComplement  = "second";

    public override void LevelUpEffect()
    {
        level++;

        switch(level)
        {
            case 2:
                RaiseLaserDuration(.5f);
            break;
            case 3:
                damageRange += Vector2.one * 3;
            break;
            case 4:
                firerate /= 2;
                textComplement = "beat";
            break;
            case 5:
                RaiseLaserDuration(1);
            break;
        }
    }

    public override void Shoot(WeaponKey key)
    {
        base.Shoot(key);
        StopAllCoroutines();
        shooting = true;
        StartCoroutine(ManageShooting());
    }

    private IEnumerator ManageShooting()
    {
        while(shooting)
        {
            MainShooter.Emit(emitAmount);
            emitter.Play();

            yield return new WaitForSeconds(firerate);
        }
    }

    public override void Stop()
    {
        shooting = false;
        base.Stop();
    }

    private void RaiseLaserDuration(float value)
    {
        var main = MainShooter.main;
        var lifetime = main.startLifetime.constant;
        main.startLifetime = new ParticleSystem.MinMaxCurve(lifetime + value);
    }

    public override string WeaponDescription()
    {
        return "While active, shoots a lance each " + textComplement + " that pierces enemies, dealing " + damageRange.x + " - " + damageRange.y + " damage to enemies.";
    }

    protected override void ApplyPerk()
    {
        emitAmount += 2;

        var main = MainShooter.main;
        main.startSize = new ParticleSystem.MinMaxCurve(.12f);

        var shape = MainShooter.shape;
        shape.randomDirectionAmount = .4f;
    }

    protected override void RemovePerk()
    {
        emitAmount -= 2;

        var main = MainShooter.main;
        main.startSize = new ParticleSystem.MinMaxCurve(.33f);

        var shape = MainShooter.shape;
        shape.randomDirectionAmount = 0;
        
    }
}
