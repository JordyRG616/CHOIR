using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySwitcher : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent<Rigidbody2D>(out var body))
        {
            body.gravityScale *= -1;
        }
    }
}
