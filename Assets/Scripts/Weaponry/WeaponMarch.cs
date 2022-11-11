using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMarch : MonoBehaviour
{
    [Header("March")]
    [SerializeField] private AnimationCurve speed, direction;
    public float speedModifier = 1;
    private float timeCounter;

    [Header("Random Position")]
    [SerializeField] private Vector2 randomRange;


    private void Update()
    {
        timeCounter += Time.deltaTime;
        transform.position += Vector3.right * speed.Evaluate(timeCounter) * speedModifier * direction.Evaluate(timeCounter) * Time.deltaTime;
    }

    public void SetRandomHorizontalPosition()
    {
        var rdm = Random.Range(randomRange.x, randomRange.y);

        transform.position = new Vector3(rdm, transform.position.y);
    }
}
