Shader "Sprites/MaskAnimation"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_MaskTex("Mask Texture", 2D) = "white" {}
		_ScrollTex("Scroll Texture", 2D) = "white" {}
		_ScrollColor("Scroll Color", Color) = (1,1,1,1)
		_Color ("Tint", Color) = (1,1,1,1)
		_ScrollXSpeed("X", Range(0,10)) = 2
		_ScrollYSpeed("Y", Range(0,10)) = 3
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _Color;
			fixed4 _ScrollColor;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _MaskTex;
			sampler2D _ScrollTex;
			sampler2D _AlphaTex;
			float _AlphaSplitEnabled;
			float4 _ScrollTex_ST;
			float4 _MaskTex2_ST;
			float _ScrollXSpeed;
			float _ScrollYSpeed;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D (_AlphaTex, uv).r;
#endif

				return color;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed xScrollValue = _ScrollXSpeed * _Time;
				fixed yScrollValue = _ScrollYSpeed * _Time;

				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
				fixed4 scrollTex = tex2D(_ScrollTex, IN.texcoord * _ScrollTex_ST.xy + _ScrollTex_ST.zw + fixed2(xScrollValue, yScrollValue)) * _ScrollColor;

				scrollTex *= tex2D(_MaskTex, IN.texcoord);

				c += scrollTex;

				c.rgb *= c.a;

				return c;
			}
		ENDCG
		}
	}
}