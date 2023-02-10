using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingBulletWeapon : MonoBehaviour
{
    [SerializeField] private WeaponDamageDealer damageDealer;
    private ParticleSystem system;
    private ParticleSystem.TriggerModule trigger;


    private void OnEnable()
    {
        system = GetComponent<ParticleSystem>();
        trigger = system.trigger;
        GetActiveColliders();
    }

    private void GetActiveColliders()
    {
        foreach (var enemy in FindObjectsOfType<EnemyHealthModule>())
        {
            ReceiveCollider(enemy.GetComponent<Collider2D>());
        }

        foreach(var spawner in FindObjectsOfType<EnemySpawner>())
        {
            foreach (var enemy in spawner.GetEnemiesInPool())
            {
                ReceiveCollider(enemy.GetComponent<Collider2D>());
            }
        }
    }

    public void ReceiveCollider(Collider2D collider)
    {
        trigger.AddCollider(collider);
    }

    public void RemoveCollider(Collider2D collider)
    {
        trigger.RemoveCollider(collider);
    }

    private void OnParticleTrigger()
    {
        List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();

        int count = system.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, particles, out var colliderData);

        for (int i = 0; i < count; i++)
        {
            for (int j = 0; j < colliderData.GetColliderCount(i); j++)
            {
                var col = colliderData.GetCollider(i, j);
                if (col == null)
                {
                    trigger.RemoveCollider(j);
                }
                else if (col.TryGetComponent<EnemyHealthModule>(out var healthController))
                {
                    if (!healthController.triggerCooldown)
                    {
                        healthController.TakeDamage(damageDealer.Damage, damageDealer.damageMultiplier);
                        healthController.SetTriggerCooldown();
                        damageDealer.ApplyWeaponEffects(healthController.statusHandler);
                    }
                }
                else if(col.transform.parent.TryGetComponent<EnemyHealthModule>(out var parentHealth))
                {
                    if (!parentHealth.triggerCooldown)
                    {
                        parentHealth.TakeDamage(damageDealer.Damage, damageDealer.damageMultiplier);
                        parentHealth.SetTriggerCooldown();
                        damageDealer.ApplyWeaponEffects(parentHealth.statusHandler);
                    }
                }
            }
        }
    }
}
