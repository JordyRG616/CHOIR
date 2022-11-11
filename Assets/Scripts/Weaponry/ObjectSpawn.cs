using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawn : MonoBehaviour
{
    [SerializeField] private GameObject model;
    [SerializeField] private Vector3 position;
    [SerializeField] private bool spawnAsChild;

    public void Spawn()
    {
        var container = Instantiate(model, position, Quaternion.identity);
        if (spawnAsChild) container.transform.SetParent(transform);
    }
}
