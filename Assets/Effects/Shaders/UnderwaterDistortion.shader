Shader "Custom/UnderwaterDistortion"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DistortionStrength ("Distortion Strength", Range(0,0.1)) = 0.02
        _Speed ("Wave Speed", Range(0, 5)) = 1
        _Color ("Tint Color", Color) = (0, 0.5, 0.7, 0.3)
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" "RenderType" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            float4 _Color;
            float _DistortionStrength;
            float _Speed;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {
                float wave = sin(i.uv.y * 20 + _Time.y * _Speed) * _DistortionStrength;
                float2 uvDistorted = i.uv + float2(wave, wave);
                float4 color = tex2D(_MainTex, uvDistorted) * _Color;
                return color;
            }
            ENDCG
        }
    }
}
