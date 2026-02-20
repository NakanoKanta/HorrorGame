using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    [SerializeField] KeyCode clearKey = KeyCode.E; // 長押しキー
    [SerializeField] float holdTime = 2f;          // 長押し時間
    [SerializeField] Text messageText;             // メッセージ表示用UI
    float holdCounter = 0f;
    bool isPlayerNear = false;
    private void Start()
    {
        if (messageText != null)
            messageText.gameObject.SetActive(false); // 初期は非表示
    }

    private void Update()
    {
        if (isPlayerNear)
        {
            if (ItemManager.Instance.HasEnoughItems())
            {
                // メッセージ非表示
                if (messageText != null) messageText.gameObject.SetActive(false);

                // クリア判定
                if (Input.GetKey(clearKey))
                {
                    Debug.Log("クリア！");
                    GameManager.Instance.GameClear();
                }
            }
            else
            {
                // 足りないときはメッセージ表示
                if (messageText != null)
                {
                    messageText.gameObject.SetActive(true);
                    messageText.text = "カギがかかっている";
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerNear = false;
            holdCounter = 0f;

            // プレイヤーが離れたら非表示に
            if (messageText != null) messageText.gameObject.SetActive(false);
        }
    }
}
