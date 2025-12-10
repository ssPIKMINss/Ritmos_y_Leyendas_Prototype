using UnityEngine;

public class PlayerScreenClamp : MonoBehaviour
{
    public Camera cam;
    public float padding = 0.3f; // margen para que no esté pegado al borde

    void Start()
    {
        if (cam == null)
            cam = Camera.main;
    }

    void LateUpdate()
    {
        Vector3 pos = transform.position;

        // Convertimos la posición del jugador a coordenadas de viewport (0 a 1)
        Vector3 viewPos = cam.WorldToViewportPoint(pos);

        // Limitamos para evitar que salga del rango visible
        viewPos.x = Mathf.Clamp(viewPos.x, 0f + padding, 1f - padding);
        viewPos.y = Mathf.Clamp(viewPos.y, 0f + padding, 1f - padding);

        // Convertimos de vuelta a coordenadas del mundo
        transform.position = cam.ViewportToWorldPoint(viewPos);
    }
}
