using UnityEngine;

public class BeatVisualizer : MonoBehaviour
{
    public Vector3 beatScale = new Vector3(1.2f, 1.2f, 1.2f);
    Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
        BeatManager.OnBeat += Pulse;
    }

    void OnDestroy()
    {
        BeatManager.OnBeat -= Pulse;
    }

    void Pulse()
    {
        StopAllCoroutines();
        StartCoroutine(PulseRoutine());
    }

    System.Collections.IEnumerator PulseRoutine()
    {
        transform.localScale = beatScale;
        yield return new WaitForSeconds(0.05f);
        transform.localScale = originalScale;
    }
}
