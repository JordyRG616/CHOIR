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
    public BeatAnimation beat;
    private bool Grounded => Physics2D.BoxCast(transform.position, new Vector2(1.5f, 1), 0f, Vector2.down, 1.5f, LayerMask.GetMask("Ground"));


    void Awake()
    {
        beat = GetComponent<BeatAnimation>();
    }

    public void ReceiveValues(float Step, float Speed)
    {
        step = Step;
        speed = Speed;
    }

    public void A_Step()
    {
        // if (frozen || !Grounded) return;
        transform.position += Vector3.right * step * directionModifier;
    }

    private void Update()
    {
        beat.OverrideBeat = !Grounded;
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
