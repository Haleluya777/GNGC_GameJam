Shader "GNGC/Sprite Outline"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (0.02, 0.02, 0.025, 1)
        _OutlineThickness ("Outline Thickness", Range(0, 0.25)) = 0.035
        _OutlineAlphaCutoff ("Outline Alpha Cutoff", Range(0, 1)) = 0.1
        _OutlineMask ("Outline Mask", 2D) = "white" {}
        _UseOutlineMask ("Use Outline Mask", Float) = 0
        _OutlineMaskCutoff ("Outline Mask Cutoff", Range(0, 1)) = 0.5

        [HideInInspector] _Color ("Tint", Color) = (1,1,1,1)
        [HideInInspector] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [HideInInspector] _AlphaTex ("External Alpha", 2D) = "white" {}
        [HideInInspector] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "RenderType" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
            "CanUseSpriteAtlas" = "True"
        }

        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            Name "Outline"
            Tags { "LightMode" = "SRPDefaultUnlit" }

            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/Core2D.hlsl"

            #pragma vertex OutlineVertex
            #pragma fragment OutlineFragment
            #pragma multi_compile_instancing

            struct Attributes
            {
                float3 positionOS : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                half4 color : COLOR;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_OutlineMask);
            SAMPLER(sampler_OutlineMask);
            float4 _MainTex_ST;
            float4 _OutlineMask_ST;
            half4 _OutlineColor;
            float _OutlineThickness;
            float _OutlineAlphaCutoff;
            float _UseOutlineMask;
            float _OutlineMaskCutoff;
            half4 _RendererColor;

            Varyings OutlineVertex(Attributes v)
            {
                Varyings o = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                #ifdef UNITY_INSTANCING_ENABLED
                v.positionOS = UnityFlipSprite(v.positionOS, unity_SpriteFlip);
                #endif

                float2 outlineDirection = v.positionOS.xy;
                float directionLength = max(length(outlineDirection), 0.0001);
                v.positionOS.xy += outlineDirection / directionLength * _OutlineThickness;

                o.positionCS = TransformObjectToHClip(v.positionOS);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color * _RendererColor;

                #ifdef UNITY_INSTANCING_ENABLED
                o.color *= unity_SpriteColor;
                #endif

                return o;
            }

            half4 OutlineFragment(Varyings i) : SV_Target
            {
                half alpha = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv).a * i.color.a;
                clip(alpha - _OutlineAlphaCutoff);

                half mask = SAMPLE_TEXTURE2D(_OutlineMask, sampler_OutlineMask, TRANSFORM_TEX(i.uv, _OutlineMask)).a;
                clip(lerp(1.0h, mask - _OutlineMaskCutoff, step(0.5, _UseOutlineMask)));

                half4 outline = _OutlineColor;
                outline.a *= alpha;
                return outline;
            }
            ENDHLSL
        }

        Pass
        {
            Name "Sprite"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/Core2D.hlsl"
            #if defined(DEBUG_DISPLAY)
            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/InputData2D.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/SurfaceData2D.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Debug/Debugging2D.hlsl"
            #endif

            #pragma vertex SpriteVertex
            #pragma fragment SpriteFragment
            #pragma multi_compile_instancing
            #pragma multi_compile_fragment _ DEBUG_DISPLAY

            struct Attributes
            {
                float3 positionOS : POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 color : COLOR;
                float2 uv : TEXCOORD0;
                #if defined(DEBUG_DISPLAY)
                float3 positionWS : TEXCOORD2;
                #endif
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            float4 _MainTex_ST;
            float4 _Color;
            half4 _RendererColor;

            Varyings SpriteVertex(Attributes v)
            {
                Varyings o = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                #ifdef UNITY_INSTANCING_ENABLED
                v.positionOS = UnityFlipSprite(v.positionOS, unity_SpriteFlip);
                #endif

                o.positionCS = TransformObjectToHClip(v.positionOS);
                #if defined(DEBUG_DISPLAY)
                o.positionWS = TransformObjectToWorld(v.positionOS);
                #endif
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color * _Color * _RendererColor;

                #ifdef UNITY_INSTANCING_ENABLED
                o.color *= unity_SpriteColor;
                #endif

                return o;
            }

            half4 SpriteFragment(Varyings i) : SV_Target
            {
                half4 mainTex = i.color * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                #if defined(DEBUG_DISPLAY)
                SurfaceData2D surfaceData;
                InputData2D inputData;
                half4 debugColor = 0;

                InitializeSurfaceData(mainTex.rgb, mainTex.a, surfaceData);
                InitializeInputData(i.uv, inputData);
                SETUP_DEBUG_DATA_2D(inputData, i.positionWS);

                if (CanDebugOverrideOutputColor(surfaceData, inputData, debugColor))
                {
                    return debugColor;
                }
                #endif

                return mainTex;
            }
            ENDHLSL
        }
    }

    Fallback "Universal Render Pipeline/2D/Sprite-Unlit-Default"
}
