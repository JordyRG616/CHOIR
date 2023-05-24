using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Modules/Fuel", fileName = "Concentrated Fuel")]
public class ConcentratedFuel : ModuleBase
{
    public override void Apply()
    {
        Inventory.Main.burnDamage = Mathf.CeilToInt(Inventory.Main.burnDamage * 1.5f);
    }
}
