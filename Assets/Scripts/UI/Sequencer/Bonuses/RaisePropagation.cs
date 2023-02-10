using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Data/Chord Bonus/Raise propagation", fileName = "Raise propagation")]
public class RaisePropagation : ChordBonus
{
    [SerializeField] private int value;

    public override string Description => "Raises the propagation by " + value + "%";

    public override string ExtraInfo => "(Electric weapons can trigger special effects based of the propagation chance)";

    public override void Apply()
    {
        WeaponMasterController.Main.propagationChance += value;
    }

    public override void Remove()
    {
        WeaponMasterController.Main.currentPotency -= value;
    }
}
