using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSwitcher : MonoBehaviour
{
    [SerializeField] private bool switchOff;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent<EnemyMarchModule>(out var march))
        {
            var anim = march.GetComponent<Animator>();
            if(switchOff) anim.speed = 0;
            else anim.speed = 1;
        }
    }
}
