using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : FlameBase
{
    [SerializeField] private StatusEffectApplier burnApplier;
    [SerializeField] private ParticleSystem subShooter;
    [SerializeField] private Color flameColor;
    private Color ogColor;

    protected override void Start()
    {
        base.Start();

        var main = subShooter.main;
        ogColor = main.startColor.color;
    }

    protected override void ApplyHeatEffect()
    {
        var main = subShooter.main;
        main.startColor = flameColor;

        subShooter.GetComponent<WeaponDamageDealer>().ReceiveStatusApplier(burnApplier);
    }

    protected override void RemoveHeatEffect()
    {
        var main = subShooter.main;
        main.startColor = ogColor;

        subShooter.GetComponent<WeaponDamageDealer>().RemoveStatusApplier(burnApplier);
    }
}
