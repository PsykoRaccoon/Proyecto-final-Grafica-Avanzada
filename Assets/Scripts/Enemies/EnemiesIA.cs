using UnityEngine.AI;
using UnityEngine;
using System.Collections;

public class IA : MonoBehaviour
{
    public NavMeshAgent agent; //tienes que ponerle un navmeshagent a los enemigos para que caminen y un navmeshsurface al piso 
    public Transform player;
    public Animator animator; //las animaciones sacalas de mixamo como le hice con el jugador, si tienes dudas, checa como esta el jugador o mandame mensaje

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
        if (!agent.hasPath || agent.remainingDistance < 1f)
        {
            SetNewPatrolPoint(); 
        }

        animator.SetBool("isWalking", true);
        animator.SetBool("isAttacking", false);     //haz que ataquen de cerca, que no disparen ni nada 
        animator.SetBool("isRunning", false);
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", true);        // en el animator controller de los robots, haces los parametros booleanos para activarlos, ponles el mismo nombre que aqui
        animator.SetBool("isAttacking", false);
    }

    void Attack()
    {
        agent.ResetPath();
        animator.SetBool("isWalking", false);
        animator.SetBool("isRunning", false);
        animator.SetBool("isAttacking", true);
    }

    void SetNewPatrolPoint()
    {
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