using UnityEngine.AI;
using UnityEngine;
using System.Collections;

public class IA : MonoBehaviour
{
    public NavMeshAgent agent; 
    private Transform player;
    public Animator animator;
    public EnemyLaser laserGun;

    [Header("Patrol")]
    public float patrolRadius;

    [Header("Attack & detection")]
    public float visionRange;
    public float attackRange;

    private Vector3 patrolPoint;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                Debug.LogWarning("No se encontró ningún objeto con la etiqueta 'Player'.");
            }
        }

        if (laserGun != null && player != null)
        {
            laserGun.player = player;
        }

        SetNewPatrolPoint();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            Attack();
        }
        else if (distanceToPlayer <= visionRange)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    void Patrol()
    {
        agent.isStopped = false;

        if (!agent.hasPath || agent.remainingDistance < 1f)
        {
            SetNewPatrolPoint(); 
        }

        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);     
        animator.SetBool("isRunning", false);
    }

    void ChasePlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", true);        
        animator.SetBool("isAttacking", false);
    }

    void Attack()
    {
        agent.isStopped = true;
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0f;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 3f);

        Vector3 origin = transform.position + Vector3.up * 1.5f;
        Vector3 target = player.position + Vector3.up * 1.5f;
        Vector3 shootDir = (target - origin).normalized;
        float distance = Vector3.Distance(origin, target);

        RaycastHit hit;
        if (Physics.Raycast(origin, shootDir, out hit, distance))
        {
            if (hit.transform == player)
            {
                laserGun.TryShoot();
            }
            else
            {
                Debug.Log("Bloqueado: no tiene línea de visión al jugador.");
            }
        }

        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isAttacking", true);
    }

    void SetNewPatrolPoint()
    {
        agent.isStopped = false;
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1))
        {
            patrolPoint = hit.position;
            agent.SetDestination(patrolPoint);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}