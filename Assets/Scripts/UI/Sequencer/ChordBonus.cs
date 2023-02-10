using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ChordBonus : ScriptableObject
{
    public Sprite icon;
    public abstract void Apply();
    public abstract void Remove();
    public abstract string Description { get; }
    public abstract string ExtraInfo { get; }
}
