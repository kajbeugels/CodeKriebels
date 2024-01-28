Shader "Unlit/FartIndicator"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Percentage("Percentage",float) = 0
		_PlayerColor("Color", Color) = (0,0,0,0)
	}
		SubShader
		{
			Tags { "RenderType" = "OpaqueCutout" }
			LOD 100
			Blend SrcAlpha OneMinusSrcAlpha

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				float _Percentage;
				float4 _PlayerColor;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);

					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					clip(-(1 - (i.uv.y < _Percentage)));

					// sample the texture
					fixed4 col = tex2D(_MainTex, i.uv) * _PlayerColor;

					return col;
				}
			ENDCG
		}
		}
}
