Shader "Custom/Fx/AdditiveOrder" {
    Properties {  
    _Color ("颜色", Color) = (1,1,1,1)
    _MainTex ("Base (RGB)", 2D) = "white" {} 
    _VNumber ("行数", float) = 1.0
    _UNumber ("列数", float) = 1.0
    _UVOrder ("帧序列", range(0.0,1.0)) = 0.0
}  
    SubShader {
      Tags {
      "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
     CGPROGRAM
	#pragma surface surf Lambert decal:add

      struct Input {
          float2 uv_MainTex;
          float4 color:COLOR;
      };
	float4 _Color;
	float _UNumber;
	float _VNumber;
	float _UVOrder;
    sampler2D _MainTex;
    void surf (Input IN, inout SurfaceOutput o) {  
    half4 c = tex2D (_MainTex, (IN.uv_MainTex+float2(floor(_UVOrder*_VNumber*_UNumber),-1.0-floor(_VNumber*_UVOrder)))/float2(_UNumber,_VNumber));  
    o.Emission = c.rgb * _Color.rgb * IN.color.a;
		}  
      ENDCG
    } 
    Fallback "Diffuse"
  }