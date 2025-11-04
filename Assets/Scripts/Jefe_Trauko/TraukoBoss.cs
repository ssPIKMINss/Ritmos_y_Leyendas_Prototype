using UnityEngine;
using System.Collections;

public class Trauko : MonoBehaviour
{
    [Header("Movimiento")]
    public float jumpForce = 7f;
    public float minIdleDelay = 4f; // Tiempo mínimo en idle
    public float maxIdleDelay = 6f; // Tiempo máximo en idle

    [Header("Disparo")]
    public GameObject proyectilPrefab;
    public Transform firePoint;

    [Header("Animación")]
    public Animator animator;

    private Rigidbody2D rb;
    private bool isGrounded = true;
    private bool hasAttacked = false;
    private bool waitingToJump = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        animator.SetBool("isGrounded", isGrounded);
    }

    void Salta()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        isGrounded = false;
        hasAttacked = false;
        animator.SetTrigger("Jump");
    }

    public void LanzarProyectil()
    {
        if (!hasAttacked)
        {
            Instantiate(proyectilPrefab, firePoint.position, firePoint.rotation);
            hasAttacked = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isGrounded && collision.contacts[0].normal.y > 0.5f)
        {
            isGrounded = true;
            animator.SetTrigger("Land");

            // Cuando aterriza, espera un tiempo antes de volver a saltar
            if (!waitingToJump)
                StartCoroutine(EsperarAntesDeSaltar());
        }
    }

    private IEnumerator EsperarAntesDeSaltar()
    {
        waitingToJump = true;
        float delay = Random.Range(minIdleDelay, maxIdleDelay);
        yield return new WaitForSeconds(delay);

        if (isGrounded)
            Salta();

        waitingToJump = false;
    }
}
