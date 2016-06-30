Shader "Custom/Uniform"
{
	Properties
	{
		_Color ("Main Color", Color) = (1,1,1,1)
		_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 0)
		_Shininess ("Shininess", Range (0.01, 1)) = 0.078125
		_MainTex ("Base (RGB) TransGloss (A)", 2D) = "white" {}
		_TransGlossTex ("TransGloss", 2D) = "black" {}
		_BumpMap ("Normalmap", 2D) = "bump" {}
		_Amount ("Amount", Range(1, 100)) = 1
	}

	SubShader
	{
		Tags {"RenderType"="Opaque"}
		LOD 200
		Cull off

		CGPROGRAM
		#pragma surface surf BlinnPhong

		sampler2D _MainTex;
		sampler2D _TransGlossTex;
		sampler2D _BumpMap;
		fixed4 _Color;
		half _Shininess;
		half _Amount;

		struct Input {
			float2 uv_MainTex;
			float2 uv_TransGlossTex;
			float2 uv_BumpMap;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 tex = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 alpha = tex2D(_TransGlossTex, IN.uv_TransGlossTex);
			o.Albedo = tex.rgb * _Color.rgb * _Amount;
			o.Gloss = alpha;
			o.Alpha = alpha * _Color.a;
			o.Specular = _Shininess;
			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
		}
		ENDCG
	}

	Fallback "VertexLit"
}