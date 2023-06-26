using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UpgradeBase: ScriptableObject
{
    [TextArea] public string description;
    
    public abstract void Apply();
    public abstract void Remove();
}
