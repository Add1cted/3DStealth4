// Upgrade NOTE: replaced 'UNITY_INSTANCE_ID' with 'UNITY_VERTEX_INPUT_INSTANCE_ID'
// Upgrade NOTE: upgraded instancing buffer 'FogAdditive' to new syntax.

// Unity built-in shader source. Copyright (c) 2016 Unity Technologies. MIT license (see license.txt)

// Simplified Additive Particle shader. Differences from regular Additive Particle one:
// - no Tint color
// - no Smooth particle support
// - no AlphaTest
// - no ColorMask

Shader "Mobile/Particles/AdditiveInstancing" {
Properties {
	_MainTex ("Albedo", 2D) = "white" {}
    _Color ("Main Color", Color) = (1.0,1.0,1.0,1.0)
    _Exposure("Exposure", Range(0.0, 10.0)) = 1.0
    _Glow ("Faked Glow Amount", Range(0.01,20.0)) = 2.5
}
 
Category {
    Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
    Blend One One
    ColorMask RGB
    Cull Off Lighting Off ZWrite Off
 
    SubShader {
        Pass {
       
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_particles
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
			#pragma target 4.0
            sampler2D _MainTex;
           
            struct appdata_t {
                float4 vertex : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };
 
            struct v2f {
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                #ifdef SOFTPARTICLES_ON
                float4 projPos : TEXCOORD1;
                #endif
            };
 
            float4 _MainTex_ST;
            fixed _Exposure;
 
            
 			UNITY_INSTANCING_BUFFER_START(FogAdditive)
                UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
#define _Color_arr FogAdditive
                UNITY_INSTANCING_BUFFER_END(FogAdditive)

            v2f vert (appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                #ifdef SOFTPARTICLES_ON
                o.projPos = ComputeScreenPos (o.vertex);
                COMPUTE_EYEDEPTH(o.projPos.z);
                #endif
                o.color = v.color;
                o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);
                return o;
            }
 
            sampler2D _CameraDepthTexture;
            float _Glow;
           
            fixed4 frag(v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                #ifdef SOFTPARTICLES_ON
                float sceneZ = LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos))));
                float partZ = i.projPos.z;
                float fade = saturate (_Glow * (sceneZ-partZ));
                i.color.a *= fade;
                #endif
               
                float4 c = UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _Color);
                clip(c.a < 0.01f ? -1:1 );
                half4 prev = i.color * c * tex2D(_MainTex, i.texcoord) * _Exposure;
                prev.rgb *= prev.a;
                return prev;
            }
            ENDCG
        }
    }
}
Fallback "Mobile/Particles/Additive"
}


