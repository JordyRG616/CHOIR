using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallisticBase : WeaponBase
{
    public override void Shoot(WeaponKey key)
    {
       
        ApplyChargeEffect();

        base.Shoot(key);
    }

    public override void Stop()
    {
        base.Stop();

        RemoveChargeEffect();
    }

    protected virtual void ApplyChargeEffect()
    {

    }

    protected virtual void RemoveChargeEffect()
    {

    }
}
