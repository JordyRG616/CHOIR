using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class BeamUpgrade : WeaponUpgradeController
{
    [Header("Size Upgrade")]
    [Range(0, 1)] [SerializeField] private float sizeIncrement;
    [Header("Speed Upgrade")]
    [Range(0, 1)] [SerializeField] private float speedIncrement;

    [Header("Uncommon Upgrades")]
    [SerializeField] private float spreadIncrement;

    [Header("Rare Upgrade")]
    [SerializeField] private GameObject magmaTrail;
    [SerializeField] private Color magmaColor;
    [SerializeField] private ParticleSystem impact;
    [SerializeField] private GameObject fountain;

    [Header("UI")]
    [SerializeField] private GameObject tab, button;
    [SerializeField] private TextMeshProUGUI damageUI, sizeUI, speedUI;
    private float size, speed, spread;

    private void OnEnable()
    {
        if (!weapon.unlocked) return;
        tab.gameObject.SetActive(true);
        button.gameObject.SetActive(true);
        size = 1;
        spread = 0;
    }

    public override void ApplyEffect(int effectIndex)
    {
        switch (effectIndex)
        {
            case 0:
                UpgradeDamage();
                break;
            case 1:
                UpgradeBeamSize();
                break;
            case 2:
                UpgradeTraversalSpeed();
                break;
            case 3:
                UpgradeSpread();
                break;
        }

        level++;
        UnlockCard();
    }

    private void UpgradeTraversalSpeed()
    {
        var march = weapon.GetComponent<WeaponMarch>();
        march.speedModifier *= 1 + speedIncrement;
    }

    private void UpgradeBeamSize()
    {
        foreach (ParticleSystem shooter in shooters)
        {
            var main = shooter.main;
            size *= 1 + sizeIncrement;
            main.startSizeMultiplier = size;
        }
    }

    private void UpgradeSpread()
    {
        foreach (ParticleSystem shooter in shooters)
        {
            var velocity = shooter.velocityOverLifetime;
            spread += spreadIncrement;
            velocity.xMultiplier = spread + 1;
        }
    }

    public override void UnlockFirstRare()
    {
        magmaTrail.SetActive(true);
        foreach (var shooter in shooters)
        {
            var main = shooter.main;
            main.startColor = magmaColor;
        }

        var _main = impact.main;
        _main.startColor = magmaColor;
        WeaponMasterController.Main.RegisterWeaponClass(WeaponClass.Flame, weapon);
    }

    public override void UnlockSecondRare()
    {
        fountain.SetActive(true);
    }

    private void Update()
    {
        if (!weapon.unlocked) return;
        damageUI.text = weapon.damageRange.x.ToString("0.0") + "-" + weapon.damageRange.y.ToString("0.0");
        sizeUI.text = size.ToString("0.0");
        var march = weapon.GetComponent<WeaponMarch>();
        //speedUI.text = march.speedModifier.ToString("0.0");
    }

    public override void DoPreview()
    { 
        tab.GetComponent<EventTrigger>().OnPointerClick(new PointerEventData(EventSystem.current));
        button.GetComponent<EventTrigger>().OnPointerClick(new PointerEventData(EventSystem.current));
    }
}
