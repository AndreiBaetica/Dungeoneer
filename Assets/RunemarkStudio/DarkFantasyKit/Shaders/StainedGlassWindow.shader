// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Runemark/Stained Window"
{
	Properties
	{
		_Albedo("Albedo", 2D) = "black" {}
		_GlassTint("GlassTint", Range( 0 , 1)) = 0.5
		_Normal("Normal", 2D) = "bump" {}
		_Distortion("Distortion", Range( 0 , 1)) = 1
		_Metallic("Metallic", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ }
		CGINCLUDE
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 4.6
		struct Input
		{
			float2 uv_texcoord;
			float4 screenPos;
		};

		struct SurfaceOutputStandardCustom
		{
			half3 Albedo;
			half3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			half Alpha;
			half3 Transmission;
		};

		uniform sampler2D _Normal;
		uniform float4 _Normal_ST;
		uniform sampler2D _GrabTexture;
		uniform float _Distortion;
		uniform sampler2D _Metallic;
		uniform float4 _Metallic_ST;
		uniform sampler2D _Albedo;
		uniform float4 _Albedo_ST;
		uniform float _GlassTint;


		inline float4 ASE_ComputeGrabScreenPos( float4 pos )
		{
			#if UNITY_UV_STARTS_AT_TOP
			float scale = -1.0;
			#else
			float scale = 1.0;
			#endif
			float4 o = pos;
			o.y = pos.w * 0.5f;
			o.y = ( pos.y - o.y ) * _ProjectionParams.x * scale + o.y;
			return o;
		}


		inline half4 LightingStandardCustom(SurfaceOutputStandardCustom s, half3 viewDir, UnityGI gi )
		{
			half3 transmission = max(0 , -dot(s.Normal, gi.light.dir)) * gi.light.color * s.Transmission;
			half4 d = half4(s.Albedo * transmission , 0);

			SurfaceOutputStandard r;
			r.Albedo = s.Albedo;
			r.Normal = s.Normal;
			r.Emission = s.Emission;
			r.Metallic = s.Metallic;
			r.Smoothness = s.Smoothness;
			r.Occlusion = s.Occlusion;
			r.Alpha = s.Alpha;
			return LightingStandard (r, viewDir, gi) + d;
		}

		inline void LightingStandardCustom_GI(SurfaceOutputStandardCustom s, UnityGIInput data, inout UnityGI gi )
		{
			#if defined(UNITY_PASS_DEFERRED) && UNITY_ENABLE_REFLECTION_BUFFERS
				gi = UnityGlobalIllumination(data, s.Occlusion, s.Normal);
			#else
				UNITY_GLOSSY_ENV_FROM_SURFACE( g, s, data );
				gi = UnityGlobalIllumination( data, s.Occlusion, s.Normal, g );
			#endif
		}

		void surf( Input i , inout SurfaceOutputStandardCustom o )
		{
			float2 uv_Normal = i.uv_texcoord * _Normal_ST.xy + _Normal_ST.zw;
			float3 NormalMap23 = UnpackNormal( tex2D( _Normal, uv_Normal ) );
			o.Normal = NormalMap23;
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float4 ase_grabScreenPos = ASE_ComputeGrabScreenPos( ase_screenPos );
			float4 ase_grabScreenPosNorm = ase_grabScreenPos / ase_grabScreenPos.w;
			float4 screenColor1 = tex2D( _GrabTexture, ( (ase_grabScreenPosNorm).xy + (( NormalMap23 * _Distortion )).xy ) );
			float4 temp_cast_0 = (0.0).xxxx;
			float2 uv_Metallic = i.uv_texcoord * _Metallic_ST.xy + _Metallic_ST.zw;
			float4 tex2DNode18 = tex2D( _Metallic, uv_Metallic );
			float Metallic25 = tex2DNode18.r;
			float4 lerpResult15 = lerp( screenColor1 , temp_cast_0 , Metallic25);
			float4 temp_cast_1 = (1.0).xxxx;
			float2 uv_Albedo = i.uv_texcoord * _Albedo_ST.xy + _Albedo_ST.zw;
			float4 tex2DNode9 = tex2D( _Albedo, uv_Albedo );
			float4 Albedo42 = tex2DNode9;
			float4 lerpResult53 = lerp( temp_cast_1 , Albedo42 , _GlassTint);
			o.Emission = ( lerpResult15 * lerpResult53 ).rgb;
			o.Metallic = Metallic25;
			float Smoothness26 = tex2DNode18.a;
			o.Smoothness = Smoothness26;
			float Occlusion27 = tex2DNode18.g;
			o.Occlusion = Occlusion27;
			o.Transmission = float4(1,0.0990566,0.0990566,0).rgb;
			float AlbedoAlpha44 = tex2DNode9.a;
			o.Alpha = AlbedoAlpha44;
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustom keepalpha fullforwardshadows exclude_path:deferred 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 4.6
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float2 customPack1 : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float4 screenPos : TEXCOORD3;
				float4 tSpace0 : TEXCOORD4;
				float4 tSpace1 : TEXCOORD5;
				float4 tSpace2 : TEXCOORD6;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				half3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				half tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				half3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
				return o;
			}
			half4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord = IN.customPack1.xy;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.screenPos = IN.screenPos;
				SurfaceOutputStandardCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandardCustom, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}
/*ASEBEGIN
Version=15401
0;103;1154;634;1708.084;-516.9338;1.044654;True;False
Node;AmplifyShaderEditor.CommentaryNode;46;-1289.254,-979.0976;Float;False;753.1539;1185.481;Input Textures;9;44;27;26;25;42;9;18;23;2;;0.5235849,0.7704858,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;13;-1221.631,416.6231;Float;False;2149.698;599.4155;Stained Glass Disortion;11;16;17;15;1;6;8;5;7;4;24;3;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;2;-1228.616,-678.6841;Float;True;Property;_Normal;Normal;2;0;Create;True;0;0;False;0;4a340edd205cd1d4b8f75592abce6e24;4a340edd205cd1d4b8f75592abce6e24;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;24;-1154.802,715.071;Float;False;23;0;1;FLOAT3;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;23;-859.9499,-681.0442;Float;False;NormalMap;-1;True;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-1153.293,825.6418;Float;False;Property;_Distortion;Distortion;3;0;Create;True;0;0;False;0;1;0.48;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;4;-851.6321,768.364;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.GrabScreenPosition;7;-775.8672,467.6231;Float;False;0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;8;-513.8672,467.6231;Float;False;True;True;False;False;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ComponentMaskNode;5;-533.6321,763.364;Float;True;True;True;False;True;1;0;FLOAT3;0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;9;-1239.254,-926.3018;Float;True;Property;_Albedo;Albedo;0;0;Create;True;0;0;False;0;None;89851ab820f6e1c4fbad9be3333f1f50;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CommentaryNode;51;977.4087,553.0229;Float;False;781.2095;504.2222;Colorize;5;48;47;53;54;55;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;6;-225.8673,608.6231;Float;True;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;18;-1229.771,-472.2632;Float;True;Property;_Metallic;Metallic;4;0;Create;True;0;0;False;0;03f09375915895746b9093c09a0c7732;03f09375915895746b9093c09a0c7732;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;16;394.8487,758.2303;Float;False;25;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;17;414.7853,684.4527;Float;False;Constant;_Saturation;Saturation;3;0;Create;True;0;0;False;0;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ScreenColorNode;1;129.7629,601.4478;Float;False;Global;_GrabScreen0;Grab Screen 0;0;0;Create;True;0;0;False;0;Object;-1;False;False;1;0;FLOAT2;0,0;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;47;1011.801,680.9338;Float;False;42;0;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;25;-858.0998,-454.6166;Float;False;Metallic;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;42;-878.6799,-929.0974;Float;False;Albedo;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;55;1030.505,758.9805;Float;False;Constant;_Colorless;Colorless;6;0;Create;True;0;0;False;0;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;54;989.7053,846.3804;Float;False;Property;_GlassTint;GlassTint;1;0;Create;True;0;0;False;0;0.5;1;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;15;679.5566,607.7407;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.CommentaryNode;50;2563.839,437.5966;Float;False;808.7632;575.9415;Output;6;0;30;45;39;28;29;;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;53;1383.505,658.5805;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;1626.616,597.0229;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;58;2585.593,903.624;Float;False;Constant;_Color0;Color 0;5;0;Create;True;0;0;False;0;1,0.0990566,0.0990566,0;0,0,0,0;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;45;2583.024,818.5294;Float;False;44;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;27;-857.0998,-384.6165;Float;False;Occlusion;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;30;2602.936,739.024;Float;False;27;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;26;-855.0998,-312.6166;Float;False;Smoothness;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;29;2591.485,656.9198;Float;False;26;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;39;2605.175,572.427;Float;False;25;0;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;56;2595.593,1080.624;Float;False;42;0;1;COLOR;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;44;-877.1079,-831.8568;Float;False;AlbedoAlpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;28;2587.059,497.9333;Float;False;23;0;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;3093.009,572.7607;Float;False;True;6;Float;;0;0;Standard;Runemark/Stained Window;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Translucent;0.5;True;True;0;False;Opaque;;Transparent;ForwardOnly;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;27.7;10;25;False;0.5;True;0;0;False;-1;0;False;-1;0;0;False;-1;0;False;-1;-1;False;-1;-1;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;23;0;2;0
WireConnection;4;0;24;0
WireConnection;4;1;3;0
WireConnection;8;0;7;0
WireConnection;5;0;4;0
WireConnection;6;0;8;0
WireConnection;6;1;5;0
WireConnection;1;0;6;0
WireConnection;25;0;18;1
WireConnection;42;0;9;0
WireConnection;15;0;1;0
WireConnection;15;1;17;0
WireConnection;15;2;16;0
WireConnection;53;0;55;0
WireConnection;53;1;47;0
WireConnection;53;2;54;0
WireConnection;48;0;15;0
WireConnection;48;1;53;0
WireConnection;27;0;18;2
WireConnection;26;0;18;4
WireConnection;44;0;9;4
WireConnection;0;1;28;0
WireConnection;0;2;48;0
WireConnection;0;3;39;0
WireConnection;0;4;29;0
WireConnection;0;5;30;0
WireConnection;0;6;58;0
WireConnection;0;9;45;0
ASEEND*/
//CHKSM=49CF6294F2381644652BD1DCF47C435C5BF63AF8