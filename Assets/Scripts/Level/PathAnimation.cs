using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathAnimation : MonoBehaviour
{
    [SerializeField] private float step;
    [SerializeField] private Material mat;
    private float total = 0;


    void Start()
    {
        ActionMarker.Main.OnBeat += DoStep;        
    }

    private void DoStep()
    {
        total += step;
        mat.SetFloat("_Offset", total);
    }
}
