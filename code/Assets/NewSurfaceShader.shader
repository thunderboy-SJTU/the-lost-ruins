Shader "Custom/NewSurfaceShader" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_BumpMap("Bumpmap", 2D) = "bump" {}
		_Outline("Outline", Range(0,1)) = 0.005
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_Tooniness("Tooniness", Range(0.1,15)) = 5
		_Conbine("Conbine",  Range(0.1,15)) = 5
		_Ramp("Ramp Texture", 2D) = "white" {}
	}

		CGINCLUDE
#include "UnityCG.cginc"  
		struct appdata {
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};

	struct v2f {
		float4 pos : POSITION;
		float4 color : COLOR;
	};

	sampler2D _MainTex;
	sampler2D _BumpMap;
	sampler2D _Ramp;
	float _Outline;
	float4 _OutlineColor;
	float _Tooniness;
	float _Conbine;
	fixed4 _Color;

	v2f vert(appdata v) {
		v2f o;
		v.vertex.xyz += v.normal * _Outline;
		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		o.color = _OutlineColor;
		return o;
	}
	ENDCG

		SubShader{
		Tags{ "RenderType" = "Opaque" }

		Pass{
			   Name "OUTLINE"
			   Tags{ "LightMode" = "Always" }
				   Cull Front
				   ZWrite On
				   ColorMask RGB
				   Blend SrcAlpha OneMinusSrcAlpha
		CGPROGRAM
#pragma vertex vert  
#pragma fragment frag  
			   half4 frag(v2f i) :COLOR{ return i.color; }
			   ENDCG
		   }
			CGPROGRAM
			#pragma surface surf Toon
			#pragma target 3.0


			struct Input {
				float2 uv_MainTex;
				float2 uv_BumpMap;
			};



			void surf(Input IN, inout SurfaceOutput o) {
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
				o.Albedo = (floor(c.rgb*_Conbine) / _Conbine);
				o.Alpha = c.a;

			}

			half4 LightingToon(SurfaceOutput s, half3 lightDir, half atten) {
				half4 color;
				half para = saturate(tex2D(_Ramp ,float2(dot(s.Normal, lightDir),0.8)));
				para = floor(para * _Tooniness) / _Tooniness;
				color.rgb = s.Albedo * _LightColor0.rgb * para * atten * 2;
				color.a = s.Alpha;
				return color;
			}
			ENDCG
	}
		FallBack "Diffuse"
}
