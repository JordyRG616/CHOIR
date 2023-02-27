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

    [SerializeField] private Image weaponSprite, classSprite, perkClassSprite, tileSprite;
    [SerializeField] private TextMeshProUGUI weaponName, description, perk, perkReqAmount, level;
    [SerializeField] private ClassToSpriteConverter converter;
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
        weaponSprite.sprite = weapon.weaponSprite;

        classSprite.color = Color.white;
        classSprite.sprite = converter.GetSprite(weapon.weaponClass);

        perkClassSprite.color = Color.white;
        perkClassSprite.sprite = converter.GetSprite(weapon.perkReqClass);
        perkReqAmount.text = weapon.perkReqAmount.ToString();

        var tile = weapon.tile;
        tileSprite.color = Color.white;
        tileSprite.sprite = tile.sprite;

        UpdateInfo();
        selectedBox = box;

        gameObject.SetActive(true);
    }

    public void UpdateInfo()
    {
        level.text = "LEVEL " + selectedWeapon.level;
        description.text = selectedWeapon.WeaponDescription();
        perk.text = selectedWeapon.ClassPerkDesc;
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
        description.text = "";
        perk.text = "";
        level.text = "";
        perkReqAmount.text = "";

        classSprite.sprite = null;
        classSprite.color = Color.clear;
        perkClassSprite.sprite = null;
        perkClassSprite.color = Color.clear;
        tileSprite.sprite = null;
        tileSprite.color = Color.clear;

        // gameObject.SetActive(false);
    }
}

[System.Serializable]
public class ClassToSpriteConverter
{
    public Sprite flameIcon, bulletIcon, electricIcon, laserIcon, nuclearIcon;

    public Sprite GetSprite(WeaponClass _class)
    {
        switch(_class)
        {
            case WeaponClass.Projectile:
            return bulletIcon;
            case WeaponClass.Laser:
            return laserIcon;
            case WeaponClass.Electric:
            return electricIcon;
            case WeaponClass.Flame:
            return flameIcon;
            case WeaponClass.Nuclear:
            return nuclearIcon;
        }

        return null;
    }
}
