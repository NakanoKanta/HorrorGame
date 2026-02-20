using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyPatrol : MonoBehaviour
{
    public Vector2 CurrentDirection { get; private set; }
    [SerializeField] float patrolSpeed = 2f;
    public float PatrolSpeed => patrolSpeed;

    Rigidbody2D rb;
    public bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        PickRandomDirection();
    }

    void Update()
    {
        if (!canMove)
        {
            if (rb != null) rb.velocity = Vector2.zero;
            return;
        }

        if (rb != null) rb.velocity = CurrentDirection * patrolSpeed;
    }

    public void PickRandomDirection()
    {
        Vector2[] dirs = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        CurrentDirection = dirs[Random.Range(0, dirs.Length)];
    }

    public void SetDirection(Vector2 dir)
    {
        CurrentDirection = dir;
    }
}