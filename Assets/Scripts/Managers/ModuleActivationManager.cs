using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleActivationManager : MonoBehaviour
{
    #region Main
    private static ModuleActivationManager _instance;
    public static ModuleActivationManager Main
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<ModuleActivationManager>();

            return _instance;
        }

    }
    #endregion


    private Inventory inventory;
    private List<ModuleSpecialEffect> activeEffects = new List<ModuleSpecialEffect>();

    void Start()
    {
        inventory = Inventory.Main;

        EvaluateEffects(ModuleTrigger.OnLevelStart);
    }

    private void EvaluateEffects(ModuleTrigger trigger)
    {
        foreach(var module in inventory.installedModules)
        {
            if(module.trigger == trigger) module.Apply();
        }
    }

    public void ReceiveSpecialEffect(ModuleSpecialEffect effect)
    {
        activeEffects.Add(effect);
    }

    public bool HasSpecialEffect(ModuleSpecialEffect effect)
    {
        return activeEffects.Contains(effect);
    }
}

public enum ModuleSpecialEffect{ExplosiveDeath, BoomBox, Regenerative, Firstborn, Cashback, Collector}