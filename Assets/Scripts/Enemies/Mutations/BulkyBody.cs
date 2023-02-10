using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Mutations/Bulky Body")]
public class BulkyBody : MutationBase
{
    [SerializeField] private float percentage;


    public override void AddMutation(MutationBase mutation)
    {
        var other = mutation as BulkyBody;
        percentage += other.percentage;
    }

    public override void ApplyMutation(EnemyStatData data)
    {
        data.MaxHealth *= 1 + percentage;
    }

    public override MutationBase GetCopy()
    {
        var copy = CreateInstance<BulkyBody>();
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
