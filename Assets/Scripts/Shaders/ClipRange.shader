Shader "Custom/ClipRange"
{
    Properties
    {
        _Color ("Color", Color) = (0.75, 0.53, 0.15, 0.8)
        _GroundMin ("Ground Min", Vector) = (0, 0, 0, 0)
        _GroundMax ("Ground Max", Vector) = (0, 0, 0, 0)
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent+1" "RenderPipeline" = "UniversalPipeline" }
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        // BlendOp Max

        Stencil
        {
            Ref 1
            Comp NotEqual
            Pass Replace
        }

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            float4 _Color;
            float2 _GroundMin;
            float2 _GroundMax;

            struct Attributes
            {
                float4 positionOS : POSITION;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 worldPos : TEXCOORD0;
            };

            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);

            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                float4 _BaseMap_ST;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionCS = TransformWorldToHClip(OUT.worldPos);
                return OUT;
            }

            float4 frag(Varyings IN) : SV_Target
            {
                if (IN.worldPos.x < _GroundMin.x - 0.05 || IN.worldPos.x > _GroundMax.x + 0.05 ||
                    IN.worldPos.z < _GroundMin.y - 0.1 || IN.worldPos.z > _GroundMax.y + 0.1)
                {
                    clip(-1);
                }
                return _Color;
            }
            ENDHLSL
        }
    }
}
