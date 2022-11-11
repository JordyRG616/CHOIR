using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class CeilingTurretUpgrade : WeaponUpgradeController
{
    [Header("Speed Upgrade")]
    [SerializeField] private float bulletSpeedIncrement;
    [Range(0, 1)] [SerializeField] private float rotationSpeed;

    [Header("Uncommon Upgrades")]
    [SerializeField] private float bulletSizeIncrement;

    [Header("Rare Upgrade")]
    [Space]
    [SerializeField] private GameObject pulseBlastOne, pulseBlastTwo, secondShooter;

    [Header("UI")]
    [SerializeField] private GameObject tab;
    [SerializeField] private TextMeshProUGUI damageUI, bulletUI, rotationUI;
    private float bulletSpeed, count, speed, size;

    private void OnEnable()
    {
        if (!weapon.unlocked) return;
        tab.gameObject.SetActive(true);
        bulletSpeed = shooters[0].main.startSpeed.constant;
        count = shooters[0].emission.GetBurst(0).count.constant;
        size = shooters[0].main.startSize.constant;
        speed = 1;
    }

    public override void ApplyEffect(int effectIndex)
    {
        switch (effectIndex)
        {
            case 0:
                UpgradeDamage();
                break;
            case 1:
                UpgradeBulletSpeed();
                break;
            case 2:
                UpgradeRotationSpeed();
                break;
            case 3:
                UpgradeSize();
                break;
        }

        level++;
        UnlockCard();
    }

    private void UpgradeBulletSpeed()
    {
        foreach (ParticleSystem shooter in shooters)
        {
            var main = shooter.main;
            bulletSpeed += bulletSpeedIncrement;
            main.startSpeed = new ParticleSystem.MinMaxCurve(bulletSpeed);
        }
    }

    private void UpgradeRotationSpeed()
    {
        var anim = weapon.GetComponent<Animator>();
        speed *= 1 + rotationSpeed;
        anim.speed = speed;
    }

    private void UpgradeSize()
    {
        foreach (ParticleSystem shooter in shooters)
        {
            var main = shooter.main;
            size += bulletSizeIncrement;
            main.startSize = new ParticleSystem.MinMaxCurve(size);
        }
    }

    public override void UnlockFirstRare()
    {
        pulseBlastOne.SetActive(true);
        pulseBlastTwo.SetActive(true);
    }

    public override void UnlockSecondRare()
    {
        secondShooter.SetActive(true);
    }

    private void Update()
    {
        if (!weapon.unlocked) return;
        damageUI.text = weapon.damageRange.x.ToString("0") + "-" + weapon.damageRange.y.ToString("0");
        bulletUI.text = Mathf.Abs(bulletSpeed).ToString("0");
        rotationUI.text = (speed * 100).ToString("0") + "%";
    }

    public override void DoPreview()
    {
        tab.GetComponent<EventTrigger>().OnPointerClick(new PointerEventData(EventSystem.current));
    }
}
