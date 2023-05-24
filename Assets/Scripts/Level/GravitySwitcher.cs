using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySwitcher : MonoBehaviour
{
    [SerializeField] private int gravity;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent<EnemyMarchModule>(out var marchModule))
        {
            marchModule.body.gravityScale = gravity;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.TryGetComponent<EnemyMarchModule>(out var marchModule))
        {
            marchModule.ResetGravityScale();
        }
    }
}
