using UnityEngine;
using System;

public class BeatManager : MonoBehaviour
{
    public static event Action OnBeat;

    [Header("Music")]
    public AudioSource music;

    [Header("BPM Setup")]
    public float bpm = 120f;

    private float secPerBeat;
    private float songPosition;
    private float dspStartTime;

    void Start()
    {
        secPerBeat = 60f / bpm;
        dspStartTime = (float)AudioSettings.dspTime;

        music.Play();
    }

    void Update()
    {
        songPosition = (float)(AudioSettings.dspTime - dspStartTime);
        
        if (songPosition >= secPerBeat)
        {
            songPosition -= secPerBeat;
            dspStartTime = (float)AudioSettings.dspTime;
            OnBeat?.Invoke();
        }
    }
}
