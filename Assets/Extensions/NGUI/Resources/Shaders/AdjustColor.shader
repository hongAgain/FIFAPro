Shader "Custom/AdjustColor"
{
	Properties
	{
		//_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Channel ("Base (Alpha)", 2D) = "white" {}
		_NormalMap ("NormalMap", 2D) = "bump" {}
		_SkinColor ("Skin", Color) = ( 1, 1, 1, 1 )
		_UniformColor ("Uniform", Color) = ( 1, 1, 1, 1 )
		_Amount ("Amount", Range(1, 100)) = 1
	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 200

		Cull off

		CGPROGRAM
		#pragma surface surf Lambert

		//fixed4 _Color;
		sampler2D _MainTex;
		sampler2D _Channel;
		sampler2D _NormalMap;
		fixed4 _SkinColor;
		fixed4 _UniformColor;
		fixed _Amount;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutput o) {
			//fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			fixed4 channel = tex2D(_Channel, IN.uv_MainTex);
			fixed4 mix = lerp(lerp(_SkinColor, fixed4(1, 1, 1, 1), channel.r), _UniformColor, channel.g);
			c *= mix;
			o.Albedo = c.rgb * _Amount;
			o.Alpha = c.a;
			o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_MainTex));
		}
		ENDCG
	}

	Fallback "VertexLit"
}