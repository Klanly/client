Shader "Custom/Terrain/Terrain4Layer" {
    Properties {  
    
    _TerrainTex01 ("Terrain01", 2D) = "black" {} 
	_TerrainTex02 ("Terrain02", 2D) = "black" {} 
	_TerrainTex03 ("Terrain03", 2D) = "black" {} 
	_TerrainTex04 ("Terrain04", 2D) = "black" {} 
	_BlendTex ("Blend", 2D) = "white" {}   
}  
    SubShader {
      Tags { "RenderType" = "Opaque" }
     CGPROGRAM  
	#pragma surface surf Lambert
	#pragma target 3.0
	
      struct Input {
          float2 uv_TerrainTex01;
          float2 uv_TerrainTex02;
          float2 uv_TerrainTex03;
          float2 uv_TerrainTex04; 
          float2 uv_BlendTex;
      };
    sampler2D _TerrainTex01;
    sampler2D _TerrainTex02;    
    sampler2D _TerrainTex03; 
    sampler2D _TerrainTex04;
    sampler2D _BlendTex;      
    void surf (Input IN, inout SurfaceOutput o) {  
    half4 c1 = tex2D (_TerrainTex01, IN.uv_TerrainTex01)*tex2D (_BlendTex, IN.uv_BlendTex).r; 
	half4 c2 = tex2D (_TerrainTex02, IN.uv_TerrainTex02)*tex2D (_BlendTex, IN.uv_BlendTex).g; 
	half4 c3 = tex2D (_TerrainTex03, IN.uv_TerrainTex03)*tex2D (_BlendTex, IN.uv_BlendTex).b;
	half4 c4 = tex2D (_TerrainTex04, IN.uv_TerrainTex04)*tex2D (_BlendTex, IN.uv_BlendTex).a;
    o.Albedo = saturate(c1.rgb+c2.rgb+c3.rgb+c4.rgb);
    	}  
      ENDCG
    } 
    Fallback "Diffuse"
  }