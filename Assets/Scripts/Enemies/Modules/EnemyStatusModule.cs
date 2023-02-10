using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusModule : MonoBehaviour, IEnemyModule
{
    [SerializeField] private List<StatusResistance> resistances;
    

    public void ReceiveResistances(List<StatusResistance> Resistances)
    {
        resistances = Resistances;
    }

    public void TakeResistanceDamage(StatusType status, float damage)
    {
        var res = resistances.Find(x => x.Status == status);
        if (res != null)
        {
            res.ReceiveResistanceDamage(damage);
        }
    }

    public void Update()
    {
        resistances.ForEach(x => x.TickDuration());
    }

    public StatusResistance GetResistance(StatusType status)
    {
        return resistances.Find(x => x.Status == status);
    }
}

[System.Serializable]
public class StatusResistance
{
    [field: SerializeField] public StatusType Status { get; private set; }
    [field: SerializeField] public float Resistance { get; private set; }
    [SerializeField] private ParticleSystem vfx;
    private float currentValue;
    public bool ResistanceBreak { get; private set; }

    public delegate void StatusEvent();
    public StatusEvent OnStatusApplied;
    public StatusEvent OnStatusReverted;

    public void ReceiveResistanceDamage(float value)
    {
        if (ResistanceBreak) return;

        currentValue += value;

        if (currentValue >= Resistance)
        {
            ResistanceBreak = true;
            vfx.Play();
            OnStatusApplied?.Invoke();
        }
    }

    public void TickDuration()
    {
        if (!ResistanceBreak) return;

        currentValue -= Time.deltaTime;

        if (currentValue <= 0)
        {
            currentValue = 0;
            ResistanceBreak = false;
            vfx.Stop();
            OnStatusReverted?.Invoke();
        }
    }

    public void ReceiveCallbacks(StatusEvent OnApply, StatusEvent OnRevert)
    {
        OnStatusApplied += OnApply;
        OnStatusReverted += OnRevert;
    }

    public void RaiseResistance(float percentage)
    {
        Resistance *= 1 + percentage;
    }
}

public enum StatusType { Frailty, Burn, Shock, Irradiating}