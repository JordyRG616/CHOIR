using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class GroundTurretUpgrade : WeaponUpgradeController
{
    [SerializeField] private ParticleSystem cartridge;
    [Header("Extra Bullet Upgrade")]
    [SerializeField] private float extraBulletChance;
    [Header("Bullet Speed Upgrade")]
    [SerializeField] private float speedMultiplerValue;

    [Header("Uncommon Upgrades")]
    [Range(0, 1)] [SerializeField] private float knockbackIncreasePercentage;
    [SerializeField] private float freezeDurationIncrement;

    [Header("Rare Upgrades")]
    [SerializeField] private GameObject caltrops, burst;

    [Header("UI")]
    [SerializeField] private GameObject tab, button;
    [SerializeField] private TextMeshProUGUI damageUI, speedUI, bulletUI, knockbackUI;
    private float speed, bullet, knockback;


    private void OnEnable()
    {
        if (!weapon.unlocked) return;
        tab.gameObject.SetActive(true);
        button.gameObject.SetActive(true);
        speed = shooters[0].main.startSpeed.constant;
        knockback = shooters[0].collision.colliderForce;
        bullet = 0;
    }

    public override void ApplyEffect(int effectIndex)
    {
        switch(effectIndex)
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
                UpgradeKnockback();
                break;
            case 4:
                UpgradeFreeze();
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
            var burst = emission.GetBurst(1);
            bullet += extraBulletChance;
            burst.probability = bullet;
            emission.SetBurst(1, burst);
        }
    }

    private void UpgradeSpeed()
    {
        foreach (ParticleSystem shooter in shooters)
        {
            var main = shooter.main;
            speed *= 1 + speedMultiplerValue;
            main.startSpeed = new ParticleSystem.MinMaxCurve(speed);
        }
    }

    private void UpgradeKnockback()
    {
        foreach(ParticleSystem shooter in shooters)
        {
            var collision = shooter.collision;
            knockback *= 1 + knockbackIncreasePercentage;
            collision.colliderForce = knockback;
        }
    }

    private void UpgradeFreeze()
    {
        //weapon.ReceiveConditionParameter(WeaponCondition.Freeze, freezeDurationIncrement);
    }

    public override void UnlockFirstRare()
    {
        caltrops.SetActive(true);
    }

    public override void UnlockSecondRare()
    {
        burst.SetActive(true);
    }

    private void Update()
    {
        if (!weapon.unlocked) return;
        damageUI.text = weapon.damageRange.x.ToString("0") + "-" + weapon.damageRange.y.ToString("0");
        speedUI.text = Mathf.Abs(speed).ToString("0.0");
        bulletUI.text = (bullet * 100).ToString("0") + "%";
        knockbackUI.text = (knockback / 10).ToString("0.00");
    }

    public override void DoPreview()
    {
        tab.GetComponent<EventTrigger>().OnPointerClick(new PointerEventData(EventSystem.current));
        button.GetComponent<EventTrigger>().OnPointerClick(new PointerEventData(EventSystem.current));
    }
}
