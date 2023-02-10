using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBase : WeaponBase
{
    private WeaponMasterController weaponMasterController;

    private void Start()
    {
        weaponMasterController = WeaponMasterController.Main;
    }

    public override void Shoot(WeaponKey key)
    {
        var rdm = Random.Range(0, 100);

        base.Shoot(key);

        //weaponMasterController.propagationChance += 25;
    }

    public override void Stop()
    {
        base.Stop();

        //weaponMasterController.propagationChance -= 25;
    }
}
