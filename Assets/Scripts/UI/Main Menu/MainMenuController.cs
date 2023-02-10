using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject loginPopup;
    [SerializeField] private GameObject startUnloggedPopup;
    [SerializeField] private Animator anim;

    public void ToAssetBrowser()
    {
        if(!TotemManager.Main.isLogged)
        {
            loginPopup.SetActive(true);
            StartCoroutine(WaitForLogin());
        }
        else anim.SetTrigger("ToAssets");
    }

    private IEnumerator WaitForLogin()
    {
        yield return new WaitUntil(() => TotemManager.Main.isLogged == true);

        anim.SetTrigger("ToAssets");
        loginPopup.SetActive(false);
        startUnloggedPopup.SetActive(false);
    }

    public void StartGame()
    {
        if (!TotemManager.Main.isLogged)
        {
            startUnloggedPopup.SetActive(true);
            StartCoroutine(WaitForLogin());
        }
        else LoadScene();
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(2);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
