using UnityEngine;

public class MinionMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 2f;             // Velocidad de movimiento
    public bool moveLeftAtStart = true;  // Dirección inicial
    public bool flipOnWall = true;       // Si debe girar al chocar con una pared

    private Rigidbody2D rb;
    private bool movingLeft;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movingLeft = moveLeftAtStart;
    }

    private void Update()
    {
        // Movimiento constante hacia la dirección actual
        float direction = movingLeft ? -1f : 1f;
        rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si choca con algo sólido y se supone que debe girar, lo hace
        if (flipOnWall && collision.contacts.Length > 0)
        {
            // Detecta si el impacto fue horizontal (pared)
            Vector2 normal = collision.contacts[0].normal;
            if (Mathf.Abs(normal.x) > 0.5f)
            {
                Flip();
            }
        }
    }

    private void Flip()
    {
        // Cambia dirección de movimiento
        movingLeft = !movingLeft;

        // Invierte la escala para reflejar el sprite
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
