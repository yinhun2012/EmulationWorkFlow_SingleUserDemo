Shader "ViveVR/ViveDotShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_MainColor("Color",Color) = (1,1,1,1)
		_MainOffset("Offset",Range(-1,1)) = 0
		_MainStep("Step",Range(0,1)) = 0.5
	}
	SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent+100" }
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
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 worldNormal : TEXCOORD1;
				float3 worldP2V : TEXCOORD2;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			float4 _MainColor;
			float _MainOffset;
			float _MainStep;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldP2V = WorldSpaceViewDir(v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float ndotv = dot(normalize(i.worldNormal),normalize(i.worldP2V));
				ndotv = step(_MainStep,ndotv);
				fixed4 col = _MainColor;
				col.a = (1 - ndotv);
				if (col.a < 0.5)
				{
					discard;
				}
				return col;
			}
			ENDCG
		}
	}
}
