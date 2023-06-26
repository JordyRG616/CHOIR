using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WorkshopInfoPanel : MonoBehaviour
{
    #region Main
    private static WorkshopInfoPanel _instance;
    public static WorkshopInfoPanel Main
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<WorkshopInfoPanel>(true);

            return _instance;
        }

    }
    #endregion

    [SerializeField] private TextMeshProUGUI weaponName;
    [SerializeField] private TextMeshProUGUI weaponDescription;
    [SerializeField] private TextMeshProUGUI perkDescription;
    [SerializeField] private TextMeshProUGUI perkAmount;
    [SerializeField] private Image classIcon;
    [SerializeField] private Image classIconTwo;
    [SerializeField] private Image perkIcon;
    [SerializeField] private Image tile;



    public void ReceiveWeapon(WeaponBase weapon)
    {
        if(weapon == null)
        {
            Clear();
            return;
        }

        weaponName.text = weapon.name;
        weaponDescription.text = weapon.WeaponDescription();

        classIcon.transform.parent.gameObject.SetActive(true);
        perkIcon.transform.parent.gameObject.SetActive(true);
        tile.transform.parent.gameObject.SetActive(true);
        
        tile.sprite = weapon.tile.sprite;
    }

    public void Clear()
    {
        weaponName.text = "";
        weaponDescription.text = "";
        perkDescription.text = "";
        perkAmount.text = "";

        classIcon.transform.parent.gameObject.SetActive(false);
        perkIcon.transform.parent.gameObject.SetActive(false);
        tile.transform.parent.gameObject.SetActive(false);
    }
}
