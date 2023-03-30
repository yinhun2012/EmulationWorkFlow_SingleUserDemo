Shader "Ugui/UIFrontShader"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
		_Color("Tint",Color) = (1,1,1,1)
        _StencilComp("Stencil Comparison",float) = 8
        _Stencil("Stencil Id",float) = 0
        _StencilOp("Stencil Operation",float) = 0
        _StencilWriteMask("Stencil Write Mask",float) = 255
        _StencilReadMask("Stencil Read Mask",float) = 255
        _ColorMask("Color Mask",float) = 15
    }
    SubShader
    {
        LOD 100
        Tags
        { 
            "RenderType"="Transparent" 
            "IgnoreProjector"="True"
            "Queue"="Transparent+10"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }
       
        Stencil
        {
            Ref[_Stencil]
            Comp[_StencilComp]
            Pass[_StencilOp]
            ReadMask[_StencilReadMask]
            WriteMask[_StencilWriteMask]
        }

        Cull off
        Lighting off
        ZWrite off
        ZTest Always
        Offset -1,-1
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask[_ColorMask]

        Pass
        {
			
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color * _Color;
#ifdef UNITY_HALF_TEXEL_OFFSET
                o.vertex.xy += (_ScreenParams.zw-1.0)*float2(-1,-1);
#endif
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = (tex2D(_MainTex, i.uv)+_TextureSampleAdd)*i.color;
                clip(col.a-0.01);
                return col;
            }
            ENDCG
        }
    }
}
