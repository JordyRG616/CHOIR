using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Mutations/High Immunity")]
public class HighImmunity : MutationBase
{
    [SerializeField] private float percentage;


    public override void AddMutation(MutationBase mutation)
    {
        var other = mutation as HighImmunity;
        percentage += other.percentage;
    }

    public override void ApplyMutation(EnemyStatData data)
    {
        data.Resistances.ForEach(x => x.RaiseResistance(percentage));
    }

    public override MutationBase GetCopy()
    {
        var copy = CreateInstance<HighImmunity>();
        copy.percentage = percentage;
        copy.Key = Key;
        copy.Description = Description;
        return copy;
    }

    public override string GetLiteralValue()
    {
        return (percentage * 100) + "%";
    }
}
