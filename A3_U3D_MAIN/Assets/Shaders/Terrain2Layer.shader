Shader "Custom/Terrain/Terrain2Layer" {
    Properties {  
    
    _TerrainTex01 ("Terrain01", 2D) = "white" {} 
	_TerrainTex02 ("Terrain02", 2D) = "white" {} 
	_BlendTex ("Blend", 2D) = "white" {}   
}  
    SubShader {
      Tags { "RenderType" = "Opaque" }
     CGPROGRAM  
#pragma surface surf Lambert
      struct Input {
          float2 uv_TerrainTex01;
          float2 uv_TerrainTex02;
          float2 uv_BlendTex;
      };
    sampler2D _TerrainTex01;
    sampler2D _TerrainTex02;    
    sampler2D _BlendTex;      
    void surf (Input IN, inout SurfaceOutput o) {  
    half4 c1 = tex2D (_TerrainTex01, IN.uv_TerrainTex01);
	half4 c2 = tex2D (_TerrainTex02, IN.uv_TerrainTex02);
	half4 bd = tex2D (_BlendTex, IN.uv_BlendTex);
    o.Albedo = lerp(c1,c2,bd);
    	}  
      ENDCG
    } 
    Fallback "Diffuse"
  }

