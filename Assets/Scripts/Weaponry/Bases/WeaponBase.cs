using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Text.RegularExpressions;
using System;

public abstract class WeaponBase : MonoBehaviour
{
    public Sprite weaponSprite;
    public WeaponClass weaponClass;
    [field:SerializeField] public WeaponClass perkReqClass { get; private set; }
    [field:SerializeField] public int perkReqAmount { get; private set; }
    [TextArea] public string ClassPerkDesc;
    [SerializeField] protected ParticleSystem MainShooter;
    [field:SerializeField] public ActionTile tile { get; protected set; }
    [field:SerializeField] public ActionTile AltTile { get; protected set; }

    [Space]
    [SerializeField] public UnityEvent OnShoot, OnStop;
    public Vector2 damageRange;
    [SerializeField] private GameObject exp;
    protected Animator anim;
    protected WeaponAudioController audioController;
    public WeaponGraphicsController graphicsController { get; protected set; }
    public bool unlocked = false;
    private WeaponClassInfo perkInfo;
    private bool perkApplied;
    public int level = 0;

    public delegate void SellEvent();
    public SellEvent OnSell;

    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        audioController = GetComponent<WeaponAudioController>();
        graphicsController = GetComponent<WeaponGraphicsController>();
    }

    public virtual void Set()
    {
        WeaponMasterController.Main.RegisterWeaponClass(weaponClass, perkReqClass, out perkInfo);
        perkInfo.OnLevelChange += HandlePerk;
    }

    protected virtual void HandlePerk(int currentLevel)
    {
        if(currentLevel >= perkReqAmount && !perkApplied) 
        {
            ApplyPerk();
            perkApplied = true;
        }

        if(currentLevel < perkReqAmount && perkApplied) 
        {
            RemovePerk();
            perkApplied = false;
        }
    }

    protected virtual void UnlockAltTile()
    {
        
    }

    public virtual void Shoot(WeaponKey key)
    {
        audioController.ChangeKey(key);
        OnShoot?.Invoke();
        anim.SetTrigger("Shoot");
    }

    public virtual void Stop()
    {
        OnStop?.Invoke();
    }

    public void Sell()
    {
        var count = Mathf.FloorToInt(ShopManager.Main.currentWeaponCost / 2f);

        for (int i = 0; i < count; i++)
        {
            Instantiate(exp, transform.position, Quaternion.identity);
        }

        OnSell?.Invoke();
        WeaponMasterController.Main.RemoveWeaponClass(weaponClass);
        perkInfo.OnLevelChange += HandlePerk;
        Stop();
        Destroy(gameObject);
    }


    public abstract void LevelUp();

    public abstract string WeaponDescription();
    

    protected abstract void ApplyPerk();
    
    protected abstract void RemovePerk();
}

public enum WeaponClass 
{ 
    Default = 0,
    Projectile = 1,
    Laser = 2, 
    Flame = 4,
    Electric = 8,
    Nuclear = 16
}
