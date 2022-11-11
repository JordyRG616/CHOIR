using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class WallTurretUpgrade : WeaponUpgradeController
{
    [SerializeField] private List<ParticleSystem> subSystems;

    [Header("Lifetime")]
    [SerializeField] private float lifetimeIncrement;

    [Header("Fragments")]
    [SerializeField] private float fragmentCount;

    [Header("Uncommon Upgrades")]
    [SerializeField] private float spreadIncrement;

    [Header("Rare Upgrades")]
    [SerializeField] private List<GameObject> electricDischarges;
    [SerializeField] private Color electricColor;

    [Header("UI")]
    [SerializeField] private GameObject tab, button;
    [SerializeField] private TextMeshProUGUI damageUI, lifetimeUI, fragmentsUI;
    private float lifetime, fragments, spread;

    private void OnEnable()
    {
        if (!weapon.unlocked) return;
        tab.gameObject.SetActive(true);
        button.gameObject.SetActive(true);
        lifetime = shooters[0].main.startLifetime.constant;
        fragments = subSystems[0].emission.GetBurst(0).count.constant;
        spread = 5;
    }

    public override void ApplyEffect(int effectIndex)
    {
        switch (effectIndex)
        {
            case 0:
                UpgradeDamage();
                break;
            case 1:
                UpgradeLifetime();
                break;
            case 2:
                UpgradeFragments();
                break;
            case 3:
                UpgradeSpread();
                break;
        }

        level++;
        UnlockCard();
    }

    private void UpgradeLifetime()
    {
        foreach (ParticleSystem shooter in shooters)
        {
            var main = shooter.main;
            lifetime *= (1 + lifetimeIncrement);
            main.startLifetime = new ParticleSystem.MinMaxCurve(lifetime);
        }
    }

    private void UpgradeFragments()
    {
        foreach (ParticleSystem shooter in subSystems)
        {
            var emission = shooter.emission;
            var burst = emission.GetBurst(0);
            fragments += fragmentCount;
            burst.count = new ParticleSystem.MinMaxCurve(fragments);
            emission.SetBurst(0, burst);
        }
    }

    private void UpgradeSpread()
    {
        foreach (ParticleSystem shooter in subSystems)
        {
            var velocity = shooter.velocityOverLifetime;
            spread += spreadIncrement;
            velocity.yMultiplier = spread;
        }
    }

    public override void UnlockFirstRare()
    {
        electricDischarges.ForEach(x => x.SetActive(true));
        foreach (ParticleSystem shooter in subSystems)
        {
            var main = shooter.main;
            main.startColor = electricColor;
        }

        WeaponMasterController.Main.RegisterWeaponClass(WeaponClass.Electric, weapon);
    }

    public override void UnlockSecondRare()
    {
        foreach (var shooter in shooters)
        {
            var renderer = shooter.GetComponent<ParticleSystemRenderer>();
            renderer.renderMode = ParticleSystemRenderMode.Billboard;
            var trigger = shooter.trigger;
            trigger.enabled = true;
        }
    }

    private void Update()
    {
        if (!weapon.unlocked) return;
        damageUI.text = weapon.damageRange.x.ToString("0") + "-" + weapon.damageRange.y.ToString("0");
        lifetimeUI.text = spread.ToString("0");
        fragmentsUI.text = fragments.ToString("0");
    }

    public override void DoPreview()
    {
        tab.GetComponent<EventTrigger>().OnPointerClick(new PointerEventData(EventSystem.current));
        button.GetComponent<EventTrigger>().OnPointerClick(new PointerEventData(EventSystem.current));
    }
}
