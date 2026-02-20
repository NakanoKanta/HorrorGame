Shader "Unlit/UI_HoleMask"
{
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (0,0,0,0.8)
        _Center ("Center", Vector) = (0.5, 0.5, 0, 0)
        _Radius ("Radius", Range(0,1)) = 0.15
        _Feather ("Feather", Range(0,0.5)) = 0.05
    }
    SubShader {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "CanUseSpriteAtlas"="True" }
        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float4 color  : COLOR;
                float2 texcoord : TEXCOORD0;
            };
            struct v2f {
                float4 vertex : SV_POSITION;
                fixed4 color  : COLOR;
                float2 uv     : TEXCOORD0;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            float2 _Center;
            float _Radius;
            float _Feather;

            v2f vert(appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.texcoord;
                o.color = v.color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                fixed4 tex = tex2D(_MainTex, i.uv) * _Color;

                // --- 正円補正 ---
                float aspect = _ScreenParams.x / _ScreenParams.y; // 画面の幅 ÷ 高さ
                float2 diff = i.uv - _Center;
                diff.x *= aspect; // 横方向を補正
                float d = length(diff);

                // --- 半径とフェザーで透明度を決める ---
                float e0 = _Radius - _Feather;
                float e1 = _Radius + _Feather;
                float mask = smoothstep(e0, e1, d);

                tex.a *= mask;
                return tex;
            }
            ENDCG
        }
    }
}
