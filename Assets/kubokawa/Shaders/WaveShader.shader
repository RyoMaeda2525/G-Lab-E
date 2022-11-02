﻿Shader "Custom/SineWave" {
    Properties{
        _MainTex("Albedo (RGB)", 2D) = "black" {}
        _Amp("Amplitude", float) = 1.0
        _Frq("Frequency", float) = 0.3
        _Spd("Speed", float) = 0.05
        _Ox("origin_x", Range(-0.5,0.5)) = 0
        _Oy("origin_y", Range(-0.5,0.5)) = 0
    }
    SubShader{
        Tags { "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert vertex:vert
        #pragma target 3.0

        sampler2D _MainTex;
        float _Amp;
        float _Frq;
        float _Spd;
        float _Ox;
        float _Oy;

        struct Input {
            float2 uv_MainTex;
            float3 customVert;
        };

        struct WaveData {
            float height;
            float2 normal;
        };

        static const float PI = 3.14159265f;

        WaveData sin_wave(float2 c) {
            WaveData wave;
            float c_dist = sqrt(c.x * c.x + c.y * c.y);
            wave.height = _Amp * sin(2.0f * PI * _Frq * (_Time.y - (c_dist / _Spd)));
            if (c_dist == 0.0f) {
                wave.normal = float2(0.0f, 0.0f);
            }
            else {
                float temp_cosin = -_Amp * 2.0f * PI * _Frq * cos(2.0f * PI * _Frq * (_Time.y - c_dist / _Spd)) / c_dist / _Spd;
                wave.normal = float2(-temp_cosin * c.x, -temp_cosin * c.y);
            }
            return wave;
        }

        void vert(inout appdata_full v, out Input o)
        {
            UNITY_INITIALIZE_OUTPUT(Input, o);

            float2 circle = float2(v.vertex.x - _Ox, v.vertex.z - _Oy);
            WaveData wave_circle = sin_wave(circle);
            float amp = wave_circle.height;
            float2 del_amp = wave_circle.normal;

            v.vertex.xyz = float3(v.vertex.x, v.vertex.y + amp, v.vertex.z);
            v.normal = float3(del_amp.x, 1.0f, del_amp.y);

            o.customVert = v.vertex.xyz;
        }

        void surf(Input IN, inout SurfaceOutput o) {
            //fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            //o.Albedo = c.rgb; +float3(IN.customVert.y + 0.1f, 0.1f, -IN.customVert.y + 0.1f);
            //o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}