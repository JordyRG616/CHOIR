using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamageDealer : MonoBehaviour
{
    [SerializeField] private WeaponBase weapon;
    [SerializeField] private float effectsCooldown;
    private float counter;
    public List<StatusType> statuses;
    public float damageMultiplier = 1;
    public bool bypassArmour = false;

    public delegate void WeaponEffectHandler(GameObject enemy);
    public WeaponEffectHandler ApplyEffects;

    public float Damage
    {
        get
        {
            var rdmDamage = UnityEngine.Random.Range(weapon.damageRange.x, weapon.damageRange.y);
            rdmDamage *= damageMultiplier;

            var rdm = UnityEngine.Random.Range(0, 1f);
            if(rdm < weapon.criticalChance)
            {
                rdmDamage *= weapon.criticalMultiplier;
            }

            return rdmDamage;
        }
    }

    public void ApplyWeaponEffects(EnemyStatusModule statusHandler)
    {
        if(counter > effectsCooldown)
        {
            statuses.ForEach(x => statusHandler.ReceiveStatus(x));
            counter = 0;
        }
    }

    public void SetWeapon(WeaponBase weapon)
    {
        this.weapon = weapon;
    }

    public WeaponBase GetWeapon()
    {
        return weapon;
    }

    void Update()
    {
        counter += Time.deltaTime;
    }
}