using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WomanZombie : TakeDoDamage
{
    NavMeshAgent agent;
    public Transform target;
    Animator animWoman;
    public bool isDead = false;
    public bool isReactinghit = false;

    [SerializeField] float chaseDistance = 1.3f;
    [SerializeField] float turnSpeed = 10f;
    [SerializeField] float attackTime;
    public bool canAttack = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();  
        animWoman = GetComponent<Animator>();     
    }

    private float speed;

    void Update()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        float distance = Vector3.Distance(transform.position, target.position);

        if (isReactinghit)
        {
            //agent.updatePosition = false;
            //agent.speed = 0;
            animWoman.SetBool("isWalking", false);
            animWoman.SetBool("isAttacking", false);
            agent.isStopped = true;

        }
        else if (distance > chaseDistance && !isDead)
        {
            ChasePlayer();
        }
        else if(distance <= chaseDistance && !isDead && canAttack)
        {
            AttackPlayer();
        }
        else if (isDead)
        {
            agent.isStopped = true;
            agent.updatePosition = false;
            animWoman.SetBool("isWalking", false);
            animWoman.SetBool("isAttacking", false);
        }
    }

    private IEnumerator startMovingAgainRoutine;

    private IEnumerator StartMovingAgain(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        isReactinghit = false;
        agent.isStopped = false;
    }

    protected override void ExecuteDeathAnim()
    {
        isDead = true;
        animWoman.SetTrigger("isDeath");
    }

    protected override void ExecuteHitAnim()
    {
        isReactinghit = true;
        float delayTime = 0.9f;
        if (startMovingAgainRoutine != null)
        {
            StopCoroutine(startMovingAgainRoutine);
        }
        startMovingAgainRoutine = StartMovingAgain(delayTime);
        StartCoroutine(startMovingAgainRoutine);
        animWoman.SetTrigger("isReacting");
    }

    void ChasePlayer()
    {
        //agent.updateRotation = true;
        //agent.updatePosition = true;
        agent.isStopped = false;
        agent.SetDestination(target.position);
        animWoman.SetBool("isWalking", true);
        animWoman.SetBool("isAttacking", false);
    }

    void AttackPlayer()
    {
        //agent.updateRotation = true;
        agent.isStopped = false;
        Vector3 direction = target.position - transform.position;
        direction.y = 0;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);
        //agent.updatePosition = true;
        animWoman.SetBool("isWalking", false);
        animWoman.SetBool("isAttacking", true);
        StartCoroutine(AttackTime());
    }

    IEnumerator AttackTime()
    {
        canAttack = false;
        yield return new WaitForSeconds(1.5f);
        PlayerController.Instance.TakingDamage(damage);
        yield return new WaitForSeconds(attackTime);
        canAttack = true;
    }
}
