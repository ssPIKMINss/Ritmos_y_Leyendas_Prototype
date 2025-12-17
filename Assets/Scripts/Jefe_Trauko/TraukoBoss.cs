using UnityEngine;

public class TraukoRhythm : MonoBehaviour
{
    [Header("Salto por ritmo")]
    [SerializeField] private int jumpEveryBeats = 4;     // 4 beats a 120bpm = 2s
    [SerializeField] private float jumpForce = 7f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.15f;

    [Header("Disparo (1 por salto)")]
    [SerializeField] private GameObject proyectilPrefab;
    [SerializeField] private Transform firePoint;

    [Header("Animación (opcional)")]
    [SerializeField] private Animator animator;
    [SerializeField] private string jumpTrigger = "Jump";
    [SerializeField] private string landTrigger = "Land";
    [SerializeField] private string groundedBool = "isGrounded";

    private Rigidbody2D rb;
    private bool isGrounded;
    private int beatCounter = 0;
    private bool hasShotThisJump = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!animator) animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        BeatManager.OnBeat += OnBeat;
    }

    private void OnDisable()
    {
        BeatManager.OnBeat -= OnBeat;
    }

    private void Update()
    {
        // Ground check estable
        if (groundCheck != null)
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (animator != null && !string.IsNullOrEmpty(groundedBool))
            animator.SetBool(groundedBool, isGrounded);
    }

    private void OnBeat()
    {
        beatCounter++;

        if (jumpEveryBeats <= 0) jumpEveryBeats = 1;

        // Solo actúa en el beat que corresponde
        if (beatCounter % jumpEveryBeats != 0) return;

        // Si no está en el suelo, no salta (espera al próximo beat)
        if (!isGrounded) return;

        JumpAndShoot();
    }

    private void JumpAndShoot()
    {
        hasShotThisJump = false;

        // SALTO
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        if (animator != null && !string.IsNullOrEmpty(jumpTrigger))
            animator.SetTrigger(jumpTrigger);

        // DISPARO 1 VEZ POR SALTO (instantáneo)
        ShootOnce();
    }

    private void ShootOnce()
    {
        if (hasShotThisJump) return;
        if (proyectilPrefab == null || firePoint == null) return;

        Instantiate(proyectilPrefab, firePoint.position, firePoint.rotation);
        hasShotThisJump = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si aterriza, dispara anim de land (opcional)
        if (collision.contacts.Length > 0 && collision.contacts[0].normal.y > 0.5f)
        {
            if (animator != null && !string.IsNullOrEmpty(landTrigger))
                animator.SetTrigger(landTrigger);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
