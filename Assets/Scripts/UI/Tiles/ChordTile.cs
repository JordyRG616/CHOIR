using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using TMPro;

public class ChordTile : MonoBehaviour
{
    public ChordNumber chord;
    [SerializeField] private StudioEventEmitter emitter;
    [SerializeField] private TextMeshProUGUI number;
    [SerializeField] private Sprite overrideSprite;
    private Image image;

    private void Start()
    {
        SetChordNumber();
        image = GetComponent<Image>();
    }

    private void SetChordNumber()
    {
        number.text = chord.ToString();

        emitter.Params[0].Value = (int)chord;
    }

    public void Play()
    {
        emitter.Play();
        image.overrideSprite = overrideSprite;
    }

    public void Stop()
    {
        emitter.Stop();
        image.overrideSprite = null;
    }
}

public enum ChordNumber
{
    I = 1,
    II = 2,
    III = 3,
    IV = 4,
    V = 5,
    VI = 6,
    VII = 7
}
