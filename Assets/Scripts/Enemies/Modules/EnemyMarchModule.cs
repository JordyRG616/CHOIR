using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMarchModule : MonoBehaviour, IEnemyModule
{
    private float step;
    private float speed;
    [SerializeField] private bool spawnInVerticalPosition;
    [SerializeField] private float verticalPosition;
    private Animator anim;
    public int directionModifier;
    public bool frozen;
    public BeatAnimation beat;
    private bool Grounded => Physics2D.BoxCast(transform.position, new Vector2(1.5f, 1), 0f, Vector2.down, 1.5f, LayerMask.GetMask("Ground"));
    public Rigidbody2D body {get; private set;}
    private float originalGravityScale;

    void Awake()
    {
        beat = GetComponent<BeatAnimation>();
        body = GetComponent<Rigidbody2D>();
        originalGravityScale = body.gravityScale;
    }
    
    public void ReceiveValues(float Step, float Speed)
    {
        step = Step;
        speed = Speed;
    }

    public void A_Step()
    {
        transform.position += Vector3.right * step * directionModifier;
    }

    private void Update()
    {
        beat.OverrideBeat = !CanMove();
    }

    private bool CanMove()
    {
        if(!Grounded || frozen) return false;
        return true;
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

    public void DoKnockback(float force)
    {
        var dir = -transform.right;
        GetComponent<Rigidbody2D>().AddForce(dir * force, ForceMode2D.Impulse);
    }

    public Vector3 GetDirectionOfMovement()
    {
        return Vector3.right * directionModifier;// * speed;
    }

    public virtual void SetDirection(int direction)
    {
        directionModifier = direction;
        transform.localScale = new Vector3
            (Mathf.Abs(transform.localScale.x) * directionModifier,
            transform.localScale.y, 1);
    }

    public void SetFrozen(bool active)
    {
        var anim = GetComponent<Animator>();
        frozen = active;
        if (frozen) anim.speed = 0;
        else anim.speed = 1;
    }

    public void ResetGravityScale()
    {
        body.gravityScale = originalGravityScale;
    }
}
