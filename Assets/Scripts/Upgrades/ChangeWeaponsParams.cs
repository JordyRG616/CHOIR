using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Set Upgrade/Weapons Upgrade", fileName = "New Weapons Upgrade")]
public class ChangeWeaponsParams : UpgradeBase
{
    [SerializeField] private float extraCritChance;
    [SerializeField] private float extraCritDamage;
    [SerializeField] private float extraDamageMultiplier;
    [SerializeField] private float extraExpMultiplier;


    public override void Apply()
    {
        Inventory.Main.extraCritChance += extraCritChance;
        Inventory.Main.extraCritDamage += extraCritDamage;
        Inventory.Main.globalDamageMultiplier += extraDamageMultiplier;
        Inventory.Main.GlobalExpMultiplier += extraExpMultiplier;
    }

    public override void Remove()
    {
        Inventory.Main.extraCritChance -= extraCritChance;
        Inventory.Main.extraCritDamage -= extraCritDamage;
        Inventory.Main.globalDamageMultiplier -= extraDamageMultiplier;
        Inventory.Main.GlobalExpMultiplier -= extraExpMultiplier;
    }
}
