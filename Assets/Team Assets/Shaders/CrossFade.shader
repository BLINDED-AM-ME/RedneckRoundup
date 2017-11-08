Shader "Unlit/CrossFade"
{
	Properties
	{

		_Stencil("Stencil (A)", 2D) = "white"{}
		_MainTex ("Texture 1 (RGBA)", 2D) = "white" {}
		_MainTex2 ("Texture 2 (RGBA)", 2D) = "white" {}


	}
	SubShader
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		//Tags { "RenderType"="Opaque" }
		Blend SrcAlpha OneMinusSrcAlpha
		LOD 100

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
				float2 _Stencil_uv  : TEXCOORD0;
				float2 _MainTex_uv  : TEXCOORD1;
				float2 _MainTex2_uv : TEXCOORD2;

				float4 vertex : SV_POSITION;
			};

			sampler2D _Stencil;
			sampler2D _MainTex;
			sampler2D _MainTex2;

			float4 _Stencil_ST;
			float4 _MainTex_ST;
			float4 _MainTex2_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);

				o._Stencil_uv = TRANSFORM_TEX(v.uv, _Stencil);
				o._MainTex_uv = TRANSFORM_TEX(v.uv, _MainTex);
				o._MainTex2_uv = TRANSFORM_TEX(v.uv, _MainTex2);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				half  stencil = tex2D(_Stencil, i._Stencil_uv).a;

				half4 firstColor =  tex2D(_MainTex, i._MainTex_uv); 
				half4 secondColor = tex2D(_MainTex2, i._MainTex2_uv);


				half4 col = (firstColor * stencil) + (secondColor * (1-stencil)) ;

				col.a = firstColor.a;

				return col;
			}
			ENDCG
		}
	}
}
