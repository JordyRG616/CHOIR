using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryGunShooter : MonoBehaviour
{
    [SerializeField] private ParticleSystem upShooter, midShooter, lowShooter;
    [SerializeField] private ParticleSystem upLaser, midLaser, lowLaser;
    
    public void ShootUp()
    {
        upShooter.Play();
        upLaser.Play();
    }

    public void ShootMid()
    {
        midShooter.Play();
        midLaser.Play();
    }

    public void ShootLow()
    {
        lowShooter.Play();
        lowLaser.Play();
    }
}
