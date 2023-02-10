using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatAnimation : MonoBehaviour
{
    private Animator anim;
    private ActionMarker marker;

    void Start()
    {
        anim = GetComponent<Animator>();
        marker = ActionMarker.Main;
        marker.OnBeat += DoBeatAnimation;
    }

    private void DoBeatAnimation()
    {
        anim.SetTrigger("Beat");
    }

    void OnDisable()
    {
        marker.OnBeat -= DoBeatAnimation;
    }
}
