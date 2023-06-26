using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleTambor : WeaponBase
{
    [SerializeField] private List<ParticleSystem> shooters;
    [SerializeField] private FMODUnity.StudioEventEmitter eventEmitter;
    private int initialKey;
    private ActionMarker actionMarker;


    private void Start() 
    {
        actionMarker = ActionMarker.Main;
        actionMarker.OnReset += Detonate;
    }

    private void Detonate()
    {
        foreach(var shooter in shooters)
        {
            var particles = new ParticleSystem.Particle[shooter.particleCount];
            shooter.GetParticles(particles);

            for(int i = 0; i < particles.Length; i++)
            {
                Debug.Log(i);
                particles[i].remainingLifetime = 0.001f;
            }

            shooter.SetParticles(particles);
        }
    }

    public override void LevelUpEffect()
    {
        level++;

        switch(level)
        {
            case 2:
                RaiseAccuracy(1.5f);
            break;
            case 3:
                RaiseSpeed();
            break;
            case 4:
                damageRange.x += 3;
            break;
            case 5:
                RaiseAccuracy(2.5f);
            break;
        }
    }

    private void RaiseAccuracy(float amount)
    {
        foreach(var shooter in shooters)
        {
            var vel = shooter.velocityOverLifetime;
            vel.yMultiplier -= amount;

            var rot = shooter.rotationOverLifetime;
            rot.zMultiplier /= amount;
        }
    }

    private void RaiseSpeed()
    {
        foreach(var shooter in shooters)
        {
            var main = shooter.main;
            main.startSpeed = new ParticleSystem.MinMaxCurve(-15, -20);
        }
    }

    public override void Shoot(WeaponKey key)
    {
        base.Shoot(key);
        initialKey = (int)key;
        eventEmitter.Play();
        shooters[0].Play();
        Invoke("RegisterShot", .1f);
    }

    private void RegisterShot()
    {
        actionMarker.OnBeat += ShootAgain;
    }

    private void ShootAgain()
    {
        anim.SetTrigger("Shoot");
        initialKey += 3;
        audioController.ChangeKey((WeaponKey)initialKey);
        eventEmitter.Play();
        shooters[1].Play();
        actionMarker.OnBeat -= ShootAgain;
    }

    public override string WeaponDescription()
    {
        return "Shoots two rockets that explodes in contact, dealing " + damageRange.x + " - " + damageRange.y + " damage and applying <color=red>burn</color> to enemies in range.";
    }
}
