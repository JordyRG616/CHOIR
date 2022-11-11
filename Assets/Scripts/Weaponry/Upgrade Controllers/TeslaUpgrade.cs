using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TeslaUpgrade : WeaponUpgradeController
{
    [Header("Range Upgrade")]
    [Range(0, 1)] [SerializeField] private float rangeIncrement;
    [Header("Duration Upgrade")]
    [Range(0, 1)] [SerializeField] private float durationIncrement;
    [Header("Ray Upgrade")]
    [SerializeField] private int raysIncrement;

    [Header("Rare Upgrade")]
    [SerializeField] private GameObject electricCage;
    [SerializeField] private List<GameObject> thunderbolts;

    [Header("UI")]
    [SerializeField] private GameObject tab;
    [SerializeField] private TextMeshProUGUI damageUI, rangeUI, durationUI, countUI;
    private float range, count;

    private void OnEnable()
    {
        if (!weapon.unlocked) return;
        tab.gameObject.SetActive(true);
        range = shooters[0].shape.radius;
        count = shooters[0].emission.GetBurst(0).count.constant;
    }

    public override void ApplyEffect(int effectIndex)
    {
        switch (effectIndex)
        {
            case 0:
                UpgradeDamage();
                break;
            case 1:
                UpgradeRange();
                break;
            case 2:
                UpgradeDuration();
                break;
            case 3:
                UpgradeCount();
                break;
        }

        level++;
        UnlockCard();
    }

    private void UpgradeRange()
    {
        foreach (var shooter in shooters)
        {
            var shape = shooter.shape;
            range *= 1 + rangeIncrement;
            shape.radius = range;
        }
    }

    private void UpgradeDuration()
    {
        foreach (var shooter in shooters)
        {
            var main = shooter.main;
            var min = main.startLifetime.constantMin + durationIncrement;
            var max = main.startLifetime.constantMax + durationIncrement;
            main.startLifetime = new ParticleSystem.MinMaxCurve(min, max);
        }
    }

    private void UpgradeCount()
    {
        foreach (var shooter in shooters)
        {
            var emission = shooter.emission;
            var burst = emission.GetBurst(0);
            burst.count = new ParticleSystem.MinMaxCurve(burst.count.constant + raysIncrement);
            emission.SetBurst(0, burst);
            count += raysIncrement;
        }
    }

    public override void UnlockFirstRare()
    {
        electricCage.SetActive(true);
    }

    public override void UnlockSecondRare()
    {
        thunderbolts.ForEach(x => x.SetActive(true));
    }

    private void Update()
    {
        if (!weapon.unlocked) return;
        damageUI.text = weapon.damageRange.x.ToString("0") + "-" + weapon.damageRange.y.ToString("0");
        rangeUI.text = range.ToString("0.0");
        countUI.text = count.ToString("0");
        durationUI.text = shooters[0].main.startLifetime.constantMin.ToString("0.0") + "-" + shooters[0].main.startLifetime.constantMax.ToString("0.0");
    }

    public override void DoPreview()
    {
        tab.GetComponent<EventTrigger>().OnPointerClick(new PointerEventData(EventSystem.current));
    }
}
