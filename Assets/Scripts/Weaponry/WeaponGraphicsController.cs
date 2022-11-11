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

    public void SetPreview(bool previewOn)
    {
        var _b = previewOn ? 1 : 0;
        _material.SetInt("_Preview", _b);
    }
}
