using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMarchModule : MonoBehaviour, IEnemyModule
{
    private float step;
    private float speed;
    [SerializeField] private bool spawnInVerticalPosition;
    [SerializeField] private float verticalPosition;
    public int directionModifier;
    public bool frozen;


    public void ReceiveValues(float Step, float Speed)
    {
        step = Step;
        speed = Speed;
    }

    public void A_Step()
    {
        if (frozen) return;
        transform.position += Vector3.right * step * directionModifier;
    }

    private void Update()
    {
        if (frozen) return;
        transform.position += Vector3.right * speed * Time.deltaTime * directionModifier;
        if (spawnInVerticalPosition)
        {
            transform.position = new Vector3(transform.position.x, verticalPosition, transform.position.y);
        }
    }

    public void RaiseSpeed(float value)
    {
        if(speed > 0)
        {
            speed += value;
        } else
        {
            step += value;
        }
    }

    public Vector3 GetDirectionOfMovement()
    {
        return Vector3.right * directionModifier;// * speed;
    }

    public virtual void SetDirection(int direction)
    {
        directionModifier = direction;
    }

    public void SetFrozen(bool active)
    {
        var anim = GetComponent<Animator>();
        frozen = active;
        if (frozen) anim.speed = 0;
        else anim.speed = 1;
    }
}
