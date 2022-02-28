Shader "Unlit/HeightShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_TopY("TopY", float) = 0.5
		_TopColor("TopColor", Color) = (1, 1, 1, 1)
		_BottomY("BottomY", float) = -0.5
		_BottomColor("BottomColor", Color) = (0, 0, 0, 1)
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
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					UNITY_FOG_COORDS(1)
					float4 vert_color : TEXCOORD2;
					float4 vertex : SV_POSITION;
					float3 worldPos : WORLD_POS;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				half _TopY;
				half _BottomY;
				float4 _TopColor;
				float4 _BottomColor;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					half t = (o.worldPos.y - _BottomY) / (_TopY - _BottomY);
					o.vert_color = lerp(_BottomColor, _TopColor, saturate(t));
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					// sample the texture
					fixed4 col = tex2D(_MainTex, i.uv);
				// gradation
				col = col * i.vert_color;
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);

				col.a = col.a;
				return col;
			}
			ENDCG
		}
		}
}
