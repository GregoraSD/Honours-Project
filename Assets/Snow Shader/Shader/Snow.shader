// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Snow/SnowShader"
{
	Properties
	{
		_Albedocolormodifier("Albedo color modifier", Color) = (1,1,1,0)
		[NoScaleOffset]_AlbedoA("Albedo (+A)", 2D) = "white" {}
		[NoScaleOffset]_Occlusion("Occlusion", 2D) = "white" {}
		[NoScaleOffset]_Normals("Normals", 2D) = "bump" {}
		[NoScaleOffset]_DetailNormals("Detail Normals", 2D) = "bump" {}
		_Detailtiling("Detail tiling", Float) = 15
		_Normalsintensity("Normals intensity", Range( 0 , 1)) = 0.4785318
		[NoScaleOffset]_MetalnessS("Metalness (+S)", 2D) = "white" {}
		_Metalness("Metalness", Range( 0 , 1)) = 0
		_Smoothness("Smoothness", Range( 0 , 1)) = 0.35
		[NoScaleOffset]_Emissive("Emissive", 2D) = "black" {}
		_GlobalTiling("Global Tiling", Float) = 1
		_Snowcolormodifier("Snow color modifier", Color) = (1,1,1,0)
		[NoScaleOffset]_Snowtexture("Snow texture", 2D) = "white" {}
		[NoScaleOffset]_SnowSpecular("Snow Specular", 2D) = "black" {}
		_IcinessSpecular("Iciness (Specular)", Range( 0 , 1)) = 0.75
		_Speculartiling("Specular tiling", Float) = 1
		[NoScaleOffset]_Snownormals("Snow normals", 2D) = "bump" {}
		_Snownormalsintensity("Snow normals intensity", Range( 0 , 1)) = 0.3
		[HideInInspector]_Sharpnesscontrol("Sharpness control", Float) = 0.58
		_Snowtiling("Snow tiling", Float) = 1
		[NoScaleOffset]_Snowdetailnormals("Snow detail normals", 2D) = "bump" {}
		_Coveragecontrol(" Coverage control", Range( -1.5 , 1.5)) = 0
		_SharpnessControl("Sharpness Control", Range( 0 , 0.99)) = 0.5
		_VerticalaxisImportance("Vertical axis Importance", Range( 0 , 1)) = 1
		_ZaxisImportance("Z axis Importance", Range( -1 , 1)) = 0
		_XaxisImportance("X axis Importance", Range( -1 , 1)) = 0
		_NormalsImportance("Normals Importance", Range( 0 , 0.5)) = 0.5
		_GroundSnow("Ground Snow", Float) = 0.56
		[Toggle]_Desactivategroundsnow("Desactivate ground snow", Float) = 1
		_Cutoff( "Mask Clip Value", Float ) = 0.5
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest+0" "IsEmissive" = "true"  }
		Cull Back
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) fixed3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			float3 worldNormal;
			INTERNAL_DATA
			float3 worldPos;
		};

		uniform float _Normalsintensity;
		uniform sampler2D _Normals;
		uniform float _GlobalTiling;
		uniform sampler2D _DetailNormals;
		uniform float _Detailtiling;
		uniform float _Snownormalsintensity;
		uniform sampler2D _Snownormals;
		uniform float _Snowtiling;
		uniform sampler2D _Snowdetailnormals;
		uniform float _SharpnessControl;
		uniform float _VerticalaxisImportance;
		uniform float _NormalsImportance;
		uniform float _ZaxisImportance;
		uniform float _XaxisImportance;
		uniform float _Coveragecontrol;
		uniform float _Sharpnesscontrol;
		uniform float _Desactivategroundsnow;
		uniform float _GroundSnow;
		uniform float4 _Albedocolormodifier;
		uniform sampler2D _AlbedoA;
		uniform sampler2D _Occlusion;
		uniform sampler2D _Snowtexture;
		uniform float4 _Snowcolormodifier;
		uniform sampler2D _Emissive;
		uniform float _Metalness;
		uniform sampler2D _MetalnessS;
		uniform float _Smoothness;
		uniform sampler2D _SnowSpecular;
		uniform float _Speculartiling;
		uniform float _IcinessSpecular;
		uniform float _Cutoff = 0.5;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_TexCoord16 = i.uv_texcoord * float2( 1,1 ) + float2( 0,0 );
			float2 temp_output_19_0 = ( _GlobalTiling * uv_TexCoord16 );
			float2 temp_output_17_0 = ( uv_TexCoord16 * _Detailtiling );
			float2 temp_output_30_0 = ( _Snowtiling * uv_TexCoord16 );
			float3 temp_output_27_0 = BlendNormals( UnpackScaleNormal( tex2D( _Snownormals, temp_output_30_0 ) ,_Snownormalsintensity ) , UnpackScaleNormal( tex2D( _Snowdetailnormals, temp_output_17_0 ) ,_Snownormalsintensity ) );
			float temp_output_98_0 = ( 1.0 - _SharpnessControl );
			float3 normalizeResult270 = normalize( BlendNormals( UnpackScaleNormal( tex2D( _Normals, temp_output_19_0 ) ,_NormalsImportance ) , UnpackScaleNormal( tex2D( _DetailNormals, temp_output_17_0 ) ,_NormalsImportance ) ) );
			float3 newWorldNormal23 = WorldNormalVector( i , normalizeResult270 );
			float clampResult78 = clamp( ( ( ( _VerticalaxisImportance * newWorldNormal23.y ) + ( _ZaxisImportance * newWorldNormal23.z ) + ( _XaxisImportance * newWorldNormal23.x ) ) + _Coveragecontrol ) , 0.0 , 1.0 );
			float smoothstepResult37 = smoothstep( 0.0 , temp_output_98_0 , clampResult78);
			float3 lerpResult28 = lerp( BlendNormals( UnpackScaleNormal( tex2D( _Normals, temp_output_19_0 ) ,_Normalsintensity ) , UnpackScaleNormal( tex2D( _DetailNormals, temp_output_17_0 ) ,_Normalsintensity ) ) , temp_output_27_0 , smoothstepResult37);
			float clampResult224 = clamp( temp_output_98_0 , -1.0 , 1.0 );
			float3 ase_vertex3Pos = mul( unity_WorldToObject, float4( i.worldPos , 1 ) );
			float clampResult227 = clamp( _Coveragecontrol , -1.5 , 0.0 );
			float3 temp_cast_2 = (( -mul( unity_ObjectToWorld, float4( ase_vertex3Pos , 0.0 ) ).xyz.y + ( clampResult227 + _GroundSnow ) )).xxx;
			float2 uv_Normals = i.uv_texcoord;
			float dotResult217 = dot( temp_cast_2 , UnpackNormal( tex2D( _Normals, uv_Normals ) ) );
			float4 temp_cast_3 = (dotResult217).xxxx;
			float4 clampResult211 = clamp( ( ( 0.1 * _Sharpnesscontrol ) * ( lerp(temp_cast_3,float4(0,0,0,0),_Desactivategroundsnow) + 0.0 ) ) , float4( 0.0,0,0,0 ) , float4( 1.0,0,0,0 ) );
			float4 temp_cast_4 = (dotResult217).xxxx;
			float4 clampResult207 = clamp( ( ( lerp(temp_cast_4,float4(0,0,0,0),_Desactivategroundsnow) + 0.0 ) * ( _Sharpnesscontrol * 0.1 ) ) , float4( 0.0,0,0,0 ) , float4( 1.0,0,0,0 ) );
			float smoothstepResult215 = smoothstep( 0.0 , ( clampResult224 * 0.3 ) , ( clampResult211 + clampResult207 ).r);
			float clampResult221 = clamp( smoothstepResult215 , 0.0 , 1.0 );
			float3 lerpResult216 = lerp( lerpResult28 , temp_output_27_0 , clampResult221);
			o.Normal = lerpResult216;
			float4 tex2DNode1 = tex2D( _AlbedoA, temp_output_19_0 );
			float4 temp_output_61_0 = ( tex2D( _Snowtexture, temp_output_30_0 ) * _Snowcolormodifier );
			float4 lerpResult22 = lerp( ( ( _Albedocolormodifier * tex2DNode1 ) * tex2D( _Occlusion, temp_output_19_0 ) ) , temp_output_61_0 , smoothstepResult37);
			float4 lerpResult184 = lerp( lerpResult22 , temp_output_61_0 , clampResult221);
			o.Albedo = lerpResult184.rgb;
			o.Emission = tex2D( _Emissive, temp_output_19_0 ).rgb;
			float4 tex2DNode3 = tex2D( _MetalnessS, temp_output_19_0 );
			float lerpResult47 = lerp( ( _Metalness * tex2DNode3.r ) , 0.0 , smoothstepResult37);
			o.Metallic = lerpResult47;
			float4 temp_cast_8 = (( _Smoothness * tex2DNode3.a )).xxxx;
			float4 lerpResult53 = lerp( temp_cast_8 , ( tex2D( _SnowSpecular, ( uv_TexCoord16 * _Speculartiling ) ) * _IcinessSpecular ) , smoothstepResult37);
			o.Smoothness = lerpResult53.r;
			o.Alpha = 1;
			clip( tex2DNode1.a - _Cutoff );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Standard keepalpha fullforwardshadows 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			# include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float4 tSpace0 : TEXCOORD1;
				float4 tSpace1 : TEXCOORD2;
				float4 tSpace2 : TEXCOORD3;
				float4 texcoords01 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal( v.normal );
				fixed3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.texcoords01 = float4( v.texcoord.xy, v.texcoord1.xy );
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				return o;
			}
			fixed4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord.xy = IN.texcoords01.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.worldPos = worldPos;
				surfIN.worldNormal = float3( IN.tSpace0.z, IN.tSpace1.z, IN.tSpace2.z );
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				SurfaceOutputStandard o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandard, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=13801
2567;29;1666;974;5398.152;-680.7701;4.177815;True;True
Node;AmplifyShaderEditor.CommentaryNode;241;-2804.344,312.5421;Float;False;854.3042;427.799;Axis masks control;6;39;98;235;78;37;145;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;256;-1790.501,-33.69477;Float;False;570.8777;780.0209;Tilings controls modifiers;10;237;232;233;50;31;30;18;20;19;17;;1,1,1,1;0;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;16;-2657.176,1070.106;Float;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;244;-5011.089,123.8731;Float;False;1029.658;507.8923;Add Normals into masks;5;182;177;178;180;270;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;20;-1726.899,135.1085;Float;False;Property;_GlobalTiling;Global Tiling;11;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;185;-3398.946,1739.522;Float;False;1291.301;493.6084;growing altitude;8;191;190;229;188;276;274;186;275;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WireNode;236;-1867.402,696.3009;Float;False;1;0;FLOAT2;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.RangedFloatNode;18;-1740.501,394.4527;Float;False;Property;_Detailtiling;Detail tiling;5;0;15;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.WireNode;235;-2204.605,441.8082;Float;False;1;0;FLOAT2;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;17;-1397.222,389.7528;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0.0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.PosVertexDataNode;186;-3343.096,2018.386;Float;True;0;0;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;19;-1402.961,224.7695;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT2;0;False;1;FLOAT2
Node;AmplifyShaderEditor.ObjectToWorldMatrixNode;275;-3325.373,1928.529;Float;False;0;1;FLOAT4x4
Node;AmplifyShaderEditor.RangedFloatNode;182;-4961.089,317.1111;Float;False;Property;_NormalsImportance;Normals Importance;27;0;0.5;0;0.5;0;1;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;242;-2857.075,1291.398;Float;False;275;230;Coverage reduces ground too;1;227;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;144;-3210.123,893.9511;Float;False;Property;_Coveragecontrol; Coverage control;22;0;0;-1.5;1.5;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;274;-3044.304,2022.699;Float;False;2;2;0;FLOAT4x4;0,0,0;False;1;FLOAT3;0.0;False;1;FLOAT3
Node;AmplifyShaderEditor.SamplerNode;177;-4560.229,173.8731;Float;True;Property;_TextureSample0;Texture Sample 0;3;0;None;True;0;False;white;Auto;True;Instance;2;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;178;-4544.778,401.7654;Float;True;Property;_TextureSample1;Texture Sample 1;4;0;None;True;0;False;white;Auto;True;Instance;5;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;227;-2807.075,1341.398;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;-1.5;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.BlendNormalsNode;180;-4254.458,304.3293;Float;False;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.BreakToComponentsNode;276;-2894.636,2014.363;Float;False;FLOAT3;1;0;FLOAT3;0.0;False;16;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;188;-2899.343,1812.422;Float;False;Property;_GroundSnow;Ground Snow;28;0;0.56;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.NormalizeNode;270;-3993.684,501.8513;Float;False;1;0;FLOAT3;0,0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.NegateNode;190;-2633.588,2050.189;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;240;-3633.658,307.7582;Float;False;632.646;445.0404;Axis controls;6;73;75;74;71;69;72;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;229;-2599.752,1846.91;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.WorldNormalVector;23;-3792.667,434.8965;Float;False;1;0;FLOAT3;0,0,0;False;4;FLOAT3;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;280;-2854.056,1526.504;Float;True;Property;_TextureSample2;Texture Sample 2;3;0;None;True;0;False;white;Auto;True;Instance;2;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;191;-2473.339,1967.967;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;69;-3561.737,605.5955;Float;False;Property;_XaxisImportance;X axis Importance;26;0;0;-1;1;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;72;-3583.658,388.4222;Float;False;Property;_VerticalaxisImportance;Vertical axis Importance;24;0;1;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;71;-3578.69,504.6271;Float;False;Property;_ZaxisImportance;Z axis Importance;25;0;0;-1;1;0;1;FLOAT
Node;AmplifyShaderEditor.ColorNode;273;-2234.112,2243.249;Float;False;Constant;_Color0;Color 0;30;0;0,0,0,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.DotProductOpNode;217;-2237.545,1975.886;Float;True;2;0;FLOAT;0,0,0;False;1;FLOAT3;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;243;-1995.727,1639.959;Float;False;968.8323;941.9065;Ground snow mask control;14;196;200;202;201;207;208;211;193;198;199;195;205;194;269;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;74;-3226.33,507.2584;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;75;-3237.031,357.7582;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;73;-3218.817,627.6581;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;195;-1920.942,2284.331;Float;False;Constant;_Float6;Float 6;12;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.ToggleSwitchNode;272;-2002.567,2161.606;Float;False;Property;_Desactivategroundsnow;Desactivate ground snow;29;0;1;2;0;COLOR;0,0,0,0;False;1;COLOR;0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;39;-2719.582,622.9164;Float;False;Property;_SharpnessControl;Sharpness Control;23;0;0.5;0;0.99;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;77;-2939.626,329.7079;Float;False;3;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;194;-1926.384,2466.866;Float;False;Constant;_Float5;Float 5;15;0;0.1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;193;-1945.727,2026.287;Float;False;Property;_Sharpnesscontrol;Sharpness control;19;1;[HideInInspector];0.58;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;198;-1920.891,1844.239;Float;False;Constant;_Float7;Float 7;13;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;199;-1919.229,1689.959;Float;False;Constant;_Float8;Float 8;15;0;0.1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;250;-1023.027,-1067.702;Float;False;1621.499;1273.347;Albedo;6;249;248;247;246;245;4;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleAddOpNode;196;-1686.4,2172.003;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;233;-1627.977,645.2881;Float;False;Property;_Speculartiling;Specular tiling;16;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;145;-2754.344,364.2979;Float;True;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;200;-1656.145,2443.388;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;205;-1662.249,1691.685;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.WireNode;237;-1689.328,631.0404;Float;False;1;0;FLOAT2;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.SimpleAddOpNode;201;-1679.355,1877.214;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.OneMinusNode;98;-2419.287,630.3411;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;202;-1449.129,2172.612;Float;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.ClampOpNode;78;-2483.148,362.5421;Float;True;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;232;-1407.893,550.3218;Float;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0.0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.CommentaryNode;251;-1025.866,209.1443;Float;False;1484.589;907.6679;Normals;10;42;25;26;5;8;27;2;9;254;255;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;261;-1022.178,1332.816;Float;False;1224.663;656.6851;Metalness + Smoothness;2;265;266;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WireNode;234;-2287.315,257.3009;Float;False;1;0;FLOAT2;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.RangedFloatNode;31;-1721.455,27.16135;Float;False;Property;_Snowtiling;Snow tiling;20;0;1;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;208;-1439.937,1883.958;Float;True;2;2;0;FLOAT;0.0;False;1;COLOR;0;False;1;COLOR
Node;AmplifyShaderEditor.ClampOpNode;224;-2136.488,1060.64;Float;True;3;0;FLOAT;0.0;False;1;FLOAT;-1.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;245;-954.4075,-1017.702;Float;False;568.6269;469.0118;Albedo;3;58;59;1;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WireNode;269;-1167.901,2104.44;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;265;-982.6038,1694.494;Float;False;498.1214;292.3781;Comment;2;176;230;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;268;-910.4814,2007.421;Float;False;919.978;288.7651;Ground snow 2nd pass (add sharpness control);3;215;212;225;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WireNode;260;-1090.532,1525.073;Float;False;1;0;FLOAT2;0,0;False;1;FLOAT2
Node;AmplifyShaderEditor.SamplerNode;1;-904.4075,-754.6899;Float;True;Property;_AlbedoA;Albedo (+A);1;1;[NoScaleOffset];None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;9;-1006.699,397.5685;Float;False;Property;_Normalsintensity;Normals intensity;6;0;0.4785318;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;247;-973.0266,-274.3854;Float;False;631.2813;480.0304;Snow Albedo;3;24;61;60;;1,1,1,1;0;0
Node;AmplifyShaderEditor.ColorNode;58;-876.7808,-967.7017;Float;False;Property;_Albedocolormodifier;Albedo color modifier;0;0;1,1,1,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;207;-1207.805,2173.206;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0.0,0,0,0;False;2;COLOR;1.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-1404.663,16.30523;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT2;0;False;1;FLOAT2
Node;AmplifyShaderEditor.CommentaryNode;264;-980.0944,1337.87;Float;False;1182.58;356.4514;Comment;7;3;13;12;14;15;52;47;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;42;-982.7342,789.5571;Float;False;Property;_Snownormalsintensity;Snow normals intensity;18;0;0.3;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.ClampOpNode;211;-1195.895,1885.475;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0.0,0,0,0;False;2;COLOR;1.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SmoothstepOpNode;37;-2206.04,363.2709;Float;True;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.63;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;3;-930.0944,1407.087;Float;True;Property;_MetalnessS;Metalness (+S);7;1;[NoScaleOffset];None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;15;-598.3154,1579.322;Float;False;Property;_Smoothness;Smoothness;9;0;0.35;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;246;-317.7077,-596.4246;Float;False;219;183;Albedo 2nd pass (+AO);1;21;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SamplerNode;25;-648.7435,683.0265;Float;True;Property;_Snownormals;Snow normals;17;1;[NoScaleOffset];Assets/Snow Shader/Textures/Snow_Normals.tga;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.ColorNode;60;-905.7803,-1.355032;Float;False;Property;_Snowcolormodifier;Snow color modifier;12;0;1,1,1,0;0;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WireNode;49;-1843.75,-39.21189;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;225;-829.8044,2173.681;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.3;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;4;-904.8377,-532.6182;Float;True;Property;_Occlusion;Occlusion;2;1;[NoScaleOffset];None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;59;-550.7806,-746.7017;Float;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.RangedFloatNode;13;-608.6542,1391.839;Float;False;Property;_Metalness;Metalness;8;0;0;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;5;-650.7758,473.1965;Float;True;Property;_DetailNormals;Detail Normals;4;1;[NoScaleOffset];None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;230;-935.2338,1733.973;Float;True;Property;_SnowSpecular;Snow Specular;14;1;[NoScaleOffset];Assets/Snow Shader/Textures/Snow_Specular.tga;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SimpleAddOpNode;212;-570.9828,2057.421;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.SamplerNode;26;-644.5435,891.2169;Float;True;Property;_Snowdetailnormals;Snow detail normals;21;1;[NoScaleOffset];Assets/Snow Shader/Textures/Snow_DetailNormals.tga;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.RangedFloatNode;176;-918.6774,1921.843;Float;False;Property;_IcinessSpecular;Iciness (Specular);15;0;0.75;0;1;0;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;24;-923.0266,-224.3854;Float;True;Property;_Snowtexture;Snow texture;13;1;[NoScaleOffset];Assets/Snow Shader/Textures/Snow_Diffuse.tga;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.SamplerNode;2;-655.3701,263.549;Float;True;Property;_Normals;Normals;3;1;[NoScaleOffset];None;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;FLOAT3;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.WireNode;51;-1809.838,539.4995;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;248;-116.6724,-395.4433;Float;False;315;303;Albedo 3rd pass (+Vertical Snow);1;22;;1,1,1,1;0;0
Node;AmplifyShaderEditor.BlendNormalsNode;8;-280.5013,378.8586;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-267.7077,-546.4246;Float;False;2;2;0;COLOR;0.0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.CommentaryNode;254;31.90354,549.6913;Float;False;217.1047;189.1047;2nd pass (+Vertical snow);1;28;;1,1,1,1;0;0
Node;AmplifyShaderEditor.WireNode;54;-1175.425,1428.206;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;61;-510.7453,-98.74108;Float;False;2;2;0;COLOR;0.0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.WireNode;55;-1132.613,1180.328;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SmoothstepOpNode;215;-246.5033,2060.556;Float;True;3;0;FLOAT;0,0,0,0;False;1;FLOAT;0.0;False;2;FLOAT;0,0,0,0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;14;-298.0382,1494.475;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.WireNode;48;-1514.941,-226.3449;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-300.42,1395.511;Float;False;2;2;0;FLOAT;0.0;False;1;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;266;-59.41794,1726.697;Float;False;234;206;Smoothness 2nd pass;1;53;;1,1,1,1;0;0
Node;AmplifyShaderEditor.RangedFloatNode;52;-280.1125,1601.629;Float;False;Constant;_SnowMetal;Snow Metal;17;0;0;0;0;0;1;FLOAT
Node;AmplifyShaderEditor.BlendNormalsNode;27;-279.7451,802.5466;Float;True;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.WireNode;50;-1380.889,665.8545;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;231;-592.9901,1813.617;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.ClampOpNode;221;83.46469,2054.593;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;1.0;False;1;FLOAT
Node;AmplifyShaderEditor.SamplerNode;7;-1024.18,1130.555;Float;True;Property;_Emissive;Emissive;10;1;[NoScaleOffset];None;True;0;False;black;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0.0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1.0;False;5;COLOR;FLOAT;FLOAT;FLOAT;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;255;218.7392,688.1596;Float;False;234;206;3rd pass (+Ground);1;216;;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;47;-17.02107,1391.815;Float;False;3;0;FLOAT;0.0;False;1;FLOAT;0.0;False;2;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.CommentaryNode;249;233.4726,-146.552;Float;False;315;303;Albedo 4th pass (+Ground snow);1;184;;1,1,1,1;0;0
Node;AmplifyShaderEditor.LerpOp;22;-66.67238,-345.4433;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.LerpOp;28;71.84548,605.1525;Float;False;3;0;FLOAT3;0.0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.LerpOp;53;-8.102892,1774.067;Float;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0;False;2;FLOAT;0.0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.LerpOp;216;268.7391,755.0549;Float;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0,0,0;False;1;FLOAT3
Node;AmplifyShaderEditor.WireNode;259;700.2455,1022.331;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.LerpOp;184;283.4725,-96.55208;Float;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.WireNode;66;475.1075,1111.181;Float;False;1;0;COLOR;0,0,0,0;False;1;COLOR
Node;AmplifyShaderEditor.WireNode;267;524.7061,1183.057;Float;False;1;0;FLOAT;0.0;False;1;FLOAT
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;906.0605,755.7721;Float;False;True;2;Float;ASEMaterialInspector;0;0;Standard;Snow/SnowShader;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Back;0;0;False;0;0;Masked;0.5;True;True;0;False;TransparentCutout;AlphaTest;All;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;True;False;0;255;255;0;0;0;0;0;0;0;0;False;2;15;10;25;False;0.5;True;0;SrcAlpha;OneMinusSrcAlpha;0;Zero;Zero;OFF;OFF;0;False;0;0,0,0,0;VertexOffset;False;Cylindrical;False;Relative;0;;30;-1;-1;-1;0;0;0;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0.0;False;4;FLOAT;0.0;False;5;FLOAT;0.0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0.0;False;9;FLOAT;0.0;False;10;FLOAT;0.0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
Node;AmplifyShaderEditor.CommentaryNode;252;-330.5013,328.8586;Float;False;344;303;Base normals;0;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;253;-329.7451,752.5466;Float;False;344;303;Snow normals;0;;1,1,1,1;0;0
WireConnection;236;0;16;0
WireConnection;235;0;16;0
WireConnection;17;0;236;0
WireConnection;17;1;18;0
WireConnection;19;0;20;0
WireConnection;19;1;235;0
WireConnection;274;0;275;0
WireConnection;274;1;186;0
WireConnection;177;1;19;0
WireConnection;177;5;182;0
WireConnection;178;1;17;0
WireConnection;178;5;182;0
WireConnection;227;0;144;0
WireConnection;180;0;177;0
WireConnection;180;1;178;0
WireConnection;276;0;274;0
WireConnection;270;0;180;0
WireConnection;190;0;276;1
WireConnection;229;0;227;0
WireConnection;229;1;188;0
WireConnection;23;0;270;0
WireConnection;191;0;190;0
WireConnection;191;1;229;0
WireConnection;217;0;191;0
WireConnection;217;1;280;0
WireConnection;74;0;71;0
WireConnection;74;1;23;3
WireConnection;75;0;72;0
WireConnection;75;1;23;2
WireConnection;73;0;69;0
WireConnection;73;1;23;1
WireConnection;272;0;217;0
WireConnection;272;1;273;0
WireConnection;77;0;75;0
WireConnection;77;1;74;0
WireConnection;77;2;73;0
WireConnection;196;0;272;0
WireConnection;196;1;195;0
WireConnection;145;0;77;0
WireConnection;145;1;144;0
WireConnection;200;0;193;0
WireConnection;200;1;194;0
WireConnection;205;0;199;0
WireConnection;205;1;193;0
WireConnection;237;0;16;0
WireConnection;201;0;272;0
WireConnection;201;1;198;0
WireConnection;98;0;39;0
WireConnection;202;0;196;0
WireConnection;202;1;200;0
WireConnection;78;0;145;0
WireConnection;232;0;237;0
WireConnection;232;1;233;0
WireConnection;234;0;16;0
WireConnection;208;0;205;0
WireConnection;208;1;201;0
WireConnection;224;0;98;0
WireConnection;269;0;224;0
WireConnection;260;0;232;0
WireConnection;1;1;19;0
WireConnection;207;0;202;0
WireConnection;30;0;31;0
WireConnection;30;1;234;0
WireConnection;211;0;208;0
WireConnection;37;0;78;0
WireConnection;37;2;98;0
WireConnection;3;1;19;0
WireConnection;25;1;30;0
WireConnection;25;5;42;0
WireConnection;49;0;37;0
WireConnection;225;0;269;0
WireConnection;4;1;19;0
WireConnection;59;0;58;0
WireConnection;59;1;1;0
WireConnection;5;1;17;0
WireConnection;5;5;9;0
WireConnection;230;1;260;0
WireConnection;212;0;211;0
WireConnection;212;1;207;0
WireConnection;26;1;17;0
WireConnection;26;5;42;0
WireConnection;24;1;30;0
WireConnection;2;1;19;0
WireConnection;2;5;9;0
WireConnection;51;0;37;0
WireConnection;8;0;2;0
WireConnection;8;1;5;0
WireConnection;21;0;59;0
WireConnection;21;1;4;0
WireConnection;54;0;37;0
WireConnection;61;0;24;0
WireConnection;61;1;60;0
WireConnection;55;0;37;0
WireConnection;215;0;212;0
WireConnection;215;2;225;0
WireConnection;14;0;15;0
WireConnection;14;1;3;4
WireConnection;48;0;49;0
WireConnection;12;0;13;0
WireConnection;12;1;3;1
WireConnection;27;0;25;0
WireConnection;27;1;26;0
WireConnection;50;0;51;0
WireConnection;231;0;230;0
WireConnection;231;1;176;0
WireConnection;221;0;215;0
WireConnection;7;1;19;0
WireConnection;47;0;12;0
WireConnection;47;1;52;0
WireConnection;47;2;55;0
WireConnection;22;0;21;0
WireConnection;22;1;61;0
WireConnection;22;2;48;0
WireConnection;28;0;8;0
WireConnection;28;1;27;0
WireConnection;28;2;50;0
WireConnection;53;0;14;0
WireConnection;53;1;231;0
WireConnection;53;2;54;0
WireConnection;216;0;28;0
WireConnection;216;1;27;0
WireConnection;216;2;221;0
WireConnection;259;0;53;0
WireConnection;184;0;22;0
WireConnection;184;1;61;0
WireConnection;184;2;221;0
WireConnection;66;0;7;0
WireConnection;267;0;47;0
WireConnection;0;0;184;0
WireConnection;0;1;216;0
WireConnection;0;2;66;0
WireConnection;0;3;267;0
WireConnection;0;4;259;0
WireConnection;0;10;1;4
ASEEND*/
//CHKSM=AF96F7237E3107AEFCFA2402FD21A3CB79B1A496