Shader "Custom/Test"
{
    Properties
    {
        _Zoom   ("Zoom",   Float) = 300.0
        _Speed  ("Speed",  Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            //–– Parameters
            float _Zoom;
            float _Speed;

            // Constants
            #define PI  3.14159265359
            #define TAU 6.28318530718

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv     : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv      : TEXCOORD0;
                float4 pos     : SV_POSITION;
                float4 screenPos : TEXCOORD1;  // for ComputeScreenPos
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv  = v.uv;
                return o;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                // 1) normalize coords
                float2 st = IN.uv;
                float3 color = float3(0., 0., 0.);
                color = float3(IN.uv.x, 0.0, 0.0);
                return float4(color, 1.0);
            }
            ENDCG
        }
    }
}
