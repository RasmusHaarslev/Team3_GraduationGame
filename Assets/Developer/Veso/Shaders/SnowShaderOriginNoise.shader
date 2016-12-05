Shader "_Mine/SnowOriginNoise" 
{
	Properties
	{
		_MainColor("Main Color", Color) = (1.0,1.0,1.0,1.0)
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Bump("Bump", 2D) = "bump" {}
		_SnowTexture("Snow Texture", 2D) = "white" {}
		_Snow("Level of snow", Range(1, -1)) = 1
		_SnowColor("Color of snow", Color) = (1.0,1.0,1.0,1.0)
		_SnowDirection("Direction of snow", Vector) = (0,1,0)
		_SnowDepth("Depth of snow", Range(0,1)) = 0
		_BumpPower ("Normal Map Power", Range (0, 2)) = 0.1 
		_NoisePower ("Noise Power", float) = 0.1 
		
	}

	SubShader
	{

	Tags{ "RenderType" = "Opaque" }
	LOD 200

        CGPROGRAM
		#pragma surface surf Standard vertex:vert
		#include "noiseSimplex.cginc"
		sampler2D _MainTex,_SnowTexture;
		sampler2D _Bump;
		half _Snow;
		half4 _SnowColor;
		half4 _MainColor;
		half4 _SnowDirection;
		half _SnowDepth,_BumpPower;
		half _NoisePower;

		struct Input {
		    half2 uv_MainTex;
		    half2 uv_SnowTexture;
		    half2 uv_Bump;
		    float3 worldNormal;
		   
		    INTERNAL_DATA
		};

		void vert(inout appdata_full v)
		{
		    float4 sn = mul(UNITY_MATRIX_IT_MV, _SnowDirection);
		    float ns = snoise(sn*_NoisePower);
		    if (dot(v.normal, sn.xyz) >= _Snow)
		        v.vertex.xyz += (sn.xyz + v.normal) * _SnowDepth * _Snow ;
		}

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
		    half4 c = tex2D(_MainTex, IN.uv_MainTex);
		    half4 sTex = tex2D(_SnowTexture, IN.uv_SnowTexture);
		    half ns = snoise(c*_NoisePower);

		    float3 unpackedNormal = UnpackNormal(tex2D(_Bump, IN.uv_Bump));
			o.Normal = half3(unpackedNormal.x*_BumpPower,unpackedNormal.y*_BumpPower,unpackedNormal.z);

		    if (dot(WorldNormalVector(IN, o.Normal), _SnowDirection.xyz) >= _Snow*ns)
		        o.Albedo = sTex.rgb*_SnowColor;
		    else
		        o.Albedo = c.rgb * _MainColor;
		    o.Alpha = 1;
		}
		ENDCG
	}
    FallBack "Diffuse"
}