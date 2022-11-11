using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private GameObject experienceSpawn;
    [SerializeField] private bool destroyOnDeath;
    private Material materialInstance;
    private float currentHealth;
    private bool dead = false;
    private bool blinking;
    public bool isBurning;
    public bool triggerCooldown;
    private WaitForSeconds triggerCooldownTime = new WaitForSeconds(0.01f);

    public delegate void OnEnemyDeath(EnemyHealthController healthController, bool destroy);
    public OnEnemyDeath onEnemyDeath;

    public delegate void OnDamageTaken(float healthPercentage);
    public OnDamageTaken onDamageTaken;
    public bool immune;
    public float exposedMultiplier;

    [Header("Deaths")]
    [SerializeField] private GameObject death;


    private void Start()
    {
        materialInstance = new Material(GetComponent<SpriteRenderer>().material);
        GetComponent<SpriteRenderer>().material = materialInstance;
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
            damageDealer.ApplyWeaponEffects(gameObject);
            TakeDamage(damageDealer.Damage);

            if (dead) return;
        }
    }

    public void TakeDamage(float damage)
    {
        if (dead) return;
        damage *= 1 + exposedMultiplier;
        currentHealth -= damage;
        onDamageTaken?.Invoke(currentHealth / maxHealth);
        if (!blinking) StartCoroutine(Blink());

        if (currentHealth <= 0) Die(true);
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

        if(exp && experienceSpawn != null) Instantiate(experienceSpawn, transform.position, Quaternion.identity);

        if(exp && death != null) TriggerDeathAnimation();
        onEnemyDeath?.Invoke(this, destroyOnDeath);
    }

    private void TriggerDeathAnimation()
    {
        string _t;

        if (isBurning) _t = "Burn";
        else if (exposedMultiplier > 0) _t = "Laser";
        else if (GetComponent<EnemyMarch>().frozen) _t = "Elec";
        else _t = "Pop";

        var container = Instantiate(death, transform.position, Quaternion.identity);
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
