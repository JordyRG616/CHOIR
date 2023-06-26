using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;

public class WeaponSelector : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image weaponIcon;
    [SerializeField] private TextMeshProUGUI weaponName;
    [SerializeField] private Image waveform;
    [SerializeField] private Image weaponClass;
    [SerializeField] private Image weaponSource;
    private WeaponBase storedWeapon = null;

    public void ReceiveWeapon(WeaponBase weapon)
    {
        storedWeapon = weapon;

        weaponIcon.sprite = weapon.weaponSprite;
        weaponName.text = weapon.name;
        waveform.sprite = weapon.waveform;

        weaponClass.sprite = weapon.classSprite;
        weaponSource.sprite = weapon.sourceSprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ShopManager.Main.BuildWeapon(storedWeapon);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * 1.1f;
        WeaponInfoPanel.Main.ReceiveWeapon(storedWeapon);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one;
        WeaponInfoPanel.Main.Clear();
    }

}
