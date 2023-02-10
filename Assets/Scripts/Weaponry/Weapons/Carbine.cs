using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carbine : BallisticBase
{
    public override void ApplyPassiveEffect()
    {
        WeaponMasterController.Main.ammoGeneration += 1;
    }
}
