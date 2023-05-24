using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ModuleBase : ScriptableObject
{
    public Sprite icon;
    public int cost;
    [TextArea] public string description;
    public ModuleType type;
    public ModuleTrigger trigger;
    public int cooldown;
    public bool scrollable;
    public bool stackable;
    public abstract void Apply();
}

public enum ModuleType {Active, Passive}
public enum ModuleTrigger {Immediate, OnLevelStart}