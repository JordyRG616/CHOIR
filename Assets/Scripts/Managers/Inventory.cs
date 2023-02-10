using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    #region Main
    private static Inventory _instance;
    public static Inventory Main
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<Inventory>();

            return _instance;
        }

    }
    #endregion

    public List<WeaponBase> ownedWeapons;
    public List<UpgradeBase> ownedUpgrades;
    [Space]
    public List<WeaponBase> availableWeapons;

    private void Awake()
    {
        //var container = new List<WeaponBase>();

        //for (int i = 0; i < 3; i++)
        //{
        //    var rdm = Random.Range(0, ownedWeapons.Count);
        //    container.Add(ownedWeapons[rdm]);
        //    ownedWeapons.RemoveAt(rdm);
        //}

        //availableWeapons = ownedWeapons;
        //ownedWeapons = container;

        //ownedWeapons = availableWeapons;
    }

    public void AddWeapon(WeaponBase weapon)
    {
        ownedWeapons.Add(weapon);
    }

    public void AddUpgrade(UpgradeBase upgrade)
    {
        var _upg = ownedUpgrades.Find(x => x == upgrade);
        if (_upg == null) ownedUpgrades.Add(upgrade);
        else _upg += upgrade;
    }

    public void ExpendUpgrade(UpgradeBase upgrade)
    {
        var _upg = ownedUpgrades.Find(x => x == upgrade);

        if (_upg == null) return;
        else
        {
            _upg--;
            if (_upg.amount == 0)
            {
                ownedUpgrades.Remove(_upg);
            }
        }
    }

    public WeaponBase GetRandomAvailableWeapon()
    {
        var rdm = Random.Range(0, ownedWeapons.Count);
        var _w = ownedWeapons[rdm];
        ownedWeapons.RemoveAt(rdm);
        return _w;
    }
}
