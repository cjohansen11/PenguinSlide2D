Shader "Unlit/SimpleLinear"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "black" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha

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


            float _DataArray[1023];
            int valueToRead;

            float completion;

            float pixelSize;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float GetHeight(float x, int offset)
            {
                float value1 = _DataArray[uint(x * valueToRead) + offset];
                float value2 = _DataArray[uint((x * valueToRead + 1) + offset)];
                float t = -((uint(x * valueToRead)) - (x * valueToRead)) / ((uint(x * valueToRead)) - (uint(x * valueToRead) - 1));

                return lerp(value1, value2, t);
            }

            fixed4 frag(v2f i) : SV_Target
            {

                float yValue = lerp(GetHeight(i.uv.x, 0), GetHeight(i.uv.x, 1), completion);
                


                float dist = lerp(0.0f, 1.0f, abs(yValue - i.uv.y) / 0.5f);

                // sample the texture
                fixed4 col = lerp(float4(0,0,0,0), float4(dist, dist, dist, 1 - dist + 0.5f), smoothstep(i.uv.y - 0.0005f, i.uv.y + 0.0005f, yValue + 0.5f));

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}