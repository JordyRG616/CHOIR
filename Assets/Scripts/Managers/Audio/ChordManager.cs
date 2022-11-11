using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ChordManager : MonoBehaviour
{
    [SerializeField] [EventRef] List<string> chordList;
    [SerializeField] List<ChordProgression> progressions;
    private int[] selectedProgression;
    private int _index;
    private int index
    {
        get => _index;
        set
        {
            if (value >= selectedProgression.Length) _index = 0;
            else _index = value;
        }
    }
    private FMOD.Studio.EventInstance currentInstance;
    private ActionMarker marker;


    private IEnumerator Start()
    {
        marker = ActionMarker.Main;
        var rdm = Random.Range(0, progressions.Count);
        selectedProgression = progressions[rdm].progression;
        marker.OnReset += PlayChord;

        yield return new WaitUntil(() => marker.On);

        currentInstance = FMODUnity.RuntimeManager.CreateInstance(chordList[0]);
        currentInstance.start();
    }

    private void PlayChord()
    {
        currentInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        index++;
        int chord = selectedProgression[index];
        currentInstance = FMODUnity.RuntimeManager.CreateInstance(chordList[chord]);
        currentInstance.start();
    }
}

[System.Serializable]
public struct ChordProgression
{
    public int[] progression;
}