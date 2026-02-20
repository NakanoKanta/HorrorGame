using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeByCoroutine : MonoBehaviour
{
    [SerializeField] Image _fadeImage = default;
    [SerializeField] float _fadeTime = 1;
    float _timer = 0;
    public void Fade()
    {
        Debug.Log("コルーチンによる Fade 開始");
        StartCoroutine(FadeRoutine());
    }

    IEnumerator FadeRoutine()
    {
        while (true)
        {
            _timer += Time.deltaTime;
            Color c = _fadeImage.color;
            c.a = _timer / _fadeTime;
            _fadeImage.color = c;
            if (_timer > _fadeTime)
            {
                Debug.Log("コルーチンによる Fade 完了");
                yield break;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}