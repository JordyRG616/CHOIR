using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeBeat : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private float step;
    [SerializeField] private int directionModifier;


    private IEnumerator Start()
    {
        animator = GetComponent<Animator>();

        while(true)
        {
            yield return new WaitForSeconds(.5f);

            animator.SetTrigger("Beat");
        }
    }

    public void A_Step()
    {
        transform.position += Vector3.right * step * directionModifier;
    }
}
