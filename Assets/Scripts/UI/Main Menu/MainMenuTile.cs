using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class MainMenuTile : ActionTile
{
    private StudioEventEmitter eventEmitter;

    private void Start()
    {
        eventEmitter = GetComponent<StudioEventEmitter>();
        ActivateTile();
    }


    public override void Activate()
    {
        eventEmitter.Play();
    }

    public override void ExitTile()
    {
        eventEmitter.Stop();
    }
}
