using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponSlotButton : MonoBehaviour
{
    public UnityEvent onMouseDown;
    [SerializeField] private Sprite on, off;
    private bool _on = false;

    private void OnMouseDown()
    {
        onMouseDown?.Invoke();
        _on = !_on;

        if (on != null && off != null)
        {
            SpriteRenderer _sp = GetComponent<SpriteRenderer>();
            if (_on) _sp.sprite = on;
            else _sp.sprite = off;
        }
    }
}
