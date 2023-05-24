using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorkshopManager : MonoBehaviour
{
    #region Main
    private static WorkshopManager _instance;
    public static WorkshopManager Main
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<WorkshopManager>();

            return _instance;
        }

    }
    #endregion

    [SerializeField] private Database database;

    [Header("Weapon List")]
    [SerializeField] private WorkshopWeaponBox weaponBoxModel;
    [SerializeField] private RectTransform weaponPanel;
    [SerializeField] private float scrollSpeed;
    private float _pos;
    private float panelPosition
    {
        get => _pos;
        set
        {
            if(value < 0) _pos = 0;
            else if(value > maxPos) _pos = maxPos;
            else _pos = value;
        }
    }
    private float maxPos
    {
        get
        {
            if(weaponPanel.childCount <= 3) return 0;
            else 
            {
                var count = weaponPanel.childCount - 4;
                return ((count * 40) + 10f) + (count * 3);
            }
        }
    }

    [Header("Module Grid")]
    [SerializeField] private ModuleBox moduleBoxModel;
    [SerializeField] private RectTransform modulePanel;

    [Space]
    [SerializeField] private int rerollCost;

    [Header("Weapon Offer")]
    [SerializeField] private List<WorkshopWeaponOffer> weaponOffers;
    [Header("Module Offer")]
    [SerializeField] private List<WorkshopModuleOffer> moduleOffers;

    private Inventory inventory;


    void Start()
    {
        inventory = Inventory.Main;

        CreateWeaponList();
        StartCoroutine(CreateOffer());
    }

    private void CreateWeaponList()
    {
        foreach(var weapon in inventory.ownedWeapons)
        {
            var box = Instantiate(weaponBoxModel, weaponPanel);
            box.ReceiveWeapon(weapon);
        }
    }

    private void CreateModuleGrid()
    {
        foreach(var module in inventory.installedModules)
        {
            var box = Instantiate(moduleBoxModel, modulePanel);
            box.SetModule(module);
        }
    }

    private IEnumerator CreateOffer()
    {
        var weaponList = database.GetRandomWeapons(weaponOffers.Count);
        var moduleList = database.GetRandomModules(moduleOffers.Count);

        for (int i = 0; i < weaponOffers.Count; i++)
        {
            weaponOffers[i].ReceiveWeapon(weaponList[i]);
            yield return new WaitForSeconds(0.1f);
        }

        for (int i = 0; i < moduleOffers.Count;  i++)
        {
            moduleOffers[i].ReceiveModule(moduleList[i]);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Reroll()
    {
        if(inventory.Credits >= rerollCost)
        {
            inventory.Credits -= rerollCost;
            weaponOffers.ForEach(x => x.Clear());
            StartCoroutine(CreateOffer());
        }
    }

    public void PurchaseWeapon(WeaponBase weapon)
    {
        inventory.AddWeapon(weapon);
        // database.RemoveWeapon(weapon);

        var box = Instantiate(weaponBoxModel, weaponPanel);
        box.ReceiveWeapon(weapon);

        inventory.Credits -= weapon.weaponCost;
    }

    public void PurchaseModule(ModuleBase module)
    {
        inventory.InstallModule(module);

        var box = Instantiate(moduleBoxModel, modulePanel);
        box.SetModule(module);

        if(!module.stackable) database.RemoveModule(module);
        inventory.Credits -= module.cost;
    }

    void Update()
    {
        panelPosition -= Input.mouseScrollDelta.y * scrollSpeed;
        weaponPanel.anchoredPosition = new Vector2(weaponPanel.anchoredPosition.x, panelPosition);
    }

    public void Continue()
    {
        var name = AlbumManager.Main.GetNextLevelName();
        SceneManager.LoadScene(name);
    }
}
