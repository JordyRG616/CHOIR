using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportTile : MonoBehaviour
{
    [SerializeField] private SupportBase support;
    public bool IsOverReseter { get; private set; }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<ActionTile>(out var tile))
        {
            support.ApplyEffect(tile.weaponToActivate);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<ActionTile>(out var tile))
        {
            support.RemoveEffect(tile.weaponToActivate);
        }
    }
}
