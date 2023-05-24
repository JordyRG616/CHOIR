using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Modules/Change Weight", fileName = "New Weight changer")]
public class ChangeEndlevelWeight : ModuleBase
{
    public WeightType weight;
    public float value;

    public override void Apply()
    {
        EndGameLog.Main.ChangeWeight(weight, value);
    }
}
