using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Supports/Extra Ammo", fileName = "Extra Ammo")]
public class ExtraAmmoSupport : SupportBase
{
    [SerializeField] private int permanentCharges;

    public override void ApplyEffect(WeaponBase weapon)
    {
        if(weapon.classes.HasFlag(WeaponClass.Ballistic))
        {
            var ballistic = weapon as BallisticBase;
        }
    }

    public override void RemoveEffect(WeaponBase weapon)
    {
        if (weapon.classes.HasFlag(WeaponClass.Ballistic))
        {
            var ballistic = weapon as BallisticBase;
        }
    }
}
