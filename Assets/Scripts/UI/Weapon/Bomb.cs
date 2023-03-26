using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;


public class Bomb : MonoBehaviour
{
    [SerializeField] private StudioEventEmitter  beatEvent;
    [SerializeField] private StudioEventEmitter explosionEvent;
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private GameObject multishot;
    [SerializeField] private List<WeaponDamageDealer> damageDealers;
    private ActionMarker marker;
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
    }


    public void BeatBomb()
    {
        if(counter == 3) return;
        counter++;

        if(counter == 3)
        {
            explosionEvent.Play();
            explosion.Play();
            GetComponent<SpriteRenderer>().enabled = false;

            Invoke("DestroyBomb", 1f);
        } else if(counter > 0)
        {
            beatEvent.Play();
        }
    }

    private void DestroyBomb()
    {
        Destroy(gameObject);
    }
}
