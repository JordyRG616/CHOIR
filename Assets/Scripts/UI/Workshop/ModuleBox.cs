using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ModuleBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image icon;
    private ModuleBase storedModule;


    public void SetModule(ModuleBase module)
    {
        icon = GetComponent<Image>();
        icon.sprite = module.icon;
        storedModule = module;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ModuleInfoPanel.Main.SetModuleInfo(storedModule);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ModuleInfoPanel.Main.Clear();
    }
}
