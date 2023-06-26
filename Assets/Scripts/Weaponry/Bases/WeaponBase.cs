using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Text.RegularExpressions;
using System;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite weaponSprite;
    public Sprite waveform;
    public Sprite classSprite;
    public Sprite sourceSprite;

    [Header("Stats")]
    public int weaponCost;
    public WeaponClass weaponClass;
    public WeaponSource weaponSource;
    public Vector2 damageRange;
    public float criticalChance = 0.05f;
    public float criticalMultiplier = 1.5f;


    [Header("Upgrades")]
    [SerializeField] protected List<string> upgradeDescriptions;
    public string nextLevelDescription => upgradeDescriptions[level - 1];

    [Header("References")]
    [SerializeField] protected ParticleSystem MainShooter;
    [field:SerializeField] public ActionTile tile { get; protected set; }
    [SerializeField] private DeathExperienceSpawn exp;

    [Header("Events")]
    public UnityEvent OnShoot;
    public UnityEvent OnStop;


    [Header("Internal")]
    private List<ActionTile> placedTiles = new List<ActionTile>();
    protected Animator anim;
    protected WeaponAudioController audioController;
    public WeaponGraphicsController graphicsController { get; protected set; }
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

        gameObject.SetActive(true);
        WeaponMasterController.Main.RegisterWeapon(this);
        GeneralStatRegistry.Main.totalWeapons++;
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
        if(shooting) OnStop?.Invoke();
        shooting = false;
    }

    public void Sell()
    {
        var count = Mathf.FloorToInt(ShopManager.Main.weaponCost / 2f);
        count += Mathf.CeilToInt(placedTiles.Count / 2f);

        var _exp = Instantiate(exp, transform.position, Quaternion.identity);
        _exp.Initialize(transform.position, count);

        placedTiles.ForEach(x => x.DestroyTile());

        Stop();

        Inventory.Main.AddWeapon(this);
        
        transform.position = Vector3.one * 1000;
        gameObject.SetActive(false);
        WeaponMasterController.Main.RemoveWeapon(this);
        GeneralStatRegistry.Main.totalWeapons--;
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
    
    // REMOVER ESTAS FUNÇÕES
    protected virtual void ApplyPerk()
    {

    }
    
    protected virtual void RemovePerk()
    {

    }
}

public enum WeaponSource 
{ 
    Default = 0,
    Projectile = 1,
    Laser = 2, 
    Flame = 4,
    Electric = 8,
}

public enum WeaponClass
{
    Default = 0,
    Strings = 1,
    Percussion = 2,
    Winds = 4
}
