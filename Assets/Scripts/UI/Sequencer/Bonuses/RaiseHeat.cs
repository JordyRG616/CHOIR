using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Chord Bonus/Raise heat", fileName = "Raise heat")]
public class RaiseHeat : ChordBonus
{
    public int value;

    public override string Description => "Raises the Heat level by one";

    public override string ExtraInfo => "(Flame weapons become more effective the higher the heat level)";

    public override void Apply()
    {
        WeaponMasterController.Main.heatLevel += value;
    }

    public override void Remove()
    {
        WeaponMasterController.Main.heatLevel -= value;
    }
}
