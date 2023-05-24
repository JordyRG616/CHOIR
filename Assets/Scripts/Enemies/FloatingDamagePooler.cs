using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class FloatingDamagePooler : MonoBehaviour
{
    [SerializeField] private FloatingDamage popupModel;
    [SerializeField] private int maxPopups;
    public FloatingDamage availablePop;
    private Queue<FloatingDamage> activePopups = new Queue<FloatingDamage>();

    void Start()
    {
        var health = GetComponent<EnemyHealthModule>();
        health.onDamageTaken += InstantiatePopup;
        health.onEnemyDeath += Clear;
    }

    private void Clear(EnemyHealthModule healthModule, bool destroy)
    {
        foreach(var pop in activePopups)
        {
            Destroy(pop.gameObject);
        }

        activePopups.Clear();
        availablePop = null;
    }

    private void InstantiatePopup(int damageTaken, bool crit)
    {
        if(availablePop != null)
        {   
            availablePop.ReceiveDamage(damageTaken, crit);
            return;
        }

        if(activePopups.Count < maxPopups)
        {
            var pop = Instantiate(popupModel, transform.position, Quaternion.identity);
            pop.Pop(damageTaken, this, crit);
            activePopups.Enqueue(pop);
            availablePop = pop;
        } else
        {
            var pop = activePopups.Dequeue();
            pop.gameObject.SetActive(false);
            pop.transform.position = transform.position;
            pop.Pop(damageTaken, this, crit);
            activePopups.Enqueue(pop);
            availablePop = pop;
        }
    }
}
