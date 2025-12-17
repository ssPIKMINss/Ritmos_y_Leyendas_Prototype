using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class MinionBeatAI : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform player;   // arrastra Pía o usa tag Player

    [Header("Beat Actions (probabilidades)")]
    [Range(0f, 1f)] public float moveChance = 0.55f;
    [Range(0f, 1f)] public float dashChance = 0.30f; // “ataque” (solo movimiento)
    [Range(0f, 1f)] public float idleChance = 0.15f;

    [Header("Movement per Beat")]
    [SerializeField] private float stepDistance = 0.55f;  // cuánto avanza por beat
    [SerializeField] private float stepSpeed = 8f;        // velocidad para llegar al destino

    [Header("Dash (Attack Movement)")]
    [SerializeField] private float dashDistance = 1.1f;   // dash hacia el player
    [SerializeField] private float dashSpeed = 12f;
    [SerializeField] private float returnSpeed = 8f;

    [Header("Ground (opcional para que no caiga)")]
    [SerializeField] private bool freezeY = true;

    private Rigidbody2D rb;
    private bool busy = false;
    private bool dead = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        // Opcional: mantenerlo en su altura si tu juego es plano
        if (freezeY)
            rb.constraints = RigidbodyConstraints2D.FreezeRotation | RigidbodyConstraints2D.FreezePositionY;
        else
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void OnEnable()
    {
        BeatManager.OnBeat += OnBeat;
    }

    void OnDisable()
    {
        BeatManager.OnBeat -= OnBeat;
    }

    void OnBeat()
    {
        if (dead || busy || player == null) return;

        // Normaliza probabilidades por si no suman 1
        float total = moveChance + dashChance + idleChance;
        float r = Random.value * total;

        if (r < moveChance)
        {
            StartCoroutine(StepTowardPlayer());
        }
        else if (r < moveChance + dashChance)
        {
            StartCoroutine(DashAttackMove());
        }
        else
        {
            // Idle en este beat (no hace nada)
        }
    }

    IEnumerator StepTowardPlayer()
    {
        busy = true;

        Vector2 start = rb.position;
        Vector2 dir = ((Vector2)player.position - start).normalized;
        Vector2 target = start + new Vector2(dir.x, 0f).normalized * stepDistance; // solo X

        while ((rb.position - target).sqrMagnitude > 0.0005f)
        {
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, stepSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
            yield return new WaitForFixedUpdate();
        }

        busy = false;
    }

    IEnumerator DashAttackMove()
    {
        busy = true;

        Vector2 original = rb.position;
        float dir = Mathf.Sign(player.position.x - transform.position.x);
        if (dir == 0f) dir = 1f;

        Vector2 dashTarget = original + Vector2.right * dashDistance * dir;

        // Dash hacia adelante
        while ((rb.position - dashTarget).sqrMagnitude > 0.0005f)
        {
            Vector2 newPos = Vector2.MoveTowards(rb.position, dashTarget, dashSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(0.05f);

        // Volver atrás
        while ((rb.position - original).sqrMagnitude > 0.0005f)
        {
            Vector2 newPos = Vector2.MoveTowards(rb.position, original, returnSpeed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
            yield return new WaitForFixedUpdate();
        }

        busy = false;
    }

    // Llama esto si tu sistema de vida mata al minion
    public void MarkDead()
    {
        dead = true;
        StopAllCoroutines();
    }
}
