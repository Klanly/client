// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Shader created with Shader Forge v1.25 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.25;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:2865,x:33072,y:32547,varname:node_2865,prsc:2|alpha-581-OUT,refract-7925-OUT;n:type:ShaderForge.SFN_Tex2d,id:5964,x:32075,y:32862,ptovrint:True,ptlb:Normal Map,ptin:_BumpMap,varname:_BumpMap,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:bbab0a6f7bae9cf42bf057d8ee2755f6,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Slider,id:1813,x:32075,y:33131,ptovrint:False,ptlb:Intensity,ptin:_Intensity,varname:_Metallic_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.5863248,max:1;n:type:ShaderForge.SFN_Vector1,id:581,x:32814,y:32742,varname:node_581,prsc:2,v1:0;n:type:ShaderForge.SFN_Append,id:8892,x:32285,y:32879,varname:node_8892,prsc:2|A-5964-R,B-5964-G;n:type:ShaderForge.SFN_Multiply,id:2997,x:32482,y:32913,varname:node_2997,prsc:2|A-8892-OUT,B-1813-OUT;n:type:ShaderForge.SFN_VertexColor,id:6506,x:32591,y:32708,varname:node_6506,prsc:2;n:type:ShaderForge.SFN_Multiply,id:7925,x:32859,y:32878,varname:node_7925,prsc:2|A-6506-A,B-4910-OUT;n:type:ShaderForge.SFN_Multiply,id:4910,x:32681,y:32995,varname:node_4910,prsc:2|A-2997-OUT,B-8569-R;n:type:ShaderForge.SFN_Tex2d,id:8569,x:32448,y:33159,ptovrint:False,ptlb:Alpha,ptin:_Alpha,varname:node_8569,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;proporder:5964-8569-1813;pass:END;sub:END;*/

Shader "Shader Forge/Heat_Distort_test" {
    Properties {
        _BumpMap ("Normal Map", 2D) = "bump" {}
        _Alpha ("Alpha", 2D) = "white" {}
        _Intensity ("Intensity", Range(0, 1)) = 0.5863248
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform sampler2D _BumpMap; uniform float4 _BumpMap_ST;
            uniform float _Intensity;
            uniform sampler2D _Alpha; uniform float4 _Alpha_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 screenPos : TEXCOORD3;
                float4 vertexColor : COLOR;
                UNITY_FOG_COORDS(4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.screenPos = o.pos;
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                #if UNITY_UV_STARTS_AT_TOP
                    float grabSign = -_ProjectionParams.x;
                #else
                    float grabSign = _ProjectionParams.x;
                #endif
                i.normalDir = normalize(i.normalDir);
                i.screenPos = float4( i.screenPos.xy / i.screenPos.w, 0, 0 );
                i.screenPos.y *= _ProjectionParams.x;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 _BumpMap_var = UnpackNormal(tex2D(_BumpMap,TRANSFORM_TEX(i.uv0, _BumpMap)));
                float4 _Alpha_var = tex2D(_Alpha,TRANSFORM_TEX(i.uv0, _Alpha));
                float2 sceneUVs = float2(1,grabSign)*i.screenPos.xy*0.5+0.5 + (i.vertexColor.a*((float2(_BumpMap_var.r,_BumpMap_var.g)*_Intensity)*_Alpha_var.r));
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
////// Lighting:
                float3 finalColor = 0;
                fixed4 finalRGBA = fixed4(lerp(sceneColor.rgb, finalColor,0.0),1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
