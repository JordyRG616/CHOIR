using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class CannonUpgrade : WeaponUpgradeController
{
    [SerializeField] private ParticleSystem explosion;
    [Header("Blast Size")]
    [Range(0, 1)] [SerializeField] private float blastIncrement;
    [Header("Extra Bullet Chance")]
    [SerializeField] private float chanceIncrement;
    [Header("Extra Blast Chance")]
    [SerializeField] private float blastChanceIncrement;

    [Header("Rare Upgrade")]
    [SerializeField] private GameObject firePilar, vortex;

    [Header("UI")]
    [SerializeField] private GameObject tab, button;
    [SerializeField] private TextMeshProUGUI damageUI, sizeUI, bulletUI, blastUI;
    private float size, bullet, blast;


    private void OnEnable()
    {
        if (!weapon.unlocked) return;
        tab.gameObject.SetActive(true);
        button.gameObject.SetActive(true);
        size = explosion.main.startSpeed.constant;
        bullet = 0;
        blast = 0;
    }

    public override void ApplyEffect(int effectIndex)
    {
        switch (effectIndex)
        {
            case 0:
                UpgradeDamage();
                break;
            case 1:
                UpgradeBlastSize();
                break;
            case 2:
                UpgradeExtraBulletChance();
                break;
            case 3:
                UpgradeExtraBlastChance();
                break;
        }

        level++;
        UnlockCard();
    }

    private void UpgradeBlastSize()
    {
        var main = explosion.main;
        size *= 1 + blastIncrement;
        main.startSpeed = new ParticleSystem.MinMaxCurve(size);
    }

    private void UpgradeExtraBulletChance()
    {
        foreach(ParticleSystem shooter in shooters)
        {
            var emission = shooter.emission;
            var burst = emission.GetBurst(1);
            bullet += chanceIncrement;
            burst.probability = bullet;
            emission.SetBurst(1, burst);
        }
    }

    private void UpgradeExtraBlastChance()
    {
            var sub = explosion.subEmitters;
            blast += blastChanceIncrement;
            sub.SetSubEmitterEmitProbability(0, blast);
    }

    public override void UnlockFirstRare()
    {
        firePilar.SetActive(true);
    }

    public override void UnlockSecondRare()
    {
        vortex.SetActive(true);
    }

    private void Update()
    {
        if (!weapon.unlocked) return;
        damageUI.text = weapon.damageRange.x.ToString("0") + "-" + weapon.damageRange.y.ToString("0");
        blastUI.text = (blast * 100).ToString("0") + "%";
        bulletUI.text = (bullet * 100).ToString("0") + "%";
        sizeUI.text = size.ToString("0.0");
    }

    public override void DoPreview()
    {
        tab.GetComponent<EventTrigger>().OnPointerClick(new PointerEventData(EventSystem.current));
        button.GetComponent<EventTrigger>().OnPointerClick(new PointerEventData(EventSystem.current));
    }
}
