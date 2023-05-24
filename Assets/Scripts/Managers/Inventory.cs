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
    private List<WeaponBase> placedWeapons = new List<WeaponBase>();
    [SerializeField] private float convertFactor;
    public int currentMaxHealth {get; private set;} = 50;
    private int _credits;
    public int Credits
    {
        get => _credits;
        set
        {
            _credits = value;
        }
    }

    public List<ModuleBase> installedModules {get; private set;} = new List<ModuleBase>();

    [Header("Modules Parameters")]
    [SerializeField] public int burnDamage; 
    [SerializeField] public int extraStaticDuration;
    [SerializeField] public int frailtyMultiplier;
    public float GlobalExpMultiplier;


    void Awake()
    {
        if(_instance != null) Destroy(gameObject);
    }

    public void AddWeapon(WeaponBase weapon)
    {
        ownedWeapons.Add(weapon);
    }

    public void PlaceWeapon(WeaponBase weapon)
    {
        ownedWeapons.Remove(weapon);
        placedWeapons.Add(weapon);
    }

    public void InstallModule(ModuleBase module)
    {
        installedModules.Add(module);

        if(module.trigger == ModuleTrigger.Immediate)
        {
            module.Apply();
        }
    }

    public void ResetList()
    {
        ownedWeapons.AddRange(placedWeapons);
        placedWeapons.Clear();
    }

    public WeaponBase GetRandomAvailableWeapon()
    {
        var rdm = Random.Range(0, ownedWeapons.Count);
        var _w = ownedWeapons[rdm];
        ownedWeapons.RemoveAt(rdm);
        return _w;
    }

    public void ReceiveEndlevelValues(int score, int health)
    {
        Credits += Mathf.FloorToInt(score / convertFactor);
        currentMaxHealth = health;
    }

    public void SetMaxHealth(int initialHealth)
    {
        currentMaxHealth = initialHealth;
    }

    public void RaiseMaxHealth(float percentage)
    {
        currentMaxHealth = Mathf.RoundToInt(currentMaxHealth * (1 + percentage));
    }
}
