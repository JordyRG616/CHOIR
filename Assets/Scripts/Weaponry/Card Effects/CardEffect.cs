using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName ="Data/Card Effect")]
public class CardEffect : ScriptableObject
{
    [SerializeField] protected int weaponID, effectID;
    [SerializeField] protected bool rare;
    [SerializeField] protected int rareIndex;

    public virtual void Apply()
    {
        if (rare) RareUpgrade();
        else GeneralUpgrade();
    }

    private void GeneralUpgrade()
    {
        foreach (var weapon in GetTargetWeapons())
        {
            weapon.GetComponent<WeaponUpgradeController>().ApplyEffect(effectID);
        }
    }

    private void RareUpgrade()
    {
        foreach (var weapon in GetTargetWeapons())
        {
            if (rareIndex == 1) weapon.GetComponent<WeaponUpgradeController>().UnlockFirstRare();
            if (rareIndex == 2) weapon.GetComponent<WeaponUpgradeController>().UnlockSecondRare();
        }
    }

    protected virtual List<WeaponBase> GetTargetWeapons()
    {
        var list = FindObjectsOfType<WeaponBase>().ToList();
        return list.FindAll(x => x.ID == weaponID);
    }
}
