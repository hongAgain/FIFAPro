
Shader "Custom/GlowMirror" {
   Properties {
        _RimColor ("Rim Color", Color) = (0.12,0.42,1,0.56)
        _InnerColor ("Inner Color", Color) = (0.057,0.128,0.875,0.56)
        _InnerColorPower ("Inner Color Power", Range(0.0,1.0)) = 0.3
        _RimPower ("Rim Power", Range(0.0,5.0)) = 2.5
        _AlphaPower ("Alpha Rim Power", Range(0.0,8.0)) = 3.0
        _AllPower ("All Power", Range(0.0, 2.0)) = 0.9
    }
    SubShader 
    {
        Tags { "Queue" = "Transparent+100" }
        CGPROGRAM
        #pragma surface surf Lambert alpha
        struct Input
        {
            float3 viewDir;
            INTERNAL_DATA
        };
        
        float4 _RimColor;
        float _RimPower;
        float _AlphaPower;
        float _AlphaMin;
        float _InnerColorPower;
        float _AllPower;
        float4 _InnerColor;
        void surf (Input IN, inout SurfaceOutput o) 
        {
			float3 kNormal = -o.Normal;
			kNormal.y = -kNormal.y;
            half rim = 1.0 - saturate(dot (normalize(IN.viewDir), kNormal));
            o.Emission = _RimColor.rgb * pow (rim, _RimPower)*_AllPower+(_InnerColor.rgb*2*_InnerColorPower);
            o.Alpha = (pow (rim, _AlphaPower))*_AllPower;
        }
        ENDCG
    }
    Fallback "VertexLit"
}
