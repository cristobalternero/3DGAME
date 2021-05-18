using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : TakeDoDamage
{
    [Header("References")]
    public NavMeshAgent agent;
    private Animator anim;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    [Header("Patrolling")]
    public float walkPointRange;
    private bool walkPointSet;
    private Vector3 walkPoint;

    [Header("Attack")]
    public float timeBetweenAttacks;
    private bool alreadyAttacked;

    [Header("States")]
    public float sightRange;
    private bool playerInSightRange;
    float chaseDistance = 1.3f;

    public bool isDead = false;
    public bool isReactinghit = false;
    public bool canAttack = true;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }
   
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        float distance = Vector3.Distance(transform.position, player.position);
        //Checking for sight 
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        anim.SetFloat("Speed", agent.velocity.magnitude);

        if (!playerInSightRange && !isDead && !isReactinghit)
        {
            Patroling();
        }
        if (playerInSightRange && !isDead && !isReactinghit)
        {
            ChasePlayer();
            
        }
        if (playerInSightRange && distance <= chaseDistance && !isDead && !isReactinghit)
        {
            AttackPlayer();
        }
        if (isDead)
        {
            agent.updatePosition = false;
        }
        if (isReactinghit)
        {
            isReactinghit = false;
        }
    }

    private void Patroling()
    {
        
        if (!walkPointSet)
        {
            SearhWalkPoint();
        }

        if (walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if(distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }

    private void SearhWalkPoint()
    {
        //Calculating random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        anim.SetFloat("Speed", agent.velocity.magnitude);
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(player.position);
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            
            alreadyAttacked = true;
            PlayerController.Instance.TakingDamage(damage);
            anim.SetTrigger("Attack");
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    protected override void ExecuteDeathAnim()
    {
        isDead = true;
        anim.SetTrigger("isDeathCop");
    }

    protected override void ExecuteHitAnim()
    {
        isReactinghit = true;
        anim.SetTrigger("isReacting");
    }

}
