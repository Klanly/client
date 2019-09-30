Shader "Custom/Scene/Rain" {
    Properties {  
    _Color ("颜色", Color) = (1,1,1,1)
    _MainTex ("Base (RGB)", 2D) = "white" {} 
    _SpeedU ("速度", range(-2,2)) = 0.0
    _SpeedV ("速度", range(-2,2)) = -1.0
}  
    SubShader {
      Tags {
      "Queue"="Transparent"
      "IgnoreProjector"="True"
      "RenderType"="Transparent"
      }
     CGPROGRAM  
#pragma surface surf Lambert decal:add

      struct Input {
          float2 uv_MainTex;
      };
	float4 _Color;
	float _SpeedU;
	float _SpeedV;
    sampler2D _MainTex;
    
    void surf (Input IN, inout SurfaceOutput o) {  
    half4 c = tex2D (_MainTex, IN.uv_MainTex + float2(_SpeedU,_SpeedV) * _Time.y);  
    o.Emission = c.rgb * _Color.rgb;
		}  
      ENDCG
    } 
    Fallback "Diffuse"
  }