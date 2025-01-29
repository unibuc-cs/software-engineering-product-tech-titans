using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CreatureAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;
    public Player playerObj;

    public bool attacks;

    public LayerMask whatIsGround, whatIsPlayer;

    //Patroling
    public Vector3 walkPoint;
    public Vector3 lastwalkPoint;
    [SerializeField] public bool walkPointSet;
    public float walkPointRange;
    private Vector3 lastPosition;
    public float idleTimer = 0;
    private float idleTimeThreshold = 5;

    public float idleTimer2 = 0;
    private float idleTimeThreshold2 = 30;
    private Vector3 playerVisionPoint = new Vector3(0, 0.5f, 0);
    private Vector3 demonVisionPoint = new Vector3(0, 1, 0);
    public float totalDistanceTraveled = 0f;
    public float floor = 0;
    private Vector3 position30sAgo;
    private float lastAttackTime = 0.0f;

    public float attackCooldown = 5.0f;


    //States
    [SerializeField] public float sightRange, attackRange;

    public bool playerInSightRange, playerInAttackRange;
    public bool isWalking, isAttacking;

    //Attack parameters
    [SerializeField] public float attackDelay;


    private void Awake()
    {
        player = GameObject.Find("PlayerObj").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        isWalking = true;
        isAttacking = false;
        lastPosition = transform.position;
        position30sAgo = transform.position;
    }


    private void Update()
    {
        CheckSightAndAttackRange();

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInSightRange && playerInAttackRange) AttackPlayer();

        ResolvePotentialStuck();

    }

    private void CheckSightAndAttackRange()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        //aici am incercat sa fac vederea cu un offset
        //if (playerInSightRange)
        //{
        //    float distanceToPlayer = Vector3.Distance(transform.position + demonVisionPoint, player.position + playerVisionPoint);
        //    Vector3 directionToPlayer = ((player.position + playerVisionPoint) - (transform.position + demonVisionPoint)).normalized;
        //    if (Physics.Raycast(transform.position + demonVisionPoint, directionToPlayer, out RaycastHit hit, sightRange, whatIsGround | whatIsPlayer))
        //    {
        //        Debug.Log(hit.collider.name);
        //        if (!hit.collider.name.StartsWith("Player"))
        //        {
        //            playerInSightRange = false;
        //        }
        //    }
        //}

        if (playerInSightRange)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, sightRange, whatIsGround | whatIsPlayer))
            {
                if (hit.collider.name.StartsWith("Door") || hit.collider.name.StartsWith("Hall") || hit.collider.name.StartsWith("Wood"))
                {
                    playerInSightRange = false;
                }
            }
        }

        if (playerInSightRange && agent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            playerInSightRange = false;
        }
        if (playerInSightRange && agent.hasPath == false)
        {
            playerInSightRange = false;
        }
    }

    private void ResolvePotentialStuck()
    {
        if (Vector3.Distance(transform.position, lastPosition) == 0)
        {
            idleTimer += Time.deltaTime;
        }
        else
        {
            idleTimer = 0;
        }

        lastPosition = transform.position;
        idleTimer2 += Time.deltaTime;

        if (idleTimer2 >= idleTimeThreshold2)
        {
            idleTimer2 = 0f;
            totalDistanceTraveled = Vector3.Distance(position30sAgo, transform.position);
            position30sAgo = transform.position;

            if (totalDistanceTraveled < 10)
            {
                agent.enabled = false;
                transform.position = new Vector3(1, floor * 5, -2);
                agent.enabled = true;
                floor = 1 - floor;
            }
        }

        if (idleTimer >= idleTimeThreshold)
        {
            idleTimer = 0f;
            idleTimer2 = 0f;
            position30sAgo = transform.position;

            agent.enabled = false;
            transform.position = new Vector3(1, floor * 5, -2);
            agent.enabled = true;
            floor = 1 - floor;
        }
    }

    private void AttackPlayer()
    {
        if (Time.time - lastAttackTime < attackCooldown)
        {
            return;
        }

        if (isAttacking == false)
        {
            playerObj.hp--;
        }

        isAttacking = true;
        isWalking = false;
        walkPointSet = false;

        lastAttackTime = Time.time;

        // Define Attack behaviour once we have Health System etc.
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        float rotationSpeed = 10f;
        bool isRotating = true;

        while (isRotating)
        {
            Vector3 facingDirection = (player.position - transform.position).normalized;

            // Ignore the Y-axis rotation by zeroing out the Y component
            facingDirection.y = 0;

            // Create the target rotation towards the player
            Quaternion targetRotation = Quaternion.LookRotation(facingDirection);

            // Smoothly rotate the creature towards the player
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // Check if the rotation is complete (or close enough to be considered done)
            //print(targetRotation);
            //print(transform.rotation);
            //print(Quaternion.Angle(transform.rotation, targetRotation));

            if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
            {
                isRotating = false;
            }

            yield return null; // Wait for the next frame
        }


        // Wait for the attack delay before setting isAttacking to false
        yield return new WaitForSeconds(attackDelay);

        // After the delay, set isAttacking to false
        isAttacking = false;
    }

    private void Patroling()
    {
        attacks = false;
        if (isAttacking == true)
        {
            isAttacking = false;
        }

        isWalking = true;
        isAttacking = false;

        if (!walkPointSet || walkPoint == transform.position)
        {
            idleTimer = 0f;
            SearchWalkPoint();
        }

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        if (agent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            walkPointSet = false;
            SearchWalkPoint();
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = UnityEngine.Random.Range(-walkPointRange, walkPointRange);
        float randomX = UnityEngine.Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        attacks = false;
        if (isAttacking == true)
        {
            isAttacking = false;
        }

        isWalking = true;
        isAttacking = false;
        walkPointSet = false;

        agent.SetDestination(player.position);
    }
    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
