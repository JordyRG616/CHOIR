using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Modules/Voltage Override", fileName = "Voltage Override")]
public class VoltageOverride : ModuleBase
{
    public override void Apply()
    {
        Inventory.Main.extraStaticDuration += 1;
    }
}
