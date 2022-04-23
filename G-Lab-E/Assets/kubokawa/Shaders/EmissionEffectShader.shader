Shader "Unlit/EmissionEffectShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        //X方向のシフトとスピードに関するパラメータを追加
        _XShift("Xuv Shift", Range(-1.0, 1.0)) = 0.1
        _XSpeed("X Scroll Speed", Range(1.0, 100.0)) = 10.0

        //Y方向のシフトとスピードに関するパラメータを追加
        _YShift("Yuv Shift", Range(-1.0, 1.0)) = 0.1
        _YSpeed("Y Scroll Speed", Range(1.0, 100.0)) = 10.0

        _MainColor("Color", Color) = (1,1,1,1)
        _EmissionMap("Emission Map", 2D) = "black" {}               //追加
        [HDR] _EmissionColor("Emission Color", Color) = (0,0,0)    //追加
        _EmissionIntensity("EmissionIntensity", Range(0.0, 100.0)) = 1
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100
            Cull off
        
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

            float _XShift;
            float _YShift;
            float _XSpeed;
            float _YSpeed;

            float4 _MainColor;
            uniform sampler2D _EmissionMap;    //追加
            float4 _EmissionColor;  
            float _EmissionIntensity;//追加

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                //Speed
                _XShift = _XShift * _XSpeed;
                _YShift = _YShift * _YSpeed;

                //add Shift
                i.uv.x = i.uv.x + _XShift * _Time;
                i.uv.y = i.uv.y + _YShift * _Time;

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) * _MainColor;
                col.a = max(1 - _Time*8, -1.0);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                col = col + tex2D(_EmissionMap, i.uv) * _EmissionColor * _EmissionIntensity;
                //col.r = _Time;
                return col;
            }
            ENDCG
        }
    }
}
