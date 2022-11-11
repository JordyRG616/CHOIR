using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMarch : MonoBehaviour
{
    [SerializeField] private float step;
    [SerializeField] private float speed;
    public int directionModifier;
    public bool frozen;

    // Update is called once per frame
    public void A_Step()
    {
        if (frozen) return;
        transform.position += Vector3.right * step * directionModifier;
    }

    private void Update()
    {
        if (frozen) return;
        transform.position += Vector3.right * speed * Time.deltaTime * directionModifier;
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
