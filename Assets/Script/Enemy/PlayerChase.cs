using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChase : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] string playerTag = "Player";

    [Header("Chase Settings")]
    [SerializeField] float detectRange = 6f;
    [SerializeField] float chaseSpeed = 4f;
    [SerializeField] float viewAngle = 90f;
    [SerializeField] float loseTargetTime = 3f;

    Rigidbody2D rb;
    EnemyPatrol patrol;
    Transform player;

    bool isChasing = false;
    float lostTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        patrol = GetComponent<EnemyPatrol>();
        player = GameObject.FindGameObjectWithTag(playerTag)?.transform;

        if (player == null)
            Debug.LogWarning("PlayerChase: Player not found in scene!");
    }

    void Update()
    {
        // 停止中なら動かさない
        if (patrol != null && !patrol.canMove)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        if (player == null) return;

        // プレイヤーを発見したかチェック
        if (CanSeePlayer())
        {
            if (!isChasing)
            {
                isChasing = true;
                BGMManager.Instance?.PlayAlertBGM(); //警戒BGM再生
                // Debug.Log("チェイス開始！");
            }
            lostTimer = 0f;
        }
        else if (isChasing)
        {
            lostTimer += Time.deltaTime;
            if (lostTimer >= loseTargetTime)
            {
                isChasing = false;
                lostTimer = 0f;
                patrol.PickRandomDirection();
                BGMManager.Instance?.ResumeNormalBGM(); //通常BGMに戻す
                // Debug.Log("チェイス解除。パトロールに戻ります。");
            }
        }

        // チェイス中はプレイヤー方向に移動
        if (isChasing)
        {
            Vector2 dir = (player.position - (Vector3)transform.position).normalized;
            dir = SnapToAxis(dir);
            patrol.SetDirection(dir);
            rb.velocity = dir * chaseSpeed;
        }
        else
        {
            // パトロール速度で移動
            rb.velocity = patrol.CurrentDirection * patrol.PatrolSpeed;
        }
    }

    // プレイヤーを視界内で発見できるか判定
    bool CanSeePlayer()
    {
        Vector2 toPlayer = player.position - transform.position;
        if (toPlayer.magnitude > detectRange) return false;

        Vector2 forward = patrol.CurrentDirection != Vector2.zero ? patrol.CurrentDirection : Vector2.right;
        return Vector2.Angle(forward, toPlayer.normalized) < viewAngle / 2f;
    }

    // 移動方向をX/Y軸にスナップ（斜め移動禁止）
    Vector2 SnapToAxis(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            return new Vector2(Mathf.Sign(dir.x), 0);
        else
            return new Vector2(0, Mathf.Sign(dir.y));
    }

    // シーンビューに視界を描画
    void OnDrawGizmosSelected()
    {
        if (patrol == null) return;

        Vector2 forward = patrol.CurrentDirection != Vector2.zero ? patrol.CurrentDirection : Vector2.right;
        Vector3 pos = transform.position;

        // 中心線
        Gizmos.color = new Color(1f, 0f, 0f, 0.3f);
        Gizmos.DrawLine(pos, pos + (Vector3)(forward.normalized * detectRange));

        // 視界角の両端
        float halfAngle = viewAngle / 2f;
        Vector2 leftDir = Quaternion.Euler(0, 0, halfAngle) * forward;
        Vector2 rightDir = Quaternion.Euler(0, 0, -halfAngle) * forward;
        Gizmos.DrawLine(pos, pos + (Vector3)(leftDir.normalized * detectRange));
        Gizmos.DrawLine(pos, pos + (Vector3)(rightDir.normalized * detectRange));

        // 半透明円（range）
        Gizmos.color = new Color(1f, 0f, 0f, 0.1f);
        Gizmos.DrawWireSphere(pos, detectRange);
    }
}
