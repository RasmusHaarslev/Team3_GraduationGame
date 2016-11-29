Shader "_Mine/SnowShaderOriginWorldCoord" {
	Properties{
		_MainColor("Main Color", Color) = (1.0,1.0,1.0,1.0)
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Bump("Bump", 2D) = "bump" {}
		_BumpPower ("Normal Map Power", Range (0, 2)) = 1 
		_Snow("Level of snow", Range(0.9, 0.9999)) = 1
		_SnowColor("Color of snow", Color) = (1.0,1.0,1.0,1.0)
		_SnowDirection("Direction of snow", Vector) = (0,1,0)
		_SnowDepth("Depth of snow", Range(0,1)) = 0
		_TextureScale ("Texture Scale", float) = 0.1 
	}
	SubShader{
	Tags{ "RenderType" = "Opaque" }
	LOD 200
	    CGPROGRAM
		#pragma surface surf Standard vertex:vert
		sampler2D _MainTex;
		sampler2D _Bump;
		float _Snow;
		float4 _SnowColor;
		float4 _MainColor;
		float4 _SnowDirection;
		float _SnowDepth,_BumpPower;
		float _TextureScale;

		struct Input {
		    float2 uv_MainTex;
		    float2 uv_Bump;
		    float3 worldNormal;
		    float3 worldPos;
		    INTERNAL_DATA
		};

		void vert(inout appdata_full v)
		{
		    float4 sn = mul(UNITY_MATRIX_IT_MV, _SnowDirection);
		    if (dot(v.normal, sn.xyz) >= _Snow)
		        v.vertex.xyz += (sn.xyz + v.normal) * _SnowDepth * _Snow;
		}

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			//half4 c;
			float2 UV;

		    half4 c = tex2D(_MainTex, IN.uv_MainTex);

		   
//			if(abs(IN.worldNormal.x)>0.5)
//			{
//				UV = IN.worldPos.yz;
//				c = tex2D(_MainTex,UV*_TextureScale);
//			}
//			else if(abs(IN.worldNormal.y)>0.5)
//			{
//				UV = IN.worldPos.xz;
//				c = tex2D(_MainTex,UV*_TextureScale);
//			} 
//			else 
//			{
//				UV = IN.worldPos.xy;
//				c = tex2D(_MainTex,UV*_TextureScale);
//			}

			float3 unpackedNormal = UnpackNormal(tex2D(_Bump, IN.uv_Bump));
			o.Normal =  normalize(float3(unpackedNormal.x,unpackedNormal.y,unpackedNormal.z*_BumpPower));


		    if (dot(WorldNormalVector(IN, o.Normal), _SnowDirection.xyz) >= _Snow)
		        o.Albedo = normalize(_SnowColor.rgb);
		    else
		        o.Albedo =c.rgb * _MainColor;

		    //o.Albedo = o.Albedo;
		    //o.Alpha = 1;
		}
		ENDCG
	}
    FallBack "Diffuse"
}