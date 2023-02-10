using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MutationBase : ScriptableObject
{
    public MutationKey Key;
    [TextArea] public string Description;

    public abstract void ApplyMutation(EnemyStatData data);
    public abstract void AddMutation(MutationBase mutation);

    public virtual bool Compare(MutationBase mutation)
    {
        return Key == mutation.Key;
    }

    public abstract string GetLiteralValue();

    public abstract MutationBase GetCopy();
}

public enum MutationKey
{
    Stronger,
    Speedster,
    BulkyBody,
    HighImmunity,
    Exoskeleton
}