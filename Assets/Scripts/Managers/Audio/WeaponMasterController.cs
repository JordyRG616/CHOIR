using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using TMPro;

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
    [Space]
    [SerializeField] private List<WeaponClassInfo> classInfo;

    [Header("Status Parameters")]
    public float burnDPS;
    public float burnTick;
    [Space]
    public float shockDuration;
    [Space]
    public float exposedMultiplier;

    [Header("Ballistics")]
    public int ammo;
    public int ammoGeneration = 0;

    [Header("Heat")]
    public int heatLevel = 0;

    [Header("Potency")]
    [SerializeField] private float overloadThreshold = 1;
    public float currentPotency = 0;
    public bool onOverload => currentPotency > overloadThreshold;
    public float potencyPercentage
    {
        get => currentPotency / overloadThreshold;
    }

    [Header("Static")]
    private int _propagation;
    public int propagationChance
    {
        get => _propagation;
        set
        {
            _propagation = value;
            if (_propagation < 0) _propagation = 0;
            if (_propagation > 100) _propagation = 100;

            OnPropagationChange?.Invoke(_propagation / 100f);
        }
    }

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI ammoUI;
    [SerializeField] private TextMeshProUGUI ammoGenerationUI;
    [SerializeField] private TextMeshProUGUI heatUI;
    [SerializeField] private TextMeshProUGUI energyUI;
    [SerializeField] private TextMeshProUGUI propagationUI;

    public delegate void PropagationUpdate(float percentage);
    public PropagationUpdate OnPropagationChange;


    private void Start()
    {
        RuntimeManager.StudioSystem.setParameterByName("Kit", initialKitIndex);

        ActionMarker.Main.OnReset += () => ammo += ammoGeneration;
    }

    public void RegisterWeaponClass(WeaponBase weapon)
    {
        var values = System.Enum.GetValues(typeof(WeaponClass));

        foreach (var value in values)
        {
            var _c = (WeaponClass)value;

            if (_c != WeaponClass.Default && weapon.classes.HasFlag(_c))
            {
                var info = classInfo.Find(x => x._class == _c);
                info.weapons.Add(weapon);
                //UpdateBuffs(info);
            }
        }

        classInfo.ForEach(x => x.UpdateUI());
    }

    private void Update()
    {
        ammoUI.text = ammo + " ammo";
        ammoGenerationUI.text = "+" + ammoGeneration + " per reset";
        heatUI.text = "Heat level:\n" + heatLevel;
        energyUI.text = "Energy reserve in:\n" + (potencyPercentage *100) + "%";
        propagationUI.text = "Propagation chance:\n" + propagationChance + "%";
    }

    public void RegisterWeaponClass(WeaponClass weaponClass, WeaponBase weapon)
    {
        var info = classInfo.Find(x => x._class == weaponClass);
        info.weapons.Add(weapon);
        //UpdateBuffs(info);

        classInfo.ForEach(x => x.UpdateUI());
    }

    private void UpdateBuffs(WeaponClassInfo classInfo)
    {
        switch (classInfo._class)
        {
            case WeaponClass.Ballistic:
                //globalBulletBuff = classInfo.GetBuffValue();
                break;
            case WeaponClass.Laser:
                exposedMultiplier = classInfo.GetBuffValue();
                break;
            case WeaponClass.Flame:
                burnDPS = classInfo.GetBuffValue();
                break;
            case WeaponClass.Electric:
                shockDuration = classInfo.GetBuffValue();
                break;
        }
    }
}

[System.Serializable]
public class WeaponClassInfo
{
    public WeaponClass _class;
    public List<WeaponBase> weapons = new List<WeaponBase>();
    public List<ClassBuffs> buffs;
    public TextMeshProUGUI levelUI;
    public int classLevel
    {
        get => weapons.Count;
    }

    public float GetBuffValue()
    {
        float value  = 0;
        ClassBuffs _buff = null;

        foreach (var buff in buffs)
        {
            if (classLevel >= buff.requiredLevel)
            {
                value = buff.buffValue;
                _buff = buff;
            }
        }

        buffs.ForEach(x => x.SetInfoUI(false));
        if (_buff != null) _buff.SetInfoUI(true);

        return value;
    }

    public void UpdateUI()
    {
        //levelUI.text = classLevel.ToString();
    }
}

[System.Serializable]
public class ClassBuffs
{
    public int requiredLevel;
    public float buffValue;
    public TextMeshProUGUI infoUI;

    public void SetInfoUI(bool active)
    {
        //if (active) infoUI.alpha = 1f;
        //else infoUI.alpha = 0.4f;
    }
}
