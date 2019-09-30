// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Shader created with Shader Forge Beta 0.23 
// Shader Forge (c) Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:0.23;sub:START;pass:START;ps:lgpr:1,nrmq:1,limd:1,blpr:2,bsrc:0,bdst:0,culm:2,dpts:2,wrdp:False,uamb:True,mssp:True,ufog:False,aust:True,igpj:True,qofs:0,lico:1,qpre:3,flbk:,rntp:2,lmpd:False,lprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,hqsc:True,hqlp:False,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0;n:type:ShaderForge.SFN_Final,id:1,x:32207,y:32660|emission-55-OUT;n:type:ShaderForge.SFN_Fresnel,id:2,x:32994,y:33174|EXP-127-OUT;n:type:ShaderForge.SFN_Tex2d,id:6,x:33409,y:32712,ptlb:node_6,tex:ee9fc75272e0d4149930f8eafa873bd7,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Add,id:19,x:32760,y:32806|A-63-OUT,B-78-OUT;n:type:ShaderForge.SFN_VertexColor,id:33,x:32740,y:32605;n:type:ShaderForge.SFN_Multiply,id:55,x:32511,y:32757|A-33-RGB,B-19-OUT;n:type:ShaderForge.SFN_Multiply,id:63,x:32984,y:32631|A-64-RGB,B-94-OUT;n:type:ShaderForge.SFN_Color,id:64,x:33315,y:32486,ptlb:tex_color,c1:0.5,c2:0.5,c3:0.5,c4:0.5;n:type:ShaderForge.SFN_Color,id:76,x:33568,y:32935,ptlb:Fresnel_color,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Multiply,id:78,x:32994,y:32983|A-76-RGB,B-2-OUT;n:type:ShaderForge.SFN_Multiply,id:94,x:33162,y:32712|A-64-A,B-6-RGB;n:type:ShaderForge.SFN_Vector1,id:121,x:33379,y:33253,v1:5;n:type:ShaderForge.SFN_Multiply,id:127,x:33168,y:33227|A-135-OUT,B-121-OUT;n:type:ShaderForge.SFN_Subtract,id:135,x:33288,y:33071|A-143-OUT,B-76-A;n:type:ShaderForge.SFN_Vector1,id:143,x:33379,y:33005,v1:1;proporder:6-64-76;pass:END;sub:END;*/

Shader "Shader Forge/feser_test" {
    Properties {
        _node6 ("node_6", 2D) = "white" {}
        _texcolor ("tex_color", Color) = (0.5,0.5,0.5,0.5)
        _Fresnelcolor ("Fresnel_color", Color) = (0.5,0.5,0.5,1)
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend One One
            Cull Off
            ZWrite Off
            
            Fog {Mode Off}
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers xbox360 ps3 flash 
            #pragma target 3.0
            uniform sampler2D _node6; uniform float4 _node6_ST;
            uniform float4 _texcolor;
            uniform float4 _Fresnelcolor;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 vertexColor : COLOR;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o;
                o.uv0 = v.uv0;
                o.vertexColor = v.vertexColor;
                o.normalDir = mul(float4(v.normal,0), unity_WorldToObject).xyz;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
/////// Normals:
                float3 normalDirection =  i.normalDir;
                
                float nSign = sign( dot( viewDirection, i.normalDir ) ); // Reverse normal if this is a backface
                i.normalDir *= nSign;
                normalDirection *= nSign;
                
////// Lighting:
////// Emissive:
                float4 node_64 = _texcolor;
                float2 node_169 = i.uv0;
                float4 node_76 = _Fresnelcolor;
                float3 emissive = (i.vertexColor.rgb*((node_64.rgb*(node_64.a*tex2D(_node6,TRANSFORM_TEX(node_169.rg, _node6)).rgb))+(node_76.rgb*pow(1.0-max(0,dot(normalDirection, viewDirection)),((1.0-node_76.a)*5.0)))));
                float3 finalColor = emissive;
/// Final Color:
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
