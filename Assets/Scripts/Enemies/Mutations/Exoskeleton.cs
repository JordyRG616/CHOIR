using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Mutations/Exoskeleton")]
public class Exoskeleton : MutationBase
{
    [SerializeField] public int increment;


    public override void AddMutation(MutationBase mutation)
    {
        var other = mutation as Exoskeleton;
        increment += other.increment;
    }

    public override void ApplyMutation(EnemyStatData data)
    {
        data.Armor += increment;
    }

    public override MutationBase GetCopy()
    {
        var copy = CreateInstance<Exoskeleton>();
        copy.increment = increment;
        copy.Key = Key;
        copy.Description = Description;
        return copy;
    }

    public override string GetLiteralValue()
    {
        return increment.ToString();
    }
}
