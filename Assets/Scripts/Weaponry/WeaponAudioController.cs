using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using FMODUnity;

public class WeaponAudioController : MonoBehaviour
{
    [SerializeField] private StudioEventEmitter emitter;
    [SerializeField] private TextMeshProUGUI kitIndexText;
    private int kitCount = 1;
    private ActionMarker marker;
    private int currentKit = 0;

    private void Start()
    {
        marker = ActionMarker.Main;
    }

    public void SetWeaponKit(int direction)
    {
        currentKit += direction;
        if (currentKit <= 0) currentKit = kitCount;
        if (currentKit > kitCount) currentKit = 0;
        kitIndexText.text = (currentKit + 1).ToString();
        marker.OnReset += ChangeKit;
    }

    private void ChangeKit()
    {
        //emitter.Params[1].Value = currentKit;
        //marker.OnReset -= ChangeKit;
    }

    public void ChangeKey(WeaponKey key)
    {
        emitter.Params[1].Value = (int)key;
    }
}

public enum WeaponKey 
{
    C = 0, 
    D = 1, 
    E = 2, 
    F = 3, 
    G = 4, 
    A = 5, 
    B = 6
}