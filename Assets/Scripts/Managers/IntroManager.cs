using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    [SerializeField] private float seconds;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(seconds);

        SceneManager.LoadScene(1);
    }
}
