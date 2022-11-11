using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private List<Animator> startAnimators;
    public bool started;

    private void StartGame()
    {
        startAnimators.ForEach(x => x.SetTrigger("Start"));
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            StartGame();
            started = true;
            enabled = false;
        }
    }
}
