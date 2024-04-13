using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MeleeAI : MonoBehaviour
{
    Rigidbody2D rb;
    public float timeBetweenAttacks = 0f;
    private float timeToAttack = 0;
    Animator animator;

    [SerializeField] protected State state { get; set; }
    [SerializeField] private bool isAttacking;
    public bool isInRange { get; private set; }

    public Transform currentTarget;
    protected Transform playerTransform;
    float attackRange = 1;
    protected EnemyMovementAI pathfinding;
    // Start is called before the first frame update

    // Update is called once per frame
    protected enum State
    {
        Idle,
        ChaseTarget,
        Attacking
    }

    /*
    private AIPath pathfinding;
    private AIDestinationSetter destination;
    */
   
    // Start is called before the first frame update
    void Start()
    {
        pathfinding = GetComponent<EnemyMovementAI>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        state = State.Idle;
        currentTarget = transform;
        attackRange = GetComponent<Enemy>().attackRange;

        rb = transform.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (getClosestEnemy() != null)
        {
            //pathfinding.desiredVelocity = Vector3.zero;
        }
        switch (state)
        {
            default:
            case State.Idle:
                FindTarget();
                break;

            case State.ChaseTarget:
                currentTarget = playerTransform;
                if (GetDistanceToTarget() <= attackRange)
                    state = State.Attacking;
                break;

            case State.Attacking:
                if (isInRange && isAttacking == false && timeToAttack >= timeBetweenAttacks)
                {
                    isAttacking = true;
                    Attack();
                }
                else
                {
                    state = State.ChaseTarget;
                    timeToAttack += Time.deltaTime;
                }
                break;
        }
    }

    private void LateUpdate()
    {
        isInRange = GetDistanceToTarget() <= attackRange;
        /*
        destination.target = currentTarget;
        */
        pathfinding.canMove = !isAttacking && !isInRange;

    }


    protected void Attack()
    {
        Vector2 initialPos = transform.position;
        rb.AddForce(currentTarget.position * 1000f);
        timeToAttack = 0;
        isAttacking = false;
    }

    void FindTarget()
    {
        float targetRange = 50f;
        if (Vector3.Distance(transform.position, GameObject.Find("Player").transform.position) < targetRange)
            state = State.ChaseTarget;
    }

    float GetDistanceToTarget()
    {
        return Vector3.Distance(transform.position, currentTarget.position);
    }

    Transform getClosestEnemy()
    {
        float dist = Mathf.Infinity;
        GameObject closest = null;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < dist)
            {
                dist = Vector3.Distance(transform.position, enemy.transform.position);
                closest = enemy;
            }

        }
        if (closest == null)
            return null;
        return closest.transform;
    }
}
