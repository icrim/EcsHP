Shader "Custom/HPShader"
{
    Properties
    { 
        _HealthColor("HealthColor", Color) = (1,0,0,1)
        _BgColor("BackgroundColor", Color) = (0,0,0,1)
        _BorderColor("BorderColor", Color) = (0,0,1,1)
        _BorderSizeX("BorderSizeX", Range(0,1)) = 0.1
        _BorderSizeY("BorderSizeY", Range(0,1)) = 0.1
        _Health("Health", Range(0,1)) = 1
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalRenderPipeline"
            "RenderType" = "Opaque"
        }
        Pass
        {
            HLSLPROGRAM

            #pragma target 4.5
            #pragma multi_compile _ DOTS_INSTANCING_ON

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            #pragma vertex vert
            #pragma fragment frag
            
            struct IData
            {
                float4 pos: POSITION;
                float4 tex: TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct VData
            {
                float4 pos: SV_POSITION;
                float4 tex: TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            CBUFFER_START(UnityPerMaterial)
                half4 _HealthColor;
                half4 _BgColor;
                half4 _BorderColor;
                float _BorderSizeX;
                float _BorderSizeY;
            CBUFFER_END

            #ifdef UNITY_DOTS_INSTANCING_ENABLED
            UNITY_DOTS_INSTANCING_START(MaterialPropertyMetadata)
                UNITY_DOTS_INSTANCED_PROP(float, _Health)
            UNITY_DOTS_INSTANCING_END(MaterialPropertyMetadata)
                #define _Health UNITY_ACCESS_DOTS_INSTANCED_PROP(float, _Health)
            #endif

            VData vert(IData i)
            {
                UNITY_SETUP_INSTANCE_ID(i);
                VData v;
                UNITY_TRANSFER_INSTANCE_ID(i, v);
                v.pos = TransformObjectToHClip(i.pos.xyz);
                v.tex = i.tex;
                return v;
            }

            half4 frag(VData v) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(v);
                #ifdef UNITY_DOTS_INSTANCING_ENABLED
                    float in_health = v.tex.x > 1 - _Health;
                #else
                    float in_health = 1;
                #endif
                half4 col = lerp(_BgColor, _HealthColor, in_health);
                float in_x_border = max(v.tex.x < _BorderSizeX, v.tex.x > 1 - _BorderSizeX);
                float in_y_border = max(v.tex.y < _BorderSizeY, v.tex.y > 1 - _BorderSizeY);
                col = lerp(col, _BorderColor, max(in_x_border, in_y_border));
                return col;
            }

            ENDHLSL
        }
    }
}
