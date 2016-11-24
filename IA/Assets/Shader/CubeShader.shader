Shader "Custom/CubeShader" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Center ("Center", Vector) = (0,0,0,0)
		_Radius ("Radius", Float) = 0.5
	}
	SubShader {
		Tags { "RenderType"="Transparent" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
		};

		fixed4 _Color;
		fixed4 _Center;
		half _Radius;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			float d = distance(_Center, IN.worldPos);
			float dN = 1 - saturate(d / (_Radius * abs(_SinTime[3])));
			
			if (dN >= 0.30)
				o.Albedo = half3(0,0,0);
			else if (dN > 0.25 && dN < 0.3)
				o.Albedo = half3(1,1,1);
			else
				o.Albedo = half3(1,0,0);
			o.Alpha = _Color.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
