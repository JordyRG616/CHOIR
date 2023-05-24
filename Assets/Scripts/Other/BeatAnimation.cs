using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatAnimation : MonoBehaviour
{
    private Animator anim;
    private ActionMarker marker;
    
    public bool OverrideBeat;


    void OnEnable()
    {
        anim = GetComponent<Animator>();
        marker = ActionMarker.Main;
        marker.OnBeat += DoBeatAnimation;
    }

    private void DoBeatAnimation()
    {
        if(OverrideBeat) return;
        anim.SetTrigger("Beat");
    }

    void OnDisable()
    {
        marker.OnBeat -= DoBeatAnimation;
    }
}
