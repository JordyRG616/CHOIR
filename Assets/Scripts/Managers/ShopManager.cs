using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    #region Main
    private static ShopManager _instance;
    public static ShopManager Main
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<ShopManager>();

            return _instance;
        }

    }
    #endregion

    [SerializeField] private float scrollSpeed;
    [SerializeField] private List<int> weaponCosts;
    [SerializeField] private List<int> upgradeCosts;

    [Header("Panels")]
    [SerializeField] private RectTransform weaponPanel;
    [SerializeField] private RectTransform upgradePanel;
    [SerializeField] private GameObject rewardPanel;

    [Header("Boxes")]
    [SerializeField] private WeaponBox weaponBox;
    [SerializeField] private UpgradeBox upgradeBox;

    [Header("Costs")]
    [SerializeField] private TMPro.TextMeshProUGUI weaponCost;
    [SerializeField] private TMPro.TextMeshProUGUI upgradeCost;

    [Header("Rewards")]
    [SerializeField] private Database database;
    [SerializeField] private List<WeaponSelector> weaponSelectors;

    private float weaponMax;
    private float upgradeMax;

    private float _wPos;
    private float weaponPanelPosition
    {
        get => _wPos;
        set
        {
            if (value < 0) _wPos = 0;
            else if (value > weaponMax) _wPos = weaponMax;
            else _wPos = value;
        }
    }

    private float _uPos;
    private float upgradePanelPosition
    {
        get => _uPos;
        set
        {
            if (value < 0) _uPos = 0;
            else if (value > upgradeMax) _uPos = upgradeMax;
            else _uPos = value;
        }
    }

    private int _wCost;
    private int weaponCostIndex
    {
        get => _wCost;
        set
        {
            if (value >= weaponCosts.Count) _wCost = weaponCosts.Count - 1;
            else _wCost = value;
        }
    }
    public int currentWeaponCost => weaponCosts[weaponCostIndex];

    private int _uCost;
    private int upgradeCostIndex
    {
        get => _uCost;
        set
        {
            if (value >= upgradeCosts.Count) _uCost = upgradeCosts.Count - 1;
            else _uCost = value;
        }
    }
    public int currentUpgradeCost => upgradeCosts[upgradeCostIndex];



    private CrystalManager crystalManager;
    private Inventory inventory;

    private void Start()
    {
        crystalManager = CrystalManager.Main;
        inventory = Inventory.Main;

        weaponCost.text = currentWeaponCost.ToString();
        upgradeCost.text = currentUpgradeCost.ToString();
    }

    public void OpenNewWeaponPanel()
    {
        if (crystalManager.buildPoints < currentWeaponCost)
        {
            crystalManager.BlinkCost();
            return;
        }

        var weapons = new List<WeaponBase>(inventory.ownedWeapons);

        foreach (var selector in weaponSelectors)
        {
            var rdm = Random.Range(0, weapons.Count);
            selector.ReceiveWeapon(weapons[rdm]);
            weapons.RemoveAt(rdm);
        }

        rewardPanel.SetActive(true);
        crystalManager.ExpendBuildPoints(currentWeaponCost);
        weaponCostIndex++;
        weaponCost.text = currentWeaponCost.ToString();

        TutorialManager.Main.RequestTutorialPage(5, 4);
    }

    public void AddNewWeapon(WeaponBase weapon)
    {
        var slot = InputManager.Main.selectedSlot;
        var _weapon = Instantiate(weapon, Vector3.one * 500, Quaternion.identity);
        slot.ReceiveWeapon(_weapon);
    }

    public bool HasEnoughPoints(int value)
    {
        return crystalManager.buildPoints >= value;
    }

    public void OpenWeaponPanel()
    {
        //if (inventory.ownedWeapons.Count > weaponPanel.childCount) GenerateWeaponList();

        upgradePanel.parent.gameObject.SetActive(false);
        weaponPanel.parent.gameObject.SetActive(true);
        weaponPanel.anchoredPosition = Vector2.zero;
        weaponPanelPosition = 0;
    }

    public void OpenUpgradePanel()
    {
        weaponPanel.parent.gameObject.SetActive(false);
        upgradePanel.parent.gameObject.SetActive(true);
        upgradePanel.anchoredPosition = Vector2.zero;
        upgradePanelPosition = 0;
    }

    private void Update()
    {
        if (weaponPanel.gameObject.activeSelf)
        {
            var scroll = -Input.mouseScrollDelta.y * scrollSpeed;
            weaponPanelPosition += scroll;
            weaponPanel.anchoredPosition = new Vector2(-weaponPanelPosition, 0);
        }

        if (upgradePanel.gameObject.activeSelf)
        {
            var scroll = -Input.mouseScrollDelta.y * scrollSpeed;
            upgradePanelPosition += scroll;
            upgradePanel.anchoredPosition = new Vector2(upgradePanelPosition, 0);
        }
    }
}
