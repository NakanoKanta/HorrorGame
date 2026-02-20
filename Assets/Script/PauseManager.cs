using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    bool _isPaused = false;
    public Action<bool> OnPauseResume = default;

    [SerializeField] GameObject pauseMenuUI; // ← InspectorでUIをアタッチ

    void Update()
    {
        if (Input.GetButtonDown("Cancel")) // Escキー
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        _isPaused = !_isPaused;

        // UIの表示/非表示
        if (pauseMenuUI != null)
            pauseMenuUI.SetActive(_isPaused);

        // BGM切替は BGMManager に任せる
        if (_isPaused)
            BGMManager.Instance?.PlayPauseBGM();
        else
            BGMManager.Instance?.ResumeGameplayBGM();

        // プレイヤー・敵への停止通知
        OnPauseResume?.Invoke(_isPaused);

        // 時間を止めたいなら Time.timeScale を利用
        Time.timeScale = _isPaused ? 0f : 1f;
    }
}
