using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    [SerializeField] private int frameRate;
    [SerializeField] private float increment;
    private float speed;
    private int frameCount;
    private Material _mat;

    private void Start()
    {
        var _r = GetComponent<SpriteRenderer>();
        _mat = new Material(_r.material);
        _r.material = _mat;
    }

    private void Update()
    {
        frameCount++;
        if(frameCount == frameRate)
        {
            speed += increment;
            _mat.SetFloat("_Speed", speed);
            frameCount = 0;
        }
    }
}
