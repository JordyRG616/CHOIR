using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChestUpgradeSelection : MonoBehaviour
{
    private UpgradeBase storedUpgrade;
    private MutationBase storedMutation;
    private WeaponBase storedWeapon;
    [Header("Upgrade Info")]
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private TextMeshProUGUI upgradeName;
    [SerializeField] private TextMeshProUGUI upgradeDescription;
    [SerializeField] private SpriteRenderer icon;
    [Header("Mutation Info")]
    [SerializeField] private GameObject mutationPanel;
    [SerializeField] private TextMeshProUGUI mutationName;
    [SerializeField] private TextMeshProUGUI mutationDescription;

    private Vector3 bigSize = new Vector3(1.2f, 1.2f, 1);

    private void OnMouseEnter()
    {
        transform.localScale = bigSize;
        infoPanel.SetActive(true);
        upgradeName.text = storedUpgrade.name;
        upgradeDescription.text = storedUpgrade.description;

        mutationPanel.SetActive(true);
        mutationName.text = storedMutation.name;
        mutationDescription.text = storedMutation.Description;
    }

    private void OnMouseExit()
    {
        infoPanel.SetActive(false);
        mutationPanel.SetActive(false);
        transform.localScale = Vector3.one;
    }

    private void OnMouseUp()
    {
        Inventory.Main.AddUpgrade(storedUpgrade);
        SpawnerManager.Main.PassMutation(storedMutation);
        Destroy(transform.parent.gameObject);
    }

    public void UnlockWeapon()
    {
        ShopManager.Main.AddNewWeapon(storedWeapon);
    }

    public void ReceiveUpgrade(UpgradeBase upgrade, MutationBase mutation)
    {
        storedUpgrade = upgrade;
        icon.sprite = storedUpgrade.icon;

        storedMutation = mutation;
    }

    public void ReceiveWeapon(WeaponBase weapon)
    {
        storedWeapon = weapon;
        icon.sprite = weapon.GetComponent<SpriteRenderer>().sprite;
    }
}
