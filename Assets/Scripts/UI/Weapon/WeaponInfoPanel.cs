using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponInfoPanel : MonoBehaviour
{
    #region Main
    private static WeaponInfoPanel _instance;
    public static WeaponInfoPanel Main
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<WeaponInfoPanel>(true);

            return _instance;
        }

    }
    #endregion

    [SerializeField] private Image weaponSprite;
    [SerializeField] private TextMeshProUGUI weaponName, classInfo, description, effect;
    [SerializeField] private Transform tiles;
    [SerializeField] private TextMeshProUGUI cost;
    [SerializeField] private GameObject costBox;
    private WeaponBase selectedWeapon;
    private WeaponBox selectedBox;
    private InputManager pointerHandler;

    private void Start()
    {
        pointerHandler = InputManager.Main;
    }

    public void ReceiveWeapon(WeaponBase weapon, WeaponBox box = null)
    {
        selectedWeapon = weapon;
        weaponName.text = weapon.name;
        weaponSprite.color = Color.white;
        weaponSprite.sprite = weapon.GetComponent<SpriteRenderer>().sprite;
        classInfo.text = weapon.classes.ToString() + " WEAPON";
        description.text = weapon.Description();
        effect.text = weapon.Effect;

        costBox.SetActive(true);
        var tile = weapon.tiles[0];

        tiles.Find(tile.type.ToString()).gameObject.SetActive(true);
        cost.text = tile.cost.ToString();

        selectedBox = box;

        gameObject.SetActive(true);
    }

    public void ReceiveUpgrade(UpgradeBase upgrade)
    {
        weaponName.text = upgrade.name;
        weaponSprite.color = Color.white;
        weaponSprite.sprite = upgrade.icon;
        description.text = upgrade.description;

        gameObject.SetActive(true);
    }

    public void Clear()
    {
        weaponName.text = "";
        weaponSprite.sprite = null;
        weaponSprite.color = Color.clear;
        classInfo.text = "";
        description.text = "";
        effect.text = "";

        costBox.SetActive(false);
        for (int i = 0; i < 3; i++)
        {
            tiles.GetChild(i).gameObject.SetActive(false);
        }

        gameObject.SetActive(false);
    }
}
