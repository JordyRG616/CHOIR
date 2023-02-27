using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthModule : MonoBehaviour, IEnemyModule
{
    [SerializeField] private bool destroyOnDeath;

    private float maxHealth;
    private int armor;

    public EnemyStatusModule statusHandler { get; private set; }
    private Material materialInstance;
    private float _health;
    private float currentHealth
    {
        get => _health;
        set
        {
            if (value < 0) _health = 0;
            else if (value > maxHealth) _health = maxHealth;
            else _health = value;
        }
    }

    private bool dead = false;
    private bool blinking;
    public bool triggerCooldown;
    private WaitForSeconds triggerCooldownTime = new WaitForSeconds(0.01f);
    public bool immune;
    public float exposedMultiplier;

    public delegate void OnEnemyDeath(EnemyHealthModule healthModule, bool destroy);
    public OnEnemyDeath onEnemyDeath;
    public delegate void OnDamageTaken(float healthPercentage);
    public OnDamageTaken onDamageTaken;

    [Header("UI")]
    [SerializeField] private Transform healthBar;
    [SerializeField] private TMPro.TextMeshPro armorValue;

    [Header("Death")]
    [SerializeField] private GameObject deathAnimation;
    [SerializeField] private List<EnemyDrop> drops;


    public void ReceiveValues(float MaxHealth, int Armor)
    {
        maxHealth = MaxHealth;
        armor = Armor;
        if(armor > 0)
        {
            armorValue.text = armor.ToString();
            armorValue.transform.parent.gameObject.SetActive(true);
        } else
        {
            armorValue.transform.parent.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        materialInstance = new Material(GetComponent<SpriteRenderer>().material);
        GetComponent<SpriteRenderer>().material = materialInstance;


        InitializeStatusHandling();
    }

    private void InitializeStatusHandling()
    {
        statusHandler = GetComponent<EnemyStatusModule>();
        
        statusHandler.GetResistance(StatusType.Frailty).ReceiveCallbacks(
            () => exposedMultiplier = WeaponMasterController.Main.exposedMultiplier,
            () => exposedMultiplier = 0);

        statusHandler.GetResistance(StatusType.Burn).ReceiveCallbacks(
            () => StartCoroutine(HandleBurn()),
            () => StopCoroutine(HandleBurn()));

    }

    private IEnumerator HandleBurn()
    {
        var damage = WeaponMasterController.Main.burnDPS * WeaponMasterController.Main.burnTick;
        var wait = new WaitForSeconds(WeaponMasterController.Main.burnTick);

        while (true)
        {
            TakeDamage(damage, 0);
            yield return wait;
        }
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;
        if(materialInstance != null) materialInstance.SetFloat("_Damaged", 0);
        blinking = false;
        dead = false;
        var coll = GetComponent<Collider2D>();
        if(coll) coll.enabled = true;
        GetComponent<SpriteRenderer>().color = Color.white;
        GetComponent<Animator>().SetTrigger("Reset");

        //healthBar.transform.localScale = new Vector3(currentHealth / maxHealth, .1f, 1);

        foreach (var weapon in FindObjectsOfType<PiercingBulletWeapon>())
        {
            weapon.ReceiveCollider(coll);
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (immune) return;
        if (other.TryGetComponent<WeaponDamageDealer>(out var damageDealer))
        {
            damageDealer.ApplyWeaponEffects(statusHandler);
            TakeDamage(damageDealer.Damage, damageDealer.damageMultiplier);

            if (dead) return;
        }
    }

    public void TakeDamage(float damage, float damageMultiplier)
    {
        if (dead) return;
        damage -= (armor * damageMultiplier);
        damage *= 1 + exposedMultiplier;
        currentHealth -= damage;
        onDamageTaken?.Invoke(currentHealth / maxHealth);
        if (!blinking) StartCoroutine(Blink());

        if (currentHealth <= 0) Die(true);

        if (!healthBar.gameObject.activeSelf) healthBar.gameObject.SetActive(true);
        healthBar.localScale = new Vector3(currentHealth / maxHealth, .1f, 1);
    }

    private IEnumerator Blink()
    {
        materialInstance.SetFloat("_Damaged", 1);
        blinking = true;

        yield return new WaitForSeconds(0.1f);

        materialInstance.SetFloat("_Damaged", 0);
        blinking = false;
    }

    public void SetTriggerCooldown()
    {
        if (dead) return;
        StartCoroutine(HandleTriggerCooldown());
    }

    private IEnumerator HandleTriggerCooldown()
    {
        yield return triggerCooldownTime;
        triggerCooldown = false;
    }

    public void Die(bool exp)
    {
        StopAllCoroutines();
        dead = true;
        EndGameLog.Main.enemies++;

        if (exp && drops.Count > 0)
        {
            var rdm = UnityEngine.Random.Range(0, 2f);
            Instantiate(drops[0].obj, transform.position + new Vector3(0, 0, 75), Quaternion.identity);

            GameObject container = null;
            for (int i = 1; i < drops.Count; i++)
            {
                var drop = drops[i];
                if (rdm <= drop.chance) container = drop.obj;
            }
            if(container != null) Instantiate(container, transform.position + new Vector3(0, 0, 75), Quaternion.identity);
        }

        if (exp && deathAnimation != null) TriggerDeathAnimation();
        onEnemyDeath?.Invoke(this, destroyOnDeath);
    }

    private void TriggerDeathAnimation()
    {
        string _t;

        if (exposedMultiplier > 0) _t = "Laser";
        else if (GetComponent<EnemyMarchModule>().frozen) _t = "Elec";
        else _t = "Pop";

        var container = Instantiate(deathAnimation, transform.position, Quaternion.identity);
        container.GetComponent<Animator>().SetTrigger(_t);
        container.transform.localScale = transform.localScale;
    }

    private void OnDisable()
    {
        foreach (var weapon in FindObjectsOfType<PiercingBulletWeapon>())
        {
            weapon.RemoveCollider(GetComponent<Collider2D>());
        }
    }

    public void RaiseMaxHealth(float value)
    {
        maxHealth += value;
        currentHealth = maxHealth;
    }
}

[Serializable]
public class EnemyDrop
{
    public GameObject obj;
    [Range(0, 1)] public float chance;
}