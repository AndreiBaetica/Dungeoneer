Shader "Custom/SeeThroughShaderURP2019"
{
   Properties
   {
      
	_Color("Color", Color) = (1,0,1,1)
	_MainTex("Albedo (RGB)", 2D) = "white" {}
	_BumpMap ("Bumpmap", 2D) = "bump" {}
	_BumpScale ("Bump Scale", Float) = 1.000000
	_DissolveColor("Dissolve Color", Color) = (1,1,1,1)
	_DissolveColorSaturation("Dissolve Color Saturation", Range(0,1)) = 1.0
	_DissolveEmission("Dissolve Emission", Range(0,1)) = 1.0
	[AbsoluteValue()] _DissolveEmissionBooster("Dissolve Emission Booster", float) = 1
	_DissolveTex("Dissolve Effect Texture", 2D) = "white" {}
	_Glossiness("Smoothness", Range(0,1)) = 0.5
	_Metallic("Metallic", Range(0,1)) = 0.0
	_TextureVisibility("TextureVisibility", Range(0,1)) = 1
	[KeywordEnum(None, AngleOnly, ConeOnly, AngleAndCone, CylinderOnly, AngleAndCylinder, Circle)] _Obstruction ("Obstruction Mode", Float) = 0
	[HideIfDisabled(_OBSTRUCTION_ANGLEONLY,_OBSTRUCTION_ANGLEANDCONE, _OBSTRUCTION_ANGLEANDCYLINDER)] _AngleStrength("Angle Obstruction Strength", Range(0,1)) = 0.0
	[HideIfDisabled(_OBSTRUCTION_ANGLEANDCONE, _OBSTRUCTION_CONEONLY)] _ConeStrength ("Cone Obstruction Strength", Range(0,1)) = 0.0
	[HideIfDisabled(_OBSTRUCTION_CONEONLY,_OBSTRUCTION_ANGLEANDCONE)] _ConeObstructionDestroyRadius ("Cone Obstruction Destroy Radius", float) = 100.0
	[HideIfDisabled(_OBSTRUCTION_CYLINDERONLY, _OBSTRUCTION_ANGLEANDCYLINDER)] _CylinderStrength ("Cylinder Obstruction Strength", Range(0,1)) = 0.0
	[HideIfDisabled(_OBSTRUCTION_CYLINDERONLY, _OBSTRUCTION_ANGLEANDCYLINDER)] _CylinderObstructionDestroyRadius ("Cylinder Obstruction Destroy Radius", float) = 100.0
	[HideIfDisabled(_OBSTRUCTION_CIRCLE)] _CircleStrength ("Circle Obstruction Strength", Range(0,1)) = 0.0
	[HideIfDisabled(_OBSTRUCTION_CIRCLE)] _CircleObstructionDestroyRadius ("Circle Obstruction Destroy Radius", float) = 100.0
	[HideIfDisabled(_OBSTRUCTION_ANGLEANDCONE, _OBSTRUCTION_ANGLEANDCYLINDER, _OBSTRUCTION_CONEONLY, _OBSTRUCTION_CYLINDERONLY, _OBSTRUCTION_CIRCLE)] _DissolveFallOff("Dissolve FallOff", Range(0,1)) = 0.0
	_IntrinsicDissolveStrength("Intrinsic Dissolve Strength", Range(0,1)) = 0.0
	[MaterialToggle] _PreviewMode("Preview Mode", float) = 0.0
	[AbsoluteValue()] _UVs ("Dissolve Texture Scale", float) = 1.0
	[MaterialToggle] _hasClippedShadows("Has Clipped Shadows", Float) = 0
	[KeywordEnum(Manual, PlayerPositon)] _Floor ("Floor Mode", Float) = 0
	[HideIfDisabled(_FLOOR_MANUAL)] _FloorY ("FloorY",  float) = 1.0	// playerPosition offset described above
	[HideIfDisabled(_FLOOR_PLAYERPOSITON)] _PlayerPosYOffset ("PlayerPos Y Offset", float) = 1.0  
	_FloorYTextureGradientLength ("FloorY Texture Gradient Length", float) = 0.0  
	[MaterialToggle] _AnimationEnabled("Animation Enabled", Float) = 0
	_AnimationSpeed("Animation Speed", Range(0,2)) = 1
	[HideInInspector] _IsReplacementShader ("Should be hidden: _IsReplacementShader", Float) = 0
	[HideInInspector] _RaycastMode ("This text isn't shown", Float) = 0
	[HideInInspector] _TriggerMode ("This text isn't shown", Float) = 0
	[HideInInspector] _IsExempt ("_IsExempt", Float) = 0
	_TransitionDuration ("Transition Duration In Seconds", Float) = 2
	_DefaultEffectRadius ("Default Effect Radius",Float) = 25
    [HideInInspector] _numOfPlayersInside ("hidden: _numOfPlayersInside", Float) = 0
    [HideInInspector] _tValue ("hidden: _tValue", Float) = 0
    [HideInInspector] _tDirection ("hidden: _tDirection", Float) = 0
    [HideInInspector] _id ("hidden: _id", Float) = 0
    [MaterialToggle] _TexturedEmissionEdge("Textured Emission Edge", float) = 1.0
    _TexturedEmissionEdgeStrength("Textured Emission Edge Strength", Range(0,1)) = 0.3
    [HideInInspector] _isReferenceMaterial("hidden: _isReferenceMaterial", float) = 0.0


   }
   SubShader
   {
      Tags { "RenderPipeline"="UniversalPipeline" "RenderType" = "Opaque" "Queue" = "Geometry" }

      
      
        Pass
        {
            Name "Universal Forward"
            Tags 
            { 
                "LightMode" = "UniversalForward"
            }
            Blend One Zero, One Zero
Cull Back
ZTest LEqual
ZWrite On

            

            HLSLPROGRAM

               #pragma vertex Vert
   #pragma fragment Frag

            #pragma target 3.0

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
        
            // Keywords
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS _ADDITIONAL_OFF
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT
            #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE

            // GraphKeywords: <None>


            #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
            #define SHADER_PASS SHADERPASS_FORWARD
            #define SHADERPASS_FORWARD

            #define _PASSFORWARD 1

            
      #pragma shader_feature_local _NORMALMAP


   #define _URP 1


            // this has to be here or specular color will be ignored. Not in SG code
            #if _SIMPLELIT
               #define _SPECULAR_COLOR
            #endif


            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Version.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"
        

               #undef WorldNormalVector
      #define WorldNormalVector(data, normal) mul(normal, data.TBNMatrix)
      
      #define UnityObjectToWorldNormal(normal) mul(GetObjectToWorldMatrix(), normal)

      #define _WorldSpaceLightPos0 _MainLightPosition
      
      #define UNITY_DECLARE_TEX2D(name) TEXTURE2D(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2D_NOSAMPLER(name) TEXTURE2D(name);
      #define UNITY_DECLARE_TEX2DARRAY(name) TEXTURE2D_ARRAY(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(name) TEXTURE2D_ARRAY(name);

      #define UNITY_SAMPLE_TEX2DARRAY(tex,coord)            SAMPLE_TEXTURE2D_ARRAY(tex, sampler##tex, coord.xy, coord.z)
      #define UNITY_SAMPLE_TEX2DARRAY_LOD(tex,coord,lod)    SAMPLE_TEXTURE2D_ARRAY_LOD(tex, sampler##tex, coord.xy, coord.z, lod)
      #define UNITY_SAMPLE_TEX2D(tex, coord)                SAMPLE_TEXTURE2D(tex, sampler##tex, coord)
      #define UNITY_SAMPLE_TEX2D_SAMPLER(tex, samp, coord)  SAMPLE_TEXTURE2D(tex, sampler##samp, coord)

      #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod)   SAMPLE_TEXTURE2D_LOD(tex, sampler_##tex, coord, lod)
      #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) SAMPLE_TEXTURE2D_LOD (tex, sampler##samplertex,coord, lod)
     
      #if defined(UNITY_COMPILER_HLSL)
         #define UNITY_INITIALIZE_OUTPUT(type,name) name = (type)0;
      #else
         #define UNITY_INITIALIZE_OUTPUT(type,name)
      #endif

      #define sampler2D_float sampler2D
      #define sampler2D_half sampler2D



      // data across stages, stripped like the above.
      struct VertexToPixel
      {
         float4 pos : SV_POSITION;
         float3 worldPos : TEXCOORD0;
         float3 worldNormal : TEXCOORD1;
         float4 worldTangent : TEXCOORD2;
          float4 texcoord0 : TEXCCOORD3;
         // float4 texcoord1 : TEXCCOORD4;
         // float4 texcoord2 : TEXCCOORD5;

         // #if %TEXCOORD3REQUIREKEY%
         // float4 texcoord3 : TEXCCOORD6;
         // #endif

         // #if %SCREENPOSREQUIREKEY%
          float4 screenPos : TEXCOORD7;
         // #endif

         // #if %VERTEXCOLORREQUIREKEY%
         // half4 vertexColor : COLOR;
         // #endif

         // #if %EXTRAV2F0REQUIREKEY%
         // float4 extraV2F0 : TEXCOORD12;
         // #endif

         // #if %EXTRAV2F1REQUIREKEY%
         // float4 extraV2F1 : TEXCOORD13;
         // #endif

         // #if %EXTRAV2F2REQUIREKEY%
         // float4 extraV2F2 : TEXCOORD14;
         // #endif

         // #if %EXTRAV2F3REQUIREKEY%
         // float4 extraV2F3 : TEXCOORD15;
         // #endif

         // #if %EXTRAV2F4REQUIREKEY%
         // float4 extraV2F4 : TEXCOORD16;
         // #endif

         // #if %EXTRAV2F5REQUIREKEY%
         // float4 extraV2F5 : TEXCOORD17;
         // #endif

         // #if %EXTRAV2F6REQUIREKEY%
         // float4 extraV2F6 : TEXCOORD18;
         // #endif

         // #if %EXTRAV2F7REQUIREKEY%
         // float4 extraV2F7 : TEXCOORD19;
         // #endif
            
         #if defined(LIGHTMAP_ON)
            float2 lightmapUV : TEXCOORD8;
         #endif
         #if !defined(LIGHTMAP_ON)
            float3 sh : TEXCOORD9;
         #endif
            float4 fogFactorAndVertexLight : TEXCOORD10;
            float4 shadowCoord : TEXCOORD11;
         #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
         #endif
         #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
         #endif
         #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
         #endif
         #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
         #endif
      };


         
            
            // data describing the user output of a pixel
            struct Surface
            {
               half3 Albedo;
               half Height;
               half3 Normal;
               half Smoothness;
               half3 Emission;
               half Metallic;
               half3 Specular;
               half Occlusion;
               half SpecularPower; // for simple lighting
               half Alpha;
               float outputDepth; // if written, SV_Depth semantic is used. ShaderData.clipPos.z is unused value
               // HDRP Only
               half SpecularOcclusion;
               half SubsurfaceMask;
               half Thickness;
               half CoatMask;
               half CoatSmoothness;
               half Anisotropy;
               half IridescenceMask;
               half IridescenceThickness;
               int DiffusionProfileHash;
               // requires _OVERRIDE_BAKEDGI to be defined, but is mapped in all pipelines
               float3 DiffuseGI;
               float3 BackDiffuseGI;
               float3 SpecularGI;
               // requires _OVERRIDE_SHADOWMASK to be defines
               float4 ShadowMask;
            };

            // Data the user declares in blackboard blocks
            struct Blackboard
            {
                
                float blackboardDummyData;
            };

            // data the user might need, this will grow to be big. But easy to strip
            struct ShaderData
            {
               float4 clipPos; // SV_POSITION
               float3 localSpacePosition;
               float3 localSpaceNormal;
               float3 localSpaceTangent;
        
               float3 worldSpacePosition;
               float3 worldSpaceNormal;
               float3 worldSpaceTangent;
               float tangentSign;

               float3 worldSpaceViewDir;
               float3 tangentSpaceViewDir;

               float4 texcoord0;
               float4 texcoord1;
               float4 texcoord2;
               float4 texcoord3;

               float2 screenUV;
               float4 screenPos;

               float4 vertexColor;
               bool isFrontFace;

               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;

               float3x3 TBNMatrix;
               Blackboard blackboard;
            };

            struct VertexData
            {
               #if SHADER_TARGET > 30
               // uint vertexID : SV_VertexID;
               #endif
               float4 vertex : POSITION;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;

               // would love to strip these, but they are used in certain
               // combinations of the lighting system, and may be used
               // by the user as well, so no easy way to strip them.

               float4 texcoord1 : TEXCOORD1;
               float4 texcoord2 : TEXCOORD2;

               // #if %TEXCOORD3REQUIREKEY%
               // float4 texcoord3 : TEXCOORD3;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               #if _HDRP && (_PASSMOTIONVECTOR || (_PASSFORWARD && defined(_WRITE_TRANSPARENT_MOTION_VECTOR)))
                  float3 previousPositionOS : TEXCOORD4; // Contain previous transform position (in case of skinning for example)
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity    : TEXCOORD5; // Add Precomputed Velocity (Alembic computes velocities on runtime side).
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct TessVertex 
            {
               float4 vertex : INTERNALTESSPOS;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;
               float4 texcoord1 : TEXCOORD1;
               float4 texcoord2 : TEXCOORD2;

               // #if %TEXCOORD3REQUIREKEY%
               // float4 texcoord3 : TEXCOORD3;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               // #if %EXTRAV2F0REQUIREKEY%
               // float4 extraV2F0 : TEXCOORD5;
               // endif

               // #if %EXTRAV2F1REQUIREKEY%
               // float4 extraV2F1 : TEXCOORD6;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // float4 extraV2F2 : TEXCOORD7;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // float4 extraV2F3 : TEXCOORD8;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // float4 extraV2F4 : TEXCOORD9;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // float4 extraV2F5 : TEXCOORD10;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // float4 extraV2F6 : TEXCOORD11;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // float4 extraV2F7 : TEXCOORD12;
               // #endif

               #if _HDRP && (_PASSMOTIONVECTOR || (_PASSFORWARD && defined(_WRITE_TRANSPARENT_MOTION_VECTOR)))
                  float3 previousPositionOS : TEXCOORD13; // Contain previous transform position (in case of skinning for example)
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity : TEXCOORD14;
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
               UNITY_VERTEX_OUTPUT_STEREO
            };

            struct ExtraV2F
            {
               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;
               Blackboard blackboard;
            };


            float3 WorldToTangentSpace(ShaderData d, float3 normal)
            {
               return mul(d.TBNMatrix, normal);
            }

            float3 TangentToWorldSpace(ShaderData d, float3 normal)
            {
               return mul(normal, d.TBNMatrix);
            }

            // in this case, make standard more like SRPs, because we can't fix
            // unity_WorldToObject in HDRP, since it already does macro-fu there

            #if _STANDARD
               float3 TransformWorldToObject(float3 p) { return mul(unity_WorldToObject, float4(p, 1)); };
               float3 TransformObjectToWorld(float3 p) { return mul(unity_ObjectToWorld, float4(p, 1)); };
               float4 TransformWorldToObject(float4 p) { return mul(unity_WorldToObject, p); };
               float4 TransformObjectToWorld(float4 p) { return mul(unity_ObjectToWorld, p); };
               float4x4 GetWorldToObjectMatrix() { return unity_WorldToObject; }
               float4x4 GetObjectToWorldMatrix() { return unity_ObjectToWorld; }
               #if (defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (SHADER_TARGET_SURFACE_ANALYSIS && !SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod) tex.SampleLevel (sampler##tex,coord, lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) tex.SampleLevel (sampler##samplertex,coord, lod)
              #else
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord,lod) tex2D (tex,coord,0,lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord,lod) tex2D (tex,coord,0,lod)
              #endif

               #undef GetObjectToWorldMatrix()
               #undef GetWorldToObjectMatrix()
               #undef GetWorldToViewMatrix()
               #undef UNITY_MATRIX_I_V
               #undef UNITY_MATRIX_P
               #undef GetWorldToHClipMatrix()
               #undef GetObjectToWorldMatrix()V
               #undef UNITY_MATRIX_T_MV
               #undef UNITY_MATRIX_IT_MV
               #undef GetObjectToWorldMatrix()VP

               #define GetObjectToWorldMatrix()     unity_ObjectToWorld
               #define GetWorldToObjectMatrix()   unity_WorldToObject
               #define GetWorldToViewMatrix()     unity_MatrixV
               #define UNITY_MATRIX_I_V   unity_MatrixInvV
               #define GetViewToHClipMatrix()     OptimizeProjectionMatrix(glstate_matrix_projection)
               #define GetWorldToHClipMatrix()    unity_MatrixVP
               #define GetObjectToWorldMatrix()V    mul(GetWorldToViewMatrix(), GetObjectToWorldMatrix())
               #define UNITY_MATRIX_T_MV  transpose(GetObjectToWorldMatrix()V)
               #define UNITY_MATRIX_IT_MV transpose(mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V))
               #define GetObjectToWorldMatrix()VP   mul(GetWorldToHClipMatrix(), GetObjectToWorldMatrix())


            #endif

            float3 GetCameraWorldPosition()
            {
               #if _HDRP
                  return GetCameraRelativePositionWS(_WorldSpaceCameraPos);
               #else
                  return _WorldSpaceCameraPos;
               #endif
            }

            #if _GRABPASSUSED
               #if _STANDARD
                  TEXTURE2D(%GRABTEXTURE%);
                  SAMPLER(sampler_%GRABTEXTURE%);
               #endif

               half3 GetSceneColor(float2 uv)
               {
                  #if _STANDARD
                     return SAMPLE_TEXTURE2D(%GRABTEXTURE%, sampler_%GRABTEXTURE%, uv).rgb;
                  #else
                     return SHADERGRAPH_SAMPLE_SCENE_COLOR(uv);
                  #endif
               }
            #endif


      
            #if _STANDARD
               UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
               float GetSceneDepth(float2 uv) { return SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv)); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv)); } 
            #else
               float GetSceneDepth(float2 uv) { return SHADERGRAPH_SAMPLE_SCENE_DEPTH(uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv), _ZBufferParams); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv), _ZBufferParams); } 
            #endif

            float3 GetWorldPositionFromDepthBuffer(float2 uv, float3 worldSpaceViewDir)
            {
               float eye = GetLinearEyeDepth(uv);
               float3 camView = mul((float3x3)GetObjectToWorldMatrix(), transpose(mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V)) [2].xyz);

               float dt = dot(worldSpaceViewDir, camView);
               float3 div = worldSpaceViewDir/dt;
               float3 wpos = (eye * div) + GetCameraWorldPosition();
               return wpos;
            }

            #if _STANDARD
               UNITY_DECLARE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture);
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  float4 depthNorms = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture, uv);
                  float3 norms = DecodeViewNormalStereo(depthNorms);
                  norms = mul((float3x3)GetWorldToViewMatrix(), norms) * 0.5 + 0.5;
                  return norms;
               }
            #elif _HDRP
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  NormalData nd;
                  DecodeFromNormalBuffer(_ScreenSize.xy * uv, nd);
                  return nd.normalWS;
               }
            #elif _URP
               #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                  #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"
               #endif

               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                     return SampleSceneNormals(uv);
                  #else
                     float3 wpos = GetWorldPositionFromDepthBuffer(uv, worldSpaceViewDir);
                     return normalize(-cross(ddx(wpos), ddy(wpos))) * 0.5 + 0.5;
                  #endif

                }
             #endif

             #if _HDRP

               half3 UnpackNormalmapRGorAG(half4 packednormal)
               {
                     // This do the trick
                  packednormal.x *= packednormal.w;

                  half3 normal;
                  normal.xy = packednormal.xy * 2 - 1;
                  normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                  return normal;
               }
               half3 UnpackNormal(half4 packednormal)
               {
                  #if defined(UNITY_NO_DXT5nm)
                     return packednormal.xyz * 2 - 1;
                  #else
                     return UnpackNormalmapRGorAG(packednormal);
                  #endif
               }
               #endif
               #if _HDRP || _URP

               half3 UnpackScaleNormal(half4 packednormal, half scale)
               {
                 #ifndef UNITY_NO_DXT5nm
                   // Unpack normal as DXT5nm (1, y, 1, x) or BC5 (x, y, 0, 1)
                   // Note neutral texture like "bump" is (0, 0, 1, 1) to work with both plain RGB normal and DXT5nm/BC5
                   packednormal.x *= packednormal.w;
                 #endif
                   half3 normal;
                   normal.xy = (packednormal.xy * 2 - 1) * scale;
                   normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                   return normal;
               }	

             #endif


            void GetSun(out float3 lightDir, out float3 color)
            {
               lightDir = float3(0.5, 0.5, 0);
               color = 1;
               #if _HDRP
                  if (_DirectionalLightCount > 0)
                  {
                     DirectionalLightData light = _DirectionalLightDatas[0];
                     lightDir = -light.forward.xyz;
                     color = light.color;
                  }
               #elif _STANDARD
			         lightDir = normalize(_WorldSpaceLightPos0.xyz);
                  color = _LightColor0.rgb;
               #elif _URP
	               Light light = GetMainLight();
	               lightDir = light.direction;
	               color = light.color;
               #endif
            }


            
         CBUFFER_START(UnityPerMaterial)

            
	half4 _Color;
	half _Glossiness;
	half _Metallic;
	half _TextureVisibility;
	half _AngleStrength;
	float _Obstruction;
	float _Floor;
	float _UVs;
	float _BumpScale; 
	half4 _DissolveColor;
	float _DissolveColorSaturation;
	float _DissolveEmission;
	float _DissolveEmissionBooster;
	float _hasClippedShadows;
	float _ConeStrength;
	float _ConeObstructionDestroyRadius;
	float _CylinderStrength;
	float _CylinderObstructionDestroyRadius;
	float _CircleStrength;
	float _CircleObstructionDestroyRadius;
	float _IntrinsicDissolveStrength;
	float _DissolveFallOff;
	float _PreviewMode;
	float _AnimationEnabled;
	float _AnimationSpeed;
	float _TriggerMode;
	float _RaycastMode;
	float _IsExempt;
	float _DefaultEffectRadius;
	float _FloorY;
	float _FloorYTextureGradientLength;
	float4 _PlayerPos;
	float _PlayerPosYOffset;
	int _ArrayLength = 0;
	float4 _PlayersPosArray[100];
	float4 _PlayersDataArray[100];        
	float _TransitionDuration;
    float _tDirection = 0;
    float _numOfPlayersInside = 0;
    float _tValue = 0;
    float _id = 0;
    float _TexturedEmissionEdge;
    float _TexturedEmissionEdgeStrength;
	float _IsReplacementShader;
	half4 _DissolveColorGlobal;
	float _DissolveColorSaturationGlobal;
	float _DissolveEmissionGlobal;
	float _DissolveEmissionBoosterGlobal;
	float _TextureVisibilityGlobal;
	float _ObstructionGlobal;
	float _AngleStrengthGlobal;
	float _ConeStrengthGlobal;
	float _ConeObstructionDestroyRadiusGlobal;
	float _CylinderStrengthGlobal;
	float _CylinderObstructionDestroyRadiusGlobal;
	float _CircleStrengthGlobal;
	float _CircleObstructionDestroyRadiusGlobal;
	float _DissolveFallOffGlobal;
	float _IntrinsicDissolveStrengthGlobal;
	float _PreviewModeGlobal;
	float _UVsGlobal;
	float _hasClippedShadowsGlobal;
	float _FloorGlobal;
	float _FloorYGlobal;
	float _PlayerPosYOffsetGlobal;
	float _FloorYTextureGradientLengthGlobal;
	float _DefaultEffectRadiusGlobal;
	float _AnimationEnabledGlobal;
	float _AnimationSpeedGlobal;
	float _TransitionDurationGlobal;
    float _TexturedEmissionEdgeGlobal;
    float _TexturedEmissionEdgeStrengthGlobal;
    float _isReferenceMaterial;



         CBUFFER_END

         

         

         #ifdef unity_WorldToObject
#undef unity_WorldToObject
#endif
#ifdef unity_ObjectToWorld
#undef unity_ObjectToWorld
#endif
#define unity_ObjectToWorld GetObjectToWorldMatrix()
#define unity_WorldToObject GetWorldToObjectMatrix()

    float lllllllllllllllllllllllllllllllllllll(float fallOff, float strength, float input)
    {
        float k = fallOff;
        k = max(k,0.00001);
        float n = 1-strength;
        float b = exp(k*6);
        float j = input;
        float v = n/(k/(k*n-0.15*(k-n)));
        float y = ((j-v)/(b*(1-j)+j))+v;
        y = 1-y;
        return y * sign(strength);
    }
	sampler2D _MainTex;
    #ifdef _NORMALMAP
        sampler2D _BumpMap;
    #endif
	sampler2D _DissolveTex;
	sampler2D _DissolveTexGlobal;

	void Ext_SurfaceFunction0 (inout Surface o, ShaderData d)
	{
        bool lllllllllllllllllllllllllllllllllllllll =  (_numOfPlayersInside > 0 || _tDirection == -1 && _Time.y - _tValue < _TransitionDuration ) || (_numOfPlayersInside >= 0 && _tDirection == 1); 
        bool llllllllllllllllllllllllllllllllllllllll = !_TriggerMode && !_RaycastMode;
        if(!_IsExempt && (lllllllllllllllllllllllllllllllllllllll || llllllllllllllllllllllllllllllllllllllll) ) {
            if(_IsReplacementShader) {    
                _DissolveColor = _DissolveColorGlobal;
                _DissolveColorSaturation = _DissolveColorSaturationGlobal;
                _DissolveEmission = _DissolveEmissionGlobal;
                _DissolveEmissionBooster = _DissolveEmissionBoosterGlobal;
                _TextureVisibility = _TextureVisibilityGlobal;
                _Obstruction = _ObstructionGlobal;
                _AngleStrength = _AngleStrengthGlobal;
                _ConeStrength = _ConeStrengthGlobal;
                _ConeObstructionDestroyRadius = _ConeObstructionDestroyRadiusGlobal;
                _CylinderStrength = _CylinderStrengthGlobal;
                _CylinderObstructionDestroyRadius = _CylinderObstructionDestroyRadiusGlobal;
                _CircleStrength = _CircleStrengthGlobal;
                _CircleObstructionDestroyRadius = _CircleObstructionDestroyRadiusGlobal;
                _DissolveFallOff = _DissolveFallOffGlobal;
                _IntrinsicDissolveStrength = _IntrinsicDissolveStrengthGlobal;
                _PreviewMode = _PreviewModeGlobal;
                _UVs = _UVsGlobal;
                _hasClippedShadows = _hasClippedShadowsGlobal;
                _Floor = _FloorGlobal;
                _FloorY = _FloorYGlobal;
                _PlayerPosYOffset = _PlayerPosYOffsetGlobal;
                _FloorYTextureGradientLength = _FloorYTextureGradientLengthGlobal; 
                _AnimationEnabled = _AnimationEnabledGlobal;
                _AnimationSpeed = _AnimationSpeedGlobal;
                _TransitionDuration = _TransitionDurationGlobal;
                _DefaultEffectRadius = _DefaultEffectRadiusGlobal;
                _TexturedEmissionEdge = _TexturedEmissionEdgeGlobal;
                _TexturedEmissionEdgeStrength = _TexturedEmissionEdgeStrengthGlobal;
            }
            if(_IntrinsicDissolveStrength < 0) {_IntrinsicDissolveStrength = 0;}
            float3 l;
            d.worldSpaceNormal = mul(o.Normal, (float3x3)d.TBNMatrix);
            float3 ll = d.worldSpacePosition / (-1.0 * abs(_UVs) );
            if(_AnimationEnabled) {ll = ll + abs(((_Time.y) * _AnimationSpeed));}       
            if(_IsReplacementShader) {
                l = lerp(lerp(tex2D(_DissolveTexGlobal,ll.xz).rgb,tex2D(_DissolveTexGlobal,ll.yz ).rgb,abs(d.worldSpaceNormal.x)).rgb,tex2D(_DissolveTexGlobal,ll.xy).rgb,abs(d.worldSpaceNormal.z)).rgb;
            } else {
                l = lerp(lerp(tex2D(_DissolveTex,ll.xz).rgb,tex2D(_DissolveTex,ll.yz ).rgb,abs(d.worldSpaceNormal.x)).rgb,tex2D(_DissolveTex,ll.xy).rgb,abs(d.worldSpaceNormal.z)).rgb;
            }
            half lllllllllllllllllll = l;           
            float llllllllllllllllllll = 0;
            for (int i = 0; i < _ArrayLength; i++){
                float lll = _PlayersDataArray[i][1];
                float llll = _PlayersDataArray[i][2];
                float lllll = 1;
                if( llll!= 0 && lll != 0 && _Time.y-lll < _TransitionDuration) {
                    if(llll == 1) {lllll = ((_TransitionDuration-(_Time.y-lll))/_TransitionDuration);
                    } else {lllll = ((_Time.y-lll)/_TransitionDuration);}
                } else if(llll ==-1) {lllll = 1;
                } else if(llll == 1) {lllll = 0;
                } else {lllll = 1;}
                lllll = 1 - lllll;   
                float llllllllllllllllllllllllllllllllllllll = _PlayersDataArray[i][3];
                lllll = lllll * float(llllllllllllllllllllllllllllllllllllll == _id);
                float llllll = distance(_WorldSpaceCameraPos, _PlayersPosArray[i]);
                float lllllll = distance(_WorldSpaceCameraPos, d.worldSpacePosition);
                float3 llllllll = _WorldSpaceCameraPos - _PlayersPosArray[i];
                float lllllllll = length(llllllll);
                float llllllllll = _ConeObstructionDestroyRadius;
                float3 lllllllllll = normalize(llllllll);
                float llllllllllll = dot(d.worldSpacePosition - _PlayersPosArray[i], lllllllllll);
                float lllllllllllll = (llllllllllll/lllllllll)*llllllllll;
                float llllllllllllll = length((d.worldSpacePosition - _PlayersPosArray[i])-llllllllllll*lllllllllll);
                float lllllllllllllll = llllllllllllll<lllllllllllll;
                float llllllllllllllll = _CylinderObstructionDestroyRadius;
                float lllllllllllllllll = (llllllllllllll<llllllllllllllll)&&llllllllllll>0;
                float llllllllllllllllll = 0;
                float3 lllllllllllllllllllll =  d.worldSpacePosition - _PlayersPosArray[i];
                float3 llllllllllllllllllllll =  d.worldSpaceNormal;
                float lllllllllllllllllllllll = acos(dot(lllllllllllllllllllll,llllllllllllllllllllll)/(length(lllllllllllllllllllll)*length(llllllllllllllllllllll)));        
                float llllllllllllllllllllllll = _ScreenParams.x / _ScreenParams.y;
		        #if _HDRP
                    float4 lllllllllllllllllllllllll = mul(GetWorldToHClipMatrix(), float4(GetCameraRelativePositionWS(_PlayersPosArray[i].xyz), 1.0));
                    float4 llllllllllllllllllllllllll = ComputeScreenPos(lllllllllllllllllllllllll , _ProjectionParams.x);
		        #else
			        float4 lllllllllllllllllllllllll = mul(GetWorldToHClipMatrix(), float4(_PlayersPosArray[i].xyz, 1.0));
                    float4 llllllllllllllllllllllllll = ComputeScreenPos(lllllllllllllllllllllllll);
		        #endif
                llllllllllllllllllllllllll.xy /= llllllllllllllllllllllllll.w;
                llllllllllllllllllllllllll.x *= llllllllllllllllllllllll;
                #if _HDRP
                    //float4 lllllllllllllllllllllllllll = mul(GetWorldToHClipMatrix(), float4(GetCameraRelativePositionWS(d.worldSpacePosition), 1.0));
                    //half4 llllllllllllllllllllllllllll = ComputeScreenPos(lllllllllllllllllllllllllll , _ProjectionParams.x);
                    half4 llllllllllllllllllllllllllll = d.screenPos;
		        #else
                    float4 lllllllllllllllllllllllllll = mul(GetWorldToHClipMatrix(), float4(d.worldSpacePosition.xyz, 1.0));
                    float4 llllllllllllllllllllllllllll = ComputeScreenPos(lllllllllllllllllllllllllll);
		        #endif
                llllllllllllllllllllllllllll.xy /= llllllllllllllllllllllllllll.w;
                llllllllllllllllllllllllllll.x *= llllllllllllllllllllllll;
                float lllllllllllllllllllllllllllll = min(1,llllllllllllllllllllllll);
                float llllllllllllllllllllllllllllll =  distance(llllllllllllllllllllllllllll.xy,llllllllllllllllllllllllll.xy) < _CircleObstructionDestroyRadius/lllllllll*lllllllllllllllllllllllllllll;
                float lllllllllllllllllllllllllllllll = (distance(llllllllllllllllllllllllllll.xy,llllllllllllllllllllllllll.xy)/(_CircleObstructionDestroyRadius/lllllllll*lllllllllllllllllllllllllllll));
                float llllllllllllllllllllllllllllllll = (llllllllllllllllllllllllllllll)&&llllllllllll>0;        
                float lllllllllllllllllllllllllllllllll = llllllllllllllllll;
                if(lllll != 0 || (!_TriggerMode && !_RaycastMode)) {
                    if (_Obstruction == 1) {
                        if(lllllllllllllllllllllll<=1.5&&llllll>lllllll){
                            llllllllllllllllll = (sqrt((llllll-lllllll))*25/lllllllllllllllllllllll) *_AngleStrength;  
                            llllllllllllllllll = max(0,log(llllllllllllllllll*0.2));
                        }
                    }  else if (_Obstruction == 2) {
                        if(lllllllllllllll){
                            float lllllllllllllllllllllllllllllllllll = llllllllllllll/lllllllllllll;
                            llllllllllllllllll = lllllllllllllllllllllllllllllllllllll(_DissolveFallOff, _ConeStrength, lllllllllllllllllllllllllllllllllll);
                        }
                    } else  if (_Obstruction == 3) {
                        if(lllllllllllllllllllllll<= 1.5 && llllll > lllllll || lllllllllllllll){
                            if(lllllllllllllllllllllll<= 1.5 && llllll > lllllll) {
                                llllllllllllllllll = (sqrt((llllll-lllllll))*25/lllllllllllllllllllllll)*_AngleStrength;                   
                                llllllllllllllllll = max(0,log(llllllllllllllllll*0.2));
                            }
                            if (lllllllllllllll) {
                                float lllllllllllllllllllllllllllllllllll = llllllllllllll/lllllllllllll;
                                llllllllllllllllll = lllllllllllllllllllllllllllllllllllll(_DissolveFallOff, _ConeStrength, lllllllllllllllllllllllllllllllllll)+llllllllllllllllll;
                            }
                        }
                    }  else if (_Obstruction == 4) {
                        if(lllllllllllllllll){
                            float lllllllllllllllllllllllllllllllllll = llllllllllllll/llllllllllllllll;
                            llllllllllllllllll = lllllllllllllllllllllllllllllllllllll(_DissolveFallOff, _CylinderStrength, lllllllllllllllllllllllllllllllllll);
                        }
                    }  else if (_Obstruction == 5) {
                        if(lllllllllllllllllllllll<=1.5&&llllll>lllllll||lllllllllllllllll){
                            if(lllllllllllllllllllllll<=1.5&&llllll>lllllll) {
                                llllllllllllllllll = (sqrt((llllll-lllllll))*25/lllllllllllllllllllllll)*_AngleStrength;
                                llllllllllllllllll = max(0,log(llllllllllllllllll*0.2));              
                            }
                            if(lllllllllllllllll){
                                float lllllllllllllllllllllllllllllllllll = llllllllllllll/llllllllllllllll;
                                llllllllllllllllll = lllllllllllllllllllllllllllllllllllll(_DissolveFallOff, _CylinderStrength, lllllllllllllllllllllllllllllllllll) + llllllllllllllllll;                        
                            }                     
                        }
                    } else if (_Obstruction == 6) {
                        if (llllllllllllllllllllllllllllllll) {
                            llllllllllllllllll = lllllllllllllllllllllllllllllllllllll(_DissolveFallOff, _CircleStrength, lllllllllllllllllllllllllllllll);                 
                        }
                    }
                    llllllllllllllllll = llllllllllllllllll+(1*_IntrinsicDissolveStrength);
                    float llllllllllllllllllllllllllllllllll = llllllllllllllllll/_FloorYTextureGradientLength;
                    if(_Floor == 1) {
        	            if(d.worldSpacePosition.y < (_PlayersPosArray[i].y+_PlayerPosYOffset)) {
                            float llllllllllllllllllllllllllllllllllll = (_PlayersPosArray[i].y+_PlayerPosYOffset) - d.worldSpacePosition.y;
                            if(llllllllllllllllllllllllllllllllllll < 0) {llllllllllllllllllllllllllllllllllll = 0;}
                            if(llllllllllllllllllllllllllllllllllll < _FloorYTextureGradientLength) {
                                llllllllllllllllll = (_FloorYTextureGradientLength-llllllllllllllllllllllllllllllllllll)*llllllllllllllllllllllllllllllllll;
                            } else {llllllllllllllllll = 0;}
                        }
                    } else {
                        if(d.worldSpacePosition.y < _FloorY) {
                            float llllllllllllllllllllllllllllllllllll = _FloorY - d.worldSpacePosition.y;
                            if(llllllllllllllllllllllllllllllllllll<0){llllllllllllllllllllllllllllllllllll=0;}
                            if(llllllllllllllllllllllllllllllllllll<_FloorYTextureGradientLength){llllllllllllllllll = (_FloorYTextureGradientLength-llllllllllllllllllllllllllllllllllll)*llllllllllllllllllllllllllllllllll;
                            } else {llllllllllllllllll = 0;}
                        }
                    }
                    if(!_TriggerMode && !_RaycastMode) {if(distance(_PlayersPosArray[i], d.worldSpacePosition) > _DefaultEffectRadius) {llllllllllllllllll = 0;}}
                }
                if(_TriggerMode || _RaycastMode) {llllllllllllllllll =  lllll * llllllllllllllllll;  
                } else {llllllllllllllllll = llllllllllllllllll;}
                llllllllllllllllllll = max(llllllllllllllllllll,llllllllllllllllll);
            }            
            float lllllllllllllllllllllllllllllllll = llllllllllllllllllll;
            if(!_PreviewMode) {
                if (lllllllllllllllllllllllllllllllll==1){lllllllllllllllllllllllllllllllll=10;}
                if (!_hasClippedShadows) {
                #if defined(UNITY_PASS_SHADOWCASTER)
                #if defined(SHADOWS_DEPTH)
                if (!any(unity_LightShadowBias)){clip(lllllllllllllllllll- lllllllllllllllllllllllllllllllll);}
                else{if(_hasClippedShadows) {clip(lllllllllllllllllll- lllllllllllllllllllllllllllllllll);}}
                #endif
                #else
                    clip(lllllllllllllllllll- lllllllllllllllllllllllllllllllll);
                #endif
                } else {clip(lllllllllllllllllll- lllllllllllllllllllllllllllllllll);}
            }       
            half4 c = lerp(1, tex2D(_MainTex, d.texcoord0.xy), _TextureVisibility) * _Color;        
            o.Albedo = clamp(_DissolveColor*lllllllllllllllllllllllllllllllll, 0, _DissolveColorSaturation) + c.rgb;
            if(_PreviewMode) {
                if((lllllllllllllllllll - lllllllllllllllllllllllllllllllll)< 0) {
                    o.Albedo = half4(1,1,1,1);
                } else {
                    o.Albedo = half4(0,0,0,1);
                }
            }
            if(_TexturedEmissionEdge) {
                _TexturedEmissionEdgeStrength = 0.2 + (_TexturedEmissionEdgeStrength*(0.8-0.2));
                o.Emission =  min( clamp(lerp(1,_DissolveColor,_DissolveColorSaturation)*lllllllllllllllllllllllllllllllll, 0, 1)*sqrt(_DissolveEmission*_DissolveEmissionBooster), clamp(lerp(1,_DissolveColor,_DissolveColorSaturation) *  clamp(((lllllllllllllllllllllllllllllllll/_TexturedEmissionEdgeStrength) - lllllllllllllllllll),0,1), 0, 1)*sqrt(_DissolveEmission*_DissolveEmissionBooster));

            } else {
                o.Emission =  clamp(lerp(1,_DissolveColor,_DissolveColorSaturation)*lllllllllllllllllllllllllllllllll, 0, 1)*sqrt(_DissolveEmission*_DissolveEmissionBooster);
            } 
            #if _HDRP
                o.Emission =  o.Emission * pow(_DissolveEmissionBooster,4);
            #endif

            #ifdef _NORMALMAP
                o.Normal = UnpackScaleNormal(tex2D(_BumpMap, d.texcoord0.xy), _BumpScale);
            #endif
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = 1;

        } else {
            half4 c = tex2D (_MainTex, d.texcoord0.xy) * _Color;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
            #ifdef _NORMALMAP
            o.Normal = UnpackScaleNormal(tex2D(_BumpMap, d.texcoord0.xy), _BumpScale);
            #endif
        }    
	}




        
            void ChainSurfaceFunction(inout Surface l, inout ShaderData d)
            {
                  Ext_SurfaceFunction0(l, d);
                 // Ext_SurfaceFunction1(l, d);
                 // Ext_SurfaceFunction2(l, d);
                 // Ext_SurfaceFunction3(l, d);
                 // Ext_SurfaceFunction4(l, d);
                 // Ext_SurfaceFunction5(l, d);
                 // Ext_SurfaceFunction6(l, d);
                 // Ext_SurfaceFunction7(l, d);
                 // Ext_SurfaceFunction8(l, d);
                 // Ext_SurfaceFunction9(l, d);
		           // Ext_SurfaceFunction10(l, d);
                 // Ext_SurfaceFunction11(l, d);
                 // Ext_SurfaceFunction12(l, d);
                 // Ext_SurfaceFunction13(l, d);
                 // Ext_SurfaceFunction14(l, d);
                 // Ext_SurfaceFunction15(l, d);
                 // Ext_SurfaceFunction16(l, d);
                 // Ext_SurfaceFunction17(l, d);
                 // Ext_SurfaceFunction18(l, d);
		           // Ext_SurfaceFunction19(l, d);
            }

            void ChainModifyVertex(inout VertexData v, inout VertexToPixel v2p)
            {
                 ExtraV2F d;
                 ZERO_INITIALIZE(ExtraV2F, d);
                 ZERO_INITIALIZE(Blackboard, d.blackboard);

                 //  Ext_ModifyVertex0(v, d);
                 // Ext_ModifyVertex1(v, d);
                 // Ext_ModifyVertex2(v, d);
                 // Ext_ModifyVertex3(v, d);
                 // Ext_ModifyVertex4(v, d);
                 // Ext_ModifyVertex5(v, d);
                 // Ext_ModifyVertex6(v, d);
                 // Ext_ModifyVertex7(v, d);
                 // Ext_ModifyVertex8(v, d);
                 // Ext_ModifyVertex9(v, d);
                 // Ext_ModifyVertex10(v, d);
                 // Ext_ModifyVertex11(v, d);
                 // Ext_ModifyVertex12(v, d);
                 // Ext_ModifyVertex13(v, d);
                 // Ext_ModifyVertex14(v, d);
                 // Ext_ModifyVertex15(v, d);
                 // Ext_ModifyVertex16(v, d);
                 // Ext_ModifyVertex17(v, d);
                 // Ext_ModifyVertex18(v, d);
                 // Ext_ModifyVertex19(v, d);


                 // #if %EXTRAV2F0REQUIREKEY%
                 // v2p.extraV2F0 = d.extraV2F0;
                 // #endif

                 // #if %EXTRAV2F1REQUIREKEY%
                 // v2p.extraV2F1 = d.extraV2F1;
                 // #endif

                 // #if %EXTRAV2F2REQUIREKEY%
                 // v2p.extraV2F2 = d.extraV2F2;
                 // #endif

                 // #if %EXTRAV2F3REQUIREKEY%
                 // v2p.extraV2F3 = d.extraV2F3;
                 // #endif

                 // #if %EXTRAV2F4REQUIREKEY%
                 // v2p.extraV2F4 = d.extraV2F4;
                 // #endif

                 // #if %EXTRAV2F5REQUIREKEY%
                 // v2p.extraV2F5 = d.extraV2F5;
                 // #endif

                 // #if %EXTRAV2F6REQUIREKEY%
                 // v2p.extraV2F6 = d.extraV2F6;
                 // #endif

                 // #if %EXTRAV2F7REQUIREKEY%
                 // v2p.extraV2F7 = d.extraV2F7;
                 // #endif
            }

            void ChainModifyTessellatedVertex(inout VertexData v, inout VertexToPixel v2p)
            {
               ExtraV2F d;
               ZERO_INITIALIZE(ExtraV2F, d);
               ZERO_INITIALIZE(Blackboard, d.blackboard);

               // #if %EXTRAV2F0REQUIREKEY%
               // d.extraV2F0 = v2p.extraV2F0;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // d.extraV2F1 = v2p.extraV2F1;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // d.extraV2F2 = v2p.extraV2F2;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // d.extraV2F3 = v2p.extraV2F3;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // d.extraV2F4 = v2p.extraV2F4;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // d.extraV2F5 = v2p.extraV2F5;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // d.extraV2F6 = v2p.extraV2F6;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // d.extraV2F7 = v2p.extraV2F7;
               // #endif


               // Ext_ModifyTessellatedVertex0(v, d);
               // Ext_ModifyTessellatedVertex1(v, d);
               // Ext_ModifyTessellatedVertex2(v, d);
               // Ext_ModifyTessellatedVertex3(v, d);
               // Ext_ModifyTessellatedVertex4(v, d);
               // Ext_ModifyTessellatedVertex5(v, d);
               // Ext_ModifyTessellatedVertex6(v, d);
               // Ext_ModifyTessellatedVertex7(v, d);
               // Ext_ModifyTessellatedVertex8(v, d);
               // Ext_ModifyTessellatedVertex9(v, d);
               // Ext_ModifyTessellatedVertex10(v, d);
               // Ext_ModifyTessellatedVertex11(v, d);
               // Ext_ModifyTessellatedVertex12(v, d);
               // Ext_ModifyTessellatedVertex13(v, d);
               // Ext_ModifyTessellatedVertex14(v, d);
               // Ext_ModifyTessellatedVertex15(v, d);
               // Ext_ModifyTessellatedVertex16(v, d);
               // Ext_ModifyTessellatedVertex17(v, d);
               // Ext_ModifyTessellatedVertex18(v, d);
               // Ext_ModifyTessellatedVertex19(v, d);

               // #if %EXTRAV2F0REQUIREKEY%
               // v2p.extraV2F0 = d.extraV2F0;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // v2p.extraV2F1 = d.extraV2F1;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // v2p.extraV2F2 = d.extraV2F2;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // v2p.extraV2F3 = d.extraV2F3;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // v2p.extraV2F4 = d.extraV2F4;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // v2p.extraV2F5 = d.extraV2F5;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // v2p.extraV2F6 = d.extraV2F6;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // v2p.extraV2F7 = d.extraV2F7;
               // #endif
            }

            void ChainFinalColorForward(inout Surface l, inout ShaderData d, inout half4 color)
            {
               //   Ext_FinalColorForward0(l, d, color);
               //   Ext_FinalColorForward1(l, d, color);
               //   Ext_FinalColorForward2(l, d, color);
               //   Ext_FinalColorForward3(l, d, color);
               //   Ext_FinalColorForward4(l, d, color);
               //   Ext_FinalColorForward5(l, d, color);
               //   Ext_FinalColorForward6(l, d, color);
               //   Ext_FinalColorForward7(l, d, color);
               //   Ext_FinalColorForward8(l, d, color);
               //   Ext_FinalColorForward9(l, d, color);
               //  Ext_FinalColorForward10(l, d, color);
               //  Ext_FinalColorForward11(l, d, color);
               //  Ext_FinalColorForward12(l, d, color);
               //  Ext_FinalColorForward13(l, d, color);
               //  Ext_FinalColorForward14(l, d, color);
               //  Ext_FinalColorForward15(l, d, color);
               //  Ext_FinalColorForward16(l, d, color);
               //  Ext_FinalColorForward17(l, d, color);
               //  Ext_FinalColorForward18(l, d, color);
               //  Ext_FinalColorForward19(l, d, color);
            }

            void ChainFinalGBufferStandard(inout Surface s, inout ShaderData d, inout half4 GBuffer0, inout half4 GBuffer1, inout half4 GBuffer2, inout half4 outEmission, inout half4 outShadowMask)
            {
               //   Ext_FinalGBufferStandard0(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard1(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard2(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard3(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard4(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard5(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard6(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard7(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard8(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard9(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard10(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard11(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard12(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard13(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard14(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard15(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard16(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard17(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard18(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard19(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
            }



         

         ShaderData CreateShaderData(VertexToPixel i
                  #if NEED_FACING
                     , bool facing
                  #endif
         )
         {
            ShaderData d = (ShaderData)0;
            d.clipPos = i.pos;
            d.worldSpacePosition = i.worldPos;

            d.worldSpaceNormal = normalize(i.worldNormal);
            d.worldSpaceTangent = normalize(i.worldTangent.xyz);
            d.tangentSign = i.worldTangent.w;
            float3 bitangent = cross(i.worldTangent.xyz, i.worldNormal) * d.tangentSign * -1;
            

            d.TBNMatrix = float3x3(d.worldSpaceTangent, bitangent, d.worldSpaceNormal);
            d.worldSpaceViewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

            d.tangentSpaceViewDir = mul(d.TBNMatrix, d.worldSpaceViewDir);
             d.texcoord0 = i.texcoord0;
            // d.texcoord1 = i.texcoord1;
            // d.texcoord2 = i.texcoord2;

            // #if %TEXCOORD3REQUIREKEY%
            // d.texcoord3 = i.texcoord3;
            // #endif

            // d.isFrontFace = facing;
            // #if %VERTEXCOLORREQUIREKEY%
            // d.vertexColor = i.vertexColor;
            // #endif

            // these rarely get used, so we back transform them. Usually will be stripped.
            #if _HDRP
                // d.localSpacePosition = mul(unity_WorldToObject, float4(GetCameraRelativePositionWS(i.worldPos), 1)).xyz;
            #else
                // d.localSpacePosition = mul(unity_WorldToObject, float4(i.worldPos, 1)).xyz;
            #endif
            // d.localSpaceNormal = normalize(mul((float3x3)unity_WorldToObject, i.worldNormal));
            // d.localSpaceTangent = normalize(mul((float3x3)unity_WorldToObject, i.worldTangent.xyz));

            // #if %SCREENPOSREQUIREKEY%
             d.screenPos = i.screenPos;
             d.screenUV = (i.screenPos.xy / i.screenPos.w);
            // #endif


            // #if %EXTRAV2F0REQUIREKEY%
            // d.extraV2F0 = i.extraV2F0;
            // #endif

            // #if %EXTRAV2F1REQUIREKEY%
            // d.extraV2F1 = i.extraV2F1;
            // #endif

            // #if %EXTRAV2F2REQUIREKEY%
            // d.extraV2F2 = i.extraV2F2;
            // #endif

            // #if %EXTRAV2F3REQUIREKEY%
            // d.extraV2F3 = i.extraV2F3;
            // #endif

            // #if %EXTRAV2F4REQUIREKEY%
            // d.extraV2F4 = i.extraV2F4;
            // #endif

            // #if %EXTRAV2F5REQUIREKEY%
            // d.extraV2F5 = i.extraV2F5;
            // #endif

            // #if %EXTRAV2F6REQUIREKEY%
            // d.extraV2F6 = i.extraV2F6;
            // #endif

            // #if %EXTRAV2F7REQUIREKEY%
            // d.extraV2F7 = i.extraV2F7;
            // #endif

            return d;
         }
         

         
         #if defined(SHADERPASS_SHADOWCASTER)
            float3 _LightDirection;
         #endif

         // vertex shader
         VertexToPixel Vert (VertexData v)
         {
           
           VertexToPixel o = (VertexToPixel)0;

           UNITY_SETUP_INSTANCE_ID(v);
           UNITY_TRANSFER_INSTANCE_ID(v, o);
           UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


#if !_TESSELLATION_ON
           ChainModifyVertex(v, o);
#endif

            o.texcoord0 = v.texcoord0;
           // o.texcoord1 = v.texcoord1;
           // o.texcoord2 = v.texcoord2;

           // #if %TEXCOORD3REQUIREKEY%
           // o.texcoord3 = v.texcoord3;
           // #endif

           // #if %VERTEXCOLORREQUIREKEY%
           // o.vertexColor = v.vertexColor;
           // #endif
           
           VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
           o.worldPos = TransformObjectToWorld(v.vertex.xyz);
           o.worldNormal = TransformObjectToWorldNormal(v.normal);
           o.worldTangent = float4(TransformObjectToWorldDir(v.tangent.xyz), v.tangent.w);


          #if defined(SHADERPASS_SHADOWCASTER)
              // Define shadow pass specific clip position for Universal
              o.pos = TransformWorldToHClip(ApplyShadowBias(o.worldPos, o.worldNormal, _LightDirection));
              #if UNITY_REVERSED_Z
                  o.pos.z = min(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
              #else
                  o.pos.z = max(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
              #endif
          #elif defined(SHADERPASS_META)
              o.pos = MetaVertexPosition(float4(v.vertex.xyz, 0), v.texcoord1, v.texcoord2, unity_LightmapST, unity_DynamicLightmapST);
          #else
              o.pos = TransformWorldToHClip(o.worldPos);
          #endif

          // #if %SCREENPOSREQUIREKEY%
           o.screenPos = ComputeScreenPos(o.pos, _ProjectionParams.x);
          // #endif

          #if defined(SHADERPASS_FORWARD)
              OUTPUT_LIGHTMAP_UV(v.texcoord1, unity_LightmapST, o.lightmapUV);
              OUTPUT_SH(o.worldNormal, o.sh);
          #endif

          #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
              half3 vertexLight = VertexLighting(o.worldPos, o.worldNormal);
              half fogFactor = ComputeFogFactor(o.pos.z);
              o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
          #endif

          #ifdef _MAIN_LIGHT_SHADOWS
              o.shadowCoord = GetShadowCoord(vertexInput);
          #endif

           return o;
         }


         

         // fragment shader
         half4 Frag (VertexToPixel IN
            #ifdef _DEPTHOFFSET_ON
              , out float outputDepth : SV_Depth
            #endif
            #if NEED_FACING
               , bool facing : SV_IsFrontFace
            #endif
         ) : SV_Target
         {
           UNITY_SETUP_INSTANCE_ID(IN);
           UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

           ShaderData d = CreateShaderData(IN
                  #if NEED_FACING
                     , facing
                  #endif
               );
           Surface l = (Surface)0;

           #ifdef _DEPTHOFFSET_ON
              l.outputDepth = outputDepth;
           #endif

           l.Albedo = half3(0.5, 0.5, 0.5);
           l.Normal = float3(0,0,1);
           l.Occlusion = 1;
           l.Alpha = 1;

           ChainSurfaceFunction(l, d);

           #ifdef _DEPTHOFFSET_ON
              outputDepth = l.outputDepth;
           #endif

           #if defined(_USESPECULAR) || _SIMPLELIT
              float3 specular = l.Specular;
              float metallic = 1;
           #else   
              float3 specular = 0;
              float metallic = l.Metallic;
           #endif

           
            InputData inputData;

            inputData.positionWS = IN.worldPos;
            #if _WORLDSPACENORMAL
              inputData.normalWS = l.Normal;
            #else
              inputData.normalWS = normalize(TangentToWorldSpace(d, l.Normal));
            #endif
            
            inputData.viewDirectionWS = SafeNormalize(d.worldSpaceViewDir);


            #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                  inputData.shadowCoord = IN.shadowCoord;
            #elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
                  inputData.shadowCoord = TransformWorldToShadowCoord(IN.worldPos);
            #else
                  inputData.shadowCoord = float4(0, 0, 0, 0);
            #endif

            inputData.fogCoord = IN.fogFactorAndVertexLight.x;
            inputData.vertexLighting = IN.fogFactorAndVertexLight.yzw;
            #if defined(_OVERRIDE_BAKEDGI)
               inputData.bakedGI = l.DiffuseGI;
            #else
               inputData.bakedGI = SAMPLE_GI(IN.lightmapUV, IN.sh, inputData.normalWS);
            #endif

            #if !_UNLIT
               #if _SIMPLELIT
                  half4 color = UniversalFragmentBlinnPhong(
                     inputData,
                     l.Albedo,
                     float4(specular * l.Smoothness, 0),
                     l.SpecularPower * 128,
                     l.Emission,
                     l.Alpha);
                  color.a = l.Alpha;
               #else
                  half4 color = UniversalFragmentPBR(
                  inputData,
                  l.Albedo,
                  metallic,
                  specular,
                  l.Smoothness,
                  l.Occlusion,
                  l.Emission,
                  l.Alpha); 
               #endif
               color.rgb = MixFog(color.rgb, inputData.fogCoord);

            #else
               half4 color = half4(l.Albedo, l.Alpha);
               color.rgb = MixFog(color.rgb, inputData.fogCoord);
            #endif
            ChainFinalColorForward(l, d, color);

            return color;

         }

         ENDHLSL

      }


      
        Pass
        {
            Name "ShadowCaster"
            Tags 
            { 
                "LightMode" = "ShadowCaster"
            }
           
            // Render State
            Blend One Zero, One Zero
            Cull Back
            ZTest LEqual
            ZWrite On
            // ColorMask: <None>

            

            HLSLPROGRAM

               #pragma vertex Vert
   #pragma fragment Frag

            #pragma target 3.0

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma multi_compile_instancing
        
            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define SHADERPASS_SHADOWCASTER
            #define _PASSSHADOW 1

            
      #pragma shader_feature_local _NORMALMAP


   #define _URP 1



                 
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

                  #undef WorldNormalVector
      #define WorldNormalVector(data, normal) mul(normal, data.TBNMatrix)
      
      #define UnityObjectToWorldNormal(normal) mul(GetObjectToWorldMatrix(), normal)

      #define _WorldSpaceLightPos0 _MainLightPosition
      
      #define UNITY_DECLARE_TEX2D(name) TEXTURE2D(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2D_NOSAMPLER(name) TEXTURE2D(name);
      #define UNITY_DECLARE_TEX2DARRAY(name) TEXTURE2D_ARRAY(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(name) TEXTURE2D_ARRAY(name);

      #define UNITY_SAMPLE_TEX2DARRAY(tex,coord)            SAMPLE_TEXTURE2D_ARRAY(tex, sampler##tex, coord.xy, coord.z)
      #define UNITY_SAMPLE_TEX2DARRAY_LOD(tex,coord,lod)    SAMPLE_TEXTURE2D_ARRAY_LOD(tex, sampler##tex, coord.xy, coord.z, lod)
      #define UNITY_SAMPLE_TEX2D(tex, coord)                SAMPLE_TEXTURE2D(tex, sampler##tex, coord)
      #define UNITY_SAMPLE_TEX2D_SAMPLER(tex, samp, coord)  SAMPLE_TEXTURE2D(tex, sampler##samp, coord)

      #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod)   SAMPLE_TEXTURE2D_LOD(tex, sampler_##tex, coord, lod)
      #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) SAMPLE_TEXTURE2D_LOD (tex, sampler##samplertex,coord, lod)
     
      #if defined(UNITY_COMPILER_HLSL)
         #define UNITY_INITIALIZE_OUTPUT(type,name) name = (type)0;
      #else
         #define UNITY_INITIALIZE_OUTPUT(type,name)
      #endif

      #define sampler2D_float sampler2D
      #define sampler2D_half sampler2D



      // data across stages, stripped like the above.
      struct VertexToPixel
      {
         float4 pos : SV_POSITION;
         float3 worldPos : TEXCOORD0;
         float3 worldNormal : TEXCOORD1;
         float4 worldTangent : TEXCOORD2;
          float4 texcoord0 : TEXCCOORD3;
         // float4 texcoord1 : TEXCCOORD4;
         // float4 texcoord2 : TEXCCOORD5;

         // #if %TEXCOORD3REQUIREKEY%
         // float4 texcoord3 : TEXCCOORD6;
         // #endif

         // #if %SCREENPOSREQUIREKEY%
          float4 screenPos : TEXCOORD7;
         // #endif

         // #if %VERTEXCOLORREQUIREKEY%
         // half4 vertexColor : COLOR;
         // #endif

         // #if %EXTRAV2F0REQUIREKEY%
         // float4 extraV2F0 : TEXCOORD12;
         // #endif

         // #if %EXTRAV2F1REQUIREKEY%
         // float4 extraV2F1 : TEXCOORD13;
         // #endif

         // #if %EXTRAV2F2REQUIREKEY%
         // float4 extraV2F2 : TEXCOORD14;
         // #endif

         // #if %EXTRAV2F3REQUIREKEY%
         // float4 extraV2F3 : TEXCOORD15;
         // #endif

         // #if %EXTRAV2F4REQUIREKEY%
         // float4 extraV2F4 : TEXCOORD16;
         // #endif

         // #if %EXTRAV2F5REQUIREKEY%
         // float4 extraV2F5 : TEXCOORD17;
         // #endif

         // #if %EXTRAV2F6REQUIREKEY%
         // float4 extraV2F6 : TEXCOORD18;
         // #endif

         // #if %EXTRAV2F7REQUIREKEY%
         // float4 extraV2F7 : TEXCOORD19;
         // #endif
            
         #if defined(LIGHTMAP_ON)
            float2 lightmapUV : TEXCOORD8;
         #endif
         #if !defined(LIGHTMAP_ON)
            float3 sh : TEXCOORD9;
         #endif
            float4 fogFactorAndVertexLight : TEXCOORD10;
            float4 shadowCoord : TEXCOORD11;
         #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
         #endif
         #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
         #endif
         #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
         #endif
         #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
         #endif
      };

         
            
            
            // data describing the user output of a pixel
            struct Surface
            {
               half3 Albedo;
               half Height;
               half3 Normal;
               half Smoothness;
               half3 Emission;
               half Metallic;
               half3 Specular;
               half Occlusion;
               half SpecularPower; // for simple lighting
               half Alpha;
               float outputDepth; // if written, SV_Depth semantic is used. ShaderData.clipPos.z is unused value
               // HDRP Only
               half SpecularOcclusion;
               half SubsurfaceMask;
               half Thickness;
               half CoatMask;
               half CoatSmoothness;
               half Anisotropy;
               half IridescenceMask;
               half IridescenceThickness;
               int DiffusionProfileHash;
               // requires _OVERRIDE_BAKEDGI to be defined, but is mapped in all pipelines
               float3 DiffuseGI;
               float3 BackDiffuseGI;
               float3 SpecularGI;
               // requires _OVERRIDE_SHADOWMASK to be defines
               float4 ShadowMask;
            };

            // Data the user declares in blackboard blocks
            struct Blackboard
            {
                
                float blackboardDummyData;
            };

            // data the user might need, this will grow to be big. But easy to strip
            struct ShaderData
            {
               float4 clipPos; // SV_POSITION
               float3 localSpacePosition;
               float3 localSpaceNormal;
               float3 localSpaceTangent;
        
               float3 worldSpacePosition;
               float3 worldSpaceNormal;
               float3 worldSpaceTangent;
               float tangentSign;

               float3 worldSpaceViewDir;
               float3 tangentSpaceViewDir;

               float4 texcoord0;
               float4 texcoord1;
               float4 texcoord2;
               float4 texcoord3;

               float2 screenUV;
               float4 screenPos;

               float4 vertexColor;
               bool isFrontFace;

               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;

               float3x3 TBNMatrix;
               Blackboard blackboard;
            };

            struct VertexData
            {
               #if SHADER_TARGET > 30
               // uint vertexID : SV_VertexID;
               #endif
               float4 vertex : POSITION;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;

               // would love to strip these, but they are used in certain
               // combinations of the lighting system, and may be used
               // by the user as well, so no easy way to strip them.

               float4 texcoord1 : TEXCOORD1;
               float4 texcoord2 : TEXCOORD2;

               // #if %TEXCOORD3REQUIREKEY%
               // float4 texcoord3 : TEXCOORD3;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               #if _HDRP && (_PASSMOTIONVECTOR || (_PASSFORWARD && defined(_WRITE_TRANSPARENT_MOTION_VECTOR)))
                  float3 previousPositionOS : TEXCOORD4; // Contain previous transform position (in case of skinning for example)
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity    : TEXCOORD5; // Add Precomputed Velocity (Alembic computes velocities on runtime side).
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct TessVertex 
            {
               float4 vertex : INTERNALTESSPOS;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;
               float4 texcoord1 : TEXCOORD1;
               float4 texcoord2 : TEXCOORD2;

               // #if %TEXCOORD3REQUIREKEY%
               // float4 texcoord3 : TEXCOORD3;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               // #if %EXTRAV2F0REQUIREKEY%
               // float4 extraV2F0 : TEXCOORD5;
               // endif

               // #if %EXTRAV2F1REQUIREKEY%
               // float4 extraV2F1 : TEXCOORD6;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // float4 extraV2F2 : TEXCOORD7;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // float4 extraV2F3 : TEXCOORD8;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // float4 extraV2F4 : TEXCOORD9;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // float4 extraV2F5 : TEXCOORD10;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // float4 extraV2F6 : TEXCOORD11;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // float4 extraV2F7 : TEXCOORD12;
               // #endif

               #if _HDRP && (_PASSMOTIONVECTOR || (_PASSFORWARD && defined(_WRITE_TRANSPARENT_MOTION_VECTOR)))
                  float3 previousPositionOS : TEXCOORD13; // Contain previous transform position (in case of skinning for example)
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity : TEXCOORD14;
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
               UNITY_VERTEX_OUTPUT_STEREO
            };

            struct ExtraV2F
            {
               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;
               Blackboard blackboard;
            };


            float3 WorldToTangentSpace(ShaderData d, float3 normal)
            {
               return mul(d.TBNMatrix, normal);
            }

            float3 TangentToWorldSpace(ShaderData d, float3 normal)
            {
               return mul(normal, d.TBNMatrix);
            }

            // in this case, make standard more like SRPs, because we can't fix
            // unity_WorldToObject in HDRP, since it already does macro-fu there

            #if _STANDARD
               float3 TransformWorldToObject(float3 p) { return mul(unity_WorldToObject, float4(p, 1)); };
               float3 TransformObjectToWorld(float3 p) { return mul(unity_ObjectToWorld, float4(p, 1)); };
               float4 TransformWorldToObject(float4 p) { return mul(unity_WorldToObject, p); };
               float4 TransformObjectToWorld(float4 p) { return mul(unity_ObjectToWorld, p); };
               float4x4 GetWorldToObjectMatrix() { return unity_WorldToObject; }
               float4x4 GetObjectToWorldMatrix() { return unity_ObjectToWorld; }
               #if (defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (SHADER_TARGET_SURFACE_ANALYSIS && !SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod) tex.SampleLevel (sampler##tex,coord, lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) tex.SampleLevel (sampler##samplertex,coord, lod)
              #else
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord,lod) tex2D (tex,coord,0,lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord,lod) tex2D (tex,coord,0,lod)
              #endif

               #undef GetObjectToWorldMatrix()
               #undef GetWorldToObjectMatrix()
               #undef GetWorldToViewMatrix()
               #undef UNITY_MATRIX_I_V
               #undef UNITY_MATRIX_P
               #undef GetWorldToHClipMatrix()
               #undef GetObjectToWorldMatrix()V
               #undef UNITY_MATRIX_T_MV
               #undef UNITY_MATRIX_IT_MV
               #undef GetObjectToWorldMatrix()VP

               #define GetObjectToWorldMatrix()     unity_ObjectToWorld
               #define GetWorldToObjectMatrix()   unity_WorldToObject
               #define GetWorldToViewMatrix()     unity_MatrixV
               #define UNITY_MATRIX_I_V   unity_MatrixInvV
               #define GetViewToHClipMatrix()     OptimizeProjectionMatrix(glstate_matrix_projection)
               #define GetWorldToHClipMatrix()    unity_MatrixVP
               #define GetObjectToWorldMatrix()V    mul(GetWorldToViewMatrix(), GetObjectToWorldMatrix())
               #define UNITY_MATRIX_T_MV  transpose(GetObjectToWorldMatrix()V)
               #define UNITY_MATRIX_IT_MV transpose(mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V))
               #define GetObjectToWorldMatrix()VP   mul(GetWorldToHClipMatrix(), GetObjectToWorldMatrix())


            #endif

            float3 GetCameraWorldPosition()
            {
               #if _HDRP
                  return GetCameraRelativePositionWS(_WorldSpaceCameraPos);
               #else
                  return _WorldSpaceCameraPos;
               #endif
            }

            #if _GRABPASSUSED
               #if _STANDARD
                  TEXTURE2D(%GRABTEXTURE%);
                  SAMPLER(sampler_%GRABTEXTURE%);
               #endif

               half3 GetSceneColor(float2 uv)
               {
                  #if _STANDARD
                     return SAMPLE_TEXTURE2D(%GRABTEXTURE%, sampler_%GRABTEXTURE%, uv).rgb;
                  #else
                     return SHADERGRAPH_SAMPLE_SCENE_COLOR(uv);
                  #endif
               }
            #endif


      
            #if _STANDARD
               UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
               float GetSceneDepth(float2 uv) { return SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv)); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv)); } 
            #else
               float GetSceneDepth(float2 uv) { return SHADERGRAPH_SAMPLE_SCENE_DEPTH(uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv), _ZBufferParams); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv), _ZBufferParams); } 
            #endif

            float3 GetWorldPositionFromDepthBuffer(float2 uv, float3 worldSpaceViewDir)
            {
               float eye = GetLinearEyeDepth(uv);
               float3 camView = mul((float3x3)GetObjectToWorldMatrix(), transpose(mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V)) [2].xyz);

               float dt = dot(worldSpaceViewDir, camView);
               float3 div = worldSpaceViewDir/dt;
               float3 wpos = (eye * div) + GetCameraWorldPosition();
               return wpos;
            }

            #if _STANDARD
               UNITY_DECLARE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture);
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  float4 depthNorms = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture, uv);
                  float3 norms = DecodeViewNormalStereo(depthNorms);
                  norms = mul((float3x3)GetWorldToViewMatrix(), norms) * 0.5 + 0.5;
                  return norms;
               }
            #elif _HDRP
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  NormalData nd;
                  DecodeFromNormalBuffer(_ScreenSize.xy * uv, nd);
                  return nd.normalWS;
               }
            #elif _URP
               #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                  #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"
               #endif

               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                     return SampleSceneNormals(uv);
                  #else
                     float3 wpos = GetWorldPositionFromDepthBuffer(uv, worldSpaceViewDir);
                     return normalize(-cross(ddx(wpos), ddy(wpos))) * 0.5 + 0.5;
                  #endif

                }
             #endif

             #if _HDRP

               half3 UnpackNormalmapRGorAG(half4 packednormal)
               {
                     // This do the trick
                  packednormal.x *= packednormal.w;

                  half3 normal;
                  normal.xy = packednormal.xy * 2 - 1;
                  normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                  return normal;
               }
               half3 UnpackNormal(half4 packednormal)
               {
                  #if defined(UNITY_NO_DXT5nm)
                     return packednormal.xyz * 2 - 1;
                  #else
                     return UnpackNormalmapRGorAG(packednormal);
                  #endif
               }
               #endif
               #if _HDRP || _URP

               half3 UnpackScaleNormal(half4 packednormal, half scale)
               {
                 #ifndef UNITY_NO_DXT5nm
                   // Unpack normal as DXT5nm (1, y, 1, x) or BC5 (x, y, 0, 1)
                   // Note neutral texture like "bump" is (0, 0, 1, 1) to work with both plain RGB normal and DXT5nm/BC5
                   packednormal.x *= packednormal.w;
                 #endif
                   half3 normal;
                   normal.xy = (packednormal.xy * 2 - 1) * scale;
                   normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                   return normal;
               }	

             #endif


            void GetSun(out float3 lightDir, out float3 color)
            {
               lightDir = float3(0.5, 0.5, 0);
               color = 1;
               #if _HDRP
                  if (_DirectionalLightCount > 0)
                  {
                     DirectionalLightData light = _DirectionalLightDatas[0];
                     lightDir = -light.forward.xyz;
                     color = light.color;
                  }
               #elif _STANDARD
			         lightDir = normalize(_WorldSpaceLightPos0.xyz);
                  color = _LightColor0.rgb;
               #elif _URP
	               Light light = GetMainLight();
	               lightDir = light.direction;
	               color = light.color;
               #endif
            }


            
            CBUFFER_START(UnityPerMaterial)

               
	half4 _Color;
	half _Glossiness;
	half _Metallic;
	half _TextureVisibility;
	half _AngleStrength;
	float _Obstruction;
	float _Floor;
	float _UVs;
	float _BumpScale; 
	half4 _DissolveColor;
	float _DissolveColorSaturation;
	float _DissolveEmission;
	float _DissolveEmissionBooster;
	float _hasClippedShadows;
	float _ConeStrength;
	float _ConeObstructionDestroyRadius;
	float _CylinderStrength;
	float _CylinderObstructionDestroyRadius;
	float _CircleStrength;
	float _CircleObstructionDestroyRadius;
	float _IntrinsicDissolveStrength;
	float _DissolveFallOff;
	float _PreviewMode;
	float _AnimationEnabled;
	float _AnimationSpeed;
	float _TriggerMode;
	float _RaycastMode;
	float _IsExempt;
	float _DefaultEffectRadius;
	float _FloorY;
	float _FloorYTextureGradientLength;
	float4 _PlayerPos;
	float _PlayerPosYOffset;
	int _ArrayLength = 0;
	float4 _PlayersPosArray[100];
	float4 _PlayersDataArray[100];        
	float _TransitionDuration;
    float _tDirection = 0;
    float _numOfPlayersInside = 0;
    float _tValue = 0;
    float _id = 0;
    float _TexturedEmissionEdge;
    float _TexturedEmissionEdgeStrength;
	float _IsReplacementShader;
	half4 _DissolveColorGlobal;
	float _DissolveColorSaturationGlobal;
	float _DissolveEmissionGlobal;
	float _DissolveEmissionBoosterGlobal;
	float _TextureVisibilityGlobal;
	float _ObstructionGlobal;
	float _AngleStrengthGlobal;
	float _ConeStrengthGlobal;
	float _ConeObstructionDestroyRadiusGlobal;
	float _CylinderStrengthGlobal;
	float _CylinderObstructionDestroyRadiusGlobal;
	float _CircleStrengthGlobal;
	float _CircleObstructionDestroyRadiusGlobal;
	float _DissolveFallOffGlobal;
	float _IntrinsicDissolveStrengthGlobal;
	float _PreviewModeGlobal;
	float _UVsGlobal;
	float _hasClippedShadowsGlobal;
	float _FloorGlobal;
	float _FloorYGlobal;
	float _PlayerPosYOffsetGlobal;
	float _FloorYTextureGradientLengthGlobal;
	float _DefaultEffectRadiusGlobal;
	float _AnimationEnabledGlobal;
	float _AnimationSpeedGlobal;
	float _TransitionDurationGlobal;
    float _TexturedEmissionEdgeGlobal;
    float _TexturedEmissionEdgeStrengthGlobal;
    float _isReferenceMaterial;



            CBUFFER_END

            

            

            #ifdef unity_WorldToObject
#undef unity_WorldToObject
#endif
#ifdef unity_ObjectToWorld
#undef unity_ObjectToWorld
#endif
#define unity_ObjectToWorld GetObjectToWorldMatrix()
#define unity_WorldToObject GetWorldToObjectMatrix()

    float lllllllllllllllllllllllllllllllllllll(float fallOff, float strength, float input)
    {
        float k = fallOff;
        k = max(k,0.00001);
        float n = 1-strength;
        float b = exp(k*6);
        float j = input;
        float v = n/(k/(k*n-0.15*(k-n)));
        float y = ((j-v)/(b*(1-j)+j))+v;
        y = 1-y;
        return y * sign(strength);
    }
	sampler2D _MainTex;
    #ifdef _NORMALMAP
        sampler2D _BumpMap;
    #endif
	sampler2D _DissolveTex;
	sampler2D _DissolveTexGlobal;

	void Ext_SurfaceFunction0 (inout Surface o, ShaderData d)
	{
        bool lllllllllllllllllllllllllllllllllllllll =  (_numOfPlayersInside > 0 || _tDirection == -1 && _Time.y - _tValue < _TransitionDuration ) || (_numOfPlayersInside >= 0 && _tDirection == 1); 
        bool llllllllllllllllllllllllllllllllllllllll = !_TriggerMode && !_RaycastMode;
        if(!_IsExempt && (lllllllllllllllllllllllllllllllllllllll || llllllllllllllllllllllllllllllllllllllll) ) {
            if(_IsReplacementShader) {    
                _DissolveColor = _DissolveColorGlobal;
                _DissolveColorSaturation = _DissolveColorSaturationGlobal;
                _DissolveEmission = _DissolveEmissionGlobal;
                _DissolveEmissionBooster = _DissolveEmissionBoosterGlobal;
                _TextureVisibility = _TextureVisibilityGlobal;
                _Obstruction = _ObstructionGlobal;
                _AngleStrength = _AngleStrengthGlobal;
                _ConeStrength = _ConeStrengthGlobal;
                _ConeObstructionDestroyRadius = _ConeObstructionDestroyRadiusGlobal;
                _CylinderStrength = _CylinderStrengthGlobal;
                _CylinderObstructionDestroyRadius = _CylinderObstructionDestroyRadiusGlobal;
                _CircleStrength = _CircleStrengthGlobal;
                _CircleObstructionDestroyRadius = _CircleObstructionDestroyRadiusGlobal;
                _DissolveFallOff = _DissolveFallOffGlobal;
                _IntrinsicDissolveStrength = _IntrinsicDissolveStrengthGlobal;
                _PreviewMode = _PreviewModeGlobal;
                _UVs = _UVsGlobal;
                _hasClippedShadows = _hasClippedShadowsGlobal;
                _Floor = _FloorGlobal;
                _FloorY = _FloorYGlobal;
                _PlayerPosYOffset = _PlayerPosYOffsetGlobal;
                _FloorYTextureGradientLength = _FloorYTextureGradientLengthGlobal; 
                _AnimationEnabled = _AnimationEnabledGlobal;
                _AnimationSpeed = _AnimationSpeedGlobal;
                _TransitionDuration = _TransitionDurationGlobal;
                _DefaultEffectRadius = _DefaultEffectRadiusGlobal;
                _TexturedEmissionEdge = _TexturedEmissionEdgeGlobal;
                _TexturedEmissionEdgeStrength = _TexturedEmissionEdgeStrengthGlobal;
            }
            if(_IntrinsicDissolveStrength < 0) {_IntrinsicDissolveStrength = 0;}
            float3 l;
            d.worldSpaceNormal = mul(o.Normal, (float3x3)d.TBNMatrix);
            float3 ll = d.worldSpacePosition / (-1.0 * abs(_UVs) );
            if(_AnimationEnabled) {ll = ll + abs(((_Time.y) * _AnimationSpeed));}       
            if(_IsReplacementShader) {
                l = lerp(lerp(tex2D(_DissolveTexGlobal,ll.xz).rgb,tex2D(_DissolveTexGlobal,ll.yz ).rgb,abs(d.worldSpaceNormal.x)).rgb,tex2D(_DissolveTexGlobal,ll.xy).rgb,abs(d.worldSpaceNormal.z)).rgb;
            } else {
                l = lerp(lerp(tex2D(_DissolveTex,ll.xz).rgb,tex2D(_DissolveTex,ll.yz ).rgb,abs(d.worldSpaceNormal.x)).rgb,tex2D(_DissolveTex,ll.xy).rgb,abs(d.worldSpaceNormal.z)).rgb;
            }
            half lllllllllllllllllll = l;           
            float llllllllllllllllllll = 0;
            for (int i = 0; i < _ArrayLength; i++){
                float lll = _PlayersDataArray[i][1];
                float llll = _PlayersDataArray[i][2];
                float lllll = 1;
                if( llll!= 0 && lll != 0 && _Time.y-lll < _TransitionDuration) {
                    if(llll == 1) {lllll = ((_TransitionDuration-(_Time.y-lll))/_TransitionDuration);
                    } else {lllll = ((_Time.y-lll)/_TransitionDuration);}
                } else if(llll ==-1) {lllll = 1;
                } else if(llll == 1) {lllll = 0;
                } else {lllll = 1;}
                lllll = 1 - lllll;   
                float llllllllllllllllllllllllllllllllllllll = _PlayersDataArray[i][3];
                lllll = lllll * float(llllllllllllllllllllllllllllllllllllll == _id);
                float llllll = distance(_WorldSpaceCameraPos, _PlayersPosArray[i]);
                float lllllll = distance(_WorldSpaceCameraPos, d.worldSpacePosition);
                float3 llllllll = _WorldSpaceCameraPos - _PlayersPosArray[i];
                float lllllllll = length(llllllll);
                float llllllllll = _ConeObstructionDestroyRadius;
                float3 lllllllllll = normalize(llllllll);
                float llllllllllll = dot(d.worldSpacePosition - _PlayersPosArray[i], lllllllllll);
                float lllllllllllll = (llllllllllll/lllllllll)*llllllllll;
                float llllllllllllll = length((d.worldSpacePosition - _PlayersPosArray[i])-llllllllllll*lllllllllll);
                float lllllllllllllll = llllllllllllll<lllllllllllll;
                float llllllllllllllll = _CylinderObstructionDestroyRadius;
                float lllllllllllllllll = (llllllllllllll<llllllllllllllll)&&llllllllllll>0;
                float llllllllllllllllll = 0;
                float3 lllllllllllllllllllll =  d.worldSpacePosition - _PlayersPosArray[i];
                float3 llllllllllllllllllllll =  d.worldSpaceNormal;
                float lllllllllllllllllllllll = acos(dot(lllllllllllllllllllll,llllllllllllllllllllll)/(length(lllllllllllllllllllll)*length(llllllllllllllllllllll)));        
                float llllllllllllllllllllllll = _ScreenParams.x / _ScreenParams.y;
		        #if _HDRP
                    float4 lllllllllllllllllllllllll = mul(GetWorldToHClipMatrix(), float4(GetCameraRelativePositionWS(_PlayersPosArray[i].xyz), 1.0));
                    float4 llllllllllllllllllllllllll = ComputeScreenPos(lllllllllllllllllllllllll , _ProjectionParams.x);
		        #else
			        float4 lllllllllllllllllllllllll = mul(GetWorldToHClipMatrix(), float4(_PlayersPosArray[i].xyz, 1.0));
                    float4 llllllllllllllllllllllllll = ComputeScreenPos(lllllllllllllllllllllllll);
		        #endif
                llllllllllllllllllllllllll.xy /= llllllllllllllllllllllllll.w;
                llllllllllllllllllllllllll.x *= llllllllllllllllllllllll;
                #if _HDRP
                    //float4 lllllllllllllllllllllllllll = mul(GetWorldToHClipMatrix(), float4(GetCameraRelativePositionWS(d.worldSpacePosition), 1.0));
                    //half4 llllllllllllllllllllllllllll = ComputeScreenPos(lllllllllllllllllllllllllll , _ProjectionParams.x);
                    half4 llllllllllllllllllllllllllll = d.screenPos;
		        #else
                    float4 lllllllllllllllllllllllllll = mul(GetWorldToHClipMatrix(), float4(d.worldSpacePosition.xyz, 1.0));
                    float4 llllllllllllllllllllllllllll = ComputeScreenPos(lllllllllllllllllllllllllll);
		        #endif
                llllllllllllllllllllllllllll.xy /= llllllllllllllllllllllllllll.w;
                llllllllllllllllllllllllllll.x *= llllllllllllllllllllllll;
                float lllllllllllllllllllllllllllll = min(1,llllllllllllllllllllllll);
                float llllllllllllllllllllllllllllll =  distance(llllllllllllllllllllllllllll.xy,llllllllllllllllllllllllll.xy) < _CircleObstructionDestroyRadius/lllllllll*lllllllllllllllllllllllllllll;
                float lllllllllllllllllllllllllllllll = (distance(llllllllllllllllllllllllllll.xy,llllllllllllllllllllllllll.xy)/(_CircleObstructionDestroyRadius/lllllllll*lllllllllllllllllllllllllllll));
                float llllllllllllllllllllllllllllllll = (llllllllllllllllllllllllllllll)&&llllllllllll>0;        
                float lllllllllllllllllllllllllllllllll = llllllllllllllllll;
                if(lllll != 0 || (!_TriggerMode && !_RaycastMode)) {
                    if (_Obstruction == 1) {
                        if(lllllllllllllllllllllll<=1.5&&llllll>lllllll){
                            llllllllllllllllll = (sqrt((llllll-lllllll))*25/lllllllllllllllllllllll) *_AngleStrength;  
                            llllllllllllllllll = max(0,log(llllllllllllllllll*0.2));
                        }
                    }  else if (_Obstruction == 2) {
                        if(lllllllllllllll){
                            float lllllllllllllllllllllllllllllllllll = llllllllllllll/lllllllllllll;
                            llllllllllllllllll = lllllllllllllllllllllllllllllllllllll(_DissolveFallOff, _ConeStrength, lllllllllllllllllllllllllllllllllll);
                        }
                    } else  if (_Obstruction == 3) {
                        if(lllllllllllllllllllllll<= 1.5 && llllll > lllllll || lllllllllllllll){
                            if(lllllllllllllllllllllll<= 1.5 && llllll > lllllll) {
                                llllllllllllllllll = (sqrt((llllll-lllllll))*25/lllllllllllllllllllllll)*_AngleStrength;                   
                                llllllllllllllllll = max(0,log(llllllllllllllllll*0.2));
                            }
                            if (lllllllllllllll) {
                                float lllllllllllllllllllllllllllllllllll = llllllllllllll/lllllllllllll;
                                llllllllllllllllll = lllllllllllllllllllllllllllllllllllll(_DissolveFallOff, _ConeStrength, lllllllllllllllllllllllllllllllllll)+llllllllllllllllll;
                            }
                        }
                    }  else if (_Obstruction == 4) {
                        if(lllllllllllllllll){
                            float lllllllllllllllllllllllllllllllllll = llllllllllllll/llllllllllllllll;
                            llllllllllllllllll = lllllllllllllllllllllllllllllllllllll(_DissolveFallOff, _CylinderStrength, lllllllllllllllllllllllllllllllllll);
                        }
                    }  else if (_Obstruction == 5) {
                        if(lllllllllllllllllllllll<=1.5&&llllll>lllllll||lllllllllllllllll){
                            if(lllllllllllllllllllllll<=1.5&&llllll>lllllll) {
                                llllllllllllllllll = (sqrt((llllll-lllllll))*25/lllllllllllllllllllllll)*_AngleStrength;
                                llllllllllllllllll = max(0,log(llllllllllllllllll*0.2));              
                            }
                            if(lllllllllllllllll){
                                float lllllllllllllllllllllllllllllllllll = llllllllllllll/llllllllllllllll;
                                llllllllllllllllll = lllllllllllllllllllllllllllllllllllll(_DissolveFallOff, _CylinderStrength, lllllllllllllllllllllllllllllllllll) + llllllllllllllllll;                        
                            }                     
                        }
                    } else if (_Obstruction == 6) {
                        if (llllllllllllllllllllllllllllllll) {
                            llllllllllllllllll = lllllllllllllllllllllllllllllllllllll(_DissolveFallOff, _CircleStrength, lllllllllllllllllllllllllllllll);                 
                        }
                    }
                    llllllllllllllllll = llllllllllllllllll+(1*_IntrinsicDissolveStrength);
                    float llllllllllllllllllllllllllllllllll = llllllllllllllllll/_FloorYTextureGradientLength;
                    if(_Floor == 1) {
        	            if(d.worldSpacePosition.y < (_PlayersPosArray[i].y+_PlayerPosYOffset)) {
                            float llllllllllllllllllllllllllllllllllll = (_PlayersPosArray[i].y+_PlayerPosYOffset) - d.worldSpacePosition.y;
                            if(llllllllllllllllllllllllllllllllllll < 0) {llllllllllllllllllllllllllllllllllll = 0;}
                            if(llllllllllllllllllllllllllllllllllll < _FloorYTextureGradientLength) {
                                llllllllllllllllll = (_FloorYTextureGradientLength-llllllllllllllllllllllllllllllllllll)*llllllllllllllllllllllllllllllllll;
                            } else {llllllllllllllllll = 0;}
                        }
                    } else {
                        if(d.worldSpacePosition.y < _FloorY) {
                            float llllllllllllllllllllllllllllllllllll = _FloorY - d.worldSpacePosition.y;
                            if(llllllllllllllllllllllllllllllllllll<0){llllllllllllllllllllllllllllllllllll=0;}
                            if(llllllllllllllllllllllllllllllllllll<_FloorYTextureGradientLength){llllllllllllllllll = (_FloorYTextureGradientLength-llllllllllllllllllllllllllllllllllll)*llllllllllllllllllllllllllllllllll;
                            } else {llllllllllllllllll = 0;}
                        }
                    }
                    if(!_TriggerMode && !_RaycastMode) {if(distance(_PlayersPosArray[i], d.worldSpacePosition) > _DefaultEffectRadius) {llllllllllllllllll = 0;}}
                }
                if(_TriggerMode || _RaycastMode) {llllllllllllllllll =  lllll * llllllllllllllllll;  
                } else {llllllllllllllllll = llllllllllllllllll;}
                llllllllllllllllllll = max(llllllllllllllllllll,llllllllllllllllll);
            }            
            float lllllllllllllllllllllllllllllllll = llllllllllllllllllll;
            if(!_PreviewMode) {
                if (lllllllllllllllllllllllllllllllll==1){lllllllllllllllllllllllllllllllll=10;}
                if (!_hasClippedShadows) {
                #if defined(UNITY_PASS_SHADOWCASTER)
                #if defined(SHADOWS_DEPTH)
                if (!any(unity_LightShadowBias)){clip(lllllllllllllllllll- lllllllllllllllllllllllllllllllll);}
                else{if(_hasClippedShadows) {clip(lllllllllllllllllll- lllllllllllllllllllllllllllllllll);}}
                #endif
                #else
                    clip(lllllllllllllllllll- lllllllllllllllllllllllllllllllll);
                #endif
                } else {clip(lllllllllllllllllll- lllllllllllllllllllllllllllllllll);}
            }       
            half4 c = lerp(1, tex2D(_MainTex, d.texcoord0.xy), _TextureVisibility) * _Color;        
            o.Albedo = clamp(_DissolveColor*lllllllllllllllllllllllllllllllll, 0, _DissolveColorSaturation) + c.rgb;
            if(_PreviewMode) {
                if((lllllllllllllllllll - lllllllllllllllllllllllllllllllll)< 0) {
                    o.Albedo = half4(1,1,1,1);
                } else {
                    o.Albedo = half4(0,0,0,1);
                }
            }
            if(_TexturedEmissionEdge) {
                _TexturedEmissionEdgeStrength = 0.2 + (_TexturedEmissionEdgeStrength*(0.8-0.2));
                o.Emission =  min( clamp(lerp(1,_DissolveColor,_DissolveColorSaturation)*lllllllllllllllllllllllllllllllll, 0, 1)*sqrt(_DissolveEmission*_DissolveEmissionBooster), clamp(lerp(1,_DissolveColor,_DissolveColorSaturation) *  clamp(((lllllllllllllllllllllllllllllllll/_TexturedEmissionEdgeStrength) - lllllllllllllllllll),0,1), 0, 1)*sqrt(_DissolveEmission*_DissolveEmissionBooster));

            } else {
                o.Emission =  clamp(lerp(1,_DissolveColor,_DissolveColorSaturation)*lllllllllllllllllllllllllllllllll, 0, 1)*sqrt(_DissolveEmission*_DissolveEmissionBooster);
            } 
            #if _HDRP
                o.Emission =  o.Emission * pow(_DissolveEmissionBooster,4);
            #endif

            #ifdef _NORMALMAP
                o.Normal = UnpackScaleNormal(tex2D(_BumpMap, d.texcoord0.xy), _BumpScale);
            #endif
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = 1;

        } else {
            half4 c = tex2D (_MainTex, d.texcoord0.xy) * _Color;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
            #ifdef _NORMALMAP
            o.Normal = UnpackScaleNormal(tex2D(_BumpMap, d.texcoord0.xy), _BumpScale);
            #endif
        }    
	}




        
            void ChainSurfaceFunction(inout Surface l, inout ShaderData d)
            {
                  Ext_SurfaceFunction0(l, d);
                 // Ext_SurfaceFunction1(l, d);
                 // Ext_SurfaceFunction2(l, d);
                 // Ext_SurfaceFunction3(l, d);
                 // Ext_SurfaceFunction4(l, d);
                 // Ext_SurfaceFunction5(l, d);
                 // Ext_SurfaceFunction6(l, d);
                 // Ext_SurfaceFunction7(l, d);
                 // Ext_SurfaceFunction8(l, d);
                 // Ext_SurfaceFunction9(l, d);
		           // Ext_SurfaceFunction10(l, d);
                 // Ext_SurfaceFunction11(l, d);
                 // Ext_SurfaceFunction12(l, d);
                 // Ext_SurfaceFunction13(l, d);
                 // Ext_SurfaceFunction14(l, d);
                 // Ext_SurfaceFunction15(l, d);
                 // Ext_SurfaceFunction16(l, d);
                 // Ext_SurfaceFunction17(l, d);
                 // Ext_SurfaceFunction18(l, d);
		           // Ext_SurfaceFunction19(l, d);
            }

            void ChainModifyVertex(inout VertexData v, inout VertexToPixel v2p)
            {
                 ExtraV2F d;
                 ZERO_INITIALIZE(ExtraV2F, d);
                 ZERO_INITIALIZE(Blackboard, d.blackboard);

                 //  Ext_ModifyVertex0(v, d);
                 // Ext_ModifyVertex1(v, d);
                 // Ext_ModifyVertex2(v, d);
                 // Ext_ModifyVertex3(v, d);
                 // Ext_ModifyVertex4(v, d);
                 // Ext_ModifyVertex5(v, d);
                 // Ext_ModifyVertex6(v, d);
                 // Ext_ModifyVertex7(v, d);
                 // Ext_ModifyVertex8(v, d);
                 // Ext_ModifyVertex9(v, d);
                 // Ext_ModifyVertex10(v, d);
                 // Ext_ModifyVertex11(v, d);
                 // Ext_ModifyVertex12(v, d);
                 // Ext_ModifyVertex13(v, d);
                 // Ext_ModifyVertex14(v, d);
                 // Ext_ModifyVertex15(v, d);
                 // Ext_ModifyVertex16(v, d);
                 // Ext_ModifyVertex17(v, d);
                 // Ext_ModifyVertex18(v, d);
                 // Ext_ModifyVertex19(v, d);


                 // #if %EXTRAV2F0REQUIREKEY%
                 // v2p.extraV2F0 = d.extraV2F0;
                 // #endif

                 // #if %EXTRAV2F1REQUIREKEY%
                 // v2p.extraV2F1 = d.extraV2F1;
                 // #endif

                 // #if %EXTRAV2F2REQUIREKEY%
                 // v2p.extraV2F2 = d.extraV2F2;
                 // #endif

                 // #if %EXTRAV2F3REQUIREKEY%
                 // v2p.extraV2F3 = d.extraV2F3;
                 // #endif

                 // #if %EXTRAV2F4REQUIREKEY%
                 // v2p.extraV2F4 = d.extraV2F4;
                 // #endif

                 // #if %EXTRAV2F5REQUIREKEY%
                 // v2p.extraV2F5 = d.extraV2F5;
                 // #endif

                 // #if %EXTRAV2F6REQUIREKEY%
                 // v2p.extraV2F6 = d.extraV2F6;
                 // #endif

                 // #if %EXTRAV2F7REQUIREKEY%
                 // v2p.extraV2F7 = d.extraV2F7;
                 // #endif
            }

            void ChainModifyTessellatedVertex(inout VertexData v, inout VertexToPixel v2p)
            {
               ExtraV2F d;
               ZERO_INITIALIZE(ExtraV2F, d);
               ZERO_INITIALIZE(Blackboard, d.blackboard);

               // #if %EXTRAV2F0REQUIREKEY%
               // d.extraV2F0 = v2p.extraV2F0;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // d.extraV2F1 = v2p.extraV2F1;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // d.extraV2F2 = v2p.extraV2F2;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // d.extraV2F3 = v2p.extraV2F3;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // d.extraV2F4 = v2p.extraV2F4;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // d.extraV2F5 = v2p.extraV2F5;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // d.extraV2F6 = v2p.extraV2F6;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // d.extraV2F7 = v2p.extraV2F7;
               // #endif


               // Ext_ModifyTessellatedVertex0(v, d);
               // Ext_ModifyTessellatedVertex1(v, d);
               // Ext_ModifyTessellatedVertex2(v, d);
               // Ext_ModifyTessellatedVertex3(v, d);
               // Ext_ModifyTessellatedVertex4(v, d);
               // Ext_ModifyTessellatedVertex5(v, d);
               // Ext_ModifyTessellatedVertex6(v, d);
               // Ext_ModifyTessellatedVertex7(v, d);
               // Ext_ModifyTessellatedVertex8(v, d);
               // Ext_ModifyTessellatedVertex9(v, d);
               // Ext_ModifyTessellatedVertex10(v, d);
               // Ext_ModifyTessellatedVertex11(v, d);
               // Ext_ModifyTessellatedVertex12(v, d);
               // Ext_ModifyTessellatedVertex13(v, d);
               // Ext_ModifyTessellatedVertex14(v, d);
               // Ext_ModifyTessellatedVertex15(v, d);
               // Ext_ModifyTessellatedVertex16(v, d);
               // Ext_ModifyTessellatedVertex17(v, d);
               // Ext_ModifyTessellatedVertex18(v, d);
               // Ext_ModifyTessellatedVertex19(v, d);

               // #if %EXTRAV2F0REQUIREKEY%
               // v2p.extraV2F0 = d.extraV2F0;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // v2p.extraV2F1 = d.extraV2F1;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // v2p.extraV2F2 = d.extraV2F2;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // v2p.extraV2F3 = d.extraV2F3;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // v2p.extraV2F4 = d.extraV2F4;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // v2p.extraV2F5 = d.extraV2F5;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // v2p.extraV2F6 = d.extraV2F6;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // v2p.extraV2F7 = d.extraV2F7;
               // #endif
            }

            void ChainFinalColorForward(inout Surface l, inout ShaderData d, inout half4 color)
            {
               //   Ext_FinalColorForward0(l, d, color);
               //   Ext_FinalColorForward1(l, d, color);
               //   Ext_FinalColorForward2(l, d, color);
               //   Ext_FinalColorForward3(l, d, color);
               //   Ext_FinalColorForward4(l, d, color);
               //   Ext_FinalColorForward5(l, d, color);
               //   Ext_FinalColorForward6(l, d, color);
               //   Ext_FinalColorForward7(l, d, color);
               //   Ext_FinalColorForward8(l, d, color);
               //   Ext_FinalColorForward9(l, d, color);
               //  Ext_FinalColorForward10(l, d, color);
               //  Ext_FinalColorForward11(l, d, color);
               //  Ext_FinalColorForward12(l, d, color);
               //  Ext_FinalColorForward13(l, d, color);
               //  Ext_FinalColorForward14(l, d, color);
               //  Ext_FinalColorForward15(l, d, color);
               //  Ext_FinalColorForward16(l, d, color);
               //  Ext_FinalColorForward17(l, d, color);
               //  Ext_FinalColorForward18(l, d, color);
               //  Ext_FinalColorForward19(l, d, color);
            }

            void ChainFinalGBufferStandard(inout Surface s, inout ShaderData d, inout half4 GBuffer0, inout half4 GBuffer1, inout half4 GBuffer2, inout half4 outEmission, inout half4 outShadowMask)
            {
               //   Ext_FinalGBufferStandard0(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard1(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard2(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard3(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard4(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard5(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard6(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard7(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard8(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard9(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard10(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard11(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard12(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard13(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard14(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard15(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard16(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard17(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard18(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard19(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
            }



            

         ShaderData CreateShaderData(VertexToPixel i
                  #if NEED_FACING
                     , bool facing
                  #endif
         )
         {
            ShaderData d = (ShaderData)0;
            d.clipPos = i.pos;
            d.worldSpacePosition = i.worldPos;

            d.worldSpaceNormal = normalize(i.worldNormal);
            d.worldSpaceTangent = normalize(i.worldTangent.xyz);
            d.tangentSign = i.worldTangent.w;
            float3 bitangent = cross(i.worldTangent.xyz, i.worldNormal) * d.tangentSign * -1;
            

            d.TBNMatrix = float3x3(d.worldSpaceTangent, bitangent, d.worldSpaceNormal);
            d.worldSpaceViewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

            d.tangentSpaceViewDir = mul(d.TBNMatrix, d.worldSpaceViewDir);
             d.texcoord0 = i.texcoord0;
            // d.texcoord1 = i.texcoord1;
            // d.texcoord2 = i.texcoord2;

            // #if %TEXCOORD3REQUIREKEY%
            // d.texcoord3 = i.texcoord3;
            // #endif

            // d.isFrontFace = facing;
            // #if %VERTEXCOLORREQUIREKEY%
            // d.vertexColor = i.vertexColor;
            // #endif

            // these rarely get used, so we back transform them. Usually will be stripped.
            #if _HDRP
                // d.localSpacePosition = mul(unity_WorldToObject, float4(GetCameraRelativePositionWS(i.worldPos), 1)).xyz;
            #else
                // d.localSpacePosition = mul(unity_WorldToObject, float4(i.worldPos, 1)).xyz;
            #endif
            // d.localSpaceNormal = normalize(mul((float3x3)unity_WorldToObject, i.worldNormal));
            // d.localSpaceTangent = normalize(mul((float3x3)unity_WorldToObject, i.worldTangent.xyz));

            // #if %SCREENPOSREQUIREKEY%
             d.screenPos = i.screenPos;
             d.screenUV = (i.screenPos.xy / i.screenPos.w);
            // #endif


            // #if %EXTRAV2F0REQUIREKEY%
            // d.extraV2F0 = i.extraV2F0;
            // #endif

            // #if %EXTRAV2F1REQUIREKEY%
            // d.extraV2F1 = i.extraV2F1;
            // #endif

            // #if %EXTRAV2F2REQUIREKEY%
            // d.extraV2F2 = i.extraV2F2;
            // #endif

            // #if %EXTRAV2F3REQUIREKEY%
            // d.extraV2F3 = i.extraV2F3;
            // #endif

            // #if %EXTRAV2F4REQUIREKEY%
            // d.extraV2F4 = i.extraV2F4;
            // #endif

            // #if %EXTRAV2F5REQUIREKEY%
            // d.extraV2F5 = i.extraV2F5;
            // #endif

            // #if %EXTRAV2F6REQUIREKEY%
            // d.extraV2F6 = i.extraV2F6;
            // #endif

            // #if %EXTRAV2F7REQUIREKEY%
            // d.extraV2F7 = i.extraV2F7;
            // #endif

            return d;
         }
         

            
         #if defined(SHADERPASS_SHADOWCASTER)
            float3 _LightDirection;
         #endif

         // vertex shader
         VertexToPixel Vert (VertexData v)
         {
           
           VertexToPixel o = (VertexToPixel)0;

           UNITY_SETUP_INSTANCE_ID(v);
           UNITY_TRANSFER_INSTANCE_ID(v, o);
           UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


#if !_TESSELLATION_ON
           ChainModifyVertex(v, o);
#endif

            o.texcoord0 = v.texcoord0;
           // o.texcoord1 = v.texcoord1;
           // o.texcoord2 = v.texcoord2;

           // #if %TEXCOORD3REQUIREKEY%
           // o.texcoord3 = v.texcoord3;
           // #endif

           // #if %VERTEXCOLORREQUIREKEY%
           // o.vertexColor = v.vertexColor;
           // #endif
           
           VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
           o.worldPos = TransformObjectToWorld(v.vertex.xyz);
           o.worldNormal = TransformObjectToWorldNormal(v.normal);
           o.worldTangent = float4(TransformObjectToWorldDir(v.tangent.xyz), v.tangent.w);


          #if defined(SHADERPASS_SHADOWCASTER)
              // Define shadow pass specific clip position for Universal
              o.pos = TransformWorldToHClip(ApplyShadowBias(o.worldPos, o.worldNormal, _LightDirection));
              #if UNITY_REVERSED_Z
                  o.pos.z = min(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
              #else
                  o.pos.z = max(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
              #endif
          #elif defined(SHADERPASS_META)
              o.pos = MetaVertexPosition(float4(v.vertex.xyz, 0), v.texcoord1, v.texcoord2, unity_LightmapST, unity_DynamicLightmapST);
          #else
              o.pos = TransformWorldToHClip(o.worldPos);
          #endif

          // #if %SCREENPOSREQUIREKEY%
           o.screenPos = ComputeScreenPos(o.pos, _ProjectionParams.x);
          // #endif

          #if defined(SHADERPASS_FORWARD)
              OUTPUT_LIGHTMAP_UV(v.texcoord1, unity_LightmapST, o.lightmapUV);
              OUTPUT_SH(o.worldNormal, o.sh);
          #endif

          #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
              half3 vertexLight = VertexLighting(o.worldPos, o.worldNormal);
              half fogFactor = ComputeFogFactor(o.pos.z);
              o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
          #endif

          #ifdef _MAIN_LIGHT_SHADOWS
              o.shadowCoord = GetShadowCoord(vertexInput);
          #endif

           return o;
         }


            

            // fragment shader
            half4 Frag (VertexToPixel IN
            #ifdef _DEPTHOFFSET_ON
              , out float outputDepth : SV_Depth
            #endif
            #if NEED_FACING
               , bool facing : SV_IsFrontFace
            #endif
            ) : SV_Target
            {
               UNITY_SETUP_INSTANCE_ID(IN);

               ShaderData d = CreateShaderData(IN
                  #if NEED_FACING
                     , facing
                  #endif
               );
               Surface l = (Surface)0;

               #ifdef _DEPTHOFFSET_ON
                  l.outputDepth = outputDepth;
               #endif

               l.Albedo = half3(0.5, 0.5, 0.5);
               l.Normal = float3(0,0,1);
               l.Occlusion = 1;
               l.Alpha = 1;

               ChainSurfaceFunction(l, d);

               #ifdef _DEPTHOFFSET_ON
                  outputDepth = l.outputDepth;
               #endif

             return 0;

            }

         ENDHLSL

      }


      
        Pass
        {
            Name "DepthOnly"
            Tags 
            { 
                "LightMode" = "DepthOnly"
            }
           
            // Render State
            Blend One Zero, One Zero
            Cull Back
            ZTest LEqual
            ZWrite On
            ColorMask 0
            
            

            HLSLPROGRAM

               #pragma vertex Vert
   #pragma fragment Frag

            #define _NORMAL_DROPOFF_TS 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define SHADERPASS_DEPTHONLY

            #pragma target 3.0
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma multi_compile_instancing

            
      #pragma shader_feature_local _NORMALMAP


   #define _URP 1


            #define _PASSDEPTH 1

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Version.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"


                  #undef WorldNormalVector
      #define WorldNormalVector(data, normal) mul(normal, data.TBNMatrix)
      
      #define UnityObjectToWorldNormal(normal) mul(GetObjectToWorldMatrix(), normal)

      #define _WorldSpaceLightPos0 _MainLightPosition
      
      #define UNITY_DECLARE_TEX2D(name) TEXTURE2D(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2D_NOSAMPLER(name) TEXTURE2D(name);
      #define UNITY_DECLARE_TEX2DARRAY(name) TEXTURE2D_ARRAY(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(name) TEXTURE2D_ARRAY(name);

      #define UNITY_SAMPLE_TEX2DARRAY(tex,coord)            SAMPLE_TEXTURE2D_ARRAY(tex, sampler##tex, coord.xy, coord.z)
      #define UNITY_SAMPLE_TEX2DARRAY_LOD(tex,coord,lod)    SAMPLE_TEXTURE2D_ARRAY_LOD(tex, sampler##tex, coord.xy, coord.z, lod)
      #define UNITY_SAMPLE_TEX2D(tex, coord)                SAMPLE_TEXTURE2D(tex, sampler##tex, coord)
      #define UNITY_SAMPLE_TEX2D_SAMPLER(tex, samp, coord)  SAMPLE_TEXTURE2D(tex, sampler##samp, coord)

      #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod)   SAMPLE_TEXTURE2D_LOD(tex, sampler_##tex, coord, lod)
      #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) SAMPLE_TEXTURE2D_LOD (tex, sampler##samplertex,coord, lod)
     
      #if defined(UNITY_COMPILER_HLSL)
         #define UNITY_INITIALIZE_OUTPUT(type,name) name = (type)0;
      #else
         #define UNITY_INITIALIZE_OUTPUT(type,name)
      #endif

      #define sampler2D_float sampler2D
      #define sampler2D_half sampler2D



      // data across stages, stripped like the above.
      struct VertexToPixel
      {
         float4 pos : SV_POSITION;
         float3 worldPos : TEXCOORD0;
         float3 worldNormal : TEXCOORD1;
         float4 worldTangent : TEXCOORD2;
          float4 texcoord0 : TEXCCOORD3;
         // float4 texcoord1 : TEXCCOORD4;
         // float4 texcoord2 : TEXCCOORD5;

         // #if %TEXCOORD3REQUIREKEY%
         // float4 texcoord3 : TEXCCOORD6;
         // #endif

         // #if %SCREENPOSREQUIREKEY%
          float4 screenPos : TEXCOORD7;
         // #endif

         // #if %VERTEXCOLORREQUIREKEY%
         // half4 vertexColor : COLOR;
         // #endif

         // #if %EXTRAV2F0REQUIREKEY%
         // float4 extraV2F0 : TEXCOORD12;
         // #endif

         // #if %EXTRAV2F1REQUIREKEY%
         // float4 extraV2F1 : TEXCOORD13;
         // #endif

         // #if %EXTRAV2F2REQUIREKEY%
         // float4 extraV2F2 : TEXCOORD14;
         // #endif

         // #if %EXTRAV2F3REQUIREKEY%
         // float4 extraV2F3 : TEXCOORD15;
         // #endif

         // #if %EXTRAV2F4REQUIREKEY%
         // float4 extraV2F4 : TEXCOORD16;
         // #endif

         // #if %EXTRAV2F5REQUIREKEY%
         // float4 extraV2F5 : TEXCOORD17;
         // #endif

         // #if %EXTRAV2F6REQUIREKEY%
         // float4 extraV2F6 : TEXCOORD18;
         // #endif

         // #if %EXTRAV2F7REQUIREKEY%
         // float4 extraV2F7 : TEXCOORD19;
         // #endif
            
         #if defined(LIGHTMAP_ON)
            float2 lightmapUV : TEXCOORD8;
         #endif
         #if !defined(LIGHTMAP_ON)
            float3 sh : TEXCOORD9;
         #endif
            float4 fogFactorAndVertexLight : TEXCOORD10;
            float4 shadowCoord : TEXCOORD11;
         #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
         #endif
         #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
         #endif
         #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
         #endif
         #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
         #endif
      };

         
            
            
            // data describing the user output of a pixel
            struct Surface
            {
               half3 Albedo;
               half Height;
               half3 Normal;
               half Smoothness;
               half3 Emission;
               half Metallic;
               half3 Specular;
               half Occlusion;
               half SpecularPower; // for simple lighting
               half Alpha;
               float outputDepth; // if written, SV_Depth semantic is used. ShaderData.clipPos.z is unused value
               // HDRP Only
               half SpecularOcclusion;
               half SubsurfaceMask;
               half Thickness;
               half CoatMask;
               half CoatSmoothness;
               half Anisotropy;
               half IridescenceMask;
               half IridescenceThickness;
               int DiffusionProfileHash;
               // requires _OVERRIDE_BAKEDGI to be defined, but is mapped in all pipelines
               float3 DiffuseGI;
               float3 BackDiffuseGI;
               float3 SpecularGI;
               // requires _OVERRIDE_SHADOWMASK to be defines
               float4 ShadowMask;
            };

            // Data the user declares in blackboard blocks
            struct Blackboard
            {
                
                float blackboardDummyData;
            };

            // data the user might need, this will grow to be big. But easy to strip
            struct ShaderData
            {
               float4 clipPos; // SV_POSITION
               float3 localSpacePosition;
               float3 localSpaceNormal;
               float3 localSpaceTangent;
        
               float3 worldSpacePosition;
               float3 worldSpaceNormal;
               float3 worldSpaceTangent;
               float tangentSign;

               float3 worldSpaceViewDir;
               float3 tangentSpaceViewDir;

               float4 texcoord0;
               float4 texcoord1;
               float4 texcoord2;
               float4 texcoord3;

               float2 screenUV;
               float4 screenPos;

               float4 vertexColor;
               bool isFrontFace;

               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;

               float3x3 TBNMatrix;
               Blackboard blackboard;
            };

            struct VertexData
            {
               #if SHADER_TARGET > 30
               // uint vertexID : SV_VertexID;
               #endif
               float4 vertex : POSITION;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;

               // would love to strip these, but they are used in certain
               // combinations of the lighting system, and may be used
               // by the user as well, so no easy way to strip them.

               float4 texcoord1 : TEXCOORD1;
               float4 texcoord2 : TEXCOORD2;

               // #if %TEXCOORD3REQUIREKEY%
               // float4 texcoord3 : TEXCOORD3;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               #if _HDRP && (_PASSMOTIONVECTOR || (_PASSFORWARD && defined(_WRITE_TRANSPARENT_MOTION_VECTOR)))
                  float3 previousPositionOS : TEXCOORD4; // Contain previous transform position (in case of skinning for example)
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity    : TEXCOORD5; // Add Precomputed Velocity (Alembic computes velocities on runtime side).
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct TessVertex 
            {
               float4 vertex : INTERNALTESSPOS;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;
               float4 texcoord1 : TEXCOORD1;
               float4 texcoord2 : TEXCOORD2;

               // #if %TEXCOORD3REQUIREKEY%
               // float4 texcoord3 : TEXCOORD3;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               // #if %EXTRAV2F0REQUIREKEY%
               // float4 extraV2F0 : TEXCOORD5;
               // endif

               // #if %EXTRAV2F1REQUIREKEY%
               // float4 extraV2F1 : TEXCOORD6;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // float4 extraV2F2 : TEXCOORD7;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // float4 extraV2F3 : TEXCOORD8;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // float4 extraV2F4 : TEXCOORD9;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // float4 extraV2F5 : TEXCOORD10;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // float4 extraV2F6 : TEXCOORD11;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // float4 extraV2F7 : TEXCOORD12;
               // #endif

               #if _HDRP && (_PASSMOTIONVECTOR || (_PASSFORWARD && defined(_WRITE_TRANSPARENT_MOTION_VECTOR)))
                  float3 previousPositionOS : TEXCOORD13; // Contain previous transform position (in case of skinning for example)
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity : TEXCOORD14;
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
               UNITY_VERTEX_OUTPUT_STEREO
            };

            struct ExtraV2F
            {
               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;
               Blackboard blackboard;
            };


            float3 WorldToTangentSpace(ShaderData d, float3 normal)
            {
               return mul(d.TBNMatrix, normal);
            }

            float3 TangentToWorldSpace(ShaderData d, float3 normal)
            {
               return mul(normal, d.TBNMatrix);
            }

            // in this case, make standard more like SRPs, because we can't fix
            // unity_WorldToObject in HDRP, since it already does macro-fu there

            #if _STANDARD
               float3 TransformWorldToObject(float3 p) { return mul(unity_WorldToObject, float4(p, 1)); };
               float3 TransformObjectToWorld(float3 p) { return mul(unity_ObjectToWorld, float4(p, 1)); };
               float4 TransformWorldToObject(float4 p) { return mul(unity_WorldToObject, p); };
               float4 TransformObjectToWorld(float4 p) { return mul(unity_ObjectToWorld, p); };
               float4x4 GetWorldToObjectMatrix() { return unity_WorldToObject; }
               float4x4 GetObjectToWorldMatrix() { return unity_ObjectToWorld; }
               #if (defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (SHADER_TARGET_SURFACE_ANALYSIS && !SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod) tex.SampleLevel (sampler##tex,coord, lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) tex.SampleLevel (sampler##samplertex,coord, lod)
              #else
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord,lod) tex2D (tex,coord,0,lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord,lod) tex2D (tex,coord,0,lod)
              #endif

               #undef GetObjectToWorldMatrix()
               #undef GetWorldToObjectMatrix()
               #undef GetWorldToViewMatrix()
               #undef UNITY_MATRIX_I_V
               #undef UNITY_MATRIX_P
               #undef GetWorldToHClipMatrix()
               #undef GetObjectToWorldMatrix()V
               #undef UNITY_MATRIX_T_MV
               #undef UNITY_MATRIX_IT_MV
               #undef GetObjectToWorldMatrix()VP

               #define GetObjectToWorldMatrix()     unity_ObjectToWorld
               #define GetWorldToObjectMatrix()   unity_WorldToObject
               #define GetWorldToViewMatrix()     unity_MatrixV
               #define UNITY_MATRIX_I_V   unity_MatrixInvV
               #define GetViewToHClipMatrix()     OptimizeProjectionMatrix(glstate_matrix_projection)
               #define GetWorldToHClipMatrix()    unity_MatrixVP
               #define GetObjectToWorldMatrix()V    mul(GetWorldToViewMatrix(), GetObjectToWorldMatrix())
               #define UNITY_MATRIX_T_MV  transpose(GetObjectToWorldMatrix()V)
               #define UNITY_MATRIX_IT_MV transpose(mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V))
               #define GetObjectToWorldMatrix()VP   mul(GetWorldToHClipMatrix(), GetObjectToWorldMatrix())


            #endif

            float3 GetCameraWorldPosition()
            {
               #if _HDRP
                  return GetCameraRelativePositionWS(_WorldSpaceCameraPos);
               #else
                  return _WorldSpaceCameraPos;
               #endif
            }

            #if _GRABPASSUSED
               #if _STANDARD
                  TEXTURE2D(%GRABTEXTURE%);
                  SAMPLER(sampler_%GRABTEXTURE%);
               #endif

               half3 GetSceneColor(float2 uv)
               {
                  #if _STANDARD
                     return SAMPLE_TEXTURE2D(%GRABTEXTURE%, sampler_%GRABTEXTURE%, uv).rgb;
                  #else
                     return SHADERGRAPH_SAMPLE_SCENE_COLOR(uv);
                  #endif
               }
            #endif


      
            #if _STANDARD
               UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
               float GetSceneDepth(float2 uv) { return SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv)); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv)); } 
            #else
               float GetSceneDepth(float2 uv) { return SHADERGRAPH_SAMPLE_SCENE_DEPTH(uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv), _ZBufferParams); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv), _ZBufferParams); } 
            #endif

            float3 GetWorldPositionFromDepthBuffer(float2 uv, float3 worldSpaceViewDir)
            {
               float eye = GetLinearEyeDepth(uv);
               float3 camView = mul((float3x3)GetObjectToWorldMatrix(), transpose(mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V)) [2].xyz);

               float dt = dot(worldSpaceViewDir, camView);
               float3 div = worldSpaceViewDir/dt;
               float3 wpos = (eye * div) + GetCameraWorldPosition();
               return wpos;
            }

            #if _STANDARD
               UNITY_DECLARE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture);
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  float4 depthNorms = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture, uv);
                  float3 norms = DecodeViewNormalStereo(depthNorms);
                  norms = mul((float3x3)GetWorldToViewMatrix(), norms) * 0.5 + 0.5;
                  return norms;
               }
            #elif _HDRP
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  NormalData nd;
                  DecodeFromNormalBuffer(_ScreenSize.xy * uv, nd);
                  return nd.normalWS;
               }
            #elif _URP
               #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                  #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"
               #endif

               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                     return SampleSceneNormals(uv);
                  #else
                     float3 wpos = GetWorldPositionFromDepthBuffer(uv, worldSpaceViewDir);
                     return normalize(-cross(ddx(wpos), ddy(wpos))) * 0.5 + 0.5;
                  #endif

                }
             #endif

             #if _HDRP

               half3 UnpackNormalmapRGorAG(half4 packednormal)
               {
                     // This do the trick
                  packednormal.x *= packednormal.w;

                  half3 normal;
                  normal.xy = packednormal.xy * 2 - 1;
                  normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                  return normal;
               }
               half3 UnpackNormal(half4 packednormal)
               {
                  #if defined(UNITY_NO_DXT5nm)
                     return packednormal.xyz * 2 - 1;
                  #else
                     return UnpackNormalmapRGorAG(packednormal);
                  #endif
               }
               #endif
               #if _HDRP || _URP

               half3 UnpackScaleNormal(half4 packednormal, half scale)
               {
                 #ifndef UNITY_NO_DXT5nm
                   // Unpack normal as DXT5nm (1, y, 1, x) or BC5 (x, y, 0, 1)
                   // Note neutral texture like "bump" is (0, 0, 1, 1) to work with both plain RGB normal and DXT5nm/BC5
                   packednormal.x *= packednormal.w;
                 #endif
                   half3 normal;
                   normal.xy = (packednormal.xy * 2 - 1) * scale;
                   normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                   return normal;
               }	

             #endif


            void GetSun(out float3 lightDir, out float3 color)
            {
               lightDir = float3(0.5, 0.5, 0);
               color = 1;
               #if _HDRP
                  if (_DirectionalLightCount > 0)
                  {
                     DirectionalLightData light = _DirectionalLightDatas[0];
                     lightDir = -light.forward.xyz;
                     color = light.color;
                  }
               #elif _STANDARD
			         lightDir = normalize(_WorldSpaceLightPos0.xyz);
                  color = _LightColor0.rgb;
               #elif _URP
	               Light light = GetMainLight();
	               lightDir = light.direction;
	               color = light.color;
               #endif
            }


            
            CBUFFER_START(UnityPerMaterial)

               
	half4 _Color;
	half _Glossiness;
	half _Metallic;
	half _TextureVisibility;
	half _AngleStrength;
	float _Obstruction;
	float _Floor;
	float _UVs;
	float _BumpScale; 
	half4 _DissolveColor;
	float _DissolveColorSaturation;
	float _DissolveEmission;
	float _DissolveEmissionBooster;
	float _hasClippedShadows;
	float _ConeStrength;
	float _ConeObstructionDestroyRadius;
	float _CylinderStrength;
	float _CylinderObstructionDestroyRadius;
	float _CircleStrength;
	float _CircleObstructionDestroyRadius;
	float _IntrinsicDissolveStrength;
	float _DissolveFallOff;
	float _PreviewMode;
	float _AnimationEnabled;
	float _AnimationSpeed;
	float _TriggerMode;
	float _RaycastMode;
	float _IsExempt;
	float _DefaultEffectRadius;
	float _FloorY;
	float _FloorYTextureGradientLength;
	float4 _PlayerPos;
	float _PlayerPosYOffset;
	int _ArrayLength = 0;
	float4 _PlayersPosArray[100];
	float4 _PlayersDataArray[100];        
	float _TransitionDuration;
    float _tDirection = 0;
    float _numOfPlayersInside = 0;
    float _tValue = 0;
    float _id = 0;
    float _TexturedEmissionEdge;
    float _TexturedEmissionEdgeStrength;
	float _IsReplacementShader;
	half4 _DissolveColorGlobal;
	float _DissolveColorSaturationGlobal;
	float _DissolveEmissionGlobal;
	float _DissolveEmissionBoosterGlobal;
	float _TextureVisibilityGlobal;
	float _ObstructionGlobal;
	float _AngleStrengthGlobal;
	float _ConeStrengthGlobal;
	float _ConeObstructionDestroyRadiusGlobal;
	float _CylinderStrengthGlobal;
	float _CylinderObstructionDestroyRadiusGlobal;
	float _CircleStrengthGlobal;
	float _CircleObstructionDestroyRadiusGlobal;
	float _DissolveFallOffGlobal;
	float _IntrinsicDissolveStrengthGlobal;
	float _PreviewModeGlobal;
	float _UVsGlobal;
	float _hasClippedShadowsGlobal;
	float _FloorGlobal;
	float _FloorYGlobal;
	float _PlayerPosYOffsetGlobal;
	float _FloorYTextureGradientLengthGlobal;
	float _DefaultEffectRadiusGlobal;
	float _AnimationEnabledGlobal;
	float _AnimationSpeedGlobal;
	float _TransitionDurationGlobal;
    float _TexturedEmissionEdgeGlobal;
    float _TexturedEmissionEdgeStrengthGlobal;
    float _isReferenceMaterial;



            CBUFFER_END

            

            

            #ifdef unity_WorldToObject
#undef unity_WorldToObject
#endif
#ifdef unity_ObjectToWorld
#undef unity_ObjectToWorld
#endif
#define unity_ObjectToWorld GetObjectToWorldMatrix()
#define unity_WorldToObject GetWorldToObjectMatrix()

    float lllllllllllllllllllllllllllllllllllll(float fallOff, float strength, float input)
    {
        float k = fallOff;
        k = max(k,0.00001);
        float n = 1-strength;
        float b = exp(k*6);
        float j = input;
        float v = n/(k/(k*n-0.15*(k-n)));
        float y = ((j-v)/(b*(1-j)+j))+v;
        y = 1-y;
        return y * sign(strength);
    }
	sampler2D _MainTex;
    #ifdef _NORMALMAP
        sampler2D _BumpMap;
    #endif
	sampler2D _DissolveTex;
	sampler2D _DissolveTexGlobal;

	void Ext_SurfaceFunction0 (inout Surface o, ShaderData d)
	{
        bool lllllllllllllllllllllllllllllllllllllll =  (_numOfPlayersInside > 0 || _tDirection == -1 && _Time.y - _tValue < _TransitionDuration ) || (_numOfPlayersInside >= 0 && _tDirection == 1); 
        bool llllllllllllllllllllllllllllllllllllllll = !_TriggerMode && !_RaycastMode;
        if(!_IsExempt && (lllllllllllllllllllllllllllllllllllllll || llllllllllllllllllllllllllllllllllllllll) ) {
            if(_IsReplacementShader) {    
                _DissolveColor = _DissolveColorGlobal;
                _DissolveColorSaturation = _DissolveColorSaturationGlobal;
                _DissolveEmission = _DissolveEmissionGlobal;
                _DissolveEmissionBooster = _DissolveEmissionBoosterGlobal;
                _TextureVisibility = _TextureVisibilityGlobal;
                _Obstruction = _ObstructionGlobal;
                _AngleStrength = _AngleStrengthGlobal;
                _ConeStrength = _ConeStrengthGlobal;
                _ConeObstructionDestroyRadius = _ConeObstructionDestroyRadiusGlobal;
                _CylinderStrength = _CylinderStrengthGlobal;
                _CylinderObstructionDestroyRadius = _CylinderObstructionDestroyRadiusGlobal;
                _CircleStrength = _CircleStrengthGlobal;
                _CircleObstructionDestroyRadius = _CircleObstructionDestroyRadiusGlobal;
                _DissolveFallOff = _DissolveFallOffGlobal;
                _IntrinsicDissolveStrength = _IntrinsicDissolveStrengthGlobal;
                _PreviewMode = _PreviewModeGlobal;
                _UVs = _UVsGlobal;
                _hasClippedShadows = _hasClippedShadowsGlobal;
                _Floor = _FloorGlobal;
                _FloorY = _FloorYGlobal;
                _PlayerPosYOffset = _PlayerPosYOffsetGlobal;
                _FloorYTextureGradientLength = _FloorYTextureGradientLengthGlobal; 
                _AnimationEnabled = _AnimationEnabledGlobal;
                _AnimationSpeed = _AnimationSpeedGlobal;
                _TransitionDuration = _TransitionDurationGlobal;
                _DefaultEffectRadius = _DefaultEffectRadiusGlobal;
                _TexturedEmissionEdge = _TexturedEmissionEdgeGlobal;
                _TexturedEmissionEdgeStrength = _TexturedEmissionEdgeStrengthGlobal;
            }
            if(_IntrinsicDissolveStrength < 0) {_IntrinsicDissolveStrength = 0;}
            float3 l;
            d.worldSpaceNormal = mul(o.Normal, (float3x3)d.TBNMatrix);
            float3 ll = d.worldSpacePosition / (-1.0 * abs(_UVs) );
            if(_AnimationEnabled) {ll = ll + abs(((_Time.y) * _AnimationSpeed));}       
            if(_IsReplacementShader) {
                l = lerp(lerp(tex2D(_DissolveTexGlobal,ll.xz).rgb,tex2D(_DissolveTexGlobal,ll.yz ).rgb,abs(d.worldSpaceNormal.x)).rgb,tex2D(_DissolveTexGlobal,ll.xy).rgb,abs(d.worldSpaceNormal.z)).rgb;
            } else {
                l = lerp(lerp(tex2D(_DissolveTex,ll.xz).rgb,tex2D(_DissolveTex,ll.yz ).rgb,abs(d.worldSpaceNormal.x)).rgb,tex2D(_DissolveTex,ll.xy).rgb,abs(d.worldSpaceNormal.z)).rgb;
            }
            half lllllllllllllllllll = l;           
            float llllllllllllllllllll = 0;
            for (int i = 0; i < _ArrayLength; i++){
                float lll = _PlayersDataArray[i][1];
                float llll = _PlayersDataArray[i][2];
                float lllll = 1;
                if( llll!= 0 && lll != 0 && _Time.y-lll < _TransitionDuration) {
                    if(llll == 1) {lllll = ((_TransitionDuration-(_Time.y-lll))/_TransitionDuration);
                    } else {lllll = ((_Time.y-lll)/_TransitionDuration);}
                } else if(llll ==-1) {lllll = 1;
                } else if(llll == 1) {lllll = 0;
                } else {lllll = 1;}
                lllll = 1 - lllll;   
                float llllllllllllllllllllllllllllllllllllll = _PlayersDataArray[i][3];
                lllll = lllll * float(llllllllllllllllllllllllllllllllllllll == _id);
                float llllll = distance(_WorldSpaceCameraPos, _PlayersPosArray[i]);
                float lllllll = distance(_WorldSpaceCameraPos, d.worldSpacePosition);
                float3 llllllll = _WorldSpaceCameraPos - _PlayersPosArray[i];
                float lllllllll = length(llllllll);
                float llllllllll = _ConeObstructionDestroyRadius;
                float3 lllllllllll = normalize(llllllll);
                float llllllllllll = dot(d.worldSpacePosition - _PlayersPosArray[i], lllllllllll);
                float lllllllllllll = (llllllllllll/lllllllll)*llllllllll;
                float llllllllllllll = length((d.worldSpacePosition - _PlayersPosArray[i])-llllllllllll*lllllllllll);
                float lllllllllllllll = llllllllllllll<lllllllllllll;
                float llllllllllllllll = _CylinderObstructionDestroyRadius;
                float lllllllllllllllll = (llllllllllllll<llllllllllllllll)&&llllllllllll>0;
                float llllllllllllllllll = 0;
                float3 lllllllllllllllllllll =  d.worldSpacePosition - _PlayersPosArray[i];
                float3 llllllllllllllllllllll =  d.worldSpaceNormal;
                float lllllllllllllllllllllll = acos(dot(lllllllllllllllllllll,llllllllllllllllllllll)/(length(lllllllllllllllllllll)*length(llllllllllllllllllllll)));        
                float llllllllllllllllllllllll = _ScreenParams.x / _ScreenParams.y;
		        #if _HDRP
                    float4 lllllllllllllllllllllllll = mul(GetWorldToHClipMatrix(), float4(GetCameraRelativePositionWS(_PlayersPosArray[i].xyz), 1.0));
                    float4 llllllllllllllllllllllllll = ComputeScreenPos(lllllllllllllllllllllllll , _ProjectionParams.x);
		        #else
			        float4 lllllllllllllllllllllllll = mul(GetWorldToHClipMatrix(), float4(_PlayersPosArray[i].xyz, 1.0));
                    float4 llllllllllllllllllllllllll = ComputeScreenPos(lllllllllllllllllllllllll);
		        #endif
                llllllllllllllllllllllllll.xy /= llllllllllllllllllllllllll.w;
                llllllllllllllllllllllllll.x *= llllllllllllllllllllllll;
                #if _HDRP
                    //float4 lllllllllllllllllllllllllll = mul(GetWorldToHClipMatrix(), float4(GetCameraRelativePositionWS(d.worldSpacePosition), 1.0));
                    //half4 llllllllllllllllllllllllllll = ComputeScreenPos(lllllllllllllllllllllllllll , _ProjectionParams.x);
                    half4 llllllllllllllllllllllllllll = d.screenPos;
		        #else
                    float4 lllllllllllllllllllllllllll = mul(GetWorldToHClipMatrix(), float4(d.worldSpacePosition.xyz, 1.0));
                    float4 llllllllllllllllllllllllllll = ComputeScreenPos(lllllllllllllllllllllllllll);
		        #endif
                llllllllllllllllllllllllllll.xy /= llllllllllllllllllllllllllll.w;
                llllllllllllllllllllllllllll.x *= llllllllllllllllllllllll;
                float lllllllllllllllllllllllllllll = min(1,llllllllllllllllllllllll);
                float llllllllllllllllllllllllllllll =  distance(llllllllllllllllllllllllllll.xy,llllllllllllllllllllllllll.xy) < _CircleObstructionDestroyRadius/lllllllll*lllllllllllllllllllllllllllll;
                float lllllllllllllllllllllllllllllll = (distance(llllllllllllllllllllllllllll.xy,llllllllllllllllllllllllll.xy)/(_CircleObstructionDestroyRadius/lllllllll*lllllllllllllllllllllllllllll));
                float llllllllllllllllllllllllllllllll = (llllllllllllllllllllllllllllll)&&llllllllllll>0;        
                float lllllllllllllllllllllllllllllllll = llllllllllllllllll;
                if(lllll != 0 || (!_TriggerMode && !_RaycastMode)) {
                    if (_Obstruction == 1) {
                        if(lllllllllllllllllllllll<=1.5&&llllll>lllllll){
                            llllllllllllllllll = (sqrt((llllll-lllllll))*25/lllllllllllllllllllllll) *_AngleStrength;  
                            llllllllllllllllll = max(0,log(llllllllllllllllll*0.2));
                        }
                    }  else if (_Obstruction == 2) {
                        if(lllllllllllllll){
                            float lllllllllllllllllllllllllllllllllll = llllllllllllll/lllllllllllll;
                            llllllllllllllllll = lllllllllllllllllllllllllllllllllllll(_DissolveFallOff, _ConeStrength, lllllllllllllllllllllllllllllllllll);
                        }
                    } else  if (_Obstruction == 3) {
                        if(lllllllllllllllllllllll<= 1.5 && llllll > lllllll || lllllllllllllll){
                            if(lllllllllllllllllllllll<= 1.5 && llllll > lllllll) {
                                llllllllllllllllll = (sqrt((llllll-lllllll))*25/lllllllllllllllllllllll)*_AngleStrength;                   
                                llllllllllllllllll = max(0,log(llllllllllllllllll*0.2));
                            }
                            if (lllllllllllllll) {
                                float lllllllllllllllllllllllllllllllllll = llllllllllllll/lllllllllllll;
                                llllllllllllllllll = lllllllllllllllllllllllllllllllllllll(_DissolveFallOff, _ConeStrength, lllllllllllllllllllllllllllllllllll)+llllllllllllllllll;
                            }
                        }
                    }  else if (_Obstruction == 4) {
                        if(lllllllllllllllll){
                            float lllllllllllllllllllllllllllllllllll = llllllllllllll/llllllllllllllll;
                            llllllllllllllllll = lllllllllllllllllllllllllllllllllllll(_DissolveFallOff, _CylinderStrength, lllllllllllllllllllllllllllllllllll);
                        }
                    }  else if (_Obstruction == 5) {
                        if(lllllllllllllllllllllll<=1.5&&llllll>lllllll||lllllllllllllllll){
                            if(lllllllllllllllllllllll<=1.5&&llllll>lllllll) {
                                llllllllllllllllll = (sqrt((llllll-lllllll))*25/lllllllllllllllllllllll)*_AngleStrength;
                                llllllllllllllllll = max(0,log(llllllllllllllllll*0.2));              
                            }
                            if(lllllllllllllllll){
                                float lllllllllllllllllllllllllllllllllll = llllllllllllll/llllllllllllllll;
                                llllllllllllllllll = lllllllllllllllllllllllllllllllllllll(_DissolveFallOff, _CylinderStrength, lllllllllllllllllllllllllllllllllll) + llllllllllllllllll;                        
                            }                     
                        }
                    } else if (_Obstruction == 6) {
                        if (llllllllllllllllllllllllllllllll) {
                            llllllllllllllllll = lllllllllllllllllllllllllllllllllllll(_DissolveFallOff, _CircleStrength, lllllllllllllllllllllllllllllll);                 
                        }
                    }
                    llllllllllllllllll = llllllllllllllllll+(1*_IntrinsicDissolveStrength);
                    float llllllllllllllllllllllllllllllllll = llllllllllllllllll/_FloorYTextureGradientLength;
                    if(_Floor == 1) {
        	            if(d.worldSpacePosition.y < (_PlayersPosArray[i].y+_PlayerPosYOffset)) {
                            float llllllllllllllllllllllllllllllllllll = (_PlayersPosArray[i].y+_PlayerPosYOffset) - d.worldSpacePosition.y;
                            if(llllllllllllllllllllllllllllllllllll < 0) {llllllllllllllllllllllllllllllllllll = 0;}
                            if(llllllllllllllllllllllllllllllllllll < _FloorYTextureGradientLength) {
                                llllllllllllllllll = (_FloorYTextureGradientLength-llllllllllllllllllllllllllllllllllll)*llllllllllllllllllllllllllllllllll;
                            } else {llllllllllllllllll = 0;}
                        }
                    } else {
                        if(d.worldSpacePosition.y < _FloorY) {
                            float llllllllllllllllllllllllllllllllllll = _FloorY - d.worldSpacePosition.y;
                            if(llllllllllllllllllllllllllllllllllll<0){llllllllllllllllllllllllllllllllllll=0;}
                            if(llllllllllllllllllllllllllllllllllll<_FloorYTextureGradientLength){llllllllllllllllll = (_FloorYTextureGradientLength-llllllllllllllllllllllllllllllllllll)*llllllllllllllllllllllllllllllllll;
                            } else {llllllllllllllllll = 0;}
                        }
                    }
                    if(!_TriggerMode && !_RaycastMode) {if(distance(_PlayersPosArray[i], d.worldSpacePosition) > _DefaultEffectRadius) {llllllllllllllllll = 0;}}
                }
                if(_TriggerMode || _RaycastMode) {llllllllllllllllll =  lllll * llllllllllllllllll;  
                } else {llllllllllllllllll = llllllllllllllllll;}
                llllllllllllllllllll = max(llllllllllllllllllll,llllllllllllllllll);
            }            
            float lllllllllllllllllllllllllllllllll = llllllllllllllllllll;
            if(!_PreviewMode) {
                if (lllllllllllllllllllllllllllllllll==1){lllllllllllllllllllllllllllllllll=10;}
                if (!_hasClippedShadows) {
                #if defined(UNITY_PASS_SHADOWCASTER)
                #if defined(SHADOWS_DEPTH)
                if (!any(unity_LightShadowBias)){clip(lllllllllllllllllll- lllllllllllllllllllllllllllllllll);}
                else{if(_hasClippedShadows) {clip(lllllllllllllllllll- lllllllllllllllllllllllllllllllll);}}
                #endif
                #else
                    clip(lllllllllllllllllll- lllllllllllllllllllllllllllllllll);
                #endif
                } else {clip(lllllllllllllllllll- lllllllllllllllllllllllllllllllll);}
            }       
            half4 c = lerp(1, tex2D(_MainTex, d.texcoord0.xy), _TextureVisibility) * _Color;        
            o.Albedo = clamp(_DissolveColor*lllllllllllllllllllllllllllllllll, 0, _DissolveColorSaturation) + c.rgb;
            if(_PreviewMode) {
                if((lllllllllllllllllll - lllllllllllllllllllllllllllllllll)< 0) {
                    o.Albedo = half4(1,1,1,1);
                } else {
                    o.Albedo = half4(0,0,0,1);
                }
            }
            if(_TexturedEmissionEdge) {
                _TexturedEmissionEdgeStrength = 0.2 + (_TexturedEmissionEdgeStrength*(0.8-0.2));
                o.Emission =  min( clamp(lerp(1,_DissolveColor,_DissolveColorSaturation)*lllllllllllllllllllllllllllllllll, 0, 1)*sqrt(_DissolveEmission*_DissolveEmissionBooster), clamp(lerp(1,_DissolveColor,_DissolveColorSaturation) *  clamp(((lllllllllllllllllllllllllllllllll/_TexturedEmissionEdgeStrength) - lllllllllllllllllll),0,1), 0, 1)*sqrt(_DissolveEmission*_DissolveEmissionBooster));

            } else {
                o.Emission =  clamp(lerp(1,_DissolveColor,_DissolveColorSaturation)*lllllllllllllllllllllllllllllllll, 0, 1)*sqrt(_DissolveEmission*_DissolveEmissionBooster);
            } 
            #if _HDRP
                o.Emission =  o.Emission * pow(_DissolveEmissionBooster,4);
            #endif

            #ifdef _NORMALMAP
                o.Normal = UnpackScaleNormal(tex2D(_BumpMap, d.texcoord0.xy), _BumpScale);
            #endif
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = 1;

        } else {
            half4 c = tex2D (_MainTex, d.texcoord0.xy) * _Color;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
            #ifdef _NORMALMAP
            o.Normal = UnpackScaleNormal(tex2D(_BumpMap, d.texcoord0.xy), _BumpScale);
            #endif
        }    
	}




        
            void ChainSurfaceFunction(inout Surface l, inout ShaderData d)
            {
                  Ext_SurfaceFunction0(l, d);
                 // Ext_SurfaceFunction1(l, d);
                 // Ext_SurfaceFunction2(l, d);
                 // Ext_SurfaceFunction3(l, d);
                 // Ext_SurfaceFunction4(l, d);
                 // Ext_SurfaceFunction5(l, d);
                 // Ext_SurfaceFunction6(l, d);
                 // Ext_SurfaceFunction7(l, d);
                 // Ext_SurfaceFunction8(l, d);
                 // Ext_SurfaceFunction9(l, d);
		           // Ext_SurfaceFunction10(l, d);
                 // Ext_SurfaceFunction11(l, d);
                 // Ext_SurfaceFunction12(l, d);
                 // Ext_SurfaceFunction13(l, d);
                 // Ext_SurfaceFunction14(l, d);
                 // Ext_SurfaceFunction15(l, d);
                 // Ext_SurfaceFunction16(l, d);
                 // Ext_SurfaceFunction17(l, d);
                 // Ext_SurfaceFunction18(l, d);
		           // Ext_SurfaceFunction19(l, d);
            }

            void ChainModifyVertex(inout VertexData v, inout VertexToPixel v2p)
            {
                 ExtraV2F d;
                 ZERO_INITIALIZE(ExtraV2F, d);
                 ZERO_INITIALIZE(Blackboard, d.blackboard);

                 //  Ext_ModifyVertex0(v, d);
                 // Ext_ModifyVertex1(v, d);
                 // Ext_ModifyVertex2(v, d);
                 // Ext_ModifyVertex3(v, d);
                 // Ext_ModifyVertex4(v, d);
                 // Ext_ModifyVertex5(v, d);
                 // Ext_ModifyVertex6(v, d);
                 // Ext_ModifyVertex7(v, d);
                 // Ext_ModifyVertex8(v, d);
                 // Ext_ModifyVertex9(v, d);
                 // Ext_ModifyVertex10(v, d);
                 // Ext_ModifyVertex11(v, d);
                 // Ext_ModifyVertex12(v, d);
                 // Ext_ModifyVertex13(v, d);
                 // Ext_ModifyVertex14(v, d);
                 // Ext_ModifyVertex15(v, d);
                 // Ext_ModifyVertex16(v, d);
                 // Ext_ModifyVertex17(v, d);
                 // Ext_ModifyVertex18(v, d);
                 // Ext_ModifyVertex19(v, d);


                 // #if %EXTRAV2F0REQUIREKEY%
                 // v2p.extraV2F0 = d.extraV2F0;
                 // #endif

                 // #if %EXTRAV2F1REQUIREKEY%
                 // v2p.extraV2F1 = d.extraV2F1;
                 // #endif

                 // #if %EXTRAV2F2REQUIREKEY%
                 // v2p.extraV2F2 = d.extraV2F2;
                 // #endif

                 // #if %EXTRAV2F3REQUIREKEY%
                 // v2p.extraV2F3 = d.extraV2F3;
                 // #endif

                 // #if %EXTRAV2F4REQUIREKEY%
                 // v2p.extraV2F4 = d.extraV2F4;
                 // #endif

                 // #if %EXTRAV2F5REQUIREKEY%
                 // v2p.extraV2F5 = d.extraV2F5;
                 // #endif

                 // #if %EXTRAV2F6REQUIREKEY%
                 // v2p.extraV2F6 = d.extraV2F6;
                 // #endif

                 // #if %EXTRAV2F7REQUIREKEY%
                 // v2p.extraV2F7 = d.extraV2F7;
                 // #endif
            }

            void ChainModifyTessellatedVertex(inout VertexData v, inout VertexToPixel v2p)
            {
               ExtraV2F d;
               ZERO_INITIALIZE(ExtraV2F, d);
               ZERO_INITIALIZE(Blackboard, d.blackboard);

               // #if %EXTRAV2F0REQUIREKEY%
               // d.extraV2F0 = v2p.extraV2F0;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // d.extraV2F1 = v2p.extraV2F1;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // d.extraV2F2 = v2p.extraV2F2;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // d.extraV2F3 = v2p.extraV2F3;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // d.extraV2F4 = v2p.extraV2F4;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // d.extraV2F5 = v2p.extraV2F5;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // d.extraV2F6 = v2p.extraV2F6;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // d.extraV2F7 = v2p.extraV2F7;
               // #endif


               // Ext_ModifyTessellatedVertex0(v, d);
               // Ext_ModifyTessellatedVertex1(v, d);
               // Ext_ModifyTessellatedVertex2(v, d);
               // Ext_ModifyTessellatedVertex3(v, d);
               // Ext_ModifyTessellatedVertex4(v, d);
               // Ext_ModifyTessellatedVertex5(v, d);
               // Ext_ModifyTessellatedVertex6(v, d);
               // Ext_ModifyTessellatedVertex7(v, d);
               // Ext_ModifyTessellatedVertex8(v, d);
               // Ext_ModifyTessellatedVertex9(v, d);
               // Ext_ModifyTessellatedVertex10(v, d);
               // Ext_ModifyTessellatedVertex11(v, d);
               // Ext_ModifyTessellatedVertex12(v, d);
               // Ext_ModifyTessellatedVertex13(v, d);
               // Ext_ModifyTessellatedVertex14(v, d);
               // Ext_ModifyTessellatedVertex15(v, d);
               // Ext_ModifyTessellatedVertex16(v, d);
               // Ext_ModifyTessellatedVertex17(v, d);
               // Ext_ModifyTessellatedVertex18(v, d);
               // Ext_ModifyTessellatedVertex19(v, d);

               // #if %EXTRAV2F0REQUIREKEY%
               // v2p.extraV2F0 = d.extraV2F0;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // v2p.extraV2F1 = d.extraV2F1;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // v2p.extraV2F2 = d.extraV2F2;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // v2p.extraV2F3 = d.extraV2F3;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // v2p.extraV2F4 = d.extraV2F4;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // v2p.extraV2F5 = d.extraV2F5;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // v2p.extraV2F6 = d.extraV2F6;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // v2p.extraV2F7 = d.extraV2F7;
               // #endif
            }

            void ChainFinalColorForward(inout Surface l, inout ShaderData d, inout half4 color)
            {
               //   Ext_FinalColorForward0(l, d, color);
               //   Ext_FinalColorForward1(l, d, color);
               //   Ext_FinalColorForward2(l, d, color);
               //   Ext_FinalColorForward3(l, d, color);
               //   Ext_FinalColorForward4(l, d, color);
               //   Ext_FinalColorForward5(l, d, color);
               //   Ext_FinalColorForward6(l, d, color);
               //   Ext_FinalColorForward7(l, d, color);
               //   Ext_FinalColorForward8(l, d, color);
               //   Ext_FinalColorForward9(l, d, color);
               //  Ext_FinalColorForward10(l, d, color);
               //  Ext_FinalColorForward11(l, d, color);
               //  Ext_FinalColorForward12(l, d, color);
               //  Ext_FinalColorForward13(l, d, color);
               //  Ext_FinalColorForward14(l, d, color);
               //  Ext_FinalColorForward15(l, d, color);
               //  Ext_FinalColorForward16(l, d, color);
               //  Ext_FinalColorForward17(l, d, color);
               //  Ext_FinalColorForward18(l, d, color);
               //  Ext_FinalColorForward19(l, d, color);
            }

            void ChainFinalGBufferStandard(inout Surface s, inout ShaderData d, inout half4 GBuffer0, inout half4 GBuffer1, inout half4 GBuffer2, inout half4 outEmission, inout half4 outShadowMask)
            {
               //   Ext_FinalGBufferStandard0(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard1(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard2(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard3(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard4(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard5(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard6(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard7(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard8(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard9(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard10(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard11(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard12(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard13(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard14(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard15(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard16(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard17(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard18(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard19(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
            }



            

         ShaderData CreateShaderData(VertexToPixel i
                  #if NEED_FACING
                     , bool facing
                  #endif
         )
         {
            ShaderData d = (ShaderData)0;
            d.clipPos = i.pos;
            d.worldSpacePosition = i.worldPos;

            d.worldSpaceNormal = normalize(i.worldNormal);
            d.worldSpaceTangent = normalize(i.worldTangent.xyz);
            d.tangentSign = i.worldTangent.w;
            float3 bitangent = cross(i.worldTangent.xyz, i.worldNormal) * d.tangentSign * -1;
            

            d.TBNMatrix = float3x3(d.worldSpaceTangent, bitangent, d.worldSpaceNormal);
            d.worldSpaceViewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

            d.tangentSpaceViewDir = mul(d.TBNMatrix, d.worldSpaceViewDir);
             d.texcoord0 = i.texcoord0;
            // d.texcoord1 = i.texcoord1;
            // d.texcoord2 = i.texcoord2;

            // #if %TEXCOORD3REQUIREKEY%
            // d.texcoord3 = i.texcoord3;
            // #endif

            // d.isFrontFace = facing;
            // #if %VERTEXCOLORREQUIREKEY%
            // d.vertexColor = i.vertexColor;
            // #endif

            // these rarely get used, so we back transform them. Usually will be stripped.
            #if _HDRP
                // d.localSpacePosition = mul(unity_WorldToObject, float4(GetCameraRelativePositionWS(i.worldPos), 1)).xyz;
            #else
                // d.localSpacePosition = mul(unity_WorldToObject, float4(i.worldPos, 1)).xyz;
            #endif
            // d.localSpaceNormal = normalize(mul((float3x3)unity_WorldToObject, i.worldNormal));
            // d.localSpaceTangent = normalize(mul((float3x3)unity_WorldToObject, i.worldTangent.xyz));

            // #if %SCREENPOSREQUIREKEY%
             d.screenPos = i.screenPos;
             d.screenUV = (i.screenPos.xy / i.screenPos.w);
            // #endif


            // #if %EXTRAV2F0REQUIREKEY%
            // d.extraV2F0 = i.extraV2F0;
            // #endif

            // #if %EXTRAV2F1REQUIREKEY%
            // d.extraV2F1 = i.extraV2F1;
            // #endif

            // #if %EXTRAV2F2REQUIREKEY%
            // d.extraV2F2 = i.extraV2F2;
            // #endif

            // #if %EXTRAV2F3REQUIREKEY%
            // d.extraV2F3 = i.extraV2F3;
            // #endif

            // #if %EXTRAV2F4REQUIREKEY%
            // d.extraV2F4 = i.extraV2F4;
            // #endif

            // #if %EXTRAV2F5REQUIREKEY%
            // d.extraV2F5 = i.extraV2F5;
            // #endif

            // #if %EXTRAV2F6REQUIREKEY%
            // d.extraV2F6 = i.extraV2F6;
            // #endif

            // #if %EXTRAV2F7REQUIREKEY%
            // d.extraV2F7 = i.extraV2F7;
            // #endif

            return d;
         }
         

            
         #if defined(SHADERPASS_SHADOWCASTER)
            float3 _LightDirection;
         #endif

         // vertex shader
         VertexToPixel Vert (VertexData v)
         {
           
           VertexToPixel o = (VertexToPixel)0;

           UNITY_SETUP_INSTANCE_ID(v);
           UNITY_TRANSFER_INSTANCE_ID(v, o);
           UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


#if !_TESSELLATION_ON
           ChainModifyVertex(v, o);
#endif

            o.texcoord0 = v.texcoord0;
           // o.texcoord1 = v.texcoord1;
           // o.texcoord2 = v.texcoord2;

           // #if %TEXCOORD3REQUIREKEY%
           // o.texcoord3 = v.texcoord3;
           // #endif

           // #if %VERTEXCOLORREQUIREKEY%
           // o.vertexColor = v.vertexColor;
           // #endif
           
           VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
           o.worldPos = TransformObjectToWorld(v.vertex.xyz);
           o.worldNormal = TransformObjectToWorldNormal(v.normal);
           o.worldTangent = float4(TransformObjectToWorldDir(v.tangent.xyz), v.tangent.w);


          #if defined(SHADERPASS_SHADOWCASTER)
              // Define shadow pass specific clip position for Universal
              o.pos = TransformWorldToHClip(ApplyShadowBias(o.worldPos, o.worldNormal, _LightDirection));
              #if UNITY_REVERSED_Z
                  o.pos.z = min(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
              #else
                  o.pos.z = max(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
              #endif
          #elif defined(SHADERPASS_META)
              o.pos = MetaVertexPosition(float4(v.vertex.xyz, 0), v.texcoord1, v.texcoord2, unity_LightmapST, unity_DynamicLightmapST);
          #else
              o.pos = TransformWorldToHClip(o.worldPos);
          #endif

          // #if %SCREENPOSREQUIREKEY%
           o.screenPos = ComputeScreenPos(o.pos, _ProjectionParams.x);
          // #endif

          #if defined(SHADERPASS_FORWARD)
              OUTPUT_LIGHTMAP_UV(v.texcoord1, unity_LightmapST, o.lightmapUV);
              OUTPUT_SH(o.worldNormal, o.sh);
          #endif

          #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
              half3 vertexLight = VertexLighting(o.worldPos, o.worldNormal);
              half fogFactor = ComputeFogFactor(o.pos.z);
              o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
          #endif

          #ifdef _MAIN_LIGHT_SHADOWS
              o.shadowCoord = GetShadowCoord(vertexInput);
          #endif

           return o;
         }


            

            // fragment shader
            half4 Frag (VertexToPixel IN
            #ifdef _DEPTHOFFSET_ON
              , out float outputDepth : SV_Depth
            #endif
            #if NEED_FACING
            , bool facing : SV_IsFrontFace
            #endif
            ) : SV_Target
            {
               UNITY_SETUP_INSTANCE_ID(IN);
               UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

               ShaderData d = CreateShaderData(IN
                  #if NEED_FACING
                     , facing
                  #endif
               );
               Surface l = (Surface)0;

               #ifdef _DEPTHOFFSET_ON
                  l.outputDepth = outputDepth;
               #endif

               l.Albedo = half3(0.5, 0.5, 0.5);
               l.Normal = float3(0,0,1);
               l.Occlusion = 1;
               l.Alpha = 1;

               ChainSurfaceFunction(l, d);

               #ifdef _DEPTHOFFSET_ON
                  outputDepth = l.outputDepth;
               #endif

               return 0;

            }

         ENDHLSL

      }


      
        Pass
        {
            Name "Meta"
            Tags 
            { 
                "LightMode" = "Meta"
            }

             // Render State
            Blend One Zero, One Zero
            Cull Back
            ZTest LEqual
            ZWrite On
            // ColorMask: <None>

            

            HLSLPROGRAM

               #pragma vertex Vert
   #pragma fragment Frag

            #pragma target 3.0

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
        
            #define SHADERPASS_META
            #define _PASSMETA 1


            
      #pragma shader_feature_local _NORMALMAP


   #define _URP 1



            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Version.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"
        

                  #undef WorldNormalVector
      #define WorldNormalVector(data, normal) mul(normal, data.TBNMatrix)
      
      #define UnityObjectToWorldNormal(normal) mul(GetObjectToWorldMatrix(), normal)

      #define _WorldSpaceLightPos0 _MainLightPosition
      
      #define UNITY_DECLARE_TEX2D(name) TEXTURE2D(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2D_NOSAMPLER(name) TEXTURE2D(name);
      #define UNITY_DECLARE_TEX2DARRAY(name) TEXTURE2D_ARRAY(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(name) TEXTURE2D_ARRAY(name);

      #define UNITY_SAMPLE_TEX2DARRAY(tex,coord)            SAMPLE_TEXTURE2D_ARRAY(tex, sampler##tex, coord.xy, coord.z)
      #define UNITY_SAMPLE_TEX2DARRAY_LOD(tex,coord,lod)    SAMPLE_TEXTURE2D_ARRAY_LOD(tex, sampler##tex, coord.xy, coord.z, lod)
      #define UNITY_SAMPLE_TEX2D(tex, coord)                SAMPLE_TEXTURE2D(tex, sampler##tex, coord)
      #define UNITY_SAMPLE_TEX2D_SAMPLER(tex, samp, coord)  SAMPLE_TEXTURE2D(tex, sampler##samp, coord)

      #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod)   SAMPLE_TEXTURE2D_LOD(tex, sampler_##tex, coord, lod)
      #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) SAMPLE_TEXTURE2D_LOD (tex, sampler##samplertex,coord, lod)
     
      #if defined(UNITY_COMPILER_HLSL)
         #define UNITY_INITIALIZE_OUTPUT(type,name) name = (type)0;
      #else
         #define UNITY_INITIALIZE_OUTPUT(type,name)
      #endif

      #define sampler2D_float sampler2D
      #define sampler2D_half sampler2D



      // data across stages, stripped like the above.
      struct VertexToPixel
      {
         float4 pos : SV_POSITION;
         float3 worldPos : TEXCOORD0;
         float3 worldNormal : TEXCOORD1;
         float4 worldTangent : TEXCOORD2;
          float4 texcoord0 : TEXCCOORD3;
         // float4 texcoord1 : TEXCCOORD4;
         // float4 texcoord2 : TEXCCOORD5;

         // #if %TEXCOORD3REQUIREKEY%
         // float4 texcoord3 : TEXCCOORD6;
         // #endif

         // #if %SCREENPOSREQUIREKEY%
          float4 screenPos : TEXCOORD7;
         // #endif

         // #if %VERTEXCOLORREQUIREKEY%
         // half4 vertexColor : COLOR;
         // #endif

         // #if %EXTRAV2F0REQUIREKEY%
         // float4 extraV2F0 : TEXCOORD12;
         // #endif

         // #if %EXTRAV2F1REQUIREKEY%
         // float4 extraV2F1 : TEXCOORD13;
         // #endif

         // #if %EXTRAV2F2REQUIREKEY%
         // float4 extraV2F2 : TEXCOORD14;
         // #endif

         // #if %EXTRAV2F3REQUIREKEY%
         // float4 extraV2F3 : TEXCOORD15;
         // #endif

         // #if %EXTRAV2F4REQUIREKEY%
         // float4 extraV2F4 : TEXCOORD16;
         // #endif

         // #if %EXTRAV2F5REQUIREKEY%
         // float4 extraV2F5 : TEXCOORD17;
         // #endif

         // #if %EXTRAV2F6REQUIREKEY%
         // float4 extraV2F6 : TEXCOORD18;
         // #endif

         // #if %EXTRAV2F7REQUIREKEY%
         // float4 extraV2F7 : TEXCOORD19;
         // #endif
            
         #if defined(LIGHTMAP_ON)
            float2 lightmapUV : TEXCOORD8;
         #endif
         #if !defined(LIGHTMAP_ON)
            float3 sh : TEXCOORD9;
         #endif
            float4 fogFactorAndVertexLight : TEXCOORD10;
            float4 shadowCoord : TEXCOORD11;
         #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
         #endif
         #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
         #endif
         #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
         #endif
         #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
         #endif
      };


            
            
            // data describing the user output of a pixel
            struct Surface
            {
               half3 Albedo;
               half Height;
               half3 Normal;
               half Smoothness;
               half3 Emission;
               half Metallic;
               half3 Specular;
               half Occlusion;
               half SpecularPower; // for simple lighting
               half Alpha;
               float outputDepth; // if written, SV_Depth semantic is used. ShaderData.clipPos.z is unused value
               // HDRP Only
               half SpecularOcclusion;
               half SubsurfaceMask;
               half Thickness;
               half CoatMask;
               half CoatSmoothness;
               half Anisotropy;
               half IridescenceMask;
               half IridescenceThickness;
               int DiffusionProfileHash;
               // requires _OVERRIDE_BAKEDGI to be defined, but is mapped in all pipelines
               float3 DiffuseGI;
               float3 BackDiffuseGI;
               float3 SpecularGI;
               // requires _OVERRIDE_SHADOWMASK to be defines
               float4 ShadowMask;
            };

            // Data the user declares in blackboard blocks
            struct Blackboard
            {
                
                float blackboardDummyData;
            };

            // data the user might need, this will grow to be big. But easy to strip
            struct ShaderData
            {
               float4 clipPos; // SV_POSITION
               float3 localSpacePosition;
               float3 localSpaceNormal;
               float3 localSpaceTangent;
        
               float3 worldSpacePosition;
               float3 worldSpaceNormal;
               float3 worldSpaceTangent;
               float tangentSign;

               float3 worldSpaceViewDir;
               float3 tangentSpaceViewDir;

               float4 texcoord0;
               float4 texcoord1;
               float4 texcoord2;
               float4 texcoord3;

               float2 screenUV;
               float4 screenPos;

               float4 vertexColor;
               bool isFrontFace;

               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;

               float3x3 TBNMatrix;
               Blackboard blackboard;
            };

            struct VertexData
            {
               #if SHADER_TARGET > 30
               // uint vertexID : SV_VertexID;
               #endif
               float4 vertex : POSITION;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;

               // would love to strip these, but they are used in certain
               // combinations of the lighting system, and may be used
               // by the user as well, so no easy way to strip them.

               float4 texcoord1 : TEXCOORD1;
               float4 texcoord2 : TEXCOORD2;

               // #if %TEXCOORD3REQUIREKEY%
               // float4 texcoord3 : TEXCOORD3;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               #if _HDRP && (_PASSMOTIONVECTOR || (_PASSFORWARD && defined(_WRITE_TRANSPARENT_MOTION_VECTOR)))
                  float3 previousPositionOS : TEXCOORD4; // Contain previous transform position (in case of skinning for example)
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity    : TEXCOORD5; // Add Precomputed Velocity (Alembic computes velocities on runtime side).
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct TessVertex 
            {
               float4 vertex : INTERNALTESSPOS;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;
               float4 texcoord1 : TEXCOORD1;
               float4 texcoord2 : TEXCOORD2;

               // #if %TEXCOORD3REQUIREKEY%
               // float4 texcoord3 : TEXCOORD3;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               // #if %EXTRAV2F0REQUIREKEY%
               // float4 extraV2F0 : TEXCOORD5;
               // endif

               // #if %EXTRAV2F1REQUIREKEY%
               // float4 extraV2F1 : TEXCOORD6;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // float4 extraV2F2 : TEXCOORD7;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // float4 extraV2F3 : TEXCOORD8;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // float4 extraV2F4 : TEXCOORD9;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // float4 extraV2F5 : TEXCOORD10;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // float4 extraV2F6 : TEXCOORD11;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // float4 extraV2F7 : TEXCOORD12;
               // #endif

               #if _HDRP && (_PASSMOTIONVECTOR || (_PASSFORWARD && defined(_WRITE_TRANSPARENT_MOTION_VECTOR)))
                  float3 previousPositionOS : TEXCOORD13; // Contain previous transform position (in case of skinning for example)
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity : TEXCOORD14;
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
               UNITY_VERTEX_OUTPUT_STEREO
            };

            struct ExtraV2F
            {
               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;
               Blackboard blackboard;
            };


            float3 WorldToTangentSpace(ShaderData d, float3 normal)
            {
               return mul(d.TBNMatrix, normal);
            }

            float3 TangentToWorldSpace(ShaderData d, float3 normal)
            {
               return mul(normal, d.TBNMatrix);
            }

            // in this case, make standard more like SRPs, because we can't fix
            // unity_WorldToObject in HDRP, since it already does macro-fu there

            #if _STANDARD
               float3 TransformWorldToObject(float3 p) { return mul(unity_WorldToObject, float4(p, 1)); };
               float3 TransformObjectToWorld(float3 p) { return mul(unity_ObjectToWorld, float4(p, 1)); };
               float4 TransformWorldToObject(float4 p) { return mul(unity_WorldToObject, p); };
               float4 TransformObjectToWorld(float4 p) { return mul(unity_ObjectToWorld, p); };
               float4x4 GetWorldToObjectMatrix() { return unity_WorldToObject; }
               float4x4 GetObjectToWorldMatrix() { return unity_ObjectToWorld; }
               #if (defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (SHADER_TARGET_SURFACE_ANALYSIS && !SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod) tex.SampleLevel (sampler##tex,coord, lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) tex.SampleLevel (sampler##samplertex,coord, lod)
              #else
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord,lod) tex2D (tex,coord,0,lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord,lod) tex2D (tex,coord,0,lod)
              #endif

               #undef GetObjectToWorldMatrix()
               #undef GetWorldToObjectMatrix()
               #undef GetWorldToViewMatrix()
               #undef UNITY_MATRIX_I_V
               #undef UNITY_MATRIX_P
               #undef GetWorldToHClipMatrix()
               #undef GetObjectToWorldMatrix()V
               #undef UNITY_MATRIX_T_MV
               #undef UNITY_MATRIX_IT_MV
               #undef GetObjectToWorldMatrix()VP

               #define GetObjectToWorldMatrix()     unity_ObjectToWorld
               #define GetWorldToObjectMatrix()   unity_WorldToObject
               #define GetWorldToViewMatrix()     unity_MatrixV
               #define UNITY_MATRIX_I_V   unity_MatrixInvV
               #define GetViewToHClipMatrix()     OptimizeProjectionMatrix(glstate_matrix_projection)
               #define GetWorldToHClipMatrix()    unity_MatrixVP
               #define GetObjectToWorldMatrix()V    mul(GetWorldToViewMatrix(), GetObjectToWorldMatrix())
               #define UNITY_MATRIX_T_MV  transpose(GetObjectToWorldMatrix()V)
               #define UNITY_MATRIX_IT_MV transpose(mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V))
               #define GetObjectToWorldMatrix()VP   mul(GetWorldToHClipMatrix(), GetObjectToWorldMatrix())


            #endif

            float3 GetCameraWorldPosition()
            {
               #if _HDRP
                  return GetCameraRelativePositionWS(_WorldSpaceCameraPos);
               #else
                  return _WorldSpaceCameraPos;
               #endif
            }

            #if _GRABPASSUSED
               #if _STANDARD
                  TEXTURE2D(%GRABTEXTURE%);
                  SAMPLER(sampler_%GRABTEXTURE%);
               #endif

               half3 GetSceneColor(float2 uv)
               {
                  #if _STANDARD
                     return SAMPLE_TEXTURE2D(%GRABTEXTURE%, sampler_%GRABTEXTURE%, uv).rgb;
                  #else
                     return SHADERGRAPH_SAMPLE_SCENE_COLOR(uv);
                  #endif
               }
            #endif


      
            #if _STANDARD
               UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
               float GetSceneDepth(float2 uv) { return SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv)); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv)); } 
            #else
               float GetSceneDepth(float2 uv) { return SHADERGRAPH_SAMPLE_SCENE_DEPTH(uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv), _ZBufferParams); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv), _ZBufferParams); } 
            #endif

            float3 GetWorldPositionFromDepthBuffer(float2 uv, float3 worldSpaceViewDir)
            {
               float eye = GetLinearEyeDepth(uv);
               float3 camView = mul((float3x3)GetObjectToWorldMatrix(), transpose(mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V)) [2].xyz);

               float dt = dot(worldSpaceViewDir, camView);
               float3 div = worldSpaceViewDir/dt;
               float3 wpos = (eye * div) + GetCameraWorldPosition();
               return wpos;
            }

            #if _STANDARD
               UNITY_DECLARE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture);
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  float4 depthNorms = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture, uv);
                  float3 norms = DecodeViewNormalStereo(depthNorms);
                  norms = mul((float3x3)GetWorldToViewMatrix(), norms) * 0.5 + 0.5;
                  return norms;
               }
            #elif _HDRP
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  NormalData nd;
                  DecodeFromNormalBuffer(_ScreenSize.xy * uv, nd);
                  return nd.normalWS;
               }
            #elif _URP
               #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                  #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"
               #endif

               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                     return SampleSceneNormals(uv);
                  #else
                     float3 wpos = GetWorldPositionFromDepthBuffer(uv, worldSpaceViewDir);
                     return normalize(-cross(ddx(wpos), ddy(wpos))) * 0.5 + 0.5;
                  #endif

                }
             #endif

             #if _HDRP

               half3 UnpackNormalmapRGorAG(half4 packednormal)
               {
                     // This do the trick
                  packednormal.x *= packednormal.w;

                  half3 normal;
                  normal.xy = packednormal.xy * 2 - 1;
                  normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                  return normal;
               }
               half3 UnpackNormal(half4 packednormal)
               {
                  #if defined(UNITY_NO_DXT5nm)
                     return packednormal.xyz * 2 - 1;
                  #else
                     return UnpackNormalmapRGorAG(packednormal);
                  #endif
               }
               #endif
               #if _HDRP || _URP

               half3 UnpackScaleNormal(half4 packednormal, half scale)
               {
                 #ifndef UNITY_NO_DXT5nm
                   // Unpack normal as DXT5nm (1, y, 1, x) or BC5 (x, y, 0, 1)
                   // Note neutral texture like "bump" is (0, 0, 1, 1) to work with both plain RGB normal and DXT5nm/BC5
                   packednormal.x *= packednormal.w;
                 #endif
                   half3 normal;
                   normal.xy = (packednormal.xy * 2 - 1) * scale;
                   normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                   return normal;
               }	

             #endif


            void GetSun(out float3 lightDir, out float3 color)
            {
               lightDir = float3(0.5, 0.5, 0);
               color = 1;
               #if _HDRP
                  if (_DirectionalLightCount > 0)
                  {
                     DirectionalLightData light = _DirectionalLightDatas[0];
                     lightDir = -light.forward.xyz;
                     color = light.color;
                  }
               #elif _STANDARD
			         lightDir = normalize(_WorldSpaceLightPos0.xyz);
                  color = _LightColor0.rgb;
               #elif _URP
	               Light light = GetMainLight();
	               lightDir = light.direction;
	               color = light.color;
               #endif
            }


            
            CBUFFER_START(UnityPerMaterial)

               
	half4 _Color;
	half _Glossiness;
	half _Metallic;
	half _TextureVisibility;
	half _AngleStrength;
	float _Obstruction;
	float _Floor;
	float _UVs;
	float _BumpScale; 
	half4 _DissolveColor;
	float _DissolveColorSaturation;
	float _DissolveEmission;
	float _DissolveEmissionBooster;
	float _hasClippedShadows;
	float _ConeStrength;
	float _ConeObstructionDestroyRadius;
	float _CylinderStrength;
	float _CylinderObstructionDestroyRadius;
	float _CircleStrength;
	float _CircleObstructionDestroyRadius;
	float _IntrinsicDissolveStrength;
	float _DissolveFallOff;
	float _PreviewMode;
	float _AnimationEnabled;
	float _AnimationSpeed;
	float _TriggerMode;
	float _RaycastMode;
	float _IsExempt;
	float _DefaultEffectRadius;
	float _FloorY;
	float _FloorYTextureGradientLength;
	float4 _PlayerPos;
	float _PlayerPosYOffset;
	int _ArrayLength = 0;
	float4 _PlayersPosArray[100];
	float4 _PlayersDataArray[100];        
	float _TransitionDuration;
    float _tDirection = 0;
    float _numOfPlayersInside = 0;
    float _tValue = 0;
    float _id = 0;
    float _TexturedEmissionEdge;
    float _TexturedEmissionEdgeStrength;
	float _IsReplacementShader;
	half4 _DissolveColorGlobal;
	float _DissolveColorSaturationGlobal;
	float _DissolveEmissionGlobal;
	float _DissolveEmissionBoosterGlobal;
	float _TextureVisibilityGlobal;
	float _ObstructionGlobal;
	float _AngleStrengthGlobal;
	float _ConeStrengthGlobal;
	float _ConeObstructionDestroyRadiusGlobal;
	float _CylinderStrengthGlobal;
	float _CylinderObstructionDestroyRadiusGlobal;
	float _CircleStrengthGlobal;
	float _CircleObstructionDestroyRadiusGlobal;
	float _DissolveFallOffGlobal;
	float _IntrinsicDissolveStrengthGlobal;
	float _PreviewModeGlobal;
	float _UVsGlobal;
	float _hasClippedShadowsGlobal;
	float _FloorGlobal;
	float _FloorYGlobal;
	float _PlayerPosYOffsetGlobal;
	float _FloorYTextureGradientLengthGlobal;
	float _DefaultEffectRadiusGlobal;
	float _AnimationEnabledGlobal;
	float _AnimationSpeedGlobal;
	float _TransitionDurationGlobal;
    float _TexturedEmissionEdgeGlobal;
    float _TexturedEmissionEdgeStrengthGlobal;
    float _isReferenceMaterial;



            CBUFFER_END

            

            

            #ifdef unity_WorldToObject
#undef unity_WorldToObject
#endif
#ifdef unity_ObjectToWorld
#undef unity_ObjectToWorld
#endif
#define unity_ObjectToWorld GetObjectToWorldMatrix()
#define unity_WorldToObject GetWorldToObjectMatrix()

    float lllllllllllllllllllllllllllllllllllll(float fallOff, float strength, float input)
    {
        float k = fallOff;
        k = max(k,0.00001);
        float n = 1-strength;
        float b = exp(k*6);
        float j = input;
        float v = n/(k/(k*n-0.15*(k-n)));
        float y = ((j-v)/(b*(1-j)+j))+v;
        y = 1-y;
        return y * sign(strength);
    }
	sampler2D _MainTex;
    #ifdef _NORMALMAP
        sampler2D _BumpMap;
    #endif
	sampler2D _DissolveTex;
	sampler2D _DissolveTexGlobal;

	void Ext_SurfaceFunction0 (inout Surface o, ShaderData d)
	{
        bool lllllllllllllllllllllllllllllllllllllll =  (_numOfPlayersInside > 0 || _tDirection == -1 && _Time.y - _tValue < _TransitionDuration ) || (_numOfPlayersInside >= 0 && _tDirection == 1); 
        bool llllllllllllllllllllllllllllllllllllllll = !_TriggerMode && !_RaycastMode;
        if(!_IsExempt && (lllllllllllllllllllllllllllllllllllllll || llllllllllllllllllllllllllllllllllllllll) ) {
            if(_IsReplacementShader) {    
                _DissolveColor = _DissolveColorGlobal;
                _DissolveColorSaturation = _DissolveColorSaturationGlobal;
                _DissolveEmission = _DissolveEmissionGlobal;
                _DissolveEmissionBooster = _DissolveEmissionBoosterGlobal;
                _TextureVisibility = _TextureVisibilityGlobal;
                _Obstruction = _ObstructionGlobal;
                _AngleStrength = _AngleStrengthGlobal;
                _ConeStrength = _ConeStrengthGlobal;
                _ConeObstructionDestroyRadius = _ConeObstructionDestroyRadiusGlobal;
                _CylinderStrength = _CylinderStrengthGlobal;
                _CylinderObstructionDestroyRadius = _CylinderObstructionDestroyRadiusGlobal;
                _CircleStrength = _CircleStrengthGlobal;
                _CircleObstructionDestroyRadius = _CircleObstructionDestroyRadiusGlobal;
                _DissolveFallOff = _DissolveFallOffGlobal;
                _IntrinsicDissolveStrength = _IntrinsicDissolveStrengthGlobal;
                _PreviewMode = _PreviewModeGlobal;
                _UVs = _UVsGlobal;
                _hasClippedShadows = _hasClippedShadowsGlobal;
                _Floor = _FloorGlobal;
                _FloorY = _FloorYGlobal;
                _PlayerPosYOffset = _PlayerPosYOffsetGlobal;
                _FloorYTextureGradientLength = _FloorYTextureGradientLengthGlobal; 
                _AnimationEnabled = _AnimationEnabledGlobal;
                _AnimationSpeed = _AnimationSpeedGlobal;
                _TransitionDuration = _TransitionDurationGlobal;
                _DefaultEffectRadius = _DefaultEffectRadiusGlobal;
                _TexturedEmissionEdge = _TexturedEmissionEdgeGlobal;
                _TexturedEmissionEdgeStrength = _TexturedEmissionEdgeStrengthGlobal;
            }
            if(_IntrinsicDissolveStrength < 0) {_IntrinsicDissolveStrength = 0;}
            float3 l;
            d.worldSpaceNormal = mul(o.Normal, (float3x3)d.TBNMatrix);
            float3 ll = d.worldSpacePosition / (-1.0 * abs(_UVs) );
            if(_AnimationEnabled) {ll = ll + abs(((_Time.y) * _AnimationSpeed));}       
            if(_IsReplacementShader) {
                l = lerp(lerp(tex2D(_DissolveTexGlobal,ll.xz).rgb,tex2D(_DissolveTexGlobal,ll.yz ).rgb,abs(d.worldSpaceNormal.x)).rgb,tex2D(_DissolveTexGlobal,ll.xy).rgb,abs(d.worldSpaceNormal.z)).rgb;
            } else {
                l = lerp(lerp(tex2D(_DissolveTex,ll.xz).rgb,tex2D(_DissolveTex,ll.yz ).rgb,abs(d.worldSpaceNormal.x)).rgb,tex2D(_DissolveTex,ll.xy).rgb,abs(d.worldSpaceNormal.z)).rgb;
            }
            half lllllllllllllllllll = l;           
            float llllllllllllllllllll = 0;
            for (int i = 0; i < _ArrayLength; i++){
                float lll = _PlayersDataArray[i][1];
                float llll = _PlayersDataArray[i][2];
                float lllll = 1;
                if( llll!= 0 && lll != 0 && _Time.y-lll < _TransitionDuration) {
                    if(llll == 1) {lllll = ((_TransitionDuration-(_Time.y-lll))/_TransitionDuration);
                    } else {lllll = ((_Time.y-lll)/_TransitionDuration);}
                } else if(llll ==-1) {lllll = 1;
                } else if(llll == 1) {lllll = 0;
                } else {lllll = 1;}
                lllll = 1 - lllll;   
                float llllllllllllllllllllllllllllllllllllll = _PlayersDataArray[i][3];
                lllll = lllll * float(llllllllllllllllllllllllllllllllllllll == _id);
                float llllll = distance(_WorldSpaceCameraPos, _PlayersPosArray[i]);
                float lllllll = distance(_WorldSpaceCameraPos, d.worldSpacePosition);
                float3 llllllll = _WorldSpaceCameraPos - _PlayersPosArray[i];
                float lllllllll = length(llllllll);
                float llllllllll = _ConeObstructionDestroyRadius;
                float3 lllllllllll = normalize(llllllll);
                float llllllllllll = dot(d.worldSpacePosition - _PlayersPosArray[i], lllllllllll);
                float lllllllllllll = (llllllllllll/lllllllll)*llllllllll;
                float llllllllllllll = length((d.worldSpacePosition - _PlayersPosArray[i])-llllllllllll*lllllllllll);
                float lllllllllllllll = llllllllllllll<lllllllllllll;
                float llllllllllllllll = _CylinderObstructionDestroyRadius;
                float lllllllllllllllll = (llllllllllllll<llllllllllllllll)&&llllllllllll>0;
                float llllllllllllllllll = 0;
                float3 lllllllllllllllllllll =  d.worldSpacePosition - _PlayersPosArray[i];
                float3 llllllllllllllllllllll =  d.worldSpaceNormal;
                float lllllllllllllllllllllll = acos(dot(lllllllllllllllllllll,llllllllllllllllllllll)/(length(lllllllllllllllllllll)*length(llllllllllllllllllllll)));        
                float llllllllllllllllllllllll = _ScreenParams.x / _ScreenParams.y;
		        #if _HDRP
                    float4 lllllllllllllllllllllllll = mul(GetWorldToHClipMatrix(), float4(GetCameraRelativePositionWS(_PlayersPosArray[i].xyz), 1.0));
                    float4 llllllllllllllllllllllllll = ComputeScreenPos(lllllllllllllllllllllllll , _ProjectionParams.x);
		        #else
			        float4 lllllllllllllllllllllllll = mul(GetWorldToHClipMatrix(), float4(_PlayersPosArray[i].xyz, 1.0));
                    float4 llllllllllllllllllllllllll = ComputeScreenPos(lllllllllllllllllllllllll);
		        #endif
                llllllllllllllllllllllllll.xy /= llllllllllllllllllllllllll.w;
                llllllllllllllllllllllllll.x *= llllllllllllllllllllllll;
                #if _HDRP
                    //float4 lllllllllllllllllllllllllll = mul(GetWorldToHClipMatrix(), float4(GetCameraRelativePositionWS(d.worldSpacePosition), 1.0));
                    //half4 llllllllllllllllllllllllllll = ComputeScreenPos(lllllllllllllllllllllllllll , _ProjectionParams.x);
                    half4 llllllllllllllllllllllllllll = d.screenPos;
		        #else
                    float4 lllllllllllllllllllllllllll = mul(GetWorldToHClipMatrix(), float4(d.worldSpacePosition.xyz, 1.0));
                    float4 llllllllllllllllllllllllllll = ComputeScreenPos(lllllllllllllllllllllllllll);
		        #endif
                llllllllllllllllllllllllllll.xy /= llllllllllllllllllllllllllll.w;
                llllllllllllllllllllllllllll.x *= llllllllllllllllllllllll;
                float lllllllllllllllllllllllllllll = min(1,llllllllllllllllllllllll);
                float llllllllllllllllllllllllllllll =  distance(llllllllllllllllllllllllllll.xy,llllllllllllllllllllllllll.xy) < _CircleObstructionDestroyRadius/lllllllll*lllllllllllllllllllllllllllll;
                float lllllllllllllllllllllllllllllll = (distance(llllllllllllllllllllllllllll.xy,llllllllllllllllllllllllll.xy)/(_CircleObstructionDestroyRadius/lllllllll*lllllllllllllllllllllllllllll));
                float llllllllllllllllllllllllllllllll = (llllllllllllllllllllllllllllll)&&llllllllllll>0;        
                float lllllllllllllllllllllllllllllllll = llllllllllllllllll;
                if(lllll != 0 || (!_TriggerMode && !_RaycastMode)) {
                    if (_Obstruction == 1) {
                        if(lllllllllllllllllllllll<=1.5&&llllll>lllllll){
                            llllllllllllllllll = (sqrt((llllll-lllllll))*25/lllllllllllllllllllllll) *_AngleStrength;  
                            llllllllllllllllll = max(0,log(llllllllllllllllll*0.2));
                        }
                    }  else if (_Obstruction == 2) {
                        if(lllllllllllllll){
                            float lllllllllllllllllllllllllllllllllll = llllllllllllll/lllllllllllll;
                            llllllllllllllllll = lllllllllllllllllllllllllllllllllllll(_DissolveFallOff, _ConeStrength, lllllllllllllllllllllllllllllllllll);
                        }
                    } else  if (_Obstruction == 3) {
                        if(lllllllllllllllllllllll<= 1.5 && llllll > lllllll || lllllllllllllll){
                            if(lllllllllllllllllllllll<= 1.5 && llllll > lllllll) {
                                llllllllllllllllll = (sqrt((llllll-lllllll))*25/lllllllllllllllllllllll)*_AngleStrength;                   
                                llllllllllllllllll = max(0,log(llllllllllllllllll*0.2));
                            }
                            if (lllllllllllllll) {
                                float lllllllllllllllllllllllllllllllllll = llllllllllllll/lllllllllllll;
                                llllllllllllllllll = lllllllllllllllllllllllllllllllllllll(_DissolveFallOff, _ConeStrength, lllllllllllllllllllllllllllllllllll)+llllllllllllllllll;
                            }
                        }
                    }  else if (_Obstruction == 4) {
                        if(lllllllllllllllll){
                            float lllllllllllllllllllllllllllllllllll = llllllllllllll/llllllllllllllll;
                            llllllllllllllllll = lllllllllllllllllllllllllllllllllllll(_DissolveFallOff, _CylinderStrength, lllllllllllllllllllllllllllllllllll);
                        }
                    }  else if (_Obstruction == 5) {
                        if(lllllllllllllllllllllll<=1.5&&llllll>lllllll||lllllllllllllllll){
                            if(lllllllllllllllllllllll<=1.5&&llllll>lllllll) {
                                llllllllllllllllll = (sqrt((llllll-lllllll))*25/lllllllllllllllllllllll)*_AngleStrength;
                                llllllllllllllllll = max(0,log(llllllllllllllllll*0.2));              
                            }
                            if(lllllllllllllllll){
                                float lllllllllllllllllllllllllllllllllll = llllllllllllll/llllllllllllllll;
                                llllllllllllllllll = lllllllllllllllllllllllllllllllllllll(_DissolveFallOff, _CylinderStrength, lllllllllllllllllllllllllllllllllll) + llllllllllllllllll;                        
                            }                     
                        }
                    } else if (_Obstruction == 6) {
                        if (llllllllllllllllllllllllllllllll) {
                            llllllllllllllllll = lllllllllllllllllllllllllllllllllllll(_DissolveFallOff, _CircleStrength, lllllllllllllllllllllllllllllll);                 
                        }
                    }
                    llllllllllllllllll = llllllllllllllllll+(1*_IntrinsicDissolveStrength);
                    float llllllllllllllllllllllllllllllllll = llllllllllllllllll/_FloorYTextureGradientLength;
                    if(_Floor == 1) {
        	            if(d.worldSpacePosition.y < (_PlayersPosArray[i].y+_PlayerPosYOffset)) {
                            float llllllllllllllllllllllllllllllllllll = (_PlayersPosArray[i].y+_PlayerPosYOffset) - d.worldSpacePosition.y;
                            if(llllllllllllllllllllllllllllllllllll < 0) {llllllllllllllllllllllllllllllllllll = 0;}
                            if(llllllllllllllllllllllllllllllllllll < _FloorYTextureGradientLength) {
                                llllllllllllllllll = (_FloorYTextureGradientLength-llllllllllllllllllllllllllllllllllll)*llllllllllllllllllllllllllllllllll;
                            } else {llllllllllllllllll = 0;}
                        }
                    } else {
                        if(d.worldSpacePosition.y < _FloorY) {
                            float llllllllllllllllllllllllllllllllllll = _FloorY - d.worldSpacePosition.y;
                            if(llllllllllllllllllllllllllllllllllll<0){llllllllllllllllllllllllllllllllllll=0;}
                            if(llllllllllllllllllllllllllllllllllll<_FloorYTextureGradientLength){llllllllllllllllll = (_FloorYTextureGradientLength-llllllllllllllllllllllllllllllllllll)*llllllllllllllllllllllllllllllllll;
                            } else {llllllllllllllllll = 0;}
                        }
                    }
                    if(!_TriggerMode && !_RaycastMode) {if(distance(_PlayersPosArray[i], d.worldSpacePosition) > _DefaultEffectRadius) {llllllllllllllllll = 0;}}
                }
                if(_TriggerMode || _RaycastMode) {llllllllllllllllll =  lllll * llllllllllllllllll;  
                } else {llllllllllllllllll = llllllllllllllllll;}
                llllllllllllllllllll = max(llllllllllllllllllll,llllllllllllllllll);
            }            
            float lllllllllllllllllllllllllllllllll = llllllllllllllllllll;
            if(!_PreviewMode) {
                if (lllllllllllllllllllllllllllllllll==1){lllllllllllllllllllllllllllllllll=10;}
                if (!_hasClippedShadows) {
                #if defined(UNITY_PASS_SHADOWCASTER)
                #if defined(SHADOWS_DEPTH)
                if (!any(unity_LightShadowBias)){clip(lllllllllllllllllll- lllllllllllllllllllllllllllllllll);}
                else{if(_hasClippedShadows) {clip(lllllllllllllllllll- lllllllllllllllllllllllllllllllll);}}
                #endif
                #else
                    clip(lllllllllllllllllll- lllllllllllllllllllllllllllllllll);
                #endif
                } else {clip(lllllllllllllllllll- lllllllllllllllllllllllllllllllll);}
            }       
            half4 c = lerp(1, tex2D(_MainTex, d.texcoord0.xy), _TextureVisibility) * _Color;        
            o.Albedo = clamp(_DissolveColor*lllllllllllllllllllllllllllllllll, 0, _DissolveColorSaturation) + c.rgb;
            if(_PreviewMode) {
                if((lllllllllllllllllll - lllllllllllllllllllllllllllllllll)< 0) {
                    o.Albedo = half4(1,1,1,1);
                } else {
                    o.Albedo = half4(0,0,0,1);
                }
            }
            if(_TexturedEmissionEdge) {
                _TexturedEmissionEdgeStrength = 0.2 + (_TexturedEmissionEdgeStrength*(0.8-0.2));
                o.Emission =  min( clamp(lerp(1,_DissolveColor,_DissolveColorSaturation)*lllllllllllllllllllllllllllllllll, 0, 1)*sqrt(_DissolveEmission*_DissolveEmissionBooster), clamp(lerp(1,_DissolveColor,_DissolveColorSaturation) *  clamp(((lllllllllllllllllllllllllllllllll/_TexturedEmissionEdgeStrength) - lllllllllllllllllll),0,1), 0, 1)*sqrt(_DissolveEmission*_DissolveEmissionBooster));

            } else {
                o.Emission =  clamp(lerp(1,_DissolveColor,_DissolveColorSaturation)*lllllllllllllllllllllllllllllllll, 0, 1)*sqrt(_DissolveEmission*_DissolveEmissionBooster);
            } 
            #if _HDRP
                o.Emission =  o.Emission * pow(_DissolveEmissionBooster,4);
            #endif

            #ifdef _NORMALMAP
                o.Normal = UnpackScaleNormal(tex2D(_BumpMap, d.texcoord0.xy), _BumpScale);
            #endif
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = 1;

        } else {
            half4 c = tex2D (_MainTex, d.texcoord0.xy) * _Color;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
            #ifdef _NORMALMAP
            o.Normal = UnpackScaleNormal(tex2D(_BumpMap, d.texcoord0.xy), _BumpScale);
            #endif
        }    
	}




        
            void ChainSurfaceFunction(inout Surface l, inout ShaderData d)
            {
                  Ext_SurfaceFunction0(l, d);
                 // Ext_SurfaceFunction1(l, d);
                 // Ext_SurfaceFunction2(l, d);
                 // Ext_SurfaceFunction3(l, d);
                 // Ext_SurfaceFunction4(l, d);
                 // Ext_SurfaceFunction5(l, d);
                 // Ext_SurfaceFunction6(l, d);
                 // Ext_SurfaceFunction7(l, d);
                 // Ext_SurfaceFunction8(l, d);
                 // Ext_SurfaceFunction9(l, d);
		           // Ext_SurfaceFunction10(l, d);
                 // Ext_SurfaceFunction11(l, d);
                 // Ext_SurfaceFunction12(l, d);
                 // Ext_SurfaceFunction13(l, d);
                 // Ext_SurfaceFunction14(l, d);
                 // Ext_SurfaceFunction15(l, d);
                 // Ext_SurfaceFunction16(l, d);
                 // Ext_SurfaceFunction17(l, d);
                 // Ext_SurfaceFunction18(l, d);
		           // Ext_SurfaceFunction19(l, d);
            }

            void ChainModifyVertex(inout VertexData v, inout VertexToPixel v2p)
            {
                 ExtraV2F d;
                 ZERO_INITIALIZE(ExtraV2F, d);
                 ZERO_INITIALIZE(Blackboard, d.blackboard);

                 //  Ext_ModifyVertex0(v, d);
                 // Ext_ModifyVertex1(v, d);
                 // Ext_ModifyVertex2(v, d);
                 // Ext_ModifyVertex3(v, d);
                 // Ext_ModifyVertex4(v, d);
                 // Ext_ModifyVertex5(v, d);
                 // Ext_ModifyVertex6(v, d);
                 // Ext_ModifyVertex7(v, d);
                 // Ext_ModifyVertex8(v, d);
                 // Ext_ModifyVertex9(v, d);
                 // Ext_ModifyVertex10(v, d);
                 // Ext_ModifyVertex11(v, d);
                 // Ext_ModifyVertex12(v, d);
                 // Ext_ModifyVertex13(v, d);
                 // Ext_ModifyVertex14(v, d);
                 // Ext_ModifyVertex15(v, d);
                 // Ext_ModifyVertex16(v, d);
                 // Ext_ModifyVertex17(v, d);
                 // Ext_ModifyVertex18(v, d);
                 // Ext_ModifyVertex19(v, d);


                 // #if %EXTRAV2F0REQUIREKEY%
                 // v2p.extraV2F0 = d.extraV2F0;
                 // #endif

                 // #if %EXTRAV2F1REQUIREKEY%
                 // v2p.extraV2F1 = d.extraV2F1;
                 // #endif

                 // #if %EXTRAV2F2REQUIREKEY%
                 // v2p.extraV2F2 = d.extraV2F2;
                 // #endif

                 // #if %EXTRAV2F3REQUIREKEY%
                 // v2p.extraV2F3 = d.extraV2F3;
                 // #endif

                 // #if %EXTRAV2F4REQUIREKEY%
                 // v2p.extraV2F4 = d.extraV2F4;
                 // #endif

                 // #if %EXTRAV2F5REQUIREKEY%
                 // v2p.extraV2F5 = d.extraV2F5;
                 // #endif

                 // #if %EXTRAV2F6REQUIREKEY%
                 // v2p.extraV2F6 = d.extraV2F6;
                 // #endif

                 // #if %EXTRAV2F7REQUIREKEY%
                 // v2p.extraV2F7 = d.extraV2F7;
                 // #endif
            }

            void ChainModifyTessellatedVertex(inout VertexData v, inout VertexToPixel v2p)
            {
               ExtraV2F d;
               ZERO_INITIALIZE(ExtraV2F, d);
               ZERO_INITIALIZE(Blackboard, d.blackboard);

               // #if %EXTRAV2F0REQUIREKEY%
               // d.extraV2F0 = v2p.extraV2F0;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // d.extraV2F1 = v2p.extraV2F1;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // d.extraV2F2 = v2p.extraV2F2;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // d.extraV2F3 = v2p.extraV2F3;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // d.extraV2F4 = v2p.extraV2F4;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // d.extraV2F5 = v2p.extraV2F5;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // d.extraV2F6 = v2p.extraV2F6;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // d.extraV2F7 = v2p.extraV2F7;
               // #endif


               // Ext_ModifyTessellatedVertex0(v, d);
               // Ext_ModifyTessellatedVertex1(v, d);
               // Ext_ModifyTessellatedVertex2(v, d);
               // Ext_ModifyTessellatedVertex3(v, d);
               // Ext_ModifyTessellatedVertex4(v, d);
               // Ext_ModifyTessellatedVertex5(v, d);
               // Ext_ModifyTessellatedVertex6(v, d);
               // Ext_ModifyTessellatedVertex7(v, d);
               // Ext_ModifyTessellatedVertex8(v, d);
               // Ext_ModifyTessellatedVertex9(v, d);
               // Ext_ModifyTessellatedVertex10(v, d);
               // Ext_ModifyTessellatedVertex11(v, d);
               // Ext_ModifyTessellatedVertex12(v, d);
               // Ext_ModifyTessellatedVertex13(v, d);
               // Ext_ModifyTessellatedVertex14(v, d);
               // Ext_ModifyTessellatedVertex15(v, d);
               // Ext_ModifyTessellatedVertex16(v, d);
               // Ext_ModifyTessellatedVertex17(v, d);
               // Ext_ModifyTessellatedVertex18(v, d);
               // Ext_ModifyTessellatedVertex19(v, d);

               // #if %EXTRAV2F0REQUIREKEY%
               // v2p.extraV2F0 = d.extraV2F0;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // v2p.extraV2F1 = d.extraV2F1;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // v2p.extraV2F2 = d.extraV2F2;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // v2p.extraV2F3 = d.extraV2F3;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // v2p.extraV2F4 = d.extraV2F4;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // v2p.extraV2F5 = d.extraV2F5;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // v2p.extraV2F6 = d.extraV2F6;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // v2p.extraV2F7 = d.extraV2F7;
               // #endif
            }

            void ChainFinalColorForward(inout Surface l, inout ShaderData d, inout half4 color)
            {
               //   Ext_FinalColorForward0(l, d, color);
               //   Ext_FinalColorForward1(l, d, color);
               //   Ext_FinalColorForward2(l, d, color);
               //   Ext_FinalColorForward3(l, d, color);
               //   Ext_FinalColorForward4(l, d, color);
               //   Ext_FinalColorForward5(l, d, color);
               //   Ext_FinalColorForward6(l, d, color);
               //   Ext_FinalColorForward7(l, d, color);
               //   Ext_FinalColorForward8(l, d, color);
               //   Ext_FinalColorForward9(l, d, color);
               //  Ext_FinalColorForward10(l, d, color);
               //  Ext_FinalColorForward11(l, d, color);
               //  Ext_FinalColorForward12(l, d, color);
               //  Ext_FinalColorForward13(l, d, color);
               //  Ext_FinalColorForward14(l, d, color);
               //  Ext_FinalColorForward15(l, d, color);
               //  Ext_FinalColorForward16(l, d, color);
               //  Ext_FinalColorForward17(l, d, color);
               //  Ext_FinalColorForward18(l, d, color);
               //  Ext_FinalColorForward19(l, d, color);
            }

            void ChainFinalGBufferStandard(inout Surface s, inout ShaderData d, inout half4 GBuffer0, inout half4 GBuffer1, inout half4 GBuffer2, inout half4 outEmission, inout half4 outShadowMask)
            {
               //   Ext_FinalGBufferStandard0(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard1(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard2(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard3(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard4(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard5(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard6(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard7(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard8(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard9(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard10(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard11(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard12(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard13(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard14(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard15(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard16(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard17(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard18(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard19(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
            }



            

         ShaderData CreateShaderData(VertexToPixel i
                  #if NEED_FACING
                     , bool facing
                  #endif
         )
         {
            ShaderData d = (ShaderData)0;
            d.clipPos = i.pos;
            d.worldSpacePosition = i.worldPos;

            d.worldSpaceNormal = normalize(i.worldNormal);
            d.worldSpaceTangent = normalize(i.worldTangent.xyz);
            d.tangentSign = i.worldTangent.w;
            float3 bitangent = cross(i.worldTangent.xyz, i.worldNormal) * d.tangentSign * -1;
            

            d.TBNMatrix = float3x3(d.worldSpaceTangent, bitangent, d.worldSpaceNormal);
            d.worldSpaceViewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

            d.tangentSpaceViewDir = mul(d.TBNMatrix, d.worldSpaceViewDir);
             d.texcoord0 = i.texcoord0;
            // d.texcoord1 = i.texcoord1;
            // d.texcoord2 = i.texcoord2;

            // #if %TEXCOORD3REQUIREKEY%
            // d.texcoord3 = i.texcoord3;
            // #endif

            // d.isFrontFace = facing;
            // #if %VERTEXCOLORREQUIREKEY%
            // d.vertexColor = i.vertexColor;
            // #endif

            // these rarely get used, so we back transform them. Usually will be stripped.
            #if _HDRP
                // d.localSpacePosition = mul(unity_WorldToObject, float4(GetCameraRelativePositionWS(i.worldPos), 1)).xyz;
            #else
                // d.localSpacePosition = mul(unity_WorldToObject, float4(i.worldPos, 1)).xyz;
            #endif
            // d.localSpaceNormal = normalize(mul((float3x3)unity_WorldToObject, i.worldNormal));
            // d.localSpaceTangent = normalize(mul((float3x3)unity_WorldToObject, i.worldTangent.xyz));

            // #if %SCREENPOSREQUIREKEY%
             d.screenPos = i.screenPos;
             d.screenUV = (i.screenPos.xy / i.screenPos.w);
            // #endif


            // #if %EXTRAV2F0REQUIREKEY%
            // d.extraV2F0 = i.extraV2F0;
            // #endif

            // #if %EXTRAV2F1REQUIREKEY%
            // d.extraV2F1 = i.extraV2F1;
            // #endif

            // #if %EXTRAV2F2REQUIREKEY%
            // d.extraV2F2 = i.extraV2F2;
            // #endif

            // #if %EXTRAV2F3REQUIREKEY%
            // d.extraV2F3 = i.extraV2F3;
            // #endif

            // #if %EXTRAV2F4REQUIREKEY%
            // d.extraV2F4 = i.extraV2F4;
            // #endif

            // #if %EXTRAV2F5REQUIREKEY%
            // d.extraV2F5 = i.extraV2F5;
            // #endif

            // #if %EXTRAV2F6REQUIREKEY%
            // d.extraV2F6 = i.extraV2F6;
            // #endif

            // #if %EXTRAV2F7REQUIREKEY%
            // d.extraV2F7 = i.extraV2F7;
            // #endif

            return d;
         }
         

            
         #if defined(SHADERPASS_SHADOWCASTER)
            float3 _LightDirection;
         #endif

         // vertex shader
         VertexToPixel Vert (VertexData v)
         {
           
           VertexToPixel o = (VertexToPixel)0;

           UNITY_SETUP_INSTANCE_ID(v);
           UNITY_TRANSFER_INSTANCE_ID(v, o);
           UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


#if !_TESSELLATION_ON
           ChainModifyVertex(v, o);
#endif

            o.texcoord0 = v.texcoord0;
           // o.texcoord1 = v.texcoord1;
           // o.texcoord2 = v.texcoord2;

           // #if %TEXCOORD3REQUIREKEY%
           // o.texcoord3 = v.texcoord3;
           // #endif

           // #if %VERTEXCOLORREQUIREKEY%
           // o.vertexColor = v.vertexColor;
           // #endif
           
           VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
           o.worldPos = TransformObjectToWorld(v.vertex.xyz);
           o.worldNormal = TransformObjectToWorldNormal(v.normal);
           o.worldTangent = float4(TransformObjectToWorldDir(v.tangent.xyz), v.tangent.w);


          #if defined(SHADERPASS_SHADOWCASTER)
              // Define shadow pass specific clip position for Universal
              o.pos = TransformWorldToHClip(ApplyShadowBias(o.worldPos, o.worldNormal, _LightDirection));
              #if UNITY_REVERSED_Z
                  o.pos.z = min(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
              #else
                  o.pos.z = max(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
              #endif
          #elif defined(SHADERPASS_META)
              o.pos = MetaVertexPosition(float4(v.vertex.xyz, 0), v.texcoord1, v.texcoord2, unity_LightmapST, unity_DynamicLightmapST);
          #else
              o.pos = TransformWorldToHClip(o.worldPos);
          #endif

          // #if %SCREENPOSREQUIREKEY%
           o.screenPos = ComputeScreenPos(o.pos, _ProjectionParams.x);
          // #endif

          #if defined(SHADERPASS_FORWARD)
              OUTPUT_LIGHTMAP_UV(v.texcoord1, unity_LightmapST, o.lightmapUV);
              OUTPUT_SH(o.worldNormal, o.sh);
          #endif

          #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
              half3 vertexLight = VertexLighting(o.worldPos, o.worldNormal);
              half fogFactor = ComputeFogFactor(o.pos.z);
              o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
          #endif

          #ifdef _MAIN_LIGHT_SHADOWS
              o.shadowCoord = GetShadowCoord(vertexInput);
          #endif

           return o;
         }


            

            // fragment shader
            half4 Frag (VertexToPixel IN
            #if NEED_FACING
               , bool facing : SV_IsFrontFace
            #endif
            ) : SV_Target
            {
               UNITY_SETUP_INSTANCE_ID(IN);

               ShaderData d = CreateShaderData(IN
                  #if NEED_FACING
                     , facing
                  #endif
               );

               Surface l = (Surface)0;

               l.Albedo = half3(0.5, 0.5, 0.5);
               l.Normal = float3(0,0,1);
               l.Occlusion = 1;
               l.Alpha = 1;

               ChainSurfaceFunction(l, d);

               MetaInput metaInput = (MetaInput)0;
               metaInput.Albedo = l.Albedo;
               metaInput.Emission = l.Emission;

               return MetaFragment(metaInput);

            }

         ENDHLSL

      }


      
        Pass
        {
            // Name: <None>
            Tags 
            { 
                "LightMode" = "Universal2D"
            }
           
            // Render State
            Blend One Zero, One Zero
            Cull Back
            ZTest LEqual
            ZWrite On
            // ColorMask: <None>

            

            HLSLPROGRAM

               #pragma vertex Vert
   #pragma fragment Frag

            #pragma target 3.0

            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma multi_compile_instancing
        
            #define SHADERPASS_2D

            
      #pragma shader_feature_local _NORMALMAP


   #define _URP 1


            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Version.hlsl"
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
            #include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"
        

            

                  #undef WorldNormalVector
      #define WorldNormalVector(data, normal) mul(normal, data.TBNMatrix)
      
      #define UnityObjectToWorldNormal(normal) mul(GetObjectToWorldMatrix(), normal)

      #define _WorldSpaceLightPos0 _MainLightPosition
      
      #define UNITY_DECLARE_TEX2D(name) TEXTURE2D(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2D_NOSAMPLER(name) TEXTURE2D(name);
      #define UNITY_DECLARE_TEX2DARRAY(name) TEXTURE2D_ARRAY(name); SAMPLER(sampler##name);
      #define UNITY_DECLARE_TEX2DARRAY_NOSAMPLER(name) TEXTURE2D_ARRAY(name);

      #define UNITY_SAMPLE_TEX2DARRAY(tex,coord)            SAMPLE_TEXTURE2D_ARRAY(tex, sampler##tex, coord.xy, coord.z)
      #define UNITY_SAMPLE_TEX2DARRAY_LOD(tex,coord,lod)    SAMPLE_TEXTURE2D_ARRAY_LOD(tex, sampler##tex, coord.xy, coord.z, lod)
      #define UNITY_SAMPLE_TEX2D(tex, coord)                SAMPLE_TEXTURE2D(tex, sampler##tex, coord)
      #define UNITY_SAMPLE_TEX2D_SAMPLER(tex, samp, coord)  SAMPLE_TEXTURE2D(tex, sampler##samp, coord)

      #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod)   SAMPLE_TEXTURE2D_LOD(tex, sampler_##tex, coord, lod)
      #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) SAMPLE_TEXTURE2D_LOD (tex, sampler##samplertex,coord, lod)
     
      #if defined(UNITY_COMPILER_HLSL)
         #define UNITY_INITIALIZE_OUTPUT(type,name) name = (type)0;
      #else
         #define UNITY_INITIALIZE_OUTPUT(type,name)
      #endif

      #define sampler2D_float sampler2D
      #define sampler2D_half sampler2D



      // data across stages, stripped like the above.
      struct VertexToPixel
      {
         float4 pos : SV_POSITION;
         float3 worldPos : TEXCOORD0;
         float3 worldNormal : TEXCOORD1;
         float4 worldTangent : TEXCOORD2;
          float4 texcoord0 : TEXCCOORD3;
         // float4 texcoord1 : TEXCCOORD4;
         // float4 texcoord2 : TEXCCOORD5;

         // #if %TEXCOORD3REQUIREKEY%
         // float4 texcoord3 : TEXCCOORD6;
         // #endif

         // #if %SCREENPOSREQUIREKEY%
          float4 screenPos : TEXCOORD7;
         // #endif

         // #if %VERTEXCOLORREQUIREKEY%
         // half4 vertexColor : COLOR;
         // #endif

         // #if %EXTRAV2F0REQUIREKEY%
         // float4 extraV2F0 : TEXCOORD12;
         // #endif

         // #if %EXTRAV2F1REQUIREKEY%
         // float4 extraV2F1 : TEXCOORD13;
         // #endif

         // #if %EXTRAV2F2REQUIREKEY%
         // float4 extraV2F2 : TEXCOORD14;
         // #endif

         // #if %EXTRAV2F3REQUIREKEY%
         // float4 extraV2F3 : TEXCOORD15;
         // #endif

         // #if %EXTRAV2F4REQUIREKEY%
         // float4 extraV2F4 : TEXCOORD16;
         // #endif

         // #if %EXTRAV2F5REQUIREKEY%
         // float4 extraV2F5 : TEXCOORD17;
         // #endif

         // #if %EXTRAV2F6REQUIREKEY%
         // float4 extraV2F6 : TEXCOORD18;
         // #endif

         // #if %EXTRAV2F7REQUIREKEY%
         // float4 extraV2F7 : TEXCOORD19;
         // #endif
            
         #if defined(LIGHTMAP_ON)
            float2 lightmapUV : TEXCOORD8;
         #endif
         #if !defined(LIGHTMAP_ON)
            float3 sh : TEXCOORD9;
         #endif
            float4 fogFactorAndVertexLight : TEXCOORD10;
            float4 shadowCoord : TEXCOORD11;
         #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
         #endif
         #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
         #endif
         #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
         #endif
         #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
         #endif
      };



            
            
            // data describing the user output of a pixel
            struct Surface
            {
               half3 Albedo;
               half Height;
               half3 Normal;
               half Smoothness;
               half3 Emission;
               half Metallic;
               half3 Specular;
               half Occlusion;
               half SpecularPower; // for simple lighting
               half Alpha;
               float outputDepth; // if written, SV_Depth semantic is used. ShaderData.clipPos.z is unused value
               // HDRP Only
               half SpecularOcclusion;
               half SubsurfaceMask;
               half Thickness;
               half CoatMask;
               half CoatSmoothness;
               half Anisotropy;
               half IridescenceMask;
               half IridescenceThickness;
               int DiffusionProfileHash;
               // requires _OVERRIDE_BAKEDGI to be defined, but is mapped in all pipelines
               float3 DiffuseGI;
               float3 BackDiffuseGI;
               float3 SpecularGI;
               // requires _OVERRIDE_SHADOWMASK to be defines
               float4 ShadowMask;
            };

            // Data the user declares in blackboard blocks
            struct Blackboard
            {
                
                float blackboardDummyData;
            };

            // data the user might need, this will grow to be big. But easy to strip
            struct ShaderData
            {
               float4 clipPos; // SV_POSITION
               float3 localSpacePosition;
               float3 localSpaceNormal;
               float3 localSpaceTangent;
        
               float3 worldSpacePosition;
               float3 worldSpaceNormal;
               float3 worldSpaceTangent;
               float tangentSign;

               float3 worldSpaceViewDir;
               float3 tangentSpaceViewDir;

               float4 texcoord0;
               float4 texcoord1;
               float4 texcoord2;
               float4 texcoord3;

               float2 screenUV;
               float4 screenPos;

               float4 vertexColor;
               bool isFrontFace;

               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;

               float3x3 TBNMatrix;
               Blackboard blackboard;
            };

            struct VertexData
            {
               #if SHADER_TARGET > 30
               // uint vertexID : SV_VertexID;
               #endif
               float4 vertex : POSITION;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;

               // would love to strip these, but they are used in certain
               // combinations of the lighting system, and may be used
               // by the user as well, so no easy way to strip them.

               float4 texcoord1 : TEXCOORD1;
               float4 texcoord2 : TEXCOORD2;

               // #if %TEXCOORD3REQUIREKEY%
               // float4 texcoord3 : TEXCOORD3;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               #if _HDRP && (_PASSMOTIONVECTOR || (_PASSFORWARD && defined(_WRITE_TRANSPARENT_MOTION_VECTOR)))
                  float3 previousPositionOS : TEXCOORD4; // Contain previous transform position (in case of skinning for example)
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity    : TEXCOORD5; // Add Precomputed Velocity (Alembic computes velocities on runtime side).
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct TessVertex 
            {
               float4 vertex : INTERNALTESSPOS;
               float3 normal : NORMAL;
               float4 tangent : TANGENT;
               float4 texcoord0 : TEXCOORD0;
               float4 texcoord1 : TEXCOORD1;
               float4 texcoord2 : TEXCOORD2;

               // #if %TEXCOORD3REQUIREKEY%
               // float4 texcoord3 : TEXCOORD3;
               // #endif

               // #if %VERTEXCOLORREQUIREKEY%
               // float4 vertexColor : COLOR;
               // #endif

               // #if %EXTRAV2F0REQUIREKEY%
               // float4 extraV2F0 : TEXCOORD5;
               // endif

               // #if %EXTRAV2F1REQUIREKEY%
               // float4 extraV2F1 : TEXCOORD6;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // float4 extraV2F2 : TEXCOORD7;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // float4 extraV2F3 : TEXCOORD8;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // float4 extraV2F4 : TEXCOORD9;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // float4 extraV2F5 : TEXCOORD10;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // float4 extraV2F6 : TEXCOORD11;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // float4 extraV2F7 : TEXCOORD12;
               // #endif

               #if _HDRP && (_PASSMOTIONVECTOR || (_PASSFORWARD && defined(_WRITE_TRANSPARENT_MOTION_VECTOR)))
                  float3 previousPositionOS : TEXCOORD13; // Contain previous transform position (in case of skinning for example)
                  #if defined (_ADD_PRECOMPUTED_VELOCITY)
                     float3 precomputedVelocity : TEXCOORD14;
                  #endif
               #endif

               UNITY_VERTEX_INPUT_INSTANCE_ID
               UNITY_VERTEX_OUTPUT_STEREO
            };

            struct ExtraV2F
            {
               float4 extraV2F0;
               float4 extraV2F1;
               float4 extraV2F2;
               float4 extraV2F3;
               float4 extraV2F4;
               float4 extraV2F5;
               float4 extraV2F6;
               float4 extraV2F7;
               Blackboard blackboard;
            };


            float3 WorldToTangentSpace(ShaderData d, float3 normal)
            {
               return mul(d.TBNMatrix, normal);
            }

            float3 TangentToWorldSpace(ShaderData d, float3 normal)
            {
               return mul(normal, d.TBNMatrix);
            }

            // in this case, make standard more like SRPs, because we can't fix
            // unity_WorldToObject in HDRP, since it already does macro-fu there

            #if _STANDARD
               float3 TransformWorldToObject(float3 p) { return mul(unity_WorldToObject, float4(p, 1)); };
               float3 TransformObjectToWorld(float3 p) { return mul(unity_ObjectToWorld, float4(p, 1)); };
               float4 TransformWorldToObject(float4 p) { return mul(unity_WorldToObject, p); };
               float4 TransformObjectToWorld(float4 p) { return mul(unity_ObjectToWorld, p); };
               float4x4 GetWorldToObjectMatrix() { return unity_WorldToObject; }
               float4x4 GetObjectToWorldMatrix() { return unity_ObjectToWorld; }
               #if (defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (SHADER_TARGET_SURFACE_ANALYSIS && !SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord, lod) tex.SampleLevel (sampler##tex,coord, lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord, lod) tex.SampleLevel (sampler##samplertex,coord, lod)
              #else
                 #define UNITY_SAMPLE_TEX2D_LOD(tex,coord,lod) tex2D (tex,coord,0,lod)
                 #define UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex,samplertex,coord,lod) tex2D (tex,coord,0,lod)
              #endif

               #undef GetObjectToWorldMatrix()
               #undef GetWorldToObjectMatrix()
               #undef GetWorldToViewMatrix()
               #undef UNITY_MATRIX_I_V
               #undef UNITY_MATRIX_P
               #undef GetWorldToHClipMatrix()
               #undef GetObjectToWorldMatrix()V
               #undef UNITY_MATRIX_T_MV
               #undef UNITY_MATRIX_IT_MV
               #undef GetObjectToWorldMatrix()VP

               #define GetObjectToWorldMatrix()     unity_ObjectToWorld
               #define GetWorldToObjectMatrix()   unity_WorldToObject
               #define GetWorldToViewMatrix()     unity_MatrixV
               #define UNITY_MATRIX_I_V   unity_MatrixInvV
               #define GetViewToHClipMatrix()     OptimizeProjectionMatrix(glstate_matrix_projection)
               #define GetWorldToHClipMatrix()    unity_MatrixVP
               #define GetObjectToWorldMatrix()V    mul(GetWorldToViewMatrix(), GetObjectToWorldMatrix())
               #define UNITY_MATRIX_T_MV  transpose(GetObjectToWorldMatrix()V)
               #define UNITY_MATRIX_IT_MV transpose(mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V))
               #define GetObjectToWorldMatrix()VP   mul(GetWorldToHClipMatrix(), GetObjectToWorldMatrix())


            #endif

            float3 GetCameraWorldPosition()
            {
               #if _HDRP
                  return GetCameraRelativePositionWS(_WorldSpaceCameraPos);
               #else
                  return _WorldSpaceCameraPos;
               #endif
            }

            #if _GRABPASSUSED
               #if _STANDARD
                  TEXTURE2D(%GRABTEXTURE%);
                  SAMPLER(sampler_%GRABTEXTURE%);
               #endif

               half3 GetSceneColor(float2 uv)
               {
                  #if _STANDARD
                     return SAMPLE_TEXTURE2D(%GRABTEXTURE%, sampler_%GRABTEXTURE%, uv).rgb;
                  #else
                     return SHADERGRAPH_SAMPLE_SCENE_COLOR(uv);
                  #endif
               }
            #endif


      
            #if _STANDARD
               UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
               float GetSceneDepth(float2 uv) { return SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv)); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv)); } 
            #else
               float GetSceneDepth(float2 uv) { return SHADERGRAPH_SAMPLE_SCENE_DEPTH(uv); }
               float GetLinear01Depth(float2 uv) { return Linear01Depth(GetSceneDepth(uv), _ZBufferParams); }
               float GetLinearEyeDepth(float2 uv) { return LinearEyeDepth(GetSceneDepth(uv), _ZBufferParams); } 
            #endif

            float3 GetWorldPositionFromDepthBuffer(float2 uv, float3 worldSpaceViewDir)
            {
               float eye = GetLinearEyeDepth(uv);
               float3 camView = mul((float3x3)GetObjectToWorldMatrix(), transpose(mul(GetWorldToObjectMatrix(), UNITY_MATRIX_I_V)) [2].xyz);

               float dt = dot(worldSpaceViewDir, camView);
               float3 div = worldSpaceViewDir/dt;
               float3 wpos = (eye * div) + GetCameraWorldPosition();
               return wpos;
            }

            #if _STANDARD
               UNITY_DECLARE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture);
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  float4 depthNorms = UNITY_SAMPLE_SCREENSPACE_TEXTURE(_CameraDepthNormalsTexture, uv);
                  float3 norms = DecodeViewNormalStereo(depthNorms);
                  norms = mul((float3x3)GetWorldToViewMatrix(), norms) * 0.5 + 0.5;
                  return norms;
               }
            #elif _HDRP
               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  NormalData nd;
                  DecodeFromNormalBuffer(_ScreenSize.xy * uv, nd);
                  return nd.normalWS;
               }
            #elif _URP
               #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                  #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareNormalsTexture.hlsl"
               #endif

               float3 GetSceneNormal(float2 uv, float3 worldSpaceViewDir)
               {
                  #if (SHADER_LIBRARY_VERSION_MAJOR >= 10)
                     return SampleSceneNormals(uv);
                  #else
                     float3 wpos = GetWorldPositionFromDepthBuffer(uv, worldSpaceViewDir);
                     return normalize(-cross(ddx(wpos), ddy(wpos))) * 0.5 + 0.5;
                  #endif

                }
             #endif

             #if _HDRP

               half3 UnpackNormalmapRGorAG(half4 packednormal)
               {
                     // This do the trick
                  packednormal.x *= packednormal.w;

                  half3 normal;
                  normal.xy = packednormal.xy * 2 - 1;
                  normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                  return normal;
               }
               half3 UnpackNormal(half4 packednormal)
               {
                  #if defined(UNITY_NO_DXT5nm)
                     return packednormal.xyz * 2 - 1;
                  #else
                     return UnpackNormalmapRGorAG(packednormal);
                  #endif
               }
               #endif
               #if _HDRP || _URP

               half3 UnpackScaleNormal(half4 packednormal, half scale)
               {
                 #ifndef UNITY_NO_DXT5nm
                   // Unpack normal as DXT5nm (1, y, 1, x) or BC5 (x, y, 0, 1)
                   // Note neutral texture like "bump" is (0, 0, 1, 1) to work with both plain RGB normal and DXT5nm/BC5
                   packednormal.x *= packednormal.w;
                 #endif
                   half3 normal;
                   normal.xy = (packednormal.xy * 2 - 1) * scale;
                   normal.z = sqrt(1 - saturate(dot(normal.xy, normal.xy)));
                   return normal;
               }	

             #endif


            void GetSun(out float3 lightDir, out float3 color)
            {
               lightDir = float3(0.5, 0.5, 0);
               color = 1;
               #if _HDRP
                  if (_DirectionalLightCount > 0)
                  {
                     DirectionalLightData light = _DirectionalLightDatas[0];
                     lightDir = -light.forward.xyz;
                     color = light.color;
                  }
               #elif _STANDARD
			         lightDir = normalize(_WorldSpaceLightPos0.xyz);
                  color = _LightColor0.rgb;
               #elif _URP
	               Light light = GetMainLight();
	               lightDir = light.direction;
	               color = light.color;
               #endif
            }


            
            CBUFFER_START(UnityPerMaterial)

               
	half4 _Color;
	half _Glossiness;
	half _Metallic;
	half _TextureVisibility;
	half _AngleStrength;
	float _Obstruction;
	float _Floor;
	float _UVs;
	float _BumpScale; 
	half4 _DissolveColor;
	float _DissolveColorSaturation;
	float _DissolveEmission;
	float _DissolveEmissionBooster;
	float _hasClippedShadows;
	float _ConeStrength;
	float _ConeObstructionDestroyRadius;
	float _CylinderStrength;
	float _CylinderObstructionDestroyRadius;
	float _CircleStrength;
	float _CircleObstructionDestroyRadius;
	float _IntrinsicDissolveStrength;
	float _DissolveFallOff;
	float _PreviewMode;
	float _AnimationEnabled;
	float _AnimationSpeed;
	float _TriggerMode;
	float _RaycastMode;
	float _IsExempt;
	float _DefaultEffectRadius;
	float _FloorY;
	float _FloorYTextureGradientLength;
	float4 _PlayerPos;
	float _PlayerPosYOffset;
	int _ArrayLength = 0;
	float4 _PlayersPosArray[100];
	float4 _PlayersDataArray[100];        
	float _TransitionDuration;
    float _tDirection = 0;
    float _numOfPlayersInside = 0;
    float _tValue = 0;
    float _id = 0;
    float _TexturedEmissionEdge;
    float _TexturedEmissionEdgeStrength;
	float _IsReplacementShader;
	half4 _DissolveColorGlobal;
	float _DissolveColorSaturationGlobal;
	float _DissolveEmissionGlobal;
	float _DissolveEmissionBoosterGlobal;
	float _TextureVisibilityGlobal;
	float _ObstructionGlobal;
	float _AngleStrengthGlobal;
	float _ConeStrengthGlobal;
	float _ConeObstructionDestroyRadiusGlobal;
	float _CylinderStrengthGlobal;
	float _CylinderObstructionDestroyRadiusGlobal;
	float _CircleStrengthGlobal;
	float _CircleObstructionDestroyRadiusGlobal;
	float _DissolveFallOffGlobal;
	float _IntrinsicDissolveStrengthGlobal;
	float _PreviewModeGlobal;
	float _UVsGlobal;
	float _hasClippedShadowsGlobal;
	float _FloorGlobal;
	float _FloorYGlobal;
	float _PlayerPosYOffsetGlobal;
	float _FloorYTextureGradientLengthGlobal;
	float _DefaultEffectRadiusGlobal;
	float _AnimationEnabledGlobal;
	float _AnimationSpeedGlobal;
	float _TransitionDurationGlobal;
    float _TexturedEmissionEdgeGlobal;
    float _TexturedEmissionEdgeStrengthGlobal;
    float _isReferenceMaterial;



            CBUFFER_END

            

            

            #ifdef unity_WorldToObject
#undef unity_WorldToObject
#endif
#ifdef unity_ObjectToWorld
#undef unity_ObjectToWorld
#endif
#define unity_ObjectToWorld GetObjectToWorldMatrix()
#define unity_WorldToObject GetWorldToObjectMatrix()

    float lllllllllllllllllllllllllllllllllllll(float fallOff, float strength, float input)
    {
        float k = fallOff;
        k = max(k,0.00001);
        float n = 1-strength;
        float b = exp(k*6);
        float j = input;
        float v = n/(k/(k*n-0.15*(k-n)));
        float y = ((j-v)/(b*(1-j)+j))+v;
        y = 1-y;
        return y * sign(strength);
    }
	sampler2D _MainTex;
    #ifdef _NORMALMAP
        sampler2D _BumpMap;
    #endif
	sampler2D _DissolveTex;
	sampler2D _DissolveTexGlobal;

	void Ext_SurfaceFunction0 (inout Surface o, ShaderData d)
	{
        bool lllllllllllllllllllllllllllllllllllllll =  (_numOfPlayersInside > 0 || _tDirection == -1 && _Time.y - _tValue < _TransitionDuration ) || (_numOfPlayersInside >= 0 && _tDirection == 1); 
        bool llllllllllllllllllllllllllllllllllllllll = !_TriggerMode && !_RaycastMode;
        if(!_IsExempt && (lllllllllllllllllllllllllllllllllllllll || llllllllllllllllllllllllllllllllllllllll) ) {
            if(_IsReplacementShader) {    
                _DissolveColor = _DissolveColorGlobal;
                _DissolveColorSaturation = _DissolveColorSaturationGlobal;
                _DissolveEmission = _DissolveEmissionGlobal;
                _DissolveEmissionBooster = _DissolveEmissionBoosterGlobal;
                _TextureVisibility = _TextureVisibilityGlobal;
                _Obstruction = _ObstructionGlobal;
                _AngleStrength = _AngleStrengthGlobal;
                _ConeStrength = _ConeStrengthGlobal;
                _ConeObstructionDestroyRadius = _ConeObstructionDestroyRadiusGlobal;
                _CylinderStrength = _CylinderStrengthGlobal;
                _CylinderObstructionDestroyRadius = _CylinderObstructionDestroyRadiusGlobal;
                _CircleStrength = _CircleStrengthGlobal;
                _CircleObstructionDestroyRadius = _CircleObstructionDestroyRadiusGlobal;
                _DissolveFallOff = _DissolveFallOffGlobal;
                _IntrinsicDissolveStrength = _IntrinsicDissolveStrengthGlobal;
                _PreviewMode = _PreviewModeGlobal;
                _UVs = _UVsGlobal;
                _hasClippedShadows = _hasClippedShadowsGlobal;
                _Floor = _FloorGlobal;
                _FloorY = _FloorYGlobal;
                _PlayerPosYOffset = _PlayerPosYOffsetGlobal;
                _FloorYTextureGradientLength = _FloorYTextureGradientLengthGlobal; 
                _AnimationEnabled = _AnimationEnabledGlobal;
                _AnimationSpeed = _AnimationSpeedGlobal;
                _TransitionDuration = _TransitionDurationGlobal;
                _DefaultEffectRadius = _DefaultEffectRadiusGlobal;
                _TexturedEmissionEdge = _TexturedEmissionEdgeGlobal;
                _TexturedEmissionEdgeStrength = _TexturedEmissionEdgeStrengthGlobal;
            }
            if(_IntrinsicDissolveStrength < 0) {_IntrinsicDissolveStrength = 0;}
            float3 l;
            d.worldSpaceNormal = mul(o.Normal, (float3x3)d.TBNMatrix);
            float3 ll = d.worldSpacePosition / (-1.0 * abs(_UVs) );
            if(_AnimationEnabled) {ll = ll + abs(((_Time.y) * _AnimationSpeed));}       
            if(_IsReplacementShader) {
                l = lerp(lerp(tex2D(_DissolveTexGlobal,ll.xz).rgb,tex2D(_DissolveTexGlobal,ll.yz ).rgb,abs(d.worldSpaceNormal.x)).rgb,tex2D(_DissolveTexGlobal,ll.xy).rgb,abs(d.worldSpaceNormal.z)).rgb;
            } else {
                l = lerp(lerp(tex2D(_DissolveTex,ll.xz).rgb,tex2D(_DissolveTex,ll.yz ).rgb,abs(d.worldSpaceNormal.x)).rgb,tex2D(_DissolveTex,ll.xy).rgb,abs(d.worldSpaceNormal.z)).rgb;
            }
            half lllllllllllllllllll = l;           
            float llllllllllllllllllll = 0;
            for (int i = 0; i < _ArrayLength; i++){
                float lll = _PlayersDataArray[i][1];
                float llll = _PlayersDataArray[i][2];
                float lllll = 1;
                if( llll!= 0 && lll != 0 && _Time.y-lll < _TransitionDuration) {
                    if(llll == 1) {lllll = ((_TransitionDuration-(_Time.y-lll))/_TransitionDuration);
                    } else {lllll = ((_Time.y-lll)/_TransitionDuration);}
                } else if(llll ==-1) {lllll = 1;
                } else if(llll == 1) {lllll = 0;
                } else {lllll = 1;}
                lllll = 1 - lllll;   
                float llllllllllllllllllllllllllllllllllllll = _PlayersDataArray[i][3];
                lllll = lllll * float(llllllllllllllllllllllllllllllllllllll == _id);
                float llllll = distance(_WorldSpaceCameraPos, _PlayersPosArray[i]);
                float lllllll = distance(_WorldSpaceCameraPos, d.worldSpacePosition);
                float3 llllllll = _WorldSpaceCameraPos - _PlayersPosArray[i];
                float lllllllll = length(llllllll);
                float llllllllll = _ConeObstructionDestroyRadius;
                float3 lllllllllll = normalize(llllllll);
                float llllllllllll = dot(d.worldSpacePosition - _PlayersPosArray[i], lllllllllll);
                float lllllllllllll = (llllllllllll/lllllllll)*llllllllll;
                float llllllllllllll = length((d.worldSpacePosition - _PlayersPosArray[i])-llllllllllll*lllllllllll);
                float lllllllllllllll = llllllllllllll<lllllllllllll;
                float llllllllllllllll = _CylinderObstructionDestroyRadius;
                float lllllllllllllllll = (llllllllllllll<llllllllllllllll)&&llllllllllll>0;
                float llllllllllllllllll = 0;
                float3 lllllllllllllllllllll =  d.worldSpacePosition - _PlayersPosArray[i];
                float3 llllllllllllllllllllll =  d.worldSpaceNormal;
                float lllllllllllllllllllllll = acos(dot(lllllllllllllllllllll,llllllllllllllllllllll)/(length(lllllllllllllllllllll)*length(llllllllllllllllllllll)));        
                float llllllllllllllllllllllll = _ScreenParams.x / _ScreenParams.y;
		        #if _HDRP
                    float4 lllllllllllllllllllllllll = mul(GetWorldToHClipMatrix(), float4(GetCameraRelativePositionWS(_PlayersPosArray[i].xyz), 1.0));
                    float4 llllllllllllllllllllllllll = ComputeScreenPos(lllllllllllllllllllllllll , _ProjectionParams.x);
		        #else
			        float4 lllllllllllllllllllllllll = mul(GetWorldToHClipMatrix(), float4(_PlayersPosArray[i].xyz, 1.0));
                    float4 llllllllllllllllllllllllll = ComputeScreenPos(lllllllllllllllllllllllll);
		        #endif
                llllllllllllllllllllllllll.xy /= llllllllllllllllllllllllll.w;
                llllllllllllllllllllllllll.x *= llllllllllllllllllllllll;
                #if _HDRP
                    //float4 lllllllllllllllllllllllllll = mul(GetWorldToHClipMatrix(), float4(GetCameraRelativePositionWS(d.worldSpacePosition), 1.0));
                    //half4 llllllllllllllllllllllllllll = ComputeScreenPos(lllllllllllllllllllllllllll , _ProjectionParams.x);
                    half4 llllllllllllllllllllllllllll = d.screenPos;
		        #else
                    float4 lllllllllllllllllllllllllll = mul(GetWorldToHClipMatrix(), float4(d.worldSpacePosition.xyz, 1.0));
                    float4 llllllllllllllllllllllllllll = ComputeScreenPos(lllllllllllllllllllllllllll);
		        #endif
                llllllllllllllllllllllllllll.xy /= llllllllllllllllllllllllllll.w;
                llllllllllllllllllllllllllll.x *= llllllllllllllllllllllll;
                float lllllllllllllllllllllllllllll = min(1,llllllllllllllllllllllll);
                float llllllllllllllllllllllllllllll =  distance(llllllllllllllllllllllllllll.xy,llllllllllllllllllllllllll.xy) < _CircleObstructionDestroyRadius/lllllllll*lllllllllllllllllllllllllllll;
                float lllllllllllllllllllllllllllllll = (distance(llllllllllllllllllllllllllll.xy,llllllllllllllllllllllllll.xy)/(_CircleObstructionDestroyRadius/lllllllll*lllllllllllllllllllllllllllll));
                float llllllllllllllllllllllllllllllll = (llllllllllllllllllllllllllllll)&&llllllllllll>0;        
                float lllllllllllllllllllllllllllllllll = llllllllllllllllll;
                if(lllll != 0 || (!_TriggerMode && !_RaycastMode)) {
                    if (_Obstruction == 1) {
                        if(lllllllllllllllllllllll<=1.5&&llllll>lllllll){
                            llllllllllllllllll = (sqrt((llllll-lllllll))*25/lllllllllllllllllllllll) *_AngleStrength;  
                            llllllllllllllllll = max(0,log(llllllllllllllllll*0.2));
                        }
                    }  else if (_Obstruction == 2) {
                        if(lllllllllllllll){
                            float lllllllllllllllllllllllllllllllllll = llllllllllllll/lllllllllllll;
                            llllllllllllllllll = lllllllllllllllllllllllllllllllllllll(_DissolveFallOff, _ConeStrength, lllllllllllllllllllllllllllllllllll);
                        }
                    } else  if (_Obstruction == 3) {
                        if(lllllllllllllllllllllll<= 1.5 && llllll > lllllll || lllllllllllllll){
                            if(lllllllllllllllllllllll<= 1.5 && llllll > lllllll) {
                                llllllllllllllllll = (sqrt((llllll-lllllll))*25/lllllllllllllllllllllll)*_AngleStrength;                   
                                llllllllllllllllll = max(0,log(llllllllllllllllll*0.2));
                            }
                            if (lllllllllllllll) {
                                float lllllllllllllllllllllllllllllllllll = llllllllllllll/lllllllllllll;
                                llllllllllllllllll = lllllllllllllllllllllllllllllllllllll(_DissolveFallOff, _ConeStrength, lllllllllllllllllllllllllllllllllll)+llllllllllllllllll;
                            }
                        }
                    }  else if (_Obstruction == 4) {
                        if(lllllllllllllllll){
                            float lllllllllllllllllllllllllllllllllll = llllllllllllll/llllllllllllllll;
                            llllllllllllllllll = lllllllllllllllllllllllllllllllllllll(_DissolveFallOff, _CylinderStrength, lllllllllllllllllllllllllllllllllll);
                        }
                    }  else if (_Obstruction == 5) {
                        if(lllllllllllllllllllllll<=1.5&&llllll>lllllll||lllllllllllllllll){
                            if(lllllllllllllllllllllll<=1.5&&llllll>lllllll) {
                                llllllllllllllllll = (sqrt((llllll-lllllll))*25/lllllllllllllllllllllll)*_AngleStrength;
                                llllllllllllllllll = max(0,log(llllllllllllllllll*0.2));              
                            }
                            if(lllllllllllllllll){
                                float lllllllllllllllllllllllllllllllllll = llllllllllllll/llllllllllllllll;
                                llllllllllllllllll = lllllllllllllllllllllllllllllllllllll(_DissolveFallOff, _CylinderStrength, lllllllllllllllllllllllllllllllllll) + llllllllllllllllll;                        
                            }                     
                        }
                    } else if (_Obstruction == 6) {
                        if (llllllllllllllllllllllllllllllll) {
                            llllllllllllllllll = lllllllllllllllllllllllllllllllllllll(_DissolveFallOff, _CircleStrength, lllllllllllllllllllllllllllllll);                 
                        }
                    }
                    llllllllllllllllll = llllllllllllllllll+(1*_IntrinsicDissolveStrength);
                    float llllllllllllllllllllllllllllllllll = llllllllllllllllll/_FloorYTextureGradientLength;
                    if(_Floor == 1) {
        	            if(d.worldSpacePosition.y < (_PlayersPosArray[i].y+_PlayerPosYOffset)) {
                            float llllllllllllllllllllllllllllllllllll = (_PlayersPosArray[i].y+_PlayerPosYOffset) - d.worldSpacePosition.y;
                            if(llllllllllllllllllllllllllllllllllll < 0) {llllllllllllllllllllllllllllllllllll = 0;}
                            if(llllllllllllllllllllllllllllllllllll < _FloorYTextureGradientLength) {
                                llllllllllllllllll = (_FloorYTextureGradientLength-llllllllllllllllllllllllllllllllllll)*llllllllllllllllllllllllllllllllll;
                            } else {llllllllllllllllll = 0;}
                        }
                    } else {
                        if(d.worldSpacePosition.y < _FloorY) {
                            float llllllllllllllllllllllllllllllllllll = _FloorY - d.worldSpacePosition.y;
                            if(llllllllllllllllllllllllllllllllllll<0){llllllllllllllllllllllllllllllllllll=0;}
                            if(llllllllllllllllllllllllllllllllllll<_FloorYTextureGradientLength){llllllllllllllllll = (_FloorYTextureGradientLength-llllllllllllllllllllllllllllllllllll)*llllllllllllllllllllllllllllllllll;
                            } else {llllllllllllllllll = 0;}
                        }
                    }
                    if(!_TriggerMode && !_RaycastMode) {if(distance(_PlayersPosArray[i], d.worldSpacePosition) > _DefaultEffectRadius) {llllllllllllllllll = 0;}}
                }
                if(_TriggerMode || _RaycastMode) {llllllllllllllllll =  lllll * llllllllllllllllll;  
                } else {llllllllllllllllll = llllllllllllllllll;}
                llllllllllllllllllll = max(llllllllllllllllllll,llllllllllllllllll);
            }            
            float lllllllllllllllllllllllllllllllll = llllllllllllllllllll;
            if(!_PreviewMode) {
                if (lllllllllllllllllllllllllllllllll==1){lllllllllllllllllllllllllllllllll=10;}
                if (!_hasClippedShadows) {
                #if defined(UNITY_PASS_SHADOWCASTER)
                #if defined(SHADOWS_DEPTH)
                if (!any(unity_LightShadowBias)){clip(lllllllllllllllllll- lllllllllllllllllllllllllllllllll);}
                else{if(_hasClippedShadows) {clip(lllllllllllllllllll- lllllllllllllllllllllllllllllllll);}}
                #endif
                #else
                    clip(lllllllllllllllllll- lllllllllllllllllllllllllllllllll);
                #endif
                } else {clip(lllllllllllllllllll- lllllllllllllllllllllllllllllllll);}
            }       
            half4 c = lerp(1, tex2D(_MainTex, d.texcoord0.xy), _TextureVisibility) * _Color;        
            o.Albedo = clamp(_DissolveColor*lllllllllllllllllllllllllllllllll, 0, _DissolveColorSaturation) + c.rgb;
            if(_PreviewMode) {
                if((lllllllllllllllllll - lllllllllllllllllllllllllllllllll)< 0) {
                    o.Albedo = half4(1,1,1,1);
                } else {
                    o.Albedo = half4(0,0,0,1);
                }
            }
            if(_TexturedEmissionEdge) {
                _TexturedEmissionEdgeStrength = 0.2 + (_TexturedEmissionEdgeStrength*(0.8-0.2));
                o.Emission =  min( clamp(lerp(1,_DissolveColor,_DissolveColorSaturation)*lllllllllllllllllllllllllllllllll, 0, 1)*sqrt(_DissolveEmission*_DissolveEmissionBooster), clamp(lerp(1,_DissolveColor,_DissolveColorSaturation) *  clamp(((lllllllllllllllllllllllllllllllll/_TexturedEmissionEdgeStrength) - lllllllllllllllllll),0,1), 0, 1)*sqrt(_DissolveEmission*_DissolveEmissionBooster));

            } else {
                o.Emission =  clamp(lerp(1,_DissolveColor,_DissolveColorSaturation)*lllllllllllllllllllllllllllllllll, 0, 1)*sqrt(_DissolveEmission*_DissolveEmissionBooster);
            } 
            #if _HDRP
                o.Emission =  o.Emission * pow(_DissolveEmissionBooster,4);
            #endif

            #ifdef _NORMALMAP
                o.Normal = UnpackScaleNormal(tex2D(_BumpMap, d.texcoord0.xy), _BumpScale);
            #endif
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = 1;

        } else {
            half4 c = tex2D (_MainTex, d.texcoord0.xy) * _Color;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
            #ifdef _NORMALMAP
            o.Normal = UnpackScaleNormal(tex2D(_BumpMap, d.texcoord0.xy), _BumpScale);
            #endif
        }    
	}




        
            void ChainSurfaceFunction(inout Surface l, inout ShaderData d)
            {
                  Ext_SurfaceFunction0(l, d);
                 // Ext_SurfaceFunction1(l, d);
                 // Ext_SurfaceFunction2(l, d);
                 // Ext_SurfaceFunction3(l, d);
                 // Ext_SurfaceFunction4(l, d);
                 // Ext_SurfaceFunction5(l, d);
                 // Ext_SurfaceFunction6(l, d);
                 // Ext_SurfaceFunction7(l, d);
                 // Ext_SurfaceFunction8(l, d);
                 // Ext_SurfaceFunction9(l, d);
		           // Ext_SurfaceFunction10(l, d);
                 // Ext_SurfaceFunction11(l, d);
                 // Ext_SurfaceFunction12(l, d);
                 // Ext_SurfaceFunction13(l, d);
                 // Ext_SurfaceFunction14(l, d);
                 // Ext_SurfaceFunction15(l, d);
                 // Ext_SurfaceFunction16(l, d);
                 // Ext_SurfaceFunction17(l, d);
                 // Ext_SurfaceFunction18(l, d);
		           // Ext_SurfaceFunction19(l, d);
            }

            void ChainModifyVertex(inout VertexData v, inout VertexToPixel v2p)
            {
                 ExtraV2F d;
                 ZERO_INITIALIZE(ExtraV2F, d);
                 ZERO_INITIALIZE(Blackboard, d.blackboard);

                 //  Ext_ModifyVertex0(v, d);
                 // Ext_ModifyVertex1(v, d);
                 // Ext_ModifyVertex2(v, d);
                 // Ext_ModifyVertex3(v, d);
                 // Ext_ModifyVertex4(v, d);
                 // Ext_ModifyVertex5(v, d);
                 // Ext_ModifyVertex6(v, d);
                 // Ext_ModifyVertex7(v, d);
                 // Ext_ModifyVertex8(v, d);
                 // Ext_ModifyVertex9(v, d);
                 // Ext_ModifyVertex10(v, d);
                 // Ext_ModifyVertex11(v, d);
                 // Ext_ModifyVertex12(v, d);
                 // Ext_ModifyVertex13(v, d);
                 // Ext_ModifyVertex14(v, d);
                 // Ext_ModifyVertex15(v, d);
                 // Ext_ModifyVertex16(v, d);
                 // Ext_ModifyVertex17(v, d);
                 // Ext_ModifyVertex18(v, d);
                 // Ext_ModifyVertex19(v, d);


                 // #if %EXTRAV2F0REQUIREKEY%
                 // v2p.extraV2F0 = d.extraV2F0;
                 // #endif

                 // #if %EXTRAV2F1REQUIREKEY%
                 // v2p.extraV2F1 = d.extraV2F1;
                 // #endif

                 // #if %EXTRAV2F2REQUIREKEY%
                 // v2p.extraV2F2 = d.extraV2F2;
                 // #endif

                 // #if %EXTRAV2F3REQUIREKEY%
                 // v2p.extraV2F3 = d.extraV2F3;
                 // #endif

                 // #if %EXTRAV2F4REQUIREKEY%
                 // v2p.extraV2F4 = d.extraV2F4;
                 // #endif

                 // #if %EXTRAV2F5REQUIREKEY%
                 // v2p.extraV2F5 = d.extraV2F5;
                 // #endif

                 // #if %EXTRAV2F6REQUIREKEY%
                 // v2p.extraV2F6 = d.extraV2F6;
                 // #endif

                 // #if %EXTRAV2F7REQUIREKEY%
                 // v2p.extraV2F7 = d.extraV2F7;
                 // #endif
            }

            void ChainModifyTessellatedVertex(inout VertexData v, inout VertexToPixel v2p)
            {
               ExtraV2F d;
               ZERO_INITIALIZE(ExtraV2F, d);
               ZERO_INITIALIZE(Blackboard, d.blackboard);

               // #if %EXTRAV2F0REQUIREKEY%
               // d.extraV2F0 = v2p.extraV2F0;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // d.extraV2F1 = v2p.extraV2F1;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // d.extraV2F2 = v2p.extraV2F2;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // d.extraV2F3 = v2p.extraV2F3;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // d.extraV2F4 = v2p.extraV2F4;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // d.extraV2F5 = v2p.extraV2F5;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // d.extraV2F6 = v2p.extraV2F6;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // d.extraV2F7 = v2p.extraV2F7;
               // #endif


               // Ext_ModifyTessellatedVertex0(v, d);
               // Ext_ModifyTessellatedVertex1(v, d);
               // Ext_ModifyTessellatedVertex2(v, d);
               // Ext_ModifyTessellatedVertex3(v, d);
               // Ext_ModifyTessellatedVertex4(v, d);
               // Ext_ModifyTessellatedVertex5(v, d);
               // Ext_ModifyTessellatedVertex6(v, d);
               // Ext_ModifyTessellatedVertex7(v, d);
               // Ext_ModifyTessellatedVertex8(v, d);
               // Ext_ModifyTessellatedVertex9(v, d);
               // Ext_ModifyTessellatedVertex10(v, d);
               // Ext_ModifyTessellatedVertex11(v, d);
               // Ext_ModifyTessellatedVertex12(v, d);
               // Ext_ModifyTessellatedVertex13(v, d);
               // Ext_ModifyTessellatedVertex14(v, d);
               // Ext_ModifyTessellatedVertex15(v, d);
               // Ext_ModifyTessellatedVertex16(v, d);
               // Ext_ModifyTessellatedVertex17(v, d);
               // Ext_ModifyTessellatedVertex18(v, d);
               // Ext_ModifyTessellatedVertex19(v, d);

               // #if %EXTRAV2F0REQUIREKEY%
               // v2p.extraV2F0 = d.extraV2F0;
               // #endif

               // #if %EXTRAV2F1REQUIREKEY%
               // v2p.extraV2F1 = d.extraV2F1;
               // #endif

               // #if %EXTRAV2F2REQUIREKEY%
               // v2p.extraV2F2 = d.extraV2F2;
               // #endif

               // #if %EXTRAV2F3REQUIREKEY%
               // v2p.extraV2F3 = d.extraV2F3;
               // #endif

               // #if %EXTRAV2F4REQUIREKEY%
               // v2p.extraV2F4 = d.extraV2F4;
               // #endif

               // #if %EXTRAV2F5REQUIREKEY%
               // v2p.extraV2F5 = d.extraV2F5;
               // #endif

               // #if %EXTRAV2F6REQUIREKEY%
               // v2p.extraV2F6 = d.extraV2F6;
               // #endif

               // #if %EXTRAV2F7REQUIREKEY%
               // v2p.extraV2F7 = d.extraV2F7;
               // #endif
            }

            void ChainFinalColorForward(inout Surface l, inout ShaderData d, inout half4 color)
            {
               //   Ext_FinalColorForward0(l, d, color);
               //   Ext_FinalColorForward1(l, d, color);
               //   Ext_FinalColorForward2(l, d, color);
               //   Ext_FinalColorForward3(l, d, color);
               //   Ext_FinalColorForward4(l, d, color);
               //   Ext_FinalColorForward5(l, d, color);
               //   Ext_FinalColorForward6(l, d, color);
               //   Ext_FinalColorForward7(l, d, color);
               //   Ext_FinalColorForward8(l, d, color);
               //   Ext_FinalColorForward9(l, d, color);
               //  Ext_FinalColorForward10(l, d, color);
               //  Ext_FinalColorForward11(l, d, color);
               //  Ext_FinalColorForward12(l, d, color);
               //  Ext_FinalColorForward13(l, d, color);
               //  Ext_FinalColorForward14(l, d, color);
               //  Ext_FinalColorForward15(l, d, color);
               //  Ext_FinalColorForward16(l, d, color);
               //  Ext_FinalColorForward17(l, d, color);
               //  Ext_FinalColorForward18(l, d, color);
               //  Ext_FinalColorForward19(l, d, color);
            }

            void ChainFinalGBufferStandard(inout Surface s, inout ShaderData d, inout half4 GBuffer0, inout half4 GBuffer1, inout half4 GBuffer2, inout half4 outEmission, inout half4 outShadowMask)
            {
               //   Ext_FinalGBufferStandard0(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard1(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard2(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard3(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard4(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard5(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard6(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard7(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard8(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //   Ext_FinalGBufferStandard9(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard10(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard11(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard12(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard13(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard14(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard15(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard16(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard17(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard18(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
               //  Ext_FinalGBufferStandard19(s, d, GBuffer0, GBuffer1, GBuffer2, outEmission, outShadowMask);
            }



            

         ShaderData CreateShaderData(VertexToPixel i
                  #if NEED_FACING
                     , bool facing
                  #endif
         )
         {
            ShaderData d = (ShaderData)0;
            d.clipPos = i.pos;
            d.worldSpacePosition = i.worldPos;

            d.worldSpaceNormal = normalize(i.worldNormal);
            d.worldSpaceTangent = normalize(i.worldTangent.xyz);
            d.tangentSign = i.worldTangent.w;
            float3 bitangent = cross(i.worldTangent.xyz, i.worldNormal) * d.tangentSign * -1;
            

            d.TBNMatrix = float3x3(d.worldSpaceTangent, bitangent, d.worldSpaceNormal);
            d.worldSpaceViewDir = normalize(_WorldSpaceCameraPos - i.worldPos);

            d.tangentSpaceViewDir = mul(d.TBNMatrix, d.worldSpaceViewDir);
             d.texcoord0 = i.texcoord0;
            // d.texcoord1 = i.texcoord1;
            // d.texcoord2 = i.texcoord2;

            // #if %TEXCOORD3REQUIREKEY%
            // d.texcoord3 = i.texcoord3;
            // #endif

            // d.isFrontFace = facing;
            // #if %VERTEXCOLORREQUIREKEY%
            // d.vertexColor = i.vertexColor;
            // #endif

            // these rarely get used, so we back transform them. Usually will be stripped.
            #if _HDRP
                // d.localSpacePosition = mul(unity_WorldToObject, float4(GetCameraRelativePositionWS(i.worldPos), 1)).xyz;
            #else
                // d.localSpacePosition = mul(unity_WorldToObject, float4(i.worldPos, 1)).xyz;
            #endif
            // d.localSpaceNormal = normalize(mul((float3x3)unity_WorldToObject, i.worldNormal));
            // d.localSpaceTangent = normalize(mul((float3x3)unity_WorldToObject, i.worldTangent.xyz));

            // #if %SCREENPOSREQUIREKEY%
             d.screenPos = i.screenPos;
             d.screenUV = (i.screenPos.xy / i.screenPos.w);
            // #endif


            // #if %EXTRAV2F0REQUIREKEY%
            // d.extraV2F0 = i.extraV2F0;
            // #endif

            // #if %EXTRAV2F1REQUIREKEY%
            // d.extraV2F1 = i.extraV2F1;
            // #endif

            // #if %EXTRAV2F2REQUIREKEY%
            // d.extraV2F2 = i.extraV2F2;
            // #endif

            // #if %EXTRAV2F3REQUIREKEY%
            // d.extraV2F3 = i.extraV2F3;
            // #endif

            // #if %EXTRAV2F4REQUIREKEY%
            // d.extraV2F4 = i.extraV2F4;
            // #endif

            // #if %EXTRAV2F5REQUIREKEY%
            // d.extraV2F5 = i.extraV2F5;
            // #endif

            // #if %EXTRAV2F6REQUIREKEY%
            // d.extraV2F6 = i.extraV2F6;
            // #endif

            // #if %EXTRAV2F7REQUIREKEY%
            // d.extraV2F7 = i.extraV2F7;
            // #endif

            return d;
         }
         

            
         #if defined(SHADERPASS_SHADOWCASTER)
            float3 _LightDirection;
         #endif

         // vertex shader
         VertexToPixel Vert (VertexData v)
         {
           
           VertexToPixel o = (VertexToPixel)0;

           UNITY_SETUP_INSTANCE_ID(v);
           UNITY_TRANSFER_INSTANCE_ID(v, o);
           UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);


#if !_TESSELLATION_ON
           ChainModifyVertex(v, o);
#endif

            o.texcoord0 = v.texcoord0;
           // o.texcoord1 = v.texcoord1;
           // o.texcoord2 = v.texcoord2;

           // #if %TEXCOORD3REQUIREKEY%
           // o.texcoord3 = v.texcoord3;
           // #endif

           // #if %VERTEXCOLORREQUIREKEY%
           // o.vertexColor = v.vertexColor;
           // #endif
           
           VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
           o.worldPos = TransformObjectToWorld(v.vertex.xyz);
           o.worldNormal = TransformObjectToWorldNormal(v.normal);
           o.worldTangent = float4(TransformObjectToWorldDir(v.tangent.xyz), v.tangent.w);


          #if defined(SHADERPASS_SHADOWCASTER)
              // Define shadow pass specific clip position for Universal
              o.pos = TransformWorldToHClip(ApplyShadowBias(o.worldPos, o.worldNormal, _LightDirection));
              #if UNITY_REVERSED_Z
                  o.pos.z = min(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
              #else
                  o.pos.z = max(o.pos.z, o.pos.w * UNITY_NEAR_CLIP_VALUE);
              #endif
          #elif defined(SHADERPASS_META)
              o.pos = MetaVertexPosition(float4(v.vertex.xyz, 0), v.texcoord1, v.texcoord2, unity_LightmapST, unity_DynamicLightmapST);
          #else
              o.pos = TransformWorldToHClip(o.worldPos);
          #endif

          // #if %SCREENPOSREQUIREKEY%
           o.screenPos = ComputeScreenPos(o.pos, _ProjectionParams.x);
          // #endif

          #if defined(SHADERPASS_FORWARD)
              OUTPUT_LIGHTMAP_UV(v.texcoord1, unity_LightmapST, o.lightmapUV);
              OUTPUT_SH(o.worldNormal, o.sh);
          #endif

          #ifdef VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
              half3 vertexLight = VertexLighting(o.worldPos, o.worldNormal);
              half fogFactor = ComputeFogFactor(o.pos.z);
              o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
          #endif

          #ifdef _MAIN_LIGHT_SHADOWS
              o.shadowCoord = GetShadowCoord(vertexInput);
          #endif

           return o;
         }


            

            // fragment shader
            half4 Frag (VertexToPixel IN
            #if NEED_FACING
               , bool facing : SV_IsFrontFace
            #endif
            ) : SV_Target
            {
               UNITY_SETUP_INSTANCE_ID(IN);
               UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);


               ShaderData d = CreateShaderData(IN
                  #if NEED_FACING
                     , facing
                  #endif
               );

               Surface l = (Surface)0;

               l.Albedo = half3(0.5, 0.5, 0.5);
               l.Normal = float3(0,0,1);
               l.Occlusion = 1;
               l.Alpha = 1;

               ChainSurfaceFunction(l, d);

               
               half4 color = half4(l.Albedo, l.Alpha);

               return color;

            }

         ENDHLSL

      }


      



   }
   
   
   CustomEditor "SeeThroughShaderEditor"
}
