using UnityEngine;
using System;

public class BeatManager : MonoBehaviour
{
    public static event Action OnBeat;

    [Header("Music")]
    public AudioSource music;       // 🎵 Arrastra aquí tu música

    [Header("BPM Setup")]
    public float bpm = 120f;        // 💥 Cambia según el tempo de tu canción

    private float secPerBeat;
    private double dspSongStart;
    private double nextBeatTime;

    void Start()
    {
        if (music == null)
        {
            Debug.LogError("BeatManager → No hay AudioSource asignado ❗");
            return;
        }

        // cuántos segundos dura un beat
        secPerBeat = 60f / bpm;

        // sincroniza el audio con el reloj DSP (0.05 para evitar cortes)
        dspSongStart = AudioSettings.dspTime + 0.05;
        music.PlayScheduled(dspSongStart);

        // programa el primer beat
        nextBeatTime = dspSongStart + secPerBeat;
    }

    void Update()
    {
        if (music == null) return;

        // si ya se alcanzó el tiempo del próximo beat
        while (AudioSettings.dspTime >= nextBeatTime)
        {
            OnBeat?.Invoke();         // 🔔 señal global para minions, partículas, luces, etc.

            nextBeatTime += secPerBeat;  // programa siguiente beat
        }
    }
}

