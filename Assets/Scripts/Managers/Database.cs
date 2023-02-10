
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Upgrade Database", fileName ="Upgrade Database")]
public class Database : ScriptableObject
{
    [SerializeField] private List<UpgradeBase> Upgrades;
    [SerializeField] private List<MutationBase> Mutations;

    public UpgradeBase GetRandomUpgrade()
    {
        var rdm = Random.Range(0, Upgrades.Count);
        var upg = new UpgradeBase(Upgrades[rdm]);
        upg.amount = 1;
        
        return upg;
    }

    public UpgradeBase GetUpgrade(UpgradeTag tag)
    {
        var upg = new UpgradeBase(Upgrades.Find(x => x.tag == tag));
        upg.amount = 1;

        return upg;
    }

    public MutationBase GetRandomMutation()
    {
        var rdm = Random.Range(0, Mutations.Count);
        return Mutations[rdm];
    }

    public MutationBase GetMatchingMutation(MutationBase mutation)
    {
        var mut = Mutations.Find(x => x.Compare(mutation));
        return mut;
    }
}
