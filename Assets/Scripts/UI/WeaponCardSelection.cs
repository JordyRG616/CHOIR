using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Linq;

public class WeaponCardSelection : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private Transform playerHand;
    [SerializeField] private Image frameImage;
    [SerializeField] private Image packImage;
    [SerializeField] private Animator animator;
    [SerializeField] private float delay;
    [SerializeField] private List<Color> rarityColors;
    [SerializeField] private bool initialSelection;
    private Material _material;
    private WeaponCard card;
    private BaseSpawnerUpgrade upgradeToApply;
    private EnemySpawner spawner;
    private EnemySpawner extraSpawner;
    private WeaponBase cachedWeapon;

    private void Awake()
    {
        _material = new Material(packImage.material);
        packImage.material = _material;
    }

    public void ConfigureCard(WeaponCard receivedCard, BaseSpawnerUpgrade spawnerUpgrade)
    {
        card = receivedCard;
        image.sprite = card.Sprite;
        description.text = card.Description;
        upgradeToApply = spawnerUpgrade;
        cachedWeapon = FindObjectsOfType<WeaponBase>(true).ToList().Find(x => x.ID == card.actionTile.weaponID);
        SelectUpgradedSpawner();

        frameImage.sprite = card.frame;
        _material.SetColor("_FinalColor", rarityColors[(int)card.rarity]);
        animator.SetTrigger("Open");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Time.timeScale = 1;
        Instantiate(card, playerHand).name = card.name;
        if (card.unique) RewardManager.Main.RemoveCard(card);
        RewardManager.Main.RemoveCard(card.incompatibleCards);

        if (card.spawnerToUpgrade != EnemySpawner.SpawnerPosition.None)
        {
            upgradeToApply.ApplyUpgrade(spawner);
            if (extraSpawner != null) upgradeToApply.ApplyUpgrade(extraSpawner);
        }

        if (!cachedWeapon.unlocked) cachedWeapon.gameObject.SetActive(false);
        else cachedWeapon.graphicsController.SetPreview(false);
        DisablePreviews();
        RewardManager.Main.CloseReward();

        if (initialSelection) gameObject.SetActive(false);
    }

    private void SelectUpgradedSpawner()
    {
        var spawners = FindObjectsOfType<EnemySpawner>().ToList();

        if (card.spawnerToUpgrade == EnemySpawner.SpawnerPosition.Both)
        {
            spawner = spawners[0];
            extraSpawner = spawners[1];
        }
        else
        {
            spawner = spawners.Find(x => x.position == card.spawnerToUpgrade);
            extraSpawner = null;
        }
    }

    public void ClearCard()
    {
        image.sprite = null;
        description.text = null;
        card = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PreviewUpgrade();
    }

    public void PreviewUpgrade()
    {
        if (cachedWeapon.unlocked == false) cachedWeapon.gameObject.SetActive(true);
        cachedWeapon.graphicsController.SetPreview(true);

        if (!card.actionTile.handler)
        {
            var upgrade = cachedWeapon.GetComponent<WeaponUpgradeController>();
            upgrade.ActivatePreview();
        }

        if (card.spawnerToUpgrade == EnemySpawner.SpawnerPosition.None) return;

        if (upgradeToApply.type != BaseSpawnerUpgrade.UpgradeType.AddEnemy)
        {
            spawner.infoUI.EnablePreview(upgradeToApply.stat, upgradeToApply.increment);
            if (extraSpawner != null) extraSpawner.infoUI.EnablePreview(upgradeToApply.stat, upgradeToApply.increment);
        }

        if (card.rarity != CardRarity.Base) TutorialManager.Main.DoTutorialStep("Spawner Upgrade");

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DisablePreviews();
    }

    public void DisablePreviews()
    {
        if(cachedWeapon.gameObject.activeSelf)
        {
            if (cachedWeapon.unlocked == false) cachedWeapon.gameObject.SetActive(false);
            cachedWeapon.graphicsController.SetPreview(false);
        }

        if (spawner != null) spawner.infoUI.DisablePreview();
        if (extraSpawner != null) extraSpawner.infoUI.DisablePreview();

        FindObjectOfType<WeaponInfoPanel>().DeactivateAll();
    }
}
