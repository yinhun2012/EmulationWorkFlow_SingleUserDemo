Shader "ViveVR/ViveLineShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainColor("Main Color",Color) = (0,1,1,1)
        _Pow("Pow",Range(1000,100000)) = 2000
        _Speed("Speed",Range(0,1)) = 0.1
        [Enum(ON,1,OFF,0)]_Hit("Hitted",int) = 0
    }
    SubShader
    {
		LOD 100
        Tags 
		{ 
			"RenderType" = "Transparent"
			"Queue" = "Transparent+100"
		}

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
            float4 _MainColor;
            float _Pow;
            float _Speed;
            int _Hit;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = _MainColor;
                if(_Hit==0)
                {
                    i.uv.x -= _Time.y*_Speed;
                    float s = sin(i.uv.x*_Pow);
                    if(s<0)
                    {
                        discard;
                    }
                }
                return col;
            }
            ENDCG
        }
    }
}
