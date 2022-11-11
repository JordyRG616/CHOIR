using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamageDealer : MonoBehaviour
{
    [SerializeField] private WeaponBase weapon;
    [SerializeField] private float damageMultiplier = 1;

    public delegate void WeaponEffectHandler(GameObject enemy);
    public WeaponEffectHandler ApplyEffects;

    public float Damage
    {
        get
        {
            var rdmDamage = Random.Range(weapon.damageRange.x, weapon.damageRange.y);
            rdmDamage += weapon.classes.HasFlag(WeaponClass.Ballistic) ? WeaponMasterController.Main.globalBulletBuff : 0;
            rdmDamage *= damageMultiplier;
            return rdmDamage;
        }
    }

    public void ApplyWeaponEffects(GameObject enemy)
    {
        ApplyEffects?.Invoke(enemy);
    }

    public void SetWeapon(WeaponBase weapon)
    {
        this.weapon = weapon;
    }

    public WeaponBase GetWeapon()
    {
        return weapon;
    }
}
