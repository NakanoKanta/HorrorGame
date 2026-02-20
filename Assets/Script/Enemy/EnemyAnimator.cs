using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;

    /// <summary>直前に動いていた方向</summary>
    Vector2 lastMovedDir = Vector2.down; // 初期は下向き待機

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Vector2 dir = rb.velocity; // Rigidbody の移動方向から判断

        if (dir.magnitude > 0.05f) // 移動しているとき
        {
            if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            {
                if (dir.x > 0)
                {
                    anim.Play("WalkRight");
                    lastMovedDir = Vector2.right;
                }
                else
                {
                    anim.Play("WalkLeft");
                    lastMovedDir = Vector2.left;
                }
            }
            else
            {
                if (dir.y > 0)
                {
                    anim.Play("WalkUp");
                    lastMovedDir = Vector2.up;
                }
                else
                {
                    anim.Play("WalkDown");
                    lastMovedDir = Vector2.down;
                }
            }
        }
        else // 停止中（Idle）
        {
            if (lastMovedDir == Vector2.right)
                anim.Play("IdleRight");
            else if (lastMovedDir == Vector2.left)
                anim.Play("IdleLeft");
            else if (lastMovedDir == Vector2.up)
                anim.Play("IdleBack");
            else
                anim.Play("IdleFront");
        }
    }
}
