Shader "DADIU_Competence/HitCameraEffect"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	_Amount("Amount",  Range(0.0,1.0)) = 0.0
	}

	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Off

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

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			float _Amount;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				float2 dist = (i.uv - 0.5f) * 2.50f;
				dist.x = 1 - dot(dist, dist)  * (_Amount * 1);

				float grayCol = (col.r + col.b + col.g) / 3.0;

				fixed4 grayScaledCol = (col*(1 - (_Amount)) + (grayCol*(_Amount)));

				fixed4 endCol = col;

				if (_Amount > 0) {
					fixed4 tempColor = grayScaledCol * dist.x;
					endCol = col * 0.4f + tempColor*0.6f;
				}

				return endCol;
			}
			ENDCG
		}
	}
}