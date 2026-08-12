using UnityEngine;

public class enemyAI : MonoBehaviour
{
    // =========================
    // REFERÊNCIAS
    // =========================

    public Transform player;
    private Rigidbody2D rb;


    // =========================
    // STATUS
    // =========================

    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth = 100f;

    [Header("Movement")]
    public float moveSpeed = 3f;

    [Header("Combat")]
    public float attackRange = 2f;
    public float attackDamage = 10f;
    public float attackCooldown = 1.5f;

    [Header("Detection")]
    public float detectionRange = 10f;


    // =========================
    // IA
    // =========================

    private AIAction currentAction;

    private enum AIAction
    {
        None,
        Approach,
        Attack,
        Retreat,
        Dodge
    }


    private float nextAttackTime;


    // =========================
    // UNITY
    // =========================

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (player == null)
            return;

        Think();
    }


    // =========================
    // CÉREBRO
    // =========================

    private void Think()
    {
        float approachScore = EvaluateApproach();
        float attackScore = EvaluateAttack();
        float retreatScore = EvaluateRetreat();
        float dodgeScore = EvaluateDodge();

        float bestScore = 0f;

        AIAction bestAction = AIAction.None;


        if (approachScore > bestScore)
        {
            bestScore = approachScore;
            bestAction = AIAction.Approach;
        }

        if (attackScore > bestScore)
        {
            bestScore = attackScore;
            bestAction = AIAction.Attack;
        }

        if (retreatScore > bestScore)
        {
            bestScore = retreatScore;
            bestAction = AIAction.Retreat;
        }

        if (dodgeScore > bestScore)
        {
            bestScore = dodgeScore;
            bestAction = AIAction.Dodge;
        }


        if (bestAction != currentAction)
        {
            currentAction = bestAction;
        }

        ExecuteAction();
    }


    // =========================
    // APROXIMAR
    // =========================

    private float EvaluateApproach()
    {
        float distance = DistanceToPlayer();

        if (distance <= attackRange)
            return 0f;

        if (distance > detectionRange)
            return 0f;

        return Mathf.InverseLerp(
            detectionRange,
            attackRange,
            distance
        );
    }


    private void Approach()
    {
        Vector2 direction =
            (player.position - transform.position).normalized;

        rb.linearVelocity =
            direction * moveSpeed;
    }


    // =========================
    // ATAQUE
    // =========================

    private float EvaluateAttack()
    {
        float distance = DistanceToPlayer();

        if (distance > attackRange)
            return 0f;

        float distanceScore =
            1f - Mathf.Clamp01(
                distance / attackRange
            );

        return distanceScore;
    }


    private void Attack()
    {
        rb.linearVelocity = Vector2.zero;

        if (Time.time < nextAttackTime)
            return;

        nextAttackTime =
            Time.time + attackCooldown;

        Debug.Log("INIMIGO ATACOU!");
    }


    // =========================
    // RECUAR
    // =========================

    private float EvaluateRetreat()
    {
        float healthPercentage =
            currentHealth / maxHealth;

        if (healthPercentage > 0.4f)
            return 0f;

        return 1f - healthPercentage / 0.4f;
    }


    private void Retreat()
    {
        Vector2 direction =
            (transform.position - player.position).normalized;

        rb.linearVelocity =
            direction * moveSpeed;
    }


    // =========================
    // ESQUIVA
    // =========================

    private float EvaluateDodge()
    {
        // Temporariamente não temos
        // informação sobre o ataque do jogador.

        return 0f;
    }


    private void Dodge()
    {
        Vector2 direction =
            (player.position - transform.position).normalized;

        Vector2 dodgeDirection =
            Vector2.Perpendicular(direction);

        rb.linearVelocity =
            dodgeDirection * moveSpeed * 1.5f;
    }


    // =========================
    // EXECUÇÃO
    // =========================

    private void ExecuteAction()
    {
        switch (currentAction)
        {
            case AIAction.Approach:
                Approach();
                break;

            case AIAction.Attack:
                Attack();
                break;

            case AIAction.Retreat:
                Retreat();
                break;

            case AIAction.Dodge:
                Dodge();
                break;

            case AIAction.None:
                rb.linearVelocity = Vector2.zero;
                break;
        }
    }


    // =========================
    // UTILITÁRIOS
    // =========================

    private float DistanceToPlayer()
    {
        return Vector2.Distance(
            transform.position,
            player.position
        );
    }
}