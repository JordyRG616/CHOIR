using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Chord Bonus/Raise ammo", fileName = "Raise ammo")]
public class RaiseAmmo : ChordBonus
{
    [SerializeField] private int value;

    public override string Description => "Raises ammo generation by " + value;

    public override string ExtraInfo => "(Projectile weapons consume ammo to enhance their base habilities)";

    public override void Apply()
    {
        WeaponMasterController.Main.ammoGeneration += value;
    }

    public override void Remove()
    {
        WeaponMasterController.Main.ammoGeneration -= value;
    }
}
