Shader "Custom/HD Panini Projection"
{
    Properties
    {
        _MainTex ("Source", 2D) = "white" {}
        _Distance ("Distance", Float) = 1.0
        _Crop ("Crop to fit", Range(0,1)) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline"}

        Pass
        {
            Name "PaniniProjection"

            ZTest Always
            ZWrite Off
            Cull Off

            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            float4 _MainTex_TexelSize;

            float _Distance;
            float _Crop;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            Varyings Vert(Attributes input)
            {
                Varyings o;
                o.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                o.uv = input.uv;
                return o;
            }

            float2 PaniniProjection(float2 uv)
            {
                float2 centered = uv * 2.0 - 1.0;

                float x = centered.x;
                float y = centered.y;

                float d = _Distance;

                float denom = d + sqrt(1.0 + x * x);
                float paniniX = x / denom;

                float2 result;
                result.x = paniniX;
                result.y = y / denom;

                result *= _Crop;

                return result * 0.5 + 0.5;
            }

            float4 Frag(Varyings input) : SV_Target
            {
                float2 uv = PaniniProjection(input.uv);

                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);

                return col;
            }

            ENDHLSL
        }
    }
}