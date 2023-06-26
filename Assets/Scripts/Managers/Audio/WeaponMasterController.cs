using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class WeaponMasterController : MonoBehaviour
{
    #region Main
    private static WeaponMasterController _instance;
    public static WeaponMasterController Main
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<WeaponMasterController>();

            return _instance;
        }

    }
    #endregion

    [SerializeField] private int initialKitIndex;
    [SerializeField] private Dictionary<WeaponClass, SetManager> classSets = new Dictionary<WeaponClass, SetManager>();
    [SerializeField] private Dictionary<WeaponSource, SetManager> sourceSets = new Dictionary<WeaponSource, SetManager>();


    private void Start()
    {
        RuntimeManager.StudioSystem.setParameterByName("Kit", initialKitIndex);
    }

    public void ReceiveSet(SetManager set)
    {
        if(set.classSet)
        {
            classSets.Add(set._class, set);
        } else
        {
            sourceSets.Add(set._source, set);
        }
    }

    public void RegisterWeapon(WeaponBase weapon)
    {
        var cSet = classSets[weapon.weaponClass];
        cSet.ReceiveWeapon();

        var sSet = sourceSets[weapon.weaponSource];
        sSet.ReceiveWeapon();
    } 

    public void RemoveWeapon(WeaponBase weapon)
    {
        var cSet = classSets[weapon.weaponClass];
        cSet.RemoveWeapon();

        var sSet = sourceSets[weapon.weaponSource];
        sSet.RemoveWeapon();
    } 
}

