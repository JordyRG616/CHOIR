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
    [SerializeField] private TextMeshProUGUI weaponClass;
    [SerializeField] private TextMeshProUGUI weaponDescription;
    [SerializeField] private TextMeshProUGUI weaponEffect;
    [SerializeField] private Transform tiles;
    [SerializeField] private TextMeshProUGUI cost;
    [SerializeField] private GameObject costBox;
    private WeaponBase storedWeapon = null;
    private ChordBonus storedChord = null;
    private UpgradeBase storedUpgrade = null;
    private MutationBase storedMutation = null;

    public void ReceiveWeapon(WeaponBase weapon)
    {
        storedWeapon = weapon;

        weaponIcon.sprite = storedWeapon.GetComponent<SpriteRenderer>().sprite;
        weaponName.text = storedWeapon.name;
        weaponClass.text = storedWeapon.classes.ToString() + " WEAPON";
        weaponDescription.text = storedWeapon.Description();
        weaponEffect.text = storedWeapon.Effect;

        costBox.SetActive(true);
        var tile = weapon.tiles[0];

        tiles.Find(tile.type.ToString()).gameObject.SetActive(true);
        cost.text = tile.cost.ToString();
    }

    public void ReceiveChord(ChordBonus chordBonus)
    {
        storedChord = chordBonus;

        weaponIcon.sprite = chordBonus.icon;
        weaponName.text = storedChord.name;
        weaponClass.text = "";
        weaponDescription.text = storedChord.Description;
        weaponEffect.text = storedChord.ExtraInfo;
    }

    public void ReceiveUpgrade(UpgradeBase upgrade, MutationBase mutation)
    {
        storedUpgrade = upgrade;
        storedMutation = mutation;

        weaponIcon.sprite = upgrade.icon;
        weaponName.text = upgrade.name;
        weaponDescription.text = upgrade.description;
        weaponEffect.color = Color.red;
        weaponEffect.text = mutation.Description;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(storedWeapon != null) ShopManager.Main.AddNewWeapon(storedWeapon);
        if (storedChord != null) storedChord.Apply();
        if(storedUpgrade != null)
        {
            Inventory.Main.AddUpgrade(storedUpgrade);
            SpawnerManager.Main.PassMutation(storedMutation);
            ShopManager.Main.GenerateUpgradeList();
        }

        foreach (var sel in FindObjectsOfType<WeaponSelector>())
        {
            sel.Clear();
        }

        panel.SetActive(false);
    }

    public void Clear()
    {
        storedChord = null;
        storedWeapon = null;
        storedUpgrade = null;
        storedMutation = null;

        weaponName.text = "";
        weaponClass.text = "";
        weaponDescription.text = "";
        weaponEffect.color = Color.white;
        weaponEffect.text = "";

        costBox.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            tiles.GetChild(i).gameObject.SetActive(false);
        }
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
