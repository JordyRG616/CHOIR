using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGraphicsController : MonoBehaviour
{
    [SerializeField] private Material weaponMaterial;
    private Material _material;
    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        _material = new Material(weaponMaterial);
        spriteRenderer.material = _material;
    }

    public void SetHighlighted(bool on)
    {
        var _b = on ? 1 : 0;
        _material.SetInt("_highlighted", _b);
    }
}
