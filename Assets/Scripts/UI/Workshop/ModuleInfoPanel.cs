using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ModuleInfoPanel : MonoBehaviour
{
    #region Main
    private static ModuleInfoPanel _instance;
    public static ModuleInfoPanel Main
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<ModuleInfoPanel>();

            return _instance;
        }

    }
    #endregion

    [SerializeField] private TextMeshProUGUI moduleName, moduleDescription;

    public void SetModuleInfo(ModuleBase module)
    {
        moduleName.text = module.name;
        moduleDescription.text = module.description;
    }

    public void Clear()
    {
        moduleName.text = "";
        moduleDescription.text = "";
    }
}