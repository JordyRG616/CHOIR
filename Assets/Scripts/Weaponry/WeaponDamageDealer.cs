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
    public bool OnTriggerCooldown {get; private set;}
    private WaitForSeconds cooldownTime = new WaitForSeconds(.1f);

    public delegate void WeaponEffectHandler(GameObject enemy);
    public WeaponEffectHandler ApplyEffects;

    public float Damage(out bool crit)
    {
        var rdmDamage = UnityEngine.Random.Range(weapon.damageRange.x, weapon.damageRange.y);
        rdmDamage *= damageMultiplier + Inventory.Main.globalDamageMultiplier;

        var rdm = UnityEngine.Random.Range(0, 1f);
        if(rdm < Mathf.Clamp01(weapon.criticalChance + Inventory.Main.extraCritChance))
        {
            rdmDamage = weapon.damageRange.y; 
            rdmDamage *= weapon.criticalMultiplier + Inventory.Main.extraCritDamage;
            crit = true;
        } else crit = false;

        return rdmDamage;
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

    public void SetTriggerCooldown()
    {
        StartCoroutine(DoCooldown());
    }

    private IEnumerator DoCooldown()
    {
        OnTriggerCooldown = true;
        yield return cooldownTime;
        OnTriggerCooldown = false;
    }
}