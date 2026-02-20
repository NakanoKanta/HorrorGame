using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UIHoleFollow : MonoBehaviour
{
    public Transform player;            // プレイヤー Transform をセット
    public Canvas canvas;               // Canvas（省略可：自動で取得）
    [Range(0f, 1f)] public float radius = 0.15f;
    [Range(0f, 0.5f)] public float feather = 0.05f;

    RectTransform rt;
    Image img;
    Material matInstance;

    void Start()
    {
        rt = GetComponent<RectTransform>();
        img = GetComponent<Image>();

        if (canvas == null)
            canvas = GetComponentInParent<Canvas>();

        if (img == null)
        {
            Debug.LogError("UIHoleFollow: Image コンポーネントが必要です。");
            enabled = false;
            return;
        }

        // Image に割り当てられたマテリアルをインスタンス化（共有マテリアルを直接書き換えない）
        if (img.material != null)
        {
            matInstance = Instantiate(img.material);
            img.material = matInstance;
        }
        else
        {
            Debug.LogError("UIHoleFollow: Image にマテリアルがアサインされていません。");
            enabled = false;
            return;
        }

        // 初期値
        matInstance.SetFloat("_Radius", radius);
        matInstance.SetFloat("_Feather", feather);
    }

    void Update()
    {
        if (player == null || matInstance == null) return;

        // プレイヤーのワールド座標をスクリーン座標に変換
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, player.position);

        // Canvas のレンダリングモードに応じてカメラを渡す
        Camera cam = (canvas.renderMode == RenderMode.ScreenSpaceCamera) ? canvas.worldCamera : null;

        // スクリーン座標 -> RectTransform のローカル座標
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, screenPoint, cam, out localPoint);

        // localPoint を 0..1 の正規化座標に変換（pivot を考慮）
        Vector2 pivot = rt.pivot;
        float normalizedX = (localPoint.x + rt.rect.width * pivot.x) / rt.rect.width;
        float normalizedY = (localPoint.y + rt.rect.height * pivot.y) / rt.rect.height;
        Vector2 normalized = new Vector2(normalizedX, normalizedY);

        // 値を0..1にクランプ（画面外でも安定動作させたい場合）
        normalized = Vector2.ClampMagnitude(normalized, 2f); // optional: 外に出ても極端な値は避ける
        normalized.x = Mathf.Clamp01(normalized.x);
        normalized.y = Mathf.Clamp01(normalized.y);

        // シェーダーに渡す（Vector4で）
        matInstance.SetVector("_Center", new Vector4(normalized.x, normalized.y, 0, 0));

        // 半径・フェザーはエディタで変えたら Update に反映できるように
        matInstance.SetFloat("_Radius", radius);
        matInstance.SetFloat("_Feather", feather);
    }
}
