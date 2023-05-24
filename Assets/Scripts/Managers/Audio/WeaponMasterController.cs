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
    [Space]
    [SerializeField] private List<WeaponClassInfo> classInfo;


    private void Start()
    {
        RuntimeManager.StudioSystem.setParameterByName("Kit", initialKitIndex);
    }

    public void RegisterWeaponClass(WeaponClass weaponClass, WeaponClass perkClass, out WeaponClassInfo perkInfo)
    {
        perkInfo = classInfo.Find(x => x._class == perkClass); 

        var mainInfo = classInfo.Find(x => x._class == weaponClass);
        mainInfo.UpdateLevel(1);
    } 

    public void RemoveWeaponClass(WeaponClass weaponClass)
    {
        var mainInfo = classInfo.Find(x => x._class == weaponClass);
        mainInfo.UpdateLevel(-1);
    } 
}

[System.Serializable]
public class WeaponClassInfo
{
    public WeaponClass _class;
    public GameObject panel;
    public Slider slider;
    public List<TMPro.TextMeshProUGUI> indexes;
    public int classLevel { get; private set; } = 0;

    public delegate void LevelChangeEvent(int currentLevel);
    public LevelChangeEvent OnLevelChange;

    public void UpdateLevel(int value)
    {
        classLevel += value;
        OnLevelChange?.Invoke(classLevel);
        UpdateUI();
        WeaponInfoPanel.Main.UpdateInfo();
    }

    public void UpdateUI()
    {
        if(classLevel == 0)
        {
            panel.SetActive(false);
        } 
        else if(classLevel > 0)
        {
            if(!panel.activeSelf) panel.SetActive(true);

            slider.value = classLevel - 1;

            indexes.ForEach(x => x.alpha = 0.1f);
            indexes[classLevel - 1].alpha = 1;
        }
    }
}

