using UnityEngine;
using System;

public class BeatManager : MonoBehaviour
{
    public static event Action OnBeat;

    [Header("Audio")]
    public AudioSource music;  // Arrastra tu MP3 aquí

    [Header("Beat Settings")]
    public float bpm = 150f;

    private double secPerBeat;
    private double nextBeatTime;

    void Start()
    {
        secPerBeat = 60f / bpm;

        music.Play();
        nextBeatTime = AudioSettings.dspTime + secPerBeat;
    }

    void Update()
    {
        if (AudioSettings.dspTime >= nextBeatTime)
        {
            OnBeat?.Invoke();
            nextBeatTime += secPerBeat;
        }
    }
}
