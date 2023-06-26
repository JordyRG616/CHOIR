using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using System;

public class Bomb : MonoBehaviour
{
    [SerializeField] private StudioEventEmitter  beatEvent;
    [SerializeField] private StudioEventEmitter explosionEvent;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private GameObject multishot;
    [SerializeField] private List<WeaponDamageDealer> damageDealers;
    private ActionMarker marker;
    private SpawnerManager spawnerManager;
    private int counter = -2;


    public void Initiate(WeaponBase mortar, bool raiseSize, bool unlockMultishot)
    {
        if(raiseSize)
        {
            var main = explosion.main;
            main.startLifetime = new ParticleSystem.MinMaxCurve(0.6f);
        }

        if(unlockMultishot) multishot.SetActive(true);

        foreach(var dealer in damageDealers)
        {
            dealer.SetWeapon(mortar);
        }

        marker = ActionMarker.Main;
        marker.OnBeat += BeatBomb;

        spawnerManager = SpawnerManager.Main;
        spawnerManager.OnEndOfWave += Explode;
    }

    private void Explode(int waveNumber)
    {
        Explode();
    }

    public void BeatBomb()
    {
        if(counter == 3) return;
        counter++;

        if(counter == 3)
        {
            Explode();
        }
    }

    private void Explode()
    {
        explosionEvent.Play();
        explosion.Play();
        GetComponent<SpriteRenderer>().enabled = false;

        Invoke("DestroyBomb", 1f);
    }

    private void DestroyBomb()
    {
        marker.OnBeat -= BeatBomb;
        spawnerManager.OnEndOfWave -= Explode;
        Destroy(gameObject);
    }
}
