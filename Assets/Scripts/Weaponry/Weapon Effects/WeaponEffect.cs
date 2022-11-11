using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponEffect : MonoBehaviour
{
    protected WeaponDamageDealer damageDealer;

    protected virtual void Awake()
    {
    }

    protected virtual void Start()
    {
        damageDealer = GetComponent<WeaponDamageDealer>();
        damageDealer.ApplyEffects += Effect;
    }

    protected abstract void Effect(GameObject enemy);
}
