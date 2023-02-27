using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPlatform : MonoBehaviour
{
    [SerializeField] private List<int> ActivateOn;
    [SerializeField] private List<int> DeactivateOn;
    [SerializeField] private bool startActive;

    private Animator anim;
    private Collider2D coll;
    private bool open;

    void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();

        SpawnerManager.Main.OnEndOfWave += HandlePlatform;
        if(startActive) ChangePlatform(true);
    }

    private void HandlePlatform(int waveNumber)
    {
        if(ActivateOn.Contains(waveNumber))
        {
            ChangePlatform(true);
        }

        if(DeactivateOn.Contains(waveNumber))
        {
            ChangePlatform(false);
        }
    }

    private void ChangePlatform(bool active)
    {
        open = active;
        anim.SetBool("Active", open);
    }

    public void ActivateCollider()
    {
        coll.enabled = true;
    }

    public void DeactivateCollider()
    {
        coll.enabled = false;
    }
}
