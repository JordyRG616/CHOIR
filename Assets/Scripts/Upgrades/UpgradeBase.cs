using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeBase : IEquatable<UpgradeBase>
{
    public string name;
    public Sprite icon;
    public int cost;
    [TextArea] public string description;
    public UpgradeTag tag;
    public int amount;

    #region Operators
    public static UpgradeBase operator +(UpgradeBase a, UpgradeBase b)
    {
        if (a.Equals(b))
        {
            a.amount += b.amount;
        }
        return a;
    }
    public static UpgradeBase operator -(UpgradeBase a, UpgradeBase b)
    {
        if (a.Equals(b))
        {
            a.amount -= b.amount;
        }
        return a;
    }
    public static UpgradeBase operator ++(UpgradeBase a)
    {
        a.amount++;
        return a;
    }
    public static UpgradeBase operator --(UpgradeBase a)
    {
        a.amount--;
        return a;
    }
    public static bool operator ==(UpgradeBase a, UpgradeBase b)
    {
        if (a is null && b is null) return true;
        else if (a is null || b is null) return false;
        else return a.name == b.name;
    }
    public static bool operator !=(UpgradeBase a, UpgradeBase b)
    {
        if(a is null && b is null) return false;
        else if (a is null || b is null) return true;
        else return a.name == b.name;
    }
    #endregion

    public UpgradeBase(UpgradeBase original)
    {
        name = original.name;
        icon = original.icon;
        cost = original.cost;
        description = original.description;
        tag = original.tag;
    }

    public bool Equals(UpgradeBase other)
    {
        if (other is null) return false;
        else return name == other.name;
    }
}

public enum UpgradeTarget { Weapon, Slot, Tile}

public enum UpgradeTag
{
    Default,
    Multishot,
    Explosive,
    Estabilizer,
    Unstable
}
