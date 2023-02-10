using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamageDealer : MonoBehaviour
{
    [SerializeField] private WeaponBase weapon;
    public float damageMultiplier = 1;
    public List<StatusEffectApplier> statusAppliers;

    public delegate void WeaponEffectHandler(GameObject enemy);
    public WeaponEffectHandler ApplyEffects;

    public float Damage
    {
        get
        {
            var rdmDamage = UnityEngine.Random.Range(weapon.damageRange.x, weapon.damageRange.y);
            rdmDamage *= damageMultiplier;
            return rdmDamage;
        }
    }

    public void ApplyWeaponEffects(EnemyStatusModule statusHandler)
    {
        foreach (var applier in statusAppliers)
        {
            statusHandler.TakeResistanceDamage(applier.type, applier.strength);
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

    public void ReceiveStatusApplier(StatusEffectApplier applier)
    {
        if (statusAppliers.Contains(applier))
        {
            var _app = statusAppliers.Find(x => x == applier);
            _app += applier;
        }
        else
        {
            statusAppliers.Add(applier);
        }
    }

    public void RemoveStatusApplier(StatusEffectApplier applier)
    {
        if (statusAppliers.Contains(applier))
        {
            var _app = statusAppliers.Find(x => x == applier);
            _app -= applier;

            if (_app.strength <= 0) statusAppliers.Remove(_app);
        }
    }
}


[System.Serializable]
public class StatusEffectApplier : IEquatable<StatusEffectApplier>
{
    public StatusType type;
    public float strength;

    #region Operators
    public static StatusEffectApplier operator +(StatusEffectApplier a, StatusEffectApplier b)
    {
        a.strength += b.strength;
        return a;
    }
    public static StatusEffectApplier operator -(StatusEffectApplier a, StatusEffectApplier b)
    {
        a.strength -= b.strength;
        return a;
    }
    public static bool operator ==(StatusEffectApplier a, StatusEffectApplier b)
    {
        if (a is null && b is null) return true;
        else if (a is null || b is null) return false;
        else return a.type == b.type;
    }
    public static bool operator !=(StatusEffectApplier a, StatusEffectApplier b)
    {
        if (a is null && b is null) return false;
        else if (a is null || b is null) return true;
        else return a.type != b.type;
    }
    #endregion


    public StatusEffectApplier(StatusType status, float str)
    {
        type = status;
        strength = str;
    }

    public bool Equals(StatusEffectApplier other)
    {
        if (other is null) return false;

        if (other.type == type) return true;
        else return false;
    }

    public override bool Equals(object obj)
    {
        if (obj.GetType() != typeof(StatusEffectApplier)) return false;
        else return Equals(obj as StatusEffectApplier);
    }

    public override int GetHashCode()
    {
        return (int)type;
    }

    public override string ToString()
    {
        return "Apply " + strength + " points of " + type;
    }
}