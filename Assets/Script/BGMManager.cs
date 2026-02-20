using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance { get; private set; }

    [SerializeField] AudioSource bgmNormal;   // ゲーム中通常BGM
    [SerializeField] AudioSource bgmAlert;    // チェイスBGM
    [SerializeField] AudioSource bgmPause;    // ポーズBGM

    bool wasNormalPlaying = false;
    bool wasAlertPlaying = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // シーン切り替え時に呼ばれるイベントを登録
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // シーンがロードされたとき
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StopAllBGM();

        // シーン名に "Game" が入っていたら通常BGM開始
        if (scene.name.Contains("Game"))
        {
            if (bgmNormal != null) bgmNormal.Play();
        }
    }

    // チェイス開始
    public void PlayAlertBGM()
    {
        if (bgmNormal != null && bgmNormal.isPlaying) bgmNormal.Pause();
        if (bgmAlert != null && !bgmAlert.isPlaying) bgmAlert.Play();
    }

    // チェイス終了
    public void ResumeNormalBGM()
    {
        if (bgmAlert != null && bgmAlert.isPlaying) bgmAlert.Stop();
        if (bgmNormal != null && !bgmNormal.isPlaying) bgmNormal.Play();
    }

    // ポーズ中
    public void PlayPauseBGM()
    {
        wasNormalPlaying = bgmNormal != null && bgmNormal.isPlaying;
        wasAlertPlaying = bgmAlert != null && bgmAlert.isPlaying;

        if (bgmNormal != null && bgmNormal.isPlaying) bgmNormal.Pause();
        if (bgmAlert != null && bgmAlert.isPlaying) bgmAlert.Pause();

        if (bgmPause != null && !bgmPause.isPlaying) bgmPause.Play();
    }

    // ポーズ解除
    public void ResumeGameplayBGM()
    {
        if (bgmPause != null && bgmPause.isPlaying) bgmPause.Stop();

        if (wasNormalPlaying && bgmNormal != null) bgmNormal.UnPause();
        if (wasAlertPlaying && bgmAlert != null) bgmAlert.UnPause();
    }

    // 全停止
    public void StopAllBGM()
    {
        if (bgmNormal != null) bgmNormal.Stop();
        if (bgmAlert != null) bgmAlert.Stop();
        if (bgmPause != null) bgmPause.Stop();

        wasNormalPlaying = false;
        wasAlertPlaying = false;
    }
}
