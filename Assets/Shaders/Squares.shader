Shader "Custom/Squares"
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
                st.x *= _ScreenParams.x / _ScreenParams.y;

                // 2) zoom and time
                float zoom = _Zoom;
                float speed = _Speed;
                float t = _Time.y * speed;
                st = st*zoom;
                float2 stn = st/zoom;
                
                // base sin waves
                float2 wave;
                wave.x = sin(st.x - t);
                wave.y = sin(st.y + t);
                
                // steps where to apply shrinking
                float2 stair;
                stair.x = 1. - (floor((st.x + PI/2.)/PI/2. - t/TAU) * (TAU*1.) + t*1.) / zoom;
                stair.y = 1. -(floor((st.y + PI/2.) / PI/2. + t/TAU) * (TAU*1.) - t*1.) / zoom;
                
                float shrinkFactor = smoothstep(-0.7, 1., -(stair.x - stair.y))*2. - 1.;
                
                // range where to apply anti alias
                float inRange = step(-1.0,  shrinkFactor)  // 1 if shrinkFactor >= -1.0
                          - step(0.9, shrinkFactor); // 1 if shrinkFactor < -0.9
                
                // wave but shrinked
                float2 shrinkwave;
                shrinkwave.x = step(shrinkFactor, wave.x);
                shrinkwave.y = step(shrinkFactor, wave.y);
                
                // wave but smoothed out
                float2 smoothwave;
                smoothwave.x = smoothstep(0., 1., wave.x);
                smoothwave.y = smoothstep(0., 1., wave.y);
                smoothwave.x = smoothstep(shrinkFactor - 0.1, shrinkFactor + 0.01, wave.x);
                smoothwave.y = smoothstep(shrinkFactor - 0.1, shrinkFactor + 0.01, wave.y);
                
                // final wave
                float2 wavef;
                wavef.x = lerp(shrinkwave.x, smoothwave.x, inRange);
                wavef.y = lerp(shrinkwave.y, smoothwave.y, inRange);
                
                float3 color = float3(0., 0., 0.);
                color = float3(wavef.x * wavef.y / 1.4, (1.0 - stn.x)/1.7, stn.y/2.0);
                return float4(color, 1.0);
            }
            ENDCG
        }
    }
}
