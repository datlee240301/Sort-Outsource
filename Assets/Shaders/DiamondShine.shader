Shader "Custom/DiamondHSVGlow"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _HueShift ("Hue Shift Speed", Float) = 1.0
        _Saturation ("Saturation", Float) = 1.0
        _Value ("Value (Brightness)", Float) = 1.0
        _GlowColor ("Glow Color", Color) = (1,1,1,1)
        _GlowStrength ("Glow Strength", Float) = 2.0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off
        Lighting Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _HueShift;
            float _Saturation;
            float _Value;
            float4 _GlowColor;
            float _GlowStrength;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            // RGB → HSV
            float3 RGBtoHSV(float3 c)
            {
                float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
                float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));
                float d = q.x - min(q.w, q.y);
                float e = 1e-10;
                return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
            }

            // HSV → RGB
            float3 HSVtoRGB(float3 c)
            {
                float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
                float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
                return c.z * lerp(K.xxx, saturate(p - K.xxx), c.y);
            }

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float alpha = col.a;

                // HSV giữ nguyên Hue
                float3 hsv = RGBtoHSV(col.rgb);

                float flicker = 0.85 + 0.15 * sin(_Time.y * 4.0); // sáng chớp chớp nhẹ
                hsv.y *= _Saturation;
                hsv.z *= _Value * flicker;

                float3 shifted = HSVtoRGB(hsv);

                // Glow nhẹ theo alpha
                float glow = pow(alpha, 0.5);
                float3 glowColor = _GlowColor.rgb * glow * _GlowStrength;

                float3 finalColor = shifted + glowColor;
                return fixed4(finalColor, alpha);
            }

            ENDCG
        }
    }
}
