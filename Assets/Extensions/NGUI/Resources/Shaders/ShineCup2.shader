Shader "Hidden/Custom/ShineCup 2"
{
	Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
		_AlphaTex("Alpha Texture",2D) = "white" {}
        _BeginTime ("BeginTime", float) = 0
        _LoopTime ("LoopTime", float) = 0.6		
		_BoolZero ("BoolZero", int) = 1
		_BrightnessPower("Brightness Power",Range(0,5)) = 1
	}

	SubShader
	{
		LOD 200

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
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			sampler2D _AlphaTex;
            float4 _MainTex_ST;
			float4 _AlphaTex_ST;
            fixed _BeginTime;
            fixed _LoopTime;
			float _BrightnessPower;
			int _BoolZero;
			
			float4 _ClipRange0 = float4(0.0, 0.0, 1.0, 1.0);
			float4 _ClipArgs0 = float4(1000.0, 1000.0, 0.0, 1.0);
			float4 _ClipRange1 = float4(0.0, 0.0, 1.0, 1.0);
			float4 _ClipArgs1 = float4(1000.0, 1000.0, 0.0, 1.0);

			struct appdata_t
			{
				float4 vertex : POSITION;
				half4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex : POSITION;
				half4 color : COLOR;
				float2 uv : TEXCOORD0;
				float2 uvAlpha : TEXCOORD1;
				float4 worldPos : TEXCOORD2;
			};

			float2 Rotate (float2 v, float2 rot)
			{
				float2 ret;
				ret.x = v.x * rot.y - v.y * rot.x;
				ret.y = v.x * rot.x + v.y * rot.y;
				return ret;
			}

			v2f o;

			v2f vert (appdata_t v)
			{
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord,_MainTex);
				o.uvAlpha = TRANSFORM_TEX(v.texcoord,_AlphaTex);
				o.color = v.color;
				o.worldPos.xy = v.vertex.xy * _ClipRange0.zw + _ClipRange0.xy;
				o.worldPos.zw = Rotate(v.vertex.xy, _ClipArgs1.zw) * _ClipRange1.zw + _ClipRange1.xy;
				return o;
			}

			float inFlash(float angle,float2 uv,float xLength,int interval,int beginTime, float offX, float loopTime )
            {
                float brightness =0;
                float angleInRad = 0.0174444 * angle;
                float currentTime = _Time.y;
           
                int currentTimeInt = _Time.y/interval;
                currentTimeInt *=interval;
               
                float currentTimePassed = currentTime -currentTimeInt;
                if(currentTimePassed >beginTime)
                {
                    float xBottomLeftBound;
                    float xBottomRightBound;
                    float xPointLeftBound;
                    float xPointRightBound;
                   
                    float x0 = currentTimePassed-beginTime;
                    x0 /= loopTime;
           
                    xBottomRightBound = x0;
                    xBottomLeftBound = x0 - xLength;
                    float xProjL;
                    xProjL= (uv.x)/tan(angleInRad);

                    xPointLeftBound = xBottomLeftBound - xProjL;
                    xPointRightBound = xBottomRightBound - xProjL;
                   
                    xPointLeftBound += offX;
                    xPointRightBound += offX;
                   
                    if(uv.y > xPointLeftBound && uv.y < xPointRightBound)
                    {
                        float midness = (xPointLeftBound + xPointRightBound)/2;               
                        float rate= (xLength -2*abs(uv.y - midness))/ (xLength);
                        brightness = rate;
                    }
                }
                brightness= max(brightness,0)*_BrightnessPower;
                return brightness;
            }
			
			half4 frag (v2f IN) : COLOR
			{
				// First clip region
				float2 factor = (float2(1.0, 1.0) - abs(IN.worldPos.xy)) * _ClipArgs0.xy;
				float f = min(factor.x, factor.y);

				// Second clip region
				factor = (float2(1.0, 1.0) - abs(IN.worldPos.zw)) * _ClipArgs1.xy;
				f = min(f, min(factor.x, factor.y));

				// Sample the texture
				half4 outCol = 0;
				half4 texCol = tex2D(_MainTex,IN.uv)* IN.color;
				half4 texAlpha = tex2D(_AlphaTex,IN.uvAlpha)* IN.color;
				texCol.a *= clamp(f, 0.0, 1.0);
				
				float tempBrightness;
				tempBrightness =inFlash(90,IN.uv,0.3,1,_BeginTime,0.1,_LoopTime)*_BoolZero;
				outCol = texCol + float4(1,1,1,1)*tempBrightness*texAlpha.w*texCol.w;
				
				return outCol;
			}
			ENDCG
		}
	}
	
	SubShader
	{
		LOD 100

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
			Fog { Mode Off }
			ColorMask RGB
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMaterial AmbientAndDiffuse
			
			SetTexture [_MainTex]
			{
				Combine Texture * Primary
			}
		}
	}
}
