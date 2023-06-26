using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthModule : MonoBehaviour, IEnemyModule
{
    [SerializeField] private Material enemyMat;
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
    public bool immune;
    public float exposedMultiplier;
    [SerializeField] private Animator anim;

    public delegate void OnEnemyDeath(EnemyHealthModule healthModule, bool destroy);
    public OnEnemyDeath onEnemyDeath;
    public delegate void OnDamageTaken(int damagetaken, bool crit);
    public OnDamageTaken onDamageTaken;

    [Header("UI")]
    [SerializeField] private Transform healthBar;

    [Header("Drops")]
    [SerializeField] private float expMultiplier = 1;
    [SerializeField] private DeathExperienceSpawn experience;
    [SerializeField] private GameObject explosion;


    public void ReceiveValues(float MaxHealth, int Armor)
    {
        maxHealth = MaxHealth;
    }

    private void Start()
    {
        materialInstance = new Material(enemyMat);
        GetComponent<SpriteRenderer>().material = materialInstance;
        
        statusHandler = GetComponent<EnemyStatusModule>();
        statusHandler.Initialize(materialInstance);
        anim = GetComponent<Animator>();

        foreach (var weapon in FindObjectsOfType<PiercingBulletWeapon>())
        {
            foreach(var col in GetComponents<Collider2D>())
            {
                weapon.ReceiveCollider(col);
            }
        }
    }

    private void OnEnable()
    {
        currentHealth = maxHealth;

        if(materialInstance != null) 
        {
            materialInstance.SetFloat("_Death", 0);
            materialInstance.SetFloat("_Damaged", 0);
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

        healthBar.transform.localScale = new Vector3(1, .1f, 1);

        GetComponent<Rigidbody2D>().WakeUp();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (immune) return;
        if (other.TryGetComponent<WeaponDamageDealer>(out var damageDealer))
        {
            damageDealer.ApplyWeaponEffects(statusHandler);
            TakeDamage(damageDealer.Damage(out var crit), damageDealer.damageMultiplier, crit, damageDealer.bypassArmour);

            if (dead) return;
        }
    }

    public void TakeDamage(float damage, float damageMultiplier, bool crit, bool bypassArmour)
    {
        if (dead) return;
        if(armoured && !bypassArmour) damage /= 2;
        damage *= 1 + exposedMultiplier;
        currentHealth -= Mathf.CeilToInt(damage);
        GeneralStatRegistry.Main.totalDamageDealt += Mathf.CeilToInt(damage);
        onDamageTaken?.Invoke(Mathf.CeilToInt(damage), crit);
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

    public IEnumerator Die()
    {
        dead = true;
        EndGameLog.Main.enemies++;
        statusHandler.Terminate();
        anim.speed = 0;
        foreach (var col in GetComponents<Collider2D>())
        {
            col.enabled = false;
        }
        GetComponent<Rigidbody2D>().Sleep();

        DropStuff();

        materialInstance.SetFloat("_Damaged", 0);
        var step = 0f;

        while (step <= 1)
        {
            materialInstance.SetFloat("_Death", step);

            step += 0.02f;
            yield return new WaitForSeconds(0.01f);
        }

        onEnemyDeath?.Invoke(this, destroyOnDeath);
        GeneralStatRegistry.Main.activeEnemies--;
    }

    private void DropStuff()
    {
        var exp = Instantiate(experience, transform.position, Quaternion.identity);
        exp.Initialize(transform.position, expMultiplier);

        if (ModuleActivationManager.Main.HasSpecialEffect(ModuleSpecialEffect.ExplosiveDeath))
        {
            if(statusHandler.HasStatus(StatusType.Burn)) Instantiate(explosion, transform.position, Quaternion.identity);
        }
    }

    public void DieFromAttacking()
    {
        StopAllCoroutines();
        dead = true;
        
        statusHandler.Terminate();
        onEnemyDeath?.Invoke(this, destroyOnDeath);
    }

    private void OnDisable()
    {
        // foreach (var weapon in FindObjectsOfType<PiercingBulletWeapon>())
        // {
        //     weapon.RemoveCollider(GetComponent<Collider2D>());
        // }
    }

    public void RaiseMaxHealth(float value)
    {
        maxHealth += value;
        currentHealth = maxHealth;
    }
}
