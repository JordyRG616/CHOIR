using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Modules/Collector", fileName = "Collector nanites")]
public class CollectorNanites : ModuleBase
{
    [SerializeField] private float multiplierIncrease;

    public override void Apply()
    {
        Inventory.Main.GlobalExpMultiplier += multiplierIncrease;
    }
}
