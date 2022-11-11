using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ThundercasterUpgrade : WeaponUpgradeController
{
    [SerializeField] private ParticleSystem rays;
    [SerializeField] private List<ElectricEffect> effects;

    [Header("Common Upgrades")]
    [SerializeField] private float propagationSpeedIncrement;
    [SerializeField] private float shockIncrement;

    [Header("Uncommon Upgrades")]
    [SerializeField] private float densityIncrement;
    [SerializeField] private float durationIncrement;

    [Header("Rare Upgrades")]
    [SerializeField] private GameObject thunderFall;
    [SerializeField] private GameObject multiOrb;

    [Header("UI")]
    [SerializeField] private GameObject tab, button;
    [SerializeField] private TextMeshProUGUI damageUI, speedUI, densityUI, durationUI, shockUI;
    private float speed, density, duration, shock;


    private void OnEnable()
    {
        if (!weapon.unlocked) return;
        tab.gameObject.SetActive(true);
        button.gameObject.SetActive(true);
        speed = shooters[0].main.startSpeed.constant;
        density = rays.emission.rateOverDistance.constant;
        duration = rays.main.startLifetime.constant;
        shock = effects[0].RaiseDuration(0);
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
                UpgradeShock();
                break;
            case 3:
                UpgradeDensity();
                break;
            case 4:
                UpgradeDuration();
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
            speed *= 1 + propagationSpeedIncrement;
            main.startSpeed = new ParticleSystem.MinMaxCurve(speed);
        }
    }

    private void UpgradeShock()
    {
        effects.ForEach(x => x.RaiseDuration(shockIncrement));
        shock = effects[0].RaiseDuration(0);
    }

    private void UpgradeDensity()
    {
        var emission = rays.emission;
        density += densityIncrement;
        emission.rateOverDistance = new ParticleSystem.MinMaxCurve(density);
    }

    private void UpgradeDuration()
    {
        var main = rays.main;
        duration *= 1 + durationIncrement;
        main.startLifetime = new ParticleSystem.MinMaxCurve(duration);
    }

    public override void UnlockFirstRare()
    {
        thunderFall.SetActive(true);
    }

    public override void UnlockSecondRare()
    {
        multiOrb.SetActive(true);
        foreach (var shooter in shooters)
        {
            var main = shooter.main;
            main.startLifetime = .75f;
        }
    }

    private void Update()
    {
        if (!weapon.unlocked) return;
        damageUI.text = weapon.damageRange.x.ToString("0") + "-" + weapon.damageRange.y.ToString("0");
        speedUI.text = speed.ToString("0.0");
        densityUI.text = density.ToString("0");
        durationUI.text = duration.ToString("0.0");
        shockUI.text = shock.ToString("0.00");
    }

    public override void DoPreview()
    {
        tab.GetComponent<EventTrigger>().OnPointerClick(new PointerEventData(EventSystem.current));
        button.GetComponent<EventTrigger>().OnPointerClick(new PointerEventData(EventSystem.current));
    }
}
