using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ScarabUpgrade : WeaponUpgradeController
{
    [SerializeField] private List<FlameEffect> effects;

    [Header("Common Upgrades")]
    [SerializeField] private float rangeIncrement;
    [SerializeField] private float spreadIncrement;

    [Header("Uncommon Upgrades")]
    [SerializeField] private float densityIncrement;
    [SerializeField] private float burningIncrement;

    [Header("Rare Upgrades")]
    [SerializeField] private GameObject emberSpore;
    [SerializeField] private GameObject flameWall;

    [Header("UI")]
    [SerializeField] private GameObject tab, button;
    [SerializeField] private TextMeshProUGUI damageUI, rangeUI, densityUI, spreadUI, burnUI;
    private float range, spread, density, burning;


    private void OnEnable()
    {
        if (!weapon.unlocked) return;
        tab.gameObject.SetActive(true);
        button.gameObject.SetActive(true);
        range = shooters[0].main.startSpeed.constant;
        spread = shooters[0].velocityOverLifetime.yMultiplier;
        density = shooters[0].emission.rateOverTime.constant;
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
                UpgradeSpread();
                break;
            case 3:
                UpgradeDensity();
                break;
            case 4:
                UpgradeBurning();
                break;
        }

        level++;
        UnlockCard();
    }

    private void UpgradeRange()
    {
        foreach (ParticleSystem shooter in shooters)
        {
            var main = shooter.main;
            range += rangeIncrement;
            main.startSpeed = new ParticleSystem.MinMaxCurve(range);
        }
    }

    private void UpgradeSpread()
    {
        foreach (ParticleSystem shooter in shooters)
        {
            var velocity = shooter.velocityOverLifetime;
            spread += spreadIncrement;
            velocity.yMultiplier = spread;
        }
    }

    private void UpgradeDensity()
    {
        foreach (ParticleSystem shooter in shooters)
        {
            var emission = shooter.emission;
            density += densityIncrement;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(density);
        }
    }

    private void UpgradeBurning()
    {
        effects.ForEach(x => x.RaiseDamage(burningIncrement));
        burning = effects[0].RaiseDamage(0);
    }

    public override void UnlockFirstRare()
    {
        emberSpore.SetActive(true);
    }

    public override void UnlockSecondRare()
    {
        flameWall.SetActive(true);
    }

    private void Update()
    {
        if (!weapon.unlocked) return;
        damageUI.text = weapon.damageRange.x.ToString("0") + "-" + weapon.damageRange.y.ToString("0");
        rangeUI.text = range.ToString("0");
        spreadUI.text = spread.ToString("0.0");
        densityUI.text = density.ToString("0");
        burnUI.text = effects[0].GetDPS().ToString("0.0");
    }

    public override void DoPreview()
    {
        tab.GetComponent<EventTrigger>().OnPointerClick(new PointerEventData(EventSystem.current));
        button.GetComponent<EventTrigger>().OnPointerClick(new PointerEventData(EventSystem.current));
    }
}
