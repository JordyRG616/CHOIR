using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class HarpoonUpgrade : WeaponUpgradeController
{
    [SerializeField] private ParticleSystem staticModel;
    [SerializeField] private ElectricEffect electricEffect;
    [SerializeField] private SpawnObjectOnCollider spawner;

    [SerializeField] private float durationIncrement;
    [Space]
    [SerializeField] private float rateIncrement;

    [Header("Rares")]
    [SerializeField] private GameObject blast;
    [SerializeField] private GameObject trail;

    [Header("UI")]
    [SerializeField] private GameObject tab, button;
    [SerializeField] private TextMeshProUGUI damageUI, durationUI, rateUI;
    private float rate, duration;

    private void OnEnable()
    {
        if (!weapon.unlocked) return;
        tab.gameObject.SetActive(true);
        button.gameObject.SetActive(true);
        rate = staticModel.emission.rateOverTime.constant;
        duration = electricEffect.RaiseDuration(0);
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
                UpgradeDuration();
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
        var emission = staticModel.emission;
        rate += rateIncrement;
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(rate);
    }

    private void UpgradeDuration()
    {
        duration = electricEffect.RaiseDuration(durationIncrement);
        spawner.RaiseDuration(durationIncrement);
    }

    private void UpgradeSpread()
    {
        
    }

    public override void UnlockFirstRare()
    {
        blast.SetActive(true);
    }

    public override void UnlockSecondRare()
    {
        trail.SetActive(true);
    }

    private void Update()
    {
        if (!weapon.unlocked) return;
        damageUI.text = weapon.damageRange.x.ToString("0") + "-" + weapon.damageRange.y.ToString("0");
        durationUI.text = duration.ToString("0.00");
        rateUI.text = rate.ToString("0");
    }

    public override void DoPreview()
    {
        tab.GetComponent<EventTrigger>().OnPointerClick(new PointerEventData(EventSystem.current));
        button.GetComponent<EventTrigger>().OnPointerClick(new PointerEventData(EventSystem.current));
    }
}
