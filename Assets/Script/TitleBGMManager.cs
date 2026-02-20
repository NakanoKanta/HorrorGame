using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class TitleBGMManager : MonoBehaviour
{
    AudioSource bgm;

    [Header("タイトルBGM")]
    [SerializeField] AudioClip titleBGM;

    void Awake()
    {
        bgm = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        // 現在シーンがタイトルなら再生
        if (IsTitleScene(SceneManager.GetActiveScene().name))
            PlayBGM();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (IsTitleScene(scene.name))
        {
            // タイトルに戻ったら最初から再生
            PlayBGM();
        }
        else
        {
            // ゲームシーンなどでは停止
            StopBGM();
        }
    }

    bool IsTitleScene(string sceneName)
    {
        return sceneName.ToLower().Contains("title"); // シーン名に"title"が含まれる場合
    }

    void PlayBGM()
    {
        if (bgm == null || titleBGM == null) return;

        bgm.clip = titleBGM;
        bgm.loop = true;
        bgm.Play();
    }

    void StopBGM()
    {
        if (bgm == null) return;
        bgm.Stop();
    }
}