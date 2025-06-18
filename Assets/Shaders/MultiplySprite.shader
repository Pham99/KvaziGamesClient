Shader "Custom/MultiplySprite"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color   ("Tint",           Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags
        {
            "Queue"        = "Transparent" 
            "IgnoreProjector" = "True"
            "RenderType"   = "Transparent"
        }
        ZWrite Off
        Cull Off
        Lighting Off
        Fog { Mode Off }

        // Multiply: result = src.rgb * dst.rgb
        Blend DstColor Zero

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4   _Color;

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color    : COLOR;
            };

            struct v2f
            {
                float2 texcoord : TEXCOORD0;
                float4 vertex   : SV_POSITION;
                float4 color    : COLOR;
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex   = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                o.color    = v.color * _Color;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 tex = tex2D(_MainTex, i.texcoord) * i.color;
                return tex;
            }
            ENDCG
        }
    }
}
