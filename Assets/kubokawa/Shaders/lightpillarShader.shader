Shader "Unlit/lightpillarShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_TopY("TopY", float) = 0.5
		_TopColor("TopColor", Color) = (1, 1, 1, 1)
		_BottomY("BottomY", float) = -0.5
		_BottomColor("BottomColor", Color) = (0, 0, 0, 1)
	    _EmissionColor("EmissionColor", Color) = (1,1,1,1)

		_XShift("Xuv Shift", Range(-1.0, 1.0)) = 0.1
		_XSpeed("X Scroll Speed", Range(1.0, 100.0)) = 10.0

		_YShift("Yuv Shift", Range(-1.0, 1.0)) = 0.1
		_YSpeed("Y Scroll Speed", Range(1.0, 100.0)) = 10.0

		[PowerSlider(0.1)] _F0("F0", Range(0.0, 1.0)) = 0.02
		_Scale("Scale", range(0, 10)) = 5
	}
		SubShader
		{
			Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha
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
					half3 normal : NORMAL;
					float2 texcoord : TEXCOORD5;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					float2 texcoord : TEXCOORD5;
					float4 vert_color : TEXCOORD2;
					float4 vertex : SV_POSITION;
					float3 worldPos : WORLD_POS;

					
					half vdotn : TEXCOORD3;
					half3 reflDir : TEXCOORD4;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				half _TopY;
				half _BottomY;
				float4 _TopColor;
				float4 _BottomColor;
				float _XShift;
				float _YShift;
				float _XSpeed;
				float _YSpeed;
				float _F0;
				half _Scale;

				v2f vert(appdata v)
				{
					v2f o;

					//Speed
					_XShift = _XShift * _XSpeed;
					_YShift = _YShift * _YSpeed;

					//add Shift
					v.uv.x = v.uv.x + _XShift * _Time;
					v.uv.y = v.uv.y + _YShift * _Time;

					float d = tex2Dlod(_MainTex, float4(v.uv.xy, 0, 0)).a;
					d = d * 2 - 1;
					v.vertex.x += d * _Scale;

					o.vertex = UnityObjectToClipPos(v.vertex);
					o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					half t = (o.worldPos.y - _BottomY) / (_TopY - _BottomY);
					o.vert_color = lerp(_BottomColor, _TopColor, saturate(t));
					UNITY_TRANSFER_FOG(o,o.vertex);

					half3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
					o.vdotn = dot(viewDir, v.normal.xyz);
					o.reflDir = mul(unity_ObjectToWorld, reflect(-viewDir, v.normal.xyz));

					return o;
				}
				float4 _EmissionColor;

				fixed4 frag(v2f i) : SV_Target
				{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				//Speed
				_XShift = _XShift * _XSpeed;
				_YShift = _YShift * _YSpeed;

				//add Shift
				i.uv.x = i.uv.x + _XShift * _Time;
				i.uv.y = i.uv.y + _YShift * _Time;


				// gradation
				col = col * i.vert_color;
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);

				col.a = col.a > tex2D(_MainTex, i.uv).a ? tex2D(_MainTex, i.uv) : col.a;
				col.rgb = col.rgb + tex2D(_MainTex, i.uv).rgb;
				//half fresnel = _F0 + (1.0h - _F0) * pow(1.0h - i.vdotn, 5);
				half fresnel = _F0 + (1.0h - _F0) * pow(i.vdotn, 5);

				return col  * fresnel + _EmissionColor;
			}
			ENDCG
		}
		}
}
