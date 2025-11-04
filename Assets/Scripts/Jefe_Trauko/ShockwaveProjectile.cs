using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ShockwaveProjectile : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] public float speed = -8f;     // signo define dirección
    [SerializeField] private float lifetime = 4f;

    [Header("Daño")]
    [SerializeField] public int damage = 1;
    [SerializeField] private LayerMask groundLayer; // si toca suelo/pared, se destruye

    private float spawnTime;

    private void Awake()
    {
        spawnTime = Time.time;
        // Asegúrate que el collider sea IsTrigger para detectar OnTriggerEnter2D
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
    }

    public void Initialize(float speed, int damage)
    {
        this.speed = speed;
        this.damage = damage;
    }

    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (Time.time - spawnTime >= lifetime)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Daño al jugador
        if (other.CompareTag("Player"))
        {
            var hp = other.GetComponent<PlayerHealth>();
            if (hp != null) hp.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // Si golpea el suelo/pared, destruye
        if (((1 << other.gameObject.layer) & groundLayer) != 0)
        {
            Destroy(gameObject);
        }
    }
}
