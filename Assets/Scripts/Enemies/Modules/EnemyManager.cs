using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private EnemyStatData defaultStats;
    public EnemyStatData currentStats { get; private set; }
    private bool birthed = false;
    private List<IEnemyModule> modules = new List<IEnemyModule>();


    private void Awake()
    {
        modules = GetComponents<IEnemyModule>().ToList();

        if(!birthed)
        {
            currentStats = new EnemyStatData(defaultStats);
            birthed = true;
        }

        SetStats();
    }

    public T GetModule<T>() where T: IEnemyModule
    {
        var module = modules.Find(x => x is T);
        return (T)module;
    }

    public void SetStats(bool _default = true)
    {
        var data = _default ? defaultStats : currentStats;
        GetModule<EnemyHealthModule>().ReceiveValues(data.MaxHealth, data.Armor);
        GetModule<EnemyMarchModule>().ReceiveValues(data.Step, data.Speed);
        GetModule<EnemyDamageModule>().damage = data.Damage;
        GetModule<EnemyStatusModule>().ReceiveResistances(data.Resistances);
    }
}

[System.Serializable]
public class EnemyStatData
{
    public float MaxHealth;
    public int Armor;
    public float Step;
    public float Speed;
    public int Damage;

    public List<StatusResistance> Resistances;

    public EnemyStatData(EnemyStatData data)
    {
        MaxHealth = data.MaxHealth;
        Armor = data.Armor;
        Step = data.Step;
        Speed = data.Speed;
        Damage = data.Damage;

        Resistances = new List<StatusResistance>(data.Resistances);
    }
}