Shader "Unlit/Ghost Shader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _OuterColor("Outer Color", Color) = (1, 1, 1, 1)
        _InnerColor("Inner Color", Color) = (0, 0, 0, 1)
        _Width("Width", float) = 0
        _Smooth("Smooth", float) = 0
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull back
        LOD 100

        Pass
        {
            // Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _OuterColor;
            fixed4 _InnerColor;
            float _Width;
            float _Height;
            float _Smooth;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float d1 = abs(i.uv.x % 1);
                float d2 = abs(i.uv.y % 1);
                float d3 = abs(i.uv.x % 1 - 1);
                float d4 = abs(i.uv.y % 1 - 1);
                // float k = 1 - (step(_Width, d1) * step(_Height, d2) * step(_Width, d3) * step(_Height, d4));
                float minD = 
                    step(d1, d2) * step(d1, d3) * step(d1, d4) * d1 +
                    step(d2, d1) * step(d2, d3) * step(d2, d4) * d2 +
                    step(d3, d1) * step(d3, d2) * step(d3, d4) * d3 +
                    step(d4, d1) * step(d4, d2) * step(d4, d3) * d4;
                float k = step(minD, _Width);
                float j = 1 - minD * _Smooth / _Width;

                return lerp(_InnerColor, _OuterColor, k * j);
            }
            ENDCG
        }
    }
}
