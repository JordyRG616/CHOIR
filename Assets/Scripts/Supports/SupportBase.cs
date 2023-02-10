using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SupportBase : ScriptableObject
{
    public abstract void ApplyEffect(WeaponBase weapon);
    public abstract void RemoveEffect(WeaponBase weapon);
}
