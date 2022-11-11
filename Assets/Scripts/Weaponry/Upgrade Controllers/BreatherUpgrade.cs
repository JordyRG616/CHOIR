using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class BreatherUpgrade : WeaponUpgradeController
{
    [SerializeField] private List<ParticleSystem> subSystems;

    [Header("Trail Lifetime")]
    [SerializeField] private float lifetimeIncrement;

    [Header("Density")]
    [SerializeField] private float densityIncrement;

    [Header("Uncommon Upgrade")]
    [SerializeField] private float rangeIncrement;

    [Header("Rare Upgrade")]
    [SerializeField] private GameObject ashRain;

    [Header("UI")]
    [SerializeField] private GameObject tab, button;
    [SerializeField] private TextMeshProUGUI damageUI, lifetimeUI, densityUI, rangeUI;
    private float lifetime, density, range;

    private void OnEnable()
    {
        if (!weapon.unlocked) return;
        tab.gameObject.SetActive(true);
        button.gameObject.SetActive(true);
        lifetime = subSystems[0].main.startLifetime.constant;
        density = subSystems[0].emission.GetBurst(0).count.constant;
        range = shooters[0].main.startSpeed.constant;
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
                UpgradeDensity();
                break;
            case 3:
                UpgradeRange();
                break;
        }

        level++;
        UnlockCard();
    }

    private void UpgradeLifetime()
    {
        foreach (ParticleSystem shooter in subSystems)
        {
            var main = shooter.main;
            lifetime *= (1 + lifetimeIncrement);
            main.startLifetime = new ParticleSystem.MinMaxCurve(lifetime);
        }
    }

    private void UpgradeDensity()
    {
        foreach (ParticleSystem shooter in subSystems)
        {
            var emission = shooter.emission;
            var burst = emission.GetBurst(0);
            density += densityIncrement;
            burst.count = new ParticleSystem.MinMaxCurve(density);
        }
    }

    private void UpgradeRange()
    {
        foreach (ParticleSystem shooter in shooters)
        {
            var main = shooter.main;
            range *= (1 + rangeIncrement);
            main.startSpeed = new ParticleSystem.MinMaxCurve(range);
        }
    }

    public override void UnlockFirstRare()
    {
        ashRain.SetActive(true);
    }

    public override void UnlockSecondRare()
    {
        foreach (var shooter in shooters)
        {
            var limit = shooter.limitVelocityOverLifetime;
            var coll = shooter.collision;
            limit.enabled = false;
            coll.enabled = true;
        }
    }

    private void Update()
    {
        if (!weapon.unlocked) return;
        damageUI.text = weapon.damageRange.x.ToString("0") + "-" + weapon.damageRange.y.ToString("0");
        lifetimeUI.text = lifetime.ToString("0.00");
        densityUI.text = density.ToString("0");
        rangeUI.text = range.ToString("0.0");
    }

    public override void DoPreview()
    {
        tab.GetComponent<EventTrigger>().OnPointerClick(new PointerEventData(EventSystem.current));
        button.GetComponent<EventTrigger>().OnPointerClick(new PointerEventData(EventSystem.current));
    }
}
