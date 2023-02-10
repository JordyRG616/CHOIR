using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBase : WeaponBase
{
    [SerializeField] private int requiredHeatLevel;
    private WeaponMasterController weaponMasterController;


    protected virtual void Start()
    {
        weaponMasterController = WeaponMasterController.Main;
    }

    public override void Shoot(WeaponKey key)
    {
        //weaponMasterController.raisingHeat = true;

        if (weaponMasterController.heatLevel >= requiredHeatLevel)
        {
            ApplyHeatEffect();
        } 
        else RemoveHeatEffect();

        base.Shoot(key);
    }

    public override void Stop()
    {
        //weaponMasterController.raisingHeat = false;

        base.Stop();
    }

    protected virtual void ApplyHeatEffect()
    {

    }

    protected virtual void RemoveHeatEffect()
    {

    }
}
