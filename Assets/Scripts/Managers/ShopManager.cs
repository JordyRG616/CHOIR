using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    #region Main
    private static ShopManager _instance;
    public static ShopManager Main
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<ShopManager>();

            return _instance;
        }

    }
    #endregion

    [field: SerializeField] public int weaponCost {get; private set;}
    public bool canBuild => crystalManager.buildPoints >= weaponCost;

    public bool panelOpen;

    private float weaponMax;
    private float upgradeMax;

    private float _wPos;
    private float weaponPanelPosition
    {
        get => _wPos;
        set
        {
            if (value < 0) _wPos = 0;
            else if (value > weaponMax) _wPos = weaponMax;
            else _wPos = value;
        }
    }

    private float _uPos;
    private float upgradePanelPosition
    {
        get => _uPos;
        set
        {
            if (value < 0) _uPos = 0;
            else if (value > upgradeMax) _uPos = upgradeMax;
            else _uPos = value;
        }
    }

    private CrystalManager crystalManager;
    private Inventory inventory;


    private void Start()
    {
        crystalManager = CrystalManager.Main;
        inventory = Inventory.Main;
    }

    public void OpenNewWeaponPanel()
    {
        var slot = InputManager.Main.selectedSlot;
        var weapons = inventory.ownedWeapons.FindAll(x => x.surfaceWeapon == slot.surface);

        slot.buildVFX.Play();
        WeaponList.Main.Open(weapons);
        panelOpen = true;
    }

    public void BuildWeapon(WeaponBase weapon)
    {
        crystalManager.ExpendBuildPoints(weaponCost);
        
        var _weapon = Instantiate(weapon, Vector3.one * 500, Quaternion.identity);
        _weapon.name = weapon.name;

        var slot = InputManager.Main.selectedSlot;
        slot.ReceiveWeapon(_weapon);
        slot.buildVFX.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        inventory.PlaceWeapon(weapon);
        WeaponList.Main.Close();
        panelOpen = false;
    }
}
