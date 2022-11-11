using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponBase : MonoBehaviour
{
    public int ID;
    [SerializeField] private UnityEvent OnShoot, OnStop;
    public Vector2 damageRange;
    private Animator anim;
    private WeaponAudioController audioController;
    public WeaponGraphicsController graphicsController { get; private set; }
    public bool unlocked = false;
    public WeaponClass classes;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        audioController = GetComponent<WeaponAudioController>();
        graphicsController = GetComponent<WeaponGraphicsController>();
    }

    public void Shoot(WeaponKey key)
    {
        audioController.ChangeKey(key);
        OnShoot?.Invoke();
        anim.SetTrigger("Shoot");
    }

    public void Stop()
    {
        OnStop?.Invoke();
    }

    public void ResetTriggers()
    {

        //var count = anim.parameterCount;
        //for (int i = 0; i < count; i++)
        //{
        //    //var _p = anim.GetParameter(i);
        //    //if (_p.type == AnimatorControllerParameterType.Trigger) 
        //    anim.ResetTrigger(i);
        //}
    }
}

[System.Flags]
public enum WeaponClass 
{ 
    Default = 0,
    Ballistic = 1,
    Laser = 2, 
    Flame = 4,
    Electric = 8
}