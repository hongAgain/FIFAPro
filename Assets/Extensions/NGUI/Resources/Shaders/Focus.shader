Shader "Custom/Focus"
{
	Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
		//_MaskTex ("Mask (RGB), Alpha (A)", 2D) = "transparent" {}
		_Params ("WorldPos&W&H", Vector) = (0.5, 0.5, 0, 0)
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Offset -1, -1
			Fog { Mode Off }
			//ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			//sampler2D _MaskTex;
			half4 _Params;
			//half4 _MaskTex_TexelSize;
			//#define screenPos = half2(_Params.x, _Params.y);
			//#define width = _Params.z;
			//#define height = _Params.w;

			struct appdata_t
			{
				float4 vertex : POSITION;
				half4 color : COLOR;
				float2 uv1 : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : POSITION;
				half4 color : COLOR;
				float2 uv1 : TEXCOORD0;
			};

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.color = v.color;
				o.uv1 = v.uv1;
				return o;
			}

			half4 frag (v2f IN) : COLOR
			{
				half2 screenPos = half2(_Params.x, _Params.y) + half2(_ScreenParams) / 2;
				//half width = 1 / _MaskTex_TexelSize.x;
				//half height = 1 / _MaskTex_TexelSize.y;
				half width = _Params.z;
				half height = _Params.w;

				half2 uv1ToScreen = half2(IN.uv1.x * _ScreenParams.x, IN.uv1.y * _ScreenParams.y);
				half2 subPos = uv1ToScreen - screenPos + half2(width, height) / 2;
				half2 subUv = half2(subPos.x / width, subPos.y / height);
				
				half4 main_clr = tex2D(_MainTex, IN.uv1) * IN.color;
				if (subUv.x < 0 || subUv.x > 1 || subUv.y < 0 || subUv.y > 1)
				{
					return main_clr;
				}
				else
				{
					//half4 mask_clr = tex2D(_MaskTex, uv2);
					return half4(0, 0, 0, 0);
				}
			}
			ENDCG
		}
	}
}