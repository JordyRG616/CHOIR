using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DirectionSwitch : MonoBehaviour
{
    [SerializeField] private int directionToSet;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent<EnemyMarchModule>(out var enemyMarch))
        {
            enemyMarch.SetDirection(directionToSet);
        }
    }
}
