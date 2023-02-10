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
    [SerializeField] private List<ChordBonus> chords;

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
    private int currentWeaponCost => weaponCosts[weaponCostIndex];

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
    private int currentUpgradeCost => upgradeCosts[upgradeCostIndex];



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
        bool chordChosen = false;

        foreach (var selector in weaponSelectors)
        {
            if (Random.Range(0, 1f) >= 0.2f || chordChosen)
            {
                var rdm = Random.Range(0, weapons.Count);
                selector.ReceiveWeapon(weapons[rdm]);
                weapons.RemoveAt(rdm);
            } else
            {
                var rdm = Random.Range(0, chords.Count);
                selector.ReceiveChord(chords[rdm]);
                chordChosen = true;
            }
        }

        rewardPanel.SetActive(true);
        crystalManager.ExpendBuildPoints(currentWeaponCost);
        weaponCostIndex++;
        weaponCost.text = currentWeaponCost.ToString();

        TutorialManager.Main.RequestTutorialPage(5, 4);
    }

    public void OpenNewUpgradePanel()
    {
        if (crystalManager.buildPoints < currentUpgradeCost)
        {
            crystalManager.BlinkCost();
            return;
        }

            foreach (var selector in weaponSelectors)
        {
            var upg = database.GetRandomUpgrade();
            var mut = database.GetRandomMutation();
            selector.ReceiveUpgrade(upg, mut);
        }

        rewardPanel.SetActive(true);
        crystalManager.ExpendBuildPoints(currentUpgradeCost);
        upgradeCostIndex++;
        upgradeCost.text = currentUpgradeCost.ToString();

        TutorialManager.Main.RequestTutorialPage(13);
    }

    public void AddNewWeapon(WeaponBase weapon)
    {
        TutorialManager.Main.RequestTutorialPage(4);

        var box = Instantiate(weaponBox, weaponPanel);
        box.ReceiveWeapon(weapon);

        weaponMax = weaponPanel.childCount > 4 ? ((weaponPanel.childCount - 4) * 41.5f) + 10f : 0;
    }

    public bool HasEnoughPoints(int value)
    {
        return crystalManager.buildPoints >= value;
    }

    public void GenerateUpgradeList()
    {
        var count = upgradePanel.childCount;
        if (count > 1)
        {
            for (int i = 1; i < count; i++)
            {
                var box = upgradePanel.GetChild(i);
                Destroy(box.gameObject);
            }
        }

        foreach (var upgrade in inventory.ownedUpgrades)
        {
            var box = Instantiate(upgradeBox, upgradePanel);
            box.ReceiveUpgrade(upgrade);
        }

        upgradeMax = upgradePanel.childCount > 4 ? ((upgradePanel.childCount - 4) * 41.5f) + 10f : 0;
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
        GenerateUpgradeList();

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
