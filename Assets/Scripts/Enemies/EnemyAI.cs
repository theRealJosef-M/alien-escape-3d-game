using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] private float detectionRange = 20f;
    [SerializeField] private float fieldOfView = 90f;
    [SerializeField] private LayerMask detectionLayer;

    [Header("Behavior")]
    [SerializeField] private float patrolSpeed = 3f;
    [SerializeField] private float chaseSpeed = 6f;
    [SerializeField] private float stoppingDistance = 1f;
    [SerializeField] private float knockoutDamage = 100;

    [Header("Difficulty Modifiers")]
    [SerializeField] private float easySpeedModifier = 0.8f;
    [SerializeField] private float mediumSpeedModifier = 1f;
    [SerializeField] private float hardSpeedModifier = 1.3f;

    private NavMeshAgent agent;
    private Transform player;
    private GameManager gameManager;
    private PlayerStats playerStats;
    private Animator animator;

    private bool isChasing = false;
    private Vector3[] patrolPoints;
    private int currentPatrolPoint = 0;

    private enum EnemyState { Patrolling, Chasing, Idle }
    private EnemyState currentState = EnemyState.Patrolling;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerController>().transform;
        gameManager = FindObjectOfType<GameManager>();
        playerStats = FindObjectOfType<PlayerStats>();

        SetupDifficultyModifiers();
        InitializePatrolPoints();
    }

    private void SetupDifficultyModifiers()
    {
        float speedModifier = gameManager.GetCurrentDifficulty() switch
        {
            GameDifficulty.Easy => easySpeedModifier,
            GameDifficulty.Medium => mediumSpeedModifier,
            GameDifficulty.Hard => hardSpeedModifier,
            _ => 1f
        };

        patrolSpeed *= speedModifier;
        chaseSpeed *= speedModifier;
    }

    private void InitializePatrolPoints()
    {
        // Get all patrol points in the room
        PatrolPoint[] points = GetComponentInParent<Transform>().GetComponentsInChildren<PatrolPoint>();
        patrolPoints = new Vector3[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            patrolPoints[i] = points[i].transform.position;
        }
    }

    private void Update()
    {
        if (!gameManager.IsGameActive() || player == null)
            return;

        DetectPlayer();
        UpdateBehavior();
    }

    private void DetectPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRange)
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

            if (angleToPlayer < fieldOfView / 2)
            {
                // Check line of sight
                if (Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, detectionLayer))
                {
                    isChasing = true;
                    currentState = EnemyState.Chasing;
                }
            }
        }
        else
        {
            isChasing = false;
        }
    }

    private void UpdateBehavior()
    {
        switch (currentState)
        {
            case EnemyState.Patrolling:
                if (!isChasing)
                    Patrol();
                break;
            case EnemyState.Chasing:
                if (isChasing)
                    Chase();
                else
                    currentState = EnemyState.Patrolling;
                break;
        }
    }

    private void Patrol()
    {
        if (patrolPoints.Length == 0)
            return;

        agent.speed = patrolSpeed;
        agent.SetDestination(patrolPoints[currentPatrolPoint]);

        if (agent.remainingDistance < stoppingDistance && !agent.hasPath || agent.velocity.sqrMagnitude == 0f)
        {
            currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length;
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    private void Chase()
    {
        agent.speed = chaseSpeed;
        agent.SetDestination(player.position);

        if (agent.remainingDistance < stoppingDistance)
        {
            // Player caught!
            CatchPlayer();
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    private void CatchPlayer()
    {
        playerStats.TakeDamage((int)knockoutDamage);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

public class PatrolPoint : MonoBehaviour
{
    // Marker class for patrol points
}
