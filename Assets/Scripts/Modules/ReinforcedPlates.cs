using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Modules/Plates", fileName = "Reinforced Plates")]
public class ReinforcedPlates : ModuleBase
{
    [SerializeField] private float percentage;
    
    public override void Apply()
    {
        Inventory.Main.RaiseMaxHealth(percentage);
    }
}
