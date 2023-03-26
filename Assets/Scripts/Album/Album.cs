using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Data/New Album", fileName = "New Album")]
public class Album : ScriptableObject
{
    public int initialHealth;

    [field:SerializeField] public List<string> levelNames {get; private set;}
}
