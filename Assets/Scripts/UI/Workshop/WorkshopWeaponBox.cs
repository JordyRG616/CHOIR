using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class WorkshopWeaponBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image weaponIcon;
    [SerializeField] private Image waveform;
    [SerializeField] private Image classIcon;
    [SerializeField] private TextMeshProUGUI weaponName;
    [Space]
    [SerializeField] private ClassToSpriteConverter converter;

    private WeaponBase cachedWeapon;


    public void ReceiveWeapon(WeaponBase weapon)
    {
        cachedWeapon = weapon;

        weaponIcon.sprite = weapon.weaponSprite;
        waveform.sprite = weapon.waveform;
        weaponName.text = weapon.name;

        classIcon.sprite = converter.GetSprite(weapon.weaponClass);
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
