using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusModule : MonoBehaviour, IEnemyModule
{
    private Inventory inventory;
    private ActionMarker actionMarker;

    private EnemyHealthModule healthModule;
    private EnemyMarchModule marchModule;

    [Header("Effects")]
    [SerializeField] private ParticleSystem burnEffect;
    [SerializeField] private ParticleSystem staticEffect;
    [SerializeField] private ParticleSystem frailEffect;
    private Material mat;

    private List<StatusHolder> appliedStatuses = new List<StatusHolder>();
    private List<StatusHolder> effectsToRemove = new List<StatusHolder>();


    public void Initialize(Material material)
    {
        healthModule = GetComponent<EnemyHealthModule>();
        marchModule = GetComponent<EnemyMarchModule>();

        actionMarker = ActionMarker.Main;
        inventory = Inventory.Main;

        mat = material;
        GetComponent<SpriteRenderer>().material = mat;

        actionMarker.OnBeat += TickEffects;
    }

    void OnEnable()
    {
        for(int i = 0; i < appliedStatuses.Count; i++)
        {
            var holder = appliedStatuses[i];
            RemoveEffect(holder);
        }
        appliedStatuses.Clear();
    }

    private void TickEffects()
    {
        for(int i = 0; i < appliedStatuses.Count; i++)
        {
            var holder = appliedStatuses[i];
            DoEffect(holder.status);

            holder.duration--;
            if(holder.duration == 0)
            {
                effectsToRemove.Add(holder);
            }
        }

        foreach (var holder in effectsToRemove)
        {
            RemoveEffect(holder);
        }

        effectsToRemove.Clear();
    }


    public void ReceiveStatus(StatusType status)
    {
        if(!status.IsStackable() && HasStatus(status)) return;
        if(HasStatus(status) && appliedStatuses.StackCount(status) >= status.MaxStack()) return;

        if(status == StatusType.Static)
        {
            var rdm = UnityEngine.Random.Range(0, 1f);
            if(rdm > inventory.staticChance) return;
        }

        appliedStatuses.Add(new StatusHolder(status));
        InitiateEffect(status);
    }

    public bool HasStatus(StatusType status)
    {
        foreach (var holder in appliedStatuses)
        {
            if(holder.status == status) return true;
        }
        return false;
    }

    private void InitiateEffect(StatusType status)
    {
        switch(status)
        {
            case StatusType.Static:
                marchModule.frozen = true;
                staticEffect.Play();
                mat.SetInt("_Static", 1);
            break;
            case StatusType.Burn:
                mat.SetInt("_Burn", 1);
            break;
            case StatusType.Frailty:
                healthModule.exposedMultiplier += inventory.frailtyMultiplier;
                frailEffect.Play();
            break;
        }
    }

    private void RemoveEffect(StatusHolder holder)
    {
        appliedStatuses.Remove(holder);
        var count = appliedStatuses.StackCount(holder.status);

        switch(holder.status)
        {
            case StatusType.Static:
                if(count == 0)
                {
                    mat.SetInt("_Static", 0);
                    staticEffect.Stop();
                    marchModule.frozen = false;
                }
            break;
            case StatusType.Burn:
                if(count == 0) mat.SetInt("_Burn", 0);
            break;
            case StatusType.Frailty:
                healthModule.exposedMultiplier -= inventory.frailtyMultiplier;
                if(count == 0) frailEffect.Stop();
            break;
        }
    }

    private void DoEffect(StatusType status)
    {
        if(status == StatusType.Burn)
        {
            burnEffect.Play();
            healthModule.TakeDamage(inventory.burnDamage, 1, false, true);
        }
    }

    public void Terminate()
    {
        for(int i = 0; i < appliedStatuses.Count; i++)
        {
            var holder = appliedStatuses[i];
            RemoveEffect(holder);
        }
        appliedStatuses.Clear();
    }
}

[System.Serializable]
public class StatusHolder
{
    public StatusType status;
    public int duration;

    public StatusHolder(StatusType status)
    {
        this.status = status;
        duration = (int)status;
        if(status == StatusType.Static)
        {
            duration += Inventory.Main.extraStaticDuration;
        } else if (status == StatusType.Burn)
        {
            duration += Inventory.Main.extraBurnDuration;
        }
    }
}

public enum StatusType 
{ 
    None = 0,
    Static = 2, 
    Frailty = 3,
    Burn = 4
}
