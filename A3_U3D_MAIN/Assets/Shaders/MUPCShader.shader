Shader "Custom/Char/MUPCShader" {
	Properties {
		_MainTex ("漫反射贴图", 2D) = "white" {}
		_MaskTex ("溶解流光R_透贴,G_Mask,B_流光", 2D) = "white" {}
		_SpeInt ("高光强度",range(0.0,2.0)) = 0.5
		_SpePow ("高光范围",range(1.0,8.0)) = 4.0
		_RimColor ("边光颜色" ,color) = (0.0,0.6,1.0,1.0)
		_RimScale ("边光强度",range(0.0,5.0)) = 2.59
		_Rimpow ("边光范围" ,range(0.01,0.8)) = 0.47
		_Movekind ("流光种类",range(0.0,1.5)) = 1.0
		_MoveColor01 ("流光颜色01" ,color) = (1.0,0.0,0.0,1.0)
		_MoveScale01 ("流光强度01",range(0.0,30.0)) = 1.0
		_MoveSpeed01 ("流光速度01",range(-1.0,1.0)) = 0.3
		_MoveColor02 ("流光颜色02" ,color) = (0.0,1.0,0.0,1.0)
		_MoveScale02 ("流光强度02",range(0.0,20.0)) = 1.0
		_MoveSpeed02 ("流光速度02",range(-1.5,1.5)) = 1.0
		
//		_MainTex ("漫反射贴图", 2D) = "white" {}
//		_MaskTex ("溶解流光R_透贴,G_Mask,B_流光", 2D) = "white" {}
//		_SpeInt ("高光强度",float) = 0.5
//		_SpePow ("高光范围",float) = 4.0
//		_RimColor ("边光颜色" ,color) = (0.0,0.6,1.0,1.0)
//		_RimScale ("边光强度",float) = 2.59
//		_Rimpow ("边光范围" ,float) = 0.47
//		_Movekind ("流光种类",range(0.0,1.0)) = 1.0
//		_MoveColor01 ("流光颜色01" ,color) = (1.0,0.0,0.0,1.0)
//		_MoveScale01 ("流光强度01",float) = 1.0
//		_MoveSpeed01 ("流光速度01",float) = 0.1
//		_MoveColor02 ("流光颜色02" ,color) = (0.0,1.0,0.0,1.0)
//		_MoveScale02 ("流光强度02",float) = 1.0
//		_MoveSpeed02 ("流光速度02",float) = 0.2
	}
	SubShader {

		Tags {"Queue"="Geometry" "IgnoreProjector"="True" "RenderType"="Opaque"}
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert
		#pragma target 3.0
				
		sampler2D _MainTex;
		sampler2D _MaskTex;
		float _SpeInt;
		float _SpePow;		
		float _RimScale;
		float4 _RimColor;
		float _Rimpow;
		float _Movekind;
		float4 _MoveColor01;
		float _MoveScale01;
		float _MoveSpeed01;
		float4 _MoveColor02;
		float _MoveScale02;
		float _MoveSpeed02;
		
		struct Input {
			float2 uv_MainTex;
			float2 uv_MaskTex;
			float3 viewDir;
			float3 worldNormal;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			half4 c = tex2D (_MainTex, IN.uv_MainTex);
			half4 c2 = tex2D (_MaskTex, IN.uv_MaskTex); 
			half nh = saturate(dot (IN.worldNormal, normalize(IN.viewDir)));
		    half3 RimColor = 1.0f/_Rimpow * saturate(_Rimpow - nh) * _RimScale * _RimColor * c.rgb;
		    half3 SpecColor = _SpeInt * pow(nh,_SpePow) * c.rgb;	  
		      
		    
			half3 MoveLight = 0.0f;
			half3 MoveColor = 1.0f;
			half4 MoveTex01 = tex2D (_MaskTex, float2(3.0f,2.0f) * IN.uv_MaskTex + float2(0.0 , _MoveSpeed01 * _Time.y) + 0.2f * c.rg).b;
			half4 MoveTex02 = 40.0f * saturate (frac((IN.uv_MaskTex.x + 0.1f * c.r) + _MoveSpeed02 * _Time.y) - 0.97f) * round (frac(0.51f * _MoveSpeed02 * _Time.y));
			
			if (_Movekind <= 0.5f)
			{
				MoveColor = lerp(_MoveColor01 , _MoveColor02 , 2.0f * abs(frac (5.0f * _MoveSpeed01 * _Time.y) - 0.5f));				
			}
			else if (_Movekind >= 1.0f)
			{
				MoveLight = c.rgb * c2.g * (0.5f * _MoveColor01 + _MoveColor01 * MoveTex01 * _MoveScale01 + _MoveColor02 * MoveTex02 * _MoveScale02);
			}			
			else
			{
				MoveLight =	c.rgb * c2.g *	(_MoveColor01 * MoveTex01 * _MoveScale01 + 0.5f * lerp(_MoveColor01 , _MoveColor02 , 2.0f * abs(frac (5.0f * _MoveSpeed02 * _Time.y) - 0.5f)));
			}
			o.Albedo = MoveColor * c.rgb;
			o.Emission = MoveLight + SpecColor + RimColor;

//			half4 MoveTex01 = tex2D (_MaskTex, IN.uv_MaskTex + float2(0.0 , _MoveSpeed01 * _Time.y)).b;
//			
//			half4 MoveTex02 = 50.0f * max( 0.0f , 0.02f - abs(IN.uv_MaskTex.x + 2.0f * frac(_MoveSpeed02 * _Time.y) - 0.5f));
//			
//			half3 MoveLight =round(_Movekind) * c.rgb * c2.g * (_MoveColor01 * MoveTex01 * _MoveScale01 + _MoveColor02 * MoveTex02 * _MoveScale02);
//			
//			half3 MoveColor = lerp(_MoveColor01 * _MoveScale01 , _MoveColor02 * _MoveScale02 , 2.0f * abs(frac (5.0f * _MoveSpeed01 * _Time.y) - 0.5f));
//			o.Albedo = (round(_Movekind)+ (1.0f - round(_Movekind)) * MoveColor) * c.rgb;
//			o.Emission = MoveLight + SpecColor + RimColor;			
			
			clip(c2.r - 0.5f);
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
