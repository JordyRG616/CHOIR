using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class SentrygunUpgrade : WeaponUpgradeController
{
    [Header("Common Upgrades")]
    [SerializeField] private float bulletSpeedIncrement;

    [Header("Uncommon Upgrades")]
    [SerializeField] private float knockbackIncrement;
    [SerializeField] private float rateIncrement;

    [Header("Rare Upgrades")]
    [SerializeField] private GameObject bullets;
    [SerializeField] private GameObject lasers;
    [SerializeField] private List<GameObject> explosions;

    [Header("UI")]
    [SerializeField] private GameObject tab, button;
    [SerializeField] private TextMeshProUGUI damageUI, speedUI, knockbackUI, rateUI;
    private float speed, knockback, rate;


    private void OnEnable()
    {
        if (!weapon.unlocked) return;
        tab.gameObject.SetActive(true);
        button.gameObject.SetActive(true);
        speed = shooters[0].main.startSpeed.constant;
        knockback = shooters[0].collision.colliderForce;
        rate = GetComponent<Animator>().speed;
    }

    public override void ApplyEffect(int effectIndex)
    {
        switch (effectIndex)
        {
            case 0:
                UpgradeDamage();
                break;
            case 1:
                UpgradeSpeed();
                break;
            case 2:
                UpgradeKnockback();
                break;
            case 3:
                UpgradeRate();
                break;
        }

        level++;
        UnlockCard();
    }

    private void UpgradeSpeed()
    {
        foreach (ParticleSystem shooter in shooters)
        {
            var main = shooter.main;
            speed += bulletSpeedIncrement;
            main.startSpeed = new ParticleSystem.MinMaxCurve(speed);
        }
    }

    private void UpgradeKnockback()
    {
        foreach (ParticleSystem shooter in shooters)
        {
            var collision = shooter.collision;
            knockback += knockbackIncrement;
            collision.colliderForce = knockback;
        }
    }

    private void UpgradeRate()
    {
        rate += rateIncrement;
        GetComponent<Animator>().speed = rate;
    }

    public override void UnlockFirstRare()
    {
        lasers.SetActive(true);
        bullets.SetActive(false);

        WeaponMasterController.Main.RegisterWeaponClass(WeaponClass.Laser, weapon);
    }

    public override void UnlockSecondRare()
    {
        explosions.ForEach(x => x.SetActive(true));

        WeaponMasterController.Main.RegisterWeaponClass(WeaponClass.Flame, weapon);
    }

    private void Update()
    {
        if (!weapon.unlocked) return;
        damageUI.text = weapon.damageRange.x.ToString("0") + "-" + weapon.damageRange.y.ToString("0");
        speedUI.text = speed.ToString("0");
        knockbackUI.text = (knockback / 10).ToString("0.0");
        rateUI.text = (rate * 3).ToString("0.0");
    }

    public override void DoPreview()
    {
        tab.GetComponent<EventTrigger>().OnPointerClick(new PointerEventData(EventSystem.current));
        button.GetComponent<EventTrigger>().OnPointerClick(new PointerEventData(EventSystem.current));
    }
}
