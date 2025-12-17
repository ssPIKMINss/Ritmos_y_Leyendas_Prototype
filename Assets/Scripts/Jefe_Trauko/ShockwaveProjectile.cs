using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class ShockwaveProjectile : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float speed = -8f;     // signo define dirección
    [SerializeField] private float lifetime = 4f;

    [Header("Daño")]
    [SerializeField] private int damage = 1;
    [SerializeField] private LayerMask groundLayer; // si toca suelo/pared, se destruye

    private float spawnTime;
    private bool hasHit = false;
    private Rigidbody2D rb;

    private void Awake()
    {
        spawnTime = Time.time;

        // Collider en modo trigger
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;

        // Rigidbody necesario para triggers fiables
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.isKinematic = true;
    }

    public void Initialize(float newSpeed, int newDamage)
    {
        speed = newSpeed;
        damage = newDamage;
    }

    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (Time.time - spawnTime >= lifetime)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;

        // 🧍 Daño al jugador
        if (other.CompareTag("Player"))
        {
            PlayerHealth hp = other.GetComponentInParent<PlayerHealth>();
            if (hp != null)
                hp.TakeDamage(damage);

            hasHit = true;
            Destroy(gameObject);
            return;
        }

        // 🧱 Suelo / paredes
        if (((1 << other.gameObject.layer) & groundLayer) != 0)
        {
            hasHit = true;
            Destroy(gameObject);
        }
    }
}
