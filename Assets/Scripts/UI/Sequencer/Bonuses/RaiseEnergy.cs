using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Chord Bonus/Raise potency", fileName = "Raise potency")]
public class RaiseEnergy : ChordBonus
{
    [SerializeField] private float value;

    public override string Description => "Raise the Energy reserve by " + (value * 100) + "%";

    public override string ExtraInfo => "(Laser weapons grow stronger the higher the Energy reserve, but becomes weak if it overflows)";

    public override void Apply()
    {
        WeaponMasterController.Main.currentPotency += value;
    }

    public override void Remove()
    {
        WeaponMasterController.Main.currentPotency -= value;
    }
}
