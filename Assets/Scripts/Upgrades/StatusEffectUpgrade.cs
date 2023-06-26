using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Set Upgrade/Status Upgrade", fileName = "New Status Upgrade")]
public class StatusEffectUpgrade : UpgradeBase
{
    [SerializeField] private int extraBurnDamage;
    [SerializeField] private int extraBurnDuration;
    [SerializeField] private float extraStaticChance;
    [SerializeField] private int extraStaticDuration;



    public override void Apply()
    {
        Inventory.Main.burnDamage += extraBurnDamage;
        Inventory.Main.extraBurnDuration += extraBurnDuration;
        Inventory.Main.extraStaticDuration += extraStaticDuration;
        Inventory.Main.staticChance += extraStaticChance;
    }

    public override void Remove()
    {
        Inventory.Main.burnDamage -= extraBurnDamage;
        Inventory.Main.extraBurnDuration -= extraBurnDuration;
        Inventory.Main.extraStaticDuration -= extraStaticDuration;
        Inventory.Main.staticChance -= extraStaticChance;
    }
}
