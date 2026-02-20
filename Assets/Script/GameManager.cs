using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI")]
    [SerializeField] GameObject clearImage;
    [SerializeField] GameObject gameOverImage;
    [SerializeField] GameObject returnButton;
    [SerializeField] GameObject startMessageUI;
    [SerializeField] float startDelay = 2f;

    [Header("参照")]
    [SerializeField] PlayerController player;
    [SerializeField] AudioSource bgm;

    EnemyPatrol[] enemies;
    bool messageActive = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        enemies = FindObjectsOfType<EnemyPatrol>();
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();

        ResetGameState();
        StartCoroutine(StartSequence());
    }

    void Start()
    {
        enemies = FindObjectsOfType<EnemyPatrol>();
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();

        ResetGameState();
        StartCoroutine(StartSequence());
    }

    IEnumerator StartSequence()
    {
        SetAllCanMove(false);

        messageActive = true;
        if (startMessageUI != null) startMessageUI.SetActive(true);

        float timer = 0f;
        while (timer < startDelay && messageActive)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        HideStartMessage();
    }

    public void SkipStartMessage()
    {
        if (!messageActive) return;
        HideStartMessage();
        SetAllCanMove(true);
    }

    void HideStartMessage()
    {
        if (startMessageUI != null) startMessageUI.SetActive(false);
        SetAllCanMove(true);
        messageActive = false;
    }

    public void GameClear()
    {
        StopGame();
        if (clearImage != null) clearImage.SetActive(true);
        if (returnButton != null) returnButton.SetActive(true);
        BGMManager.Instance?.StopAllBGM();
    }

    public void GameOver()
    {
        StopGame();
        if (gameOverImage != null) gameOverImage.SetActive(true);
        if (returnButton != null) returnButton.SetActive(true);
        BGMManager.Instance?.StopAllBGM();
    }

    void StopGame()
    {
        SetAllCanMove(false);
    }

    void SetAllCanMove(bool canMove)
    {
        // プレイヤー
        if (player != null)
        {
            player.canMove = canMove;
            var rb = player.GetComponent<Rigidbody2D>();
            if (rb != null && !canMove) rb.velocity = Vector2.zero;
        }

        // 敵
        if (enemies != null)
        {
            foreach (var enemy in enemies)
            {
                if (enemy != null)
                {
                    enemy.canMove = canMove;
                    var rb = enemy.GetComponent<Rigidbody2D>();
                    if (rb != null && !canMove) rb.velocity = Vector2.zero;
                }
            }
        }
    }

    void ResetGameState()
    {
        if (clearImage != null) clearImage.SetActive(false);
        if (gameOverImage != null) gameOverImage.SetActive(false);
        if (returnButton != null) returnButton.SetActive(false);
        if (startMessageUI != null) startMessageUI.SetActive(false);

        messageActive = false;
        SetAllCanMove(true);
    }
}
