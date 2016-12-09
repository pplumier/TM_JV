Shader "Custom/DetectRayIntersection" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1) 
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
		half _Radius;
		uniform fixed4 _Center[1000]; 
		half _Size;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			o.Albedo = _Color;
			for(int i=0; i<_Size; i++){
				float d = distance(_Center[i], IN.worldPos);
				float dN = 1 - saturate(d / _Radius);;
				if (dN >= 0.30)
					o.Albedo = half3(0,0,0);
				else if (dN > 0.25 && dN < 0.3)
					o.Albedo = half3(1,1,1);
			}	
			o.Alpha = _Color.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
