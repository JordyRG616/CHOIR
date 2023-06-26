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
    public bool panelOpen;
    public int weaponsBuild;
    public int weaponsSold;
    public int weaponCount => weaponsBuild - weaponsSold;

    public bool canBuild => crystalManager.buildPoints >= weaponCost;
    private List<WeaponBase> weapons = new List<WeaponBase>();


    private CrystalManager crystalManager;
    private Inventory inventory;


    private void Start()
    {
        crystalManager = CrystalManager.Main;
        inventory = Inventory.Main;
        Time.timeScale = 1;
    }

    public void OpenNewWeaponPanel()
    {
        var slot = InputManager.Main.selectedSlot;
        weapons = new List<WeaponBase>(inventory.ownedWeapons);

        slot.buildVFX.Play();

        var _weapons = new List<WeaponBase>();
        for (int i = 0; i < 3; i++)
        {
            var weapon = weapons[Random.Range(0, weapons.Count)];
            _weapons.Add(weapon);
            weapons.Remove(weapon);
        }

        WeaponList.Main.Open(_weapons);
        panelOpen = true;
    }

    //DEBUG ONLY
    void Update()
    {
        if(panelOpen)
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                if(Input.GetKeyDown(KeyCode.R))
                {
                    var random = Random.Range(0, weapons.Count);
                    BuildWeapon(weapons[random]);
                }
            }
        }
    }

    public void BuildWeapon(WeaponBase weapon)
    {
        crystalManager.ExpendBuildPoints(weaponCost);
        
        var _weapon = Instantiate(weapon, Vector3.one * 500, Quaternion.identity);
        _weapon.name = weapon.name;

        if(weaponsBuild == 0 && ModuleActivationManager.Main.HasSpecialEffect(ModuleSpecialEffect.Firstborn))
        {
            _weapon.LevelUp();
        }

        var slot = InputManager.Main.selectedSlot;
        slot.ReceiveWeapon(_weapon);
        slot.buildVFX.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        inventory.PlaceWeapon(weapon);
        WeaponList.Main.Close();
        panelOpen = false;

        weaponsBuild++;
    }
}
