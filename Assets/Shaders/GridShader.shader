﻿Shader "Unlit/Grid Shader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _OuterColor ("Outer Color", Color) = (1, 1, 1, 1)
        _InnerColor("Inner Color", Color) = (0, 0, 0, 1)
        _Width ("Width", float) = 0
        _Height ("Height", float) = 0
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

            v2f vert (appdata v)
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
                float k = 1 - (step(_Width, d1) * step(_Height, d2) * step(_Width, d3) * step(_Height, d4));
                return lerp(_InnerColor, _OuterColor, k);
            }
            ENDCG
        }
    }
}
