using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthModule : MonoBehaviour, IEnemyModule
{
    [SerializeField] private bool destroyOnDeath;

    private float maxHealth;
    public bool armoured;

    public EnemyStatusModule statusHandler { get; private set; }
    public Material materialInstance {get; private set;}
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
    [SerializeField] private Animator anim;

    public delegate void OnEnemyDeath(EnemyHealthModule healthModule, bool destroy);
    public OnEnemyDeath onEnemyDeath;
    public delegate void OnDamageTaken(float healthPercentage);
    public OnDamageTaken onDamageTaken;

    [Header("UI")]
    [SerializeField] private Transform healthBar;

    [Header("Death")]
    [SerializeField] private GameObject deathAnimation;
    [SerializeField] private List<EnemyDrop> drops;


    public void ReceiveValues(float MaxHealth, int Armor)
    {
        maxHealth = MaxHealth;
    }

    private void Start()
    {
        materialInstance = new Material(GetComponent<SpriteRenderer>().material);
        GetComponent<SpriteRenderer>().material = materialInstance;
        
        statusHandler = GetComponent<EnemyStatusModule>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;

        if(materialInstance != null) 
        {
            materialInstance.SetFloat("_Death", 0);
        }

        blinking = false;
        dead = false;
        foreach(var col in GetComponents<Collider2D>())
        {
            col.enabled = true;
        }
        GetComponent<SpriteRenderer>().color = Color.white;
        anim.SetTrigger("Reset");
        anim.speed = 1;

        //healthBar.transform.localScale = new Vector3(currentHealth / maxHealth, .1f, 1);

        foreach (var weapon in FindObjectsOfType<PiercingBulletWeapon>())
        {
            foreach(var col in GetComponents<Collider2D>())
            {
                col.enabled = true;
                weapon.ReceiveCollider(col);
            }
        }
        GetComponent<Rigidbody2D>().WakeUp();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (immune) return;
        if (other.TryGetComponent<WeaponDamageDealer>(out var damageDealer))
        {
            damageDealer.ApplyWeaponEffects(statusHandler);
            TakeDamage(damageDealer.Damage, damageDealer.damageMultiplier, damageDealer.bypassArmour);

            if (dead) return;
        }
    }

    public void TakeDamage(float damage, float damageMultiplier, bool bypassArmour = false)
    {
        if (dead) return;
        if(armoured && !bypassArmour) damage /= 2;
        damage *= 1 + exposedMultiplier;
        currentHealth -= damage;
        onDamageTaken?.Invoke(currentHealth / maxHealth);
        if (!blinking) StartCoroutine(Blink());

        if (currentHealth <= 0) StartCoroutine(Die());

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

    public IEnumerator Die()
    {
        dead = true;
        EndGameLog.Main.enemies++;
        statusHandler.Terminate();
        anim.speed = 0;
        foreach(var col in GetComponents<Collider2D>())
        {
            col.enabled = false;
        }
        GetComponent<Rigidbody2D>().Sleep();

        if (drops.Count > 0)
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

        materialInstance.SetFloat("_Damaged", 0);
        var step = 0f;

        while(step <= 1)
        {
            materialInstance.SetFloat("_Death", step);

            step += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }

        onEnemyDeath?.Invoke(this, destroyOnDeath);
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

        statusHandler.Terminate();
        onEnemyDeath?.Invoke(this, destroyOnDeath);
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