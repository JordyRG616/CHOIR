using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Mutations/Stronger")]
public class Stronger : MutationBase
{
    [SerializeField] private int increment;


    public override void AddMutation(MutationBase mutation)
    {
        var other = mutation as Stronger;
        increment += other.increment;
    }

    public override void ApplyMutation(EnemyStatData data)
    {
        data.Damage += increment;
    }

    public override MutationBase GetCopy()
    {
        var copy = CreateInstance<Stronger>();
        copy.Key = Key;
        copy.Description = Description;
        copy.increment = increment;
        return copy;
    }

    public override string GetLiteralValue()
    {
        return increment.ToString();
    }
}
