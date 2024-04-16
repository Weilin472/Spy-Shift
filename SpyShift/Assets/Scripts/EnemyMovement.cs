using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform player;
    public float chaseRange = 10f;
    public float moveSpeed = 5f;
    public float changeDirectionInterval = 3f;
    public LayerMask obstacleLayer;

    private Rigidbody enemyRigidbody;
    private bool isChasing = false;
    private Vector3 randomDirection;
    private float nextDirectionChangeTime = 0f;

    void Start()
    {
        enemyRigidbody = GetComponent<Rigidbody>();
        ChooseRandomDirection();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange)
        {
            isChasing = true;
            ChasePlayer();
        }
        else
        {
            isChasing = false;
            if (Time.time >= nextDirectionChangeTime)
            {
                ChooseRandomDirection();
                nextDirectionChangeTime = Time.time + changeDirectionInterval;
            }
            MoveRandomly();
        }
    }

    void ChasePlayer()
    {
        if (isChasing)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            Vector3 movePosition = transform.position + direction * moveSpeed * Time.deltaTime;
            enemyRigidbody.MovePosition(movePosition);

            // Rotate towards the player
            transform.LookAt(player);
        }
    }

    void MoveRandomly()
    {
        Vector3 movePosition = transform.position + randomDirection * moveSpeed * Time.deltaTime;
        enemyRigidbody.MovePosition(movePosition);
        if (Time.time >= nextDirectionChangeTime)
        {
            ChooseRandomDirection();
            nextDirectionChangeTime = Time.time + changeDirectionInterval;
        }
    }

    void ChooseRandomDirection()
    {
        randomDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            // Change direction upon colliding with a wall
            ChooseRandomDirection();
        }
    }
}