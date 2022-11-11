using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentipedeController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private List<Transform> children;

    private void Start()
    {
        children.ForEach(x => x.SetParent(null, true));
    }


    private void FixedUpdate()
    {
        transform.position += new Vector3(0, Mathf.Sin(Time.time) * speed);
    }
}
