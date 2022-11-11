using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class GroundLaserUpgrade : WeaponUpgradeController
{
    [SerializeField] private float lifetimeIncrement;
    [Space]
    [SerializeField] private float rateIncrement;
    [Space]
    [SerializeField] private float spreadIncrement;

    [Header("Rares")]
    [SerializeField] private GameObject plasmaLaser;
    [SerializeField] private GameObject omniBurst;

    [Header("UI")]
    [SerializeField] private GameObject tab, button;
    [SerializeField] private TextMeshProUGUI damageUI, rangeUI, rateUI, spreadUI;
    private float rate, lifetime, spread;

    private void OnEnable()
    {
        if (!weapon.unlocked) return;
        tab.gameObject.SetActive(true);
        button.gameObject.SetActive(true);
        rate = shooters[0].emission.rateOverTime.constant;
        lifetime = shooters[0].main.startLifetime.constant;
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
                UpgradeRate();
                break;
            case 2:
                UpgradeRange();
                break;
            case 3:
                UpgradeSpread();
                break;
        }

        level++;
        UnlockCard();
    }

    private void UpgradeRate()
    {
        foreach (ParticleSystem shooter in shooters)
        {
            var emission = shooter.emission;
            rate += rateIncrement;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(rate);
        }

    }

    private void UpgradeRange()
    {
        foreach (ParticleSystem shooter in shooters)
        {
            var main = shooter.main;
            lifetime *= (1 + lifetimeIncrement);
            main.startLifetime = new ParticleSystem.MinMaxCurve(lifetime);
        }
    }

    private void UpgradeSpread()
    {
        foreach (ParticleSystem shooter in shooters)
        {
            var velocity = shooter.velocityOverLifetime;
            spread += spreadIncrement;
            velocity.y = new ParticleSystem.MinMaxCurve(0, spread);
        }
    }

    public override void UnlockFirstRare()
    {
        omniBurst.SetActive(true);
    }

    public override void UnlockSecondRare()
    {
        plasmaLaser.SetActive(true);
    }

    private void Update()
    {
        if (!weapon.unlocked) return;
        damageUI.text = weapon.damageRange.x.ToString("0") + "-" + weapon.damageRange.y.ToString("0");
        rangeUI.text = (lifetime * 25).ToString("0.0");
        rateUI.text = rate.ToString("0.0");
    }

    public override void DoPreview()
    {
        tab.GetComponent<EventTrigger>().OnPointerClick(new PointerEventData(EventSystem.current));
        button.GetComponent<EventTrigger>().OnPointerClick(new PointerEventData(EventSystem.current));
    }
}
