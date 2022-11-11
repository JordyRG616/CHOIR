using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;


public class ShotgunUpgrade : WeaponUpgradeController
{
    [Header("Common Upgrades")]
    [SerializeField] private float sizeMultiplerValue;
    [SerializeField] private float speedMultiplierIncrement;

    [Header("Uncommon Upgrades")]
    [Range(0, 1)] [SerializeField] private float lifetimeLoss;
    [SerializeField] private float extraBullets;

    [Header("Rare Upgrades")]
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject sparkTyphoon;

    [Header("UI")]
    [SerializeField] private GameObject tab, button;
    [SerializeField] private TextMeshProUGUI damageUI, speedUI, bulletUI, sizeUI;
    private float speed, bullet, size, ricochet;


    private void OnEnable()
    {
        if (!weapon.unlocked) return;
        tab.gameObject.SetActive(true);
        button.gameObject.SetActive(true);
        speed = shooters[0].velocityOverLifetime.speedModifier.constant;
        size = shooters[0].main.startSize.constant;
        ricochet = shooters[0].collision.lifetimeLoss.constant;
        bullet = shooters[0].emission.GetBurst(0).count.constant;
    }

    public override void ApplyEffect(int effectIndex)
    {
        switch (effectIndex)
        {
            case 0:
                UpgradeDamage();
                break;
            case 1:
                UpgradeBullet();
                break;
            case 2:
                UpgradeSpeed();
                break;
            case 3:
                UpgradeSize();
                break;
            case 4:
                UpgradeRicochet();
                break;
        }

        level++;
        UnlockCard();
    }

    private void UpgradeBullet()
    {
        foreach (ParticleSystem shooter in shooters)
        {
            var emission = shooter.emission;
            var burst = emission.GetBurst(0);
            bullet += extraBullets;
            burst.count = bullet;
            emission.SetBurst(0, burst);
        }
    }

    private void UpgradeSpeed()
    {
        foreach (ParticleSystem shooter in shooters)
        {
            var main = shooter.velocityOverLifetime;
            speed += speedMultiplierIncrement;
            main.speedModifier = new ParticleSystem.MinMaxCurve(speed);
        }
    }

    private void UpgradeSize()
    {
        foreach (ParticleSystem shooter in shooters)
        {
            var main = shooter.main;
            size *= 1 + sizeMultiplerValue;
            main.startSize = new ParticleSystem.MinMaxCurve(size);
        }
    }

    private void UpgradeRicochet()
    {
        foreach (ParticleSystem shooter in shooters)
        {
            var collision = shooter.collision;
            ricochet *= 1 - lifetimeLoss;
            collision.lifetimeLoss = new ParticleSystem.MinMaxCurve(ricochet);
        }
    }

    public override void UnlockFirstRare()
    {
        explosion.SetActive(true);

        WeaponMasterController.Main.RegisterWeaponClass(WeaponClass.Flame, weapon);
    }

    public override void UnlockSecondRare()
    {
        sparkTyphoon.SetActive(true);
    }

    private void Update()
    {
        if (!weapon.unlocked) return;
        damageUI.text = weapon.damageRange.x.ToString("0") + "-" + weapon.damageRange.y.ToString("0");
        speedUI.text = (speed * 10).ToString("0.0");
        bulletUI.text = bullet.ToString("0");
        sizeUI.text = size.ToString("0.00");
    }

    public override void DoPreview()
    {
        tab.GetComponent<EventTrigger>().OnPointerClick(new PointerEventData(EventSystem.current));
        button.GetComponent<EventTrigger>().OnPointerClick(new PointerEventData(EventSystem.current));
    }
}
