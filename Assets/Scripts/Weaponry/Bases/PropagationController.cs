using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropagationController : MonoBehaviour
{
    [SerializeField] private int subEmitterIndex;
    private WeaponMasterController controller;
    private ParticleSystem ps;

    private void Start()
    {
        controller = WeaponMasterController.Main;
        // controller.OnPropagationChange += UpdatePropagationChance;

        ps = GetComponent<ParticleSystem>();
    }

    private void UpdatePropagationChance(float percentage)
    {
        var sub = ps.subEmitters;
        sub.SetSubEmitterEmitProbability(subEmitterIndex, percentage);
    }
}
