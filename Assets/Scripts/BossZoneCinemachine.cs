using UnityEngine;
using Unity.Cinemachine;   // <- importante, NO "using Cinemachine;"

public class BossZoneCinemachine : MonoBehaviour
{
    [Header("Cinemachine")]
    public CinemachineConfiner2D confiner;   // componente Confiner2D de tu CinemachineCamera

    [Tooltip("Collider de los límites NORMALES del nivel")]
    public Collider2D normalBounds;

    [Tooltip("Collider de los límites cuando peleas con el BOSS")]
    public Collider2D bossBounds;

    [Header("Paredes que cierran la arena (opcional)")]
    public GameObject leftWall;
    public GameObject rightWall;

    [Header("Config")]
    public bool oneTime = true;

    private bool activated = false;

    void Start()
    {
        // Si no asignas el confiner, lo buscamos automáticamente en la escena
        if (confiner == null)
        {
            // Busca cualquier CinemachineConfiner2D en la escena (por ejemplo, en tu 2D Camera)
            confiner = FindObjectOfType<CinemachineConfiner2D>();
        }

        // Limites normales al iniciar
        if (confiner != null && normalBounds != null)
            confiner.BoundingShape2D = normalBounds;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (activated && oneTime) return;

        activated = true;

        // Cambiar límites a la arena del boss
        if (confiner != null && bossBounds != null)
            confiner.BoundingShape2D = bossBounds;

        if (leftWall != null) leftWall.SetActive(true);
        if (rightWall != null) rightWall.SetActive(true);
    }

    // Llama esto desde el script del boss cuando muera
    public void ReleaseZone()
    {
        // Volver a los límites normales
        if (confiner != null && normalBounds != null)
            confiner.BoundingShape2D = normalBounds;

        if (leftWall != null) leftWall.SetActive(false);
        if (rightWall != null) rightWall.SetActive(false);
    }
}
