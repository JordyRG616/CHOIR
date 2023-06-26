using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Set Upgrade/Change BPM", fileName = "New BPM Upgrade")]
public class ChangeBPM : UpgradeBase
{
    [SerializeField] private int amount;

    public override void Apply()
    {
        ActionMarker.Main.ChangeBPM(amount);
    }

    public override void Remove()
    {
        ActionMarker.Main.ChangeBPM(-amount);
    }
}
