using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] float m_walkSpeed = 1f;
    Vector2 m_lastMovedDirection;
    SpriteRenderer m_sprite;
    Animator m_anim;
    Rigidbody2D m_rb;

    public bool canMove = true; // 停止フラグ

    void Start()
    {
        m_sprite = GetComponent<SpriteRenderer>();
        m_rb = GetComponent<Rigidbody2D>();
        m_anim = GetComponent<Animator>();
    }

    void OnEnable()
    {
        var pauseMgr = FindObjectOfType<PauseManager>();
        if (pauseMgr != null)
            pauseMgr.OnPauseResume += HandlePause;
    }

    void OnDisable()
    {
        var pauseMgr = FindObjectOfType<PauseManager>();
        if (pauseMgr != null)
            pauseMgr.OnPauseResume -= HandlePause;
    }

    void HandlePause(bool isPaused)
    {
        canMove = !isPaused;
        if (!canMove) m_rb.velocity = Vector2.zero;
    }

    void Update()
    {
        if (!canMove)
        {
            m_rb.velocity = Vector2.zero;
            return;
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector2 dir = AdjustInputDirection(h, v);

        m_rb.velocity = dir * m_walkSpeed;

        if (dir.x != 0) m_sprite.flipX = (dir.x < 0);

        Animate(dir.x, dir.y);

        m_lastMovedDirection = dir;
    }

    Vector2 AdjustInputDirection(float inputX, float inputY)
    {
        Vector2 dir = new Vector2(inputX, inputY);

        if (m_lastMovedDirection == Vector2.zero)
        {
            if (dir.x != 0 && dir.y != 0) dir.y = 0;
        }
        else if (m_lastMovedDirection.x != 0) dir.y = 0;
        else if (m_lastMovedDirection.y != 0) dir.x = 0;

        return dir;
    }

    void Animate(float inputX, float inputY)
    {
        if (m_anim == null) return;

        if (inputX != 0) m_anim.Play("WalkRight");
        else if (inputY > 0) m_anim.Play("WalkUp");
        else if (inputY < 0) m_anim.Play("WalkDown");
        else
        {
            if (m_lastMovedDirection.x != 0) m_anim.Play("IdleRight");
            else if (m_lastMovedDirection.y > 0) m_anim.Play("IdleBack");
            else if (m_lastMovedDirection.y < 0) m_anim.Play("IdleFront");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Die();
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Die();
        }
    }
    void Die()
    {
        if (!canMove) return; // 多重呼び出し防止

        canMove = false;
        m_rb.velocity = Vector2.zero;

        // GameManager を通してゲームオーバー処理
        GameManager.Instance.GameOver();
    }
}
