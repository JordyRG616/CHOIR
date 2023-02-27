using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class WeaponSelector : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject panel;
    [Space]
    [SerializeField] private Image weaponIcon;
    [SerializeField] private TextMeshProUGUI weaponName;
    [SerializeField] private Image weaponClass;
    [SerializeField] private TextMeshProUGUI weaponDescription;
    [SerializeField] private TextMeshProUGUI weaponPerk;
    [SerializeField] private Image tile;
    [SerializeField] private Image perkClass;
    [SerializeField] private TextMeshProUGUI perkAmount;
    public ClassToSpriteConverter converter;
    private WeaponBase storedWeapon = null;

    public void ReceiveWeapon(WeaponBase weapon)
    {
        storedWeapon = weapon;

        weaponIcon.sprite = storedWeapon.GetComponent<SpriteRenderer>().sprite;
        weaponName.text = storedWeapon.name;
        weaponClass.sprite = converter.GetSprite(storedWeapon.weaponClass);
        weaponDescription.text = storedWeapon.WeaponDescription();
        weaponPerk.text = storedWeapon.ClassPerkDesc;

        perkClass.sprite = converter.GetSprite(storedWeapon.perkReqClass);
        perkAmount.text = storedWeapon.perkReqAmount.ToString();

        tile.sprite = storedWeapon.tile.sprite;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        ShopManager.Main.AddNewWeapon(storedWeapon);

        foreach (var sel in FindObjectsOfType<WeaponSelector>())
        {
            sel.Clear();
        }

        panel.SetActive(false);
    }

    public void Clear()
    {
        storedWeapon = null;

        weaponName.text = "";
        weaponDescription.text = "";
        weaponPerk.text = "";
        perkAmount.text = "";

        weaponClass.color = Color.clear;
        tile.color = Color.clear;
        perkClass.color = Color.clear;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * 1.1f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
    }

}
