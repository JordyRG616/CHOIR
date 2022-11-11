using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobGravityRandomizer : MonoBehaviour
{
    private Rigidbody2D body;
    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        var rdm = Mathf.Sign(Random.Range(-1f, 1f));

        body.gravityScale = rdm;
        transform.localScale = new Vector3(transform.localScale.x, Mathf.Abs(transform.localScale.y) * Mathf.Sign(body.gravityScale), 1);
    }
}
