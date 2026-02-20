using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAvoidance : MonoBehaviour
{
    [SerializeField] string wallTag = "Wall";
    [SerializeField] float checkDistance = 1f;
    [SerializeField] float waitTime = 0.5f;

    Collider2D col;
    EnemyPatrol patrol;

    void Start()
    {
        col = GetComponent<Collider2D>();
        patrol = GetComponent<EnemyPatrol>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(wallTag))
        {
            patrol.SetDirection(Vector2.zero);
            StartCoroutine(ChangeDirectionAfterWait());
        }
    }

    System.Collections.IEnumerator ChangeDirectionAfterWait()
    {
        yield return new WaitForSeconds(waitTime);

        List<Vector2> possibleDirs = new List<Vector2>();
        Vector2[] dirs = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        Vector2 size = col.bounds.size * 0.9f;

        foreach (var dir in dirs)
        {
            RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, size, 0f, dir, checkDistance);
            bool blocked = false;
            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag(wallTag))
                {
                    blocked = true;
                    break;
                }
            }
            if (!blocked)
                possibleDirs.Add(dir);
        }

        if (possibleDirs.Count > 0)
            patrol.SetDirection(possibleDirs[Random.Range(0, possibleDirs.Count)]);
    }
}
