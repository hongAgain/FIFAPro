Shader "Custom/ShineCup"
{
	Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
		_AlphaTex("Alpha Texture",2D) = "white" {}
        _BeginTime ("BeginTime", float) = 0
        _LoopTime ("LoopTime", float) = 0.6		
		_BoolZero ("BoolZero", int) = 1
		_BrightnessPower("Brightness Power",Range(0,5)) = 1
		_Rate ("Explose Rate", float) = 1
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
			Fog { Mode Off }
			Offset -1, -1
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
			
			float _Rate;
	
			struct appdata_t
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				fixed4 color : COLOR;
			};
	
			struct v2f
			{
				float4 vertex : SV_POSITION;
				half2 uv : TEXCOORD0;
				half2 uvAlpha : TEXCOORD1;
				fixed4 color : COLOR;
			};
	
			v2f o;

			v2f vert (appdata_t v)
			{
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord,_MainTex);
				o.uvAlpha = TRANSFORM_TEX(v.texcoord,_AlphaTex);
				o.color = v.color;
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
				
			fixed4 frag (v2f IN) : COLOR
			{
				float4 outCol = 0;
				float4 texCol = tex2D(_MainTex,IN.uv)* IN.color * _Rate;
				float4 texAlpha = tex2D(_AlphaTex,IN.uvAlpha)* IN.color * _Rate;
				
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
			Offset -1, -1
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
