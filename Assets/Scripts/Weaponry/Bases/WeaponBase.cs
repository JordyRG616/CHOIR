using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Text.RegularExpressions;
using System;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Workshop info")]
    public Sprite weaponSprite;
    public int weaponCost;
    public Sprite waveform;
    public WeaponClass weaponClass;
    public bool surfaceWeapon;

    [Space]
    [TextArea] public string ClassPerkDesc;
    [field:SerializeField] public WeaponClass perkReqClass { get; private set; }
    [field:SerializeField] public int perkReqAmount { get; private set; }

    [SerializeField] protected List<string> upgradeDescriptions;
    public string nextLevelDescription => upgradeDescriptions[level - 1];
    [HideInInspector] public bool perkApplied;
    private WeaponClassInfo perkInfo;

    [SerializeField] protected ParticleSystem MainShooter;
    [field:SerializeField] public ActionTile tile { get; protected set; }
    private List<ActionTile> placedTiles = new List<ActionTile>();

    [Space]
    [SerializeField] public UnityEvent OnShoot, OnStop;
    public Vector2 damageRange;
    public float criticalChance = 0.05f;
    public float criticalMultiplier = 1.5f;
    [SerializeField] private DeathExperienceSpawn exp;
    protected Animator anim;
    protected WeaponAudioController audioController;
    public WeaponGraphicsController graphicsController { get; protected set; }
    public bool unlocked = false;
    protected WeaponSlot slot;
    protected bool shooting;

    public int level { get; protected set; } = 1;
    public bool maxLevel => level == 5;


    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        audioController = GetComponent<WeaponAudioController>();
        graphicsController = GetComponent<WeaponGraphicsController>();
    }

    public virtual void Set(WeaponSlot slot, bool fullSet)
    {
        this.slot = slot;
        if(!fullSet) return;

        WeaponMasterController.Main.RegisterWeaponClass(weaponClass, perkReqClass, out perkInfo);
        perkInfo.OnLevelChange += HandlePerk;
        if(perkInfo.classLevel >= perkReqAmount && !perkApplied) 
        {
            ApplyPerk();
            perkApplied = true;
        }
        gameObject.SetActive(true);
    }

    protected virtual void HandlePerk(int currentLevel)
    {
        if(currentLevel >= perkReqAmount && !perkApplied) 
        {
            perkApplied = true;
            ApplyPerk();
        }

        if(currentLevel < perkReqAmount && perkApplied) 
        {
            perkApplied = false;
            RemovePerk();
        }
    }

    public void ReceiveTile(ActionTile tile)
    {
        placedTiles.Add(tile);
    }

    public virtual void Shoot(WeaponKey key)
    {
        // if(shooting) return;

        audioController.ChangeKey(key);
        OnShoot?.Invoke();
        anim.SetTrigger("Shoot");
        shooting = true;
        CrystalManager.Main.DoWaveformBurst();
    }

    [ContextMenu("Shoot")]
    public virtual void TestShoot()
    {
        OnShoot?.Invoke();
        anim.SetTrigger("Shoot");
    }

    public virtual void Stop()
    {
        shooting = false;
        OnStop?.Invoke();
    }

    public void Sell()
    {
        var count = Mathf.FloorToInt(ShopManager.Main.weaponCost / 2f);
        count += Mathf.CeilToInt(placedTiles.Count / 2f);

        var _exp = Instantiate(exp, transform.position, Quaternion.identity);
        _exp.Initialize(transform.position, count);

        placedTiles.ForEach(x => x.DestroyTile());

        WeaponMasterController.Main.RemoveWeaponClass(weaponClass);
        perkInfo.OnLevelChange -= HandlePerk;
        Stop();

        Inventory.Main.AddWeapon(this);
        
        transform.position = Vector3.one * 1000;
        gameObject.SetActive(false);
    }

    public virtual void LevelUp()
    {
        LevelUpEffect();
        EndGameLog.Main.weaponLevel++;

        if(ModuleActivationManager.Main.HasSpecialEffect(ModuleSpecialEffect.Cashback))
        {
            var _exp = Instantiate(exp, transform.position, Quaternion.identity);
            _exp.Initialize(transform.position, level);
        }
    }

    public abstract void LevelUpEffect();

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
