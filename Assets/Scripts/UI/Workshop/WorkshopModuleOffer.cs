using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class WorkshopModuleOffer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI cost;
    [SerializeField] private GameObject light2d, costBox;
    private ModuleBase storedModule;


    public void ReceiveModule(ModuleBase module)
    {
        icon.sprite = module.icon;
        cost.text = module.cost.ToString();
        
        light2d.SetActive(true);
        icon.gameObject.SetActive(true);
        costBox.SetActive(true);

        storedModule = module;
    }

    private void Clear()
    {
        light2d.SetActive(false);
        icon.gameObject.SetActive(false);
        costBox.SetActive(false);

        storedModule = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(storedModule == null) return;
        ModuleInfoPanel.Main.SetModuleInfo(storedModule);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ModuleInfoPanel.Main.Clear();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(Inventory.Main.Credits >= storedModule.cost) 
        {
            WorkshopManager.Main.PurchaseModule(storedModule);
            Clear();
        }
    }

}
