using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private List<Animator> animators;
    private bool open;
    private int frameCount;
    private float elapsedTime;
    private double frameRate;

    private void OpenMenu()
    {
        Time.timeScale = 0;
        AudioManager.Main.PauseAudio();
        animators.ForEach(x => x.SetTrigger("ToOptions"));
        open = true;
    }

    private void CloseMenu()
    {
        Time.timeScale = 1;
        AudioManager.Main.UnpauseAudio();
        animators.ForEach(x => x.SetTrigger("FromOptions"));
        open = false;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (open) CloseMenu();
            else OpenMenu();
        }
    }

    public void Continue()
    {
        CloseMenu();
    }

    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
