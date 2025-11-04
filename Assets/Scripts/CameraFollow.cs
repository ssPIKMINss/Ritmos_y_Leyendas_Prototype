using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;   // el Player

    [Header("Offset")]
    public Vector3 offset = new Vector3(3f, 1f, -10f); // X mueve la cámara a la derecha

    [Header("Smooth")]
    public float smoothSpeed = 6f;

    private void LateUpdate()
    {
        if (!target) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothed = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothed;
    }
}
