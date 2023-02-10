using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBase : WeaponBase
{

    protected WeaponMasterController weaponMasterController;


    private void Start()
    {
        weaponMasterController = WeaponMasterController.Main;
    }

    public override void Shoot(WeaponKey key)
    {
        //weaponMasterController.raisingPotency = true;

        if (weaponMasterController.onOverload) ApplyOverloadEffect();
        else ApplyPotency();

        base.Shoot(key);
    }

    public override void Stop()
    {
        base.Stop();

        //weaponMasterController.raisingPotency = false;

        RemoveEffects();
    }


    protected virtual void ApplyOverloadEffect()
    {

    }

    protected virtual void RemoveEffects()
    {

    }

    protected virtual void ApplyPotency()
    {

    }
}
