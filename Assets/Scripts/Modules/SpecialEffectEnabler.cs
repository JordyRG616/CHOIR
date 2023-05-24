using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Modules/Enable Special Effect", fileName = "New Special Effect")]
public class SpecialEffectEnabler : ModuleBase
{
    [SerializeField] private ModuleSpecialEffect effect;

    public override void Apply()
    {
        ModuleActivationManager.Main.ReceiveSpecialEffect(effect);
    }
}
