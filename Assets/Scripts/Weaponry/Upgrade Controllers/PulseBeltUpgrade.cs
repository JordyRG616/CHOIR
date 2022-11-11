using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class PulseBeltUpgrade : WeaponUpgradeController
{
    [Header("Rate")]
    [SerializeField] private float rateIncrement;

    [Header("Size")]
    [SerializeField] private float sizeIncrement;

    [Header("Force")]
    [SerializeField] private float forceIncrement;

    [Header("Rare Upgrade")]
    [SerializeField] private GameObject electricDischarge;
    [SerializeField] private Color electricColor;
    [SerializeField] private GameObject blazeTrail;
    [SerializeField] private Material flameMaterial;

    [Header("UI")]
    [SerializeField] private GameObject tab;
    [SerializeField] private TextMeshProUGUI damageUI, rateUI, sizeUI, forceUI;
    private float rate, size, force;

    private void OnEnable()
    {
        if (!weapon.unlocked) return;
        tab.gameObject.SetActive(true);
        rate = shooters[0].emission.rateOverTime.constant;
        size = shooters[0].main.startSize.constant;
        force = shooters[0].externalForces.multiplier;
    }

    public override void ApplyEffect(int effectIndex)
    {
        switch (effectIndex)
        {
            case 0:
                UpgradeDamage();
                break;
            case 1:
                UpgradeRate();
                break;
            case 2:
                UpgradeSize();
                break;
            case 3:
                UpgradeForce();
            break;
        }

        level++;
        UnlockCard();
    }

    private void UpgradeForce()
    {
        foreach (ParticleSystem system in shooters)
        {
            var module = system.externalForces;
            force += forceIncrement;
            module.multiplier = force;
        }
    }

    private void UpgradeRate()
    {
        foreach (ParticleSystem system in shooters)
        {
            var emission = system.emission;
            rate += rateIncrement;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(rate);
        }
    }

    private void UpgradeSize()
    {
        foreach (ParticleSystem system in shooters)
        {
            var main = system.main;
            size += sizeIncrement;
            main.startSize = new ParticleSystem.MinMaxCurve(size);
        }
    }

    public override void UnlockFirstRare()
    {
        electricDischarge.SetActive(true);
        foreach (var shooter in shooters)
        {
            var main = shooter.main;
            main.startColor = electricColor;
        }

        WeaponMasterController.Main.RegisterWeaponClass(WeaponClass.Electric, weapon);
    }

    public override void UnlockSecondRare()
    {
        blazeTrail.SetActive(true);
        foreach (var shooter in shooters)
        {
            var renderer = shooter.GetComponent<ParticleSystemRenderer>();
            renderer.material = flameMaterial;
            renderer.trailMaterial = flameMaterial;
        }

        WeaponMasterController.Main.RegisterWeaponClass(WeaponClass.Flame, weapon);
    }

    private void Update()
    {
        if (!weapon.unlocked) return;
        damageUI.text = weapon.damageRange.x.ToString("0") + "-" + weapon.damageRange.y.ToString("0");
        rateUI.text = rate.ToString("0");
        sizeUI.text = size.ToString("0.0");
        forceUI.text = (force * 10).ToString("0.0");
    }

    public override void DoPreview()
    {
        tab.GetComponent<EventTrigger>().OnPointerClick(new PointerEventData(EventSystem.current));
    }
}
