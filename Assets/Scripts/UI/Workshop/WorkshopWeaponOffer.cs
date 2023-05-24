using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class WorkshopWeaponOffer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image weaponIcon;
    [SerializeField] private TextMeshProUGUI cost;
    [SerializeField] private GameObject costBox;
    [SerializeField] private GameObject classBox;
    [SerializeField] private Image classIcon;
    [SerializeField] private GameObject bgLight;
    private WeaponBase cachedWeapon;

    [SerializeField] private ClassToSpriteConverter converter;


    public void ReceiveWeapon(WeaponBase weapon)
    {
        cachedWeapon = weapon;

        weaponIcon.gameObject.SetActive(true);
        costBox.SetActive(true);
        bgLight.SetActive(true);
        classBox.SetActive(true);

        weaponIcon.sprite = weapon.weaponSprite;
        cost.text = weapon.weaponCost.ToString();
        classIcon.sprite = converter.GetSprite(weapon.weaponClass);
    }

    public void Clear()
    {
        cachedWeapon = null;

        weaponIcon.gameObject.SetActive(false);
        costBox.SetActive(false);
        bgLight.SetActive(false);
        classBox.SetActive(false);
    }

    public void PurchaseWeapon()
    {
        if(Inventory.Main.Credits >= cachedWeapon.weaponCost)
        {
            WorkshopManager.Main.PurchaseWeapon(cachedWeapon);
            Clear();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        WorkshopInfoPanel.Main.ReceiveWeapon(cachedWeapon);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        WorkshopInfoPanel.Main.Clear();
    }
}
