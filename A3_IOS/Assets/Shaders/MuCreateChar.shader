Shader "Custom/Char/MuCreateChar" {
    Properties {
      _MainTex ("基本贴图", 2D) = "white" {}
	  _LightTex ("闪光纹理", 2D) = "black" {}
	  _MaskTex ("流光蒙版",2D) = "white" {}
	  
	  _EnvTex ("环境贴图", CUBE) = "" {}
      _EnvColor ("环境色", Color) = (1, 0, 0, 1)
      _EnvIntensity ("环境反射强度", Float) = 1.0     
      _EnvSpeed ("环境反射旋转速度", Float) = 30.0
      
      _SpecularColor ("高光颜色", Color) = (0, 0.05, 1, 1)
      _SpIntensity ("高光反射强度", Float) = 2.0
      _SpSpeed ("高光流动速度", Float) = -3.0
      _SpInterval ("高光流动间隔", int) = 2.0
      
	  _FlashColor ("闪光颜色",Color) = (1, 1, 1, 1)
	  _FlashIntensity ("闪光强度",float) = 2.0
	  _FlashInterval ("闪光间隔", Float) = 3.0
    }
    SubShader {
			Tags { "RenderType" = "Opaque" }
			CGPROGRAM
			#pragma surface surf Lambert
			#pragma target 3.0
			
			struct Input {
				float2 uv_MainTex;
				float2 uv_MaskTex;
				float2 uv2_LightTex;
				float3 worldRefl;
			};
			sampler2D _MainTex;
			sampler2D _LightTex;
			sampler2D _MaskTex;
			samplerCUBE _EnvTex;			

			float4 _EnvColor;
			float _EnvIntensity;
			float _EnvSpeed;
			
			float4 _SpecularColor;
			float _SpIntensity;			
			float _SpSpeed;
			int _SpInterval;

	  		float4 _FlashColor;
	  	  	float _FlashIntensity;
	  		float _FlashInterval;

			void surf (Input IN, inout SurfaceOutput o) {				
				//diffuse texture map	                                                         
				float4 tex = tex2D (_MainTex, IN.uv_MainTex);
				float3 MaskTex = tex2D (_MaskTex, IN.uv_MaskTex);
          
				//environment texture map
				fixed rad = _Time.x * _EnvSpeed;
				float3 rotUV = float3(IN.worldRefl.z * sin(rad) + IN.worldRefl.x * cos(rad), IN.worldRefl.y, IN.worldRefl.z * cos(rad) - IN.worldRefl.x * sin(rad));
				float3 Env = texCUBE (_EnvTex, rotUV).rgb * _EnvColor.rgb * _EnvIntensity;
          
				//specular				
				fixed SpInv = frac( _Time.y / (_SpInterval + 1.0f / abs(_SpSpeed)) );	//间隔
				fixed Spspeed = frac( _SpSpeed * _Time.y );	//速度	
				
				fixed TmpInt = 1.0f; //原理：流光时间相对于间隔时间的长度比率，小于这个值，强度给1，保证走完一次，大于这个值，强度给0，保证间隔时间没有流光。
				if (SpInv >= (1.0f/(abs(_SpSpeed) * _SpInterval + 1.0f)))
				{
					TmpInt = 0.0f;
				}
				else
				{
					TmpInt = 1.0f;
				}						 
				float3 Sp = TmpInt * tex2D (_LightTex, (IN.uv2_LightTex + fixed2(Spspeed,0.0f))) * texCUBE (_EnvTex, rotUV).rgb * _SpecularColor * _SpIntensity;
				
				//flash
				fixed FlashInv = frac( _Time.y / (_FlashInterval + 0.033f) );
				fixed Flashspeed = frac(30.0f * _Time.y);
				
				fixed TmpFlh = 1.0f;
				if (FlashInv >= (1.0f/(30.0f * _FlashInterval + 1.0f)))
				{
					TmpFlh = 0.0f;
				}
				else
				{
					TmpFlh = 1.0f;
				}
				float3 Flash = TmpFlh * tex2D (_LightTex, (IN.uv2_LightTex + fixed2(Flashspeed,0.5f))) * texCUBE (_EnvTex, rotUV).rgb * _FlashColor * _FlashIntensity;
				
				o.Emission = tex + MaskTex.g * (Env + Sp + Flash);
				clip (tex.a - 0.5);
			}
			ENDCG
	} 
    Fallback "Diffuse"
}
