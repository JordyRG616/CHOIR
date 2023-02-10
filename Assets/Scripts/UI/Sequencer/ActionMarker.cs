using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionMarker : MonoBehaviour
{
    #region Main
    private static ActionMarker _instance;
    public static ActionMarker Main
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<ActionMarker>();

            return _instance;
        }

    }
    #endregion
    
    [SerializeField] private float speed;
    [SerializeField] private RectTransform anchor;
    [SerializeField] private List<float> bpmSpeeds;
    private int bpmIndex = 0;
    private Vector2 originalPosition;
    private RectTransform rect;
    public bool On;
    public bool MainMenu;

    public delegate void ActionMarkerEvent();
    public ActionMarkerEvent OnReset;

    public List<float> beatMarker;
    private List<float> timeContainer = new List<float>();

    public delegate void BeatEvent();
    public BeatEvent OnBeat;


    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        originalPosition = rect.anchoredPosition;
    }

    void FixedUpdate()
    {
        if (On)
        {
            rect.anchoredPosition += Vector2.right * speed * Time.fixedDeltaTime;

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<ActionTile>(out var tile))
        {
            tile.Activate();
            timeContainer.Add(Time.time);
            if(timeContainer.Count == 2)
            {
                beatMarker.Add(timeContainer[1] - timeContainer[0]);
                timeContainer.Clear();
            }
        }
        
        else if(collision.TryGetComponent<ChordTile>(out var cTile))
        {
            cTile.Play();
        }
        
        else if (collision.tag == "Reseter")
        {
            Reset();
        }
    }

    public void DoBeat()
    {
        OnBeat?.Invoke();
    }

    public void Reset()
    {
        // var pos = new Vector2(anchor.anchoredPosition.x, originalPosition);
        rect.anchoredPosition = originalPosition;
        OnReset?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<ActionTile>(out var tile))
        {
            tile.ExitTile();
        }

        else if (collision.TryGetComponent<ChordTile>(out var cTile))
        {
            cTile.Stop();
        }
    }
    public void RaiseSpeed()
    {
        bpmIndex++;
        FMODUnity.RuntimeManager.StudioSystem.setParameterByName("BPM", bpmIndex);
        speed = bpmSpeeds[bpmIndex];
        OnReset -= RaiseSpeed;
    }
}
