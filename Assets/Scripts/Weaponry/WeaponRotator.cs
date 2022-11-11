using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRotator : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;
    private float angle;

    private void Update()
    {
        angle += Time.deltaTime * rotationSpeed;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
