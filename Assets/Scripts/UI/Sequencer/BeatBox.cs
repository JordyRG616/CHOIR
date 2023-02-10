using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class BeatBox : MonoBehaviour
{
    private StudioEventEmitter emitter;

    
    void Start()
    {
        emitter = GetComponent<StudioEventEmitter>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent<ActionMarker>(out var marker))
        {
            emitter.Play();
            marker.DoBeat();
        }
    }
}
