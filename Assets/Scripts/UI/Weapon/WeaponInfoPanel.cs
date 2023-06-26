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

    [Header("Weapon Panel")]
    [SerializeField] private GameObject weaponPanel;
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private Image classSprite;
    [SerializeField] private Image sourceSprite;
    [SerializeField] private Image tileSprite;
    [SerializeField] private TextMeshProUGUI weaponName;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private TextMeshProUGUI perk;
    [SerializeField] private TextMeshProUGUI perkReqAmount;
    [SerializeField] private TextMeshProUGUI level;
    [SerializeField] private TextMeshProUGUI nextLevelInfo;
    [SerializeField] private TextMeshProUGUI nextLevelCost;

    [Header("Set Panel")]
    [SerializeField] private GameObject setPanel;
    [SerializeField] private CanvasGroup firstGroup;
    [SerializeField] private TextMeshProUGUI firstRequirement;
    [SerializeField] private TextMeshProUGUI firstDescription;
    [SerializeField] private CanvasGroup secondGroup;
    [SerializeField] private TextMeshProUGUI secondRequirement;
    [SerializeField] private TextMeshProUGUI secondDescription;

    private WeaponBase selectedWeapon;
    private WeaponBox selectedBox;
    private InputManager pointerHandler;
    private RectTransform rect;


    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        pointerHandler = InputManager.Main;
        Clear();
    }

    public void ReceiveWeapon(WeaponBase weapon, WeaponBox box = null)
    {
        setPanel.SetActive(false);
        weaponPanel.SetActive(true);


        selectedWeapon = weapon;
        weaponName.text = weapon.name;

        classSprite.color = Color.white;
        classSprite.sprite = weapon.classSprite;

        sourceSprite.color = Color.white;
        sourceSprite.sprite = weapon.sourceSprite;

        var tile = weapon.tile;
        tileSprite.color = Color.white;
        tileSprite.sprite = tile.sprite;

        UpdateInfo();
        selectedBox = box;

        gameObject.SetActive(true);
    }

    public void UpdateInfo()
    {
        if(selectedWeapon == null) return;

        level.text = "LEVEL " + selectedWeapon.level;
        description.text = selectedWeapon.WeaponDescription();

        if(InputManager.Main.interactionMode == InteractionMode.Upgrade)
        {
            upgradePanel.SetActive(true);
            nextLevelInfo.text = selectedWeapon.nextLevelDescription;
            var cost = selectedWeapon.level + 1;
            nextLevelCost.text = cost + "$";
            if(CrystalManager.Main.buildPoints >= cost) nextLevelCost.color = Color.green;
            else nextLevelCost.color = Color.red;
        } 
        else upgradePanel.SetActive(false);
    }

    public void ReceiveSet(SetBonus firstBonus, SetBonus secondBonus)
    {
        weaponPanel.SetActive(false);
        setPanel.SetActive(true);

        firstRequirement.text = firstBonus.requiredCount + "+";
        firstDescription.text = firstBonus.upgrade.description;
        firstGroup.alpha = firstBonus.applied ? 1 : 0.3f;

        secondRequirement.text = secondBonus.requiredCount + "+";
        secondDescription.text = secondBonus.upgrade.description;
        secondGroup.alpha = secondBonus.applied ? 1 : 0.3f;

        gameObject.SetActive(true);
    }

    public void Clear()
    {
        weaponPanel.SetActive(false);
        setPanel.SetActive(false);

        // weaponName.text = "";
        // description.text = "";
        // perk.text = "";
        // level.text = "";
        // perkReqAmount.text = "";

        // classSprite.sprite = null;
        // classSprite.color = Color.clear;
        // sourceSprite.sprite = null;
        // sourceSprite.color = Color.clear;
        // tileSprite.sprite = null;
        // tileSprite.color = Color.clear;

        gameObject.SetActive(false);
    }

    void Update()
    {
        CalculatePivot();
        rect.anchoredPosition = CalculatePointerPosition();
    }

    private void CalculatePivot()
    {
        if(rect.anchoredPosition.x + rect.sizeDelta.x > 320) rect.pivot = new Vector2(1.05f, rect.pivot.y);
        else rect.pivot = new Vector2(-0.05f, rect.pivot.y);

        if(rect.anchoredPosition.y + rect.sizeDelta.y > 180) rect.pivot = new Vector2(rect.pivot.x, 1.05f);
        else rect.pivot = new Vector2(rect.pivot.x, -0.05f);
    }

    private Vector2 CalculatePointerPosition()
    {
        var position = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        position -= Vector3.one * 0.5f;

        position.x *= 640;
        position.y *= 360;

        return position;
    }
}