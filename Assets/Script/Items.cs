using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    [SerializeField] AudioClip getSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ItemManager.Instance.AddItem();
            if (getSound != null)
            {
                AudioSource.PlayClipAtPoint(getSound, transform.position);
            }
            Destroy(gameObject);
        }
    }
}
