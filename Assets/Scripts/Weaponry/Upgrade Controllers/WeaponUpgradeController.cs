using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponUpgradeController : MonoBehaviour
{
    [SerializeField] protected List<ParticleSystem> shooters;
    protected WeaponBase weapon;

    [Header("Uncommom Cards")]
    [SerializeField] protected List<UncommonCardData> uncommomCards;

    [Header("Damage Upgrade")]
    [SerializeField] protected float damageValue;
    [SerializeField] protected bool min, max;

    [Header("Class Icons")]
    [SerializeField] private GameObject bulletIcon;
    [SerializeField] private GameObject laserIcon;
    [SerializeField] private GameObject flameIcon;
    [SerializeField] private GameObject electricIcon;

    private int _level;

    protected int level
    {
        get => _level;
        set
        {
            _level = value;
            OnLevelUp?.Invoke(weapon.ID);
        }
    }

    public delegate void LevelUpEvent(int weaponID);
    public LevelUpEvent OnLevelUp;

    protected virtual void Awake()
    {
        weapon = GetComponent<WeaponBase>();
    }

    protected void UpgradeDamage()
    {
        if (min) weapon.damageRange.x += damageValue;
        if (max) weapon.damageRange.y += damageValue;
    }

    public abstract void ApplyEffect(int effectIndex);

    protected virtual void UnlockCard()
    {
        if (uncommomCards.Count == 0) return;

        foreach (var card in uncommomCards)
        {
            if (level == card.levelToUnlock)
            {
                RewardManager.Main.ReceiveCard(card.card);
            }
        }
    }

    public virtual void UnlockFirstRare() { }
    public virtual void UnlockSecondRare() { }

    public abstract void DoPreview();

    public virtual void ActivatePreview()
    {
        SetWeaponClasses();
        DoPreview();
    }

    public void SetWeaponClasses()
    {
        bulletIcon.SetActive(false);
        laserIcon.SetActive(false);
        flameIcon.SetActive(false);
        electricIcon.SetActive(false);

        if (weapon.classes.HasFlag(WeaponClass.Ballistic)) bulletIcon.SetActive(true);
        if (weapon.classes.HasFlag(WeaponClass.Laser)) laserIcon.SetActive(true);
        if (weapon.classes.HasFlag(WeaponClass.Flame)) flameIcon.SetActive(true);
        if (weapon.classes.HasFlag(WeaponClass.Electric)) electricIcon.SetActive(true);
    }
}

[System.Serializable]
public struct UncommonCardData
{
    public int levelToUnlock;
    public WeaponCard card;
}