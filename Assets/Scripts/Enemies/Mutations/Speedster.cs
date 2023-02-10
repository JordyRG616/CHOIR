using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Mutations/Speedster")]
public class Speedster : MutationBase
{
    [SerializeField] private float percentage;


    public override void AddMutation(MutationBase mutation)
    {
        var other = mutation as Speedster;
        percentage += other.percentage;
    }

    public override void ApplyMutation(EnemyStatData data)
    {
        data.Speed *= 1 + percentage;
        data.Step *= 1 + percentage;
    }

    public override MutationBase GetCopy()
    {
        var copy = CreateInstance<Speedster>();
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
