using UnityEngine;
using System;

public class BeatManager : MonoBehaviour
{
    public static BeatManager Instance { get; private set; }
    public static event Action OnBeat;

    [Header("Music")]
    public AudioSource music;

    [Header("BPM Setup")]
    public float bpm = 120f;

    private float secPerBeat;
    private double dspSongStart;
    private double nextBeatTime;

    private void Awake()
    {
        // ✅ Solo 1 BeatManager en toda la escena/juego
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        if (music == null)
        {
            Debug.LogError("BeatManager → No hay AudioSource asignado");
            return;
        }

        secPerBeat = 60f / bpm;

        dspSongStart = AudioSettings.dspTime + 0.05;
        music.PlayScheduled(dspSongStart);

        nextBeatTime = dspSongStart + secPerBeat;
    }

    void Update()
    {
        if (music == null) return;

        while (AudioSettings.dspTime >= nextBeatTime)
        {
            OnBeat?.Invoke();
            nextBeatTime += secPerBeat;
        }
    }
}
