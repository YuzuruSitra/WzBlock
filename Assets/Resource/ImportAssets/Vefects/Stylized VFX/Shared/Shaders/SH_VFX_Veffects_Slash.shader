// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SH_VFX_Veffects_Slash"
{
	Properties
	{
		[Space(13)][Header(Slash)][Space(13)]_Slash_Texture("Slash_Texture", 2D) = "white" {}
		_Slash_Scale("Slash_Scale", Float) = 1
		_Slash_Speed("Slash_Speed", Float) = 1
		[Space(13)][Header(Slash Noise)][Space(13)]_Slash_Noise_Texture("Slash_Noise_Texture", 2D) = "white" {}
		_Slash_Noise_Scale("Slash_Noise_Scale", Vector) = (1,1,0,0)
		_Slash_Noise_Speed("Slash_Noise_Speed", Vector) = (-1,0.5,0,0)
		_Slash_Noise_Intensity("Slash_Noise_Intensity", Float) = 1
		[Space(13)][Header(Emissive)][Space(13)]_Emissive_Slash_Texture("Emissive_Slash_Texture", 2D) = "white" {}
		_Emissive_Slash_Scale("Emissive_Slash_Scale", Float) = 1
		_Emissive_Slash_Speed("Emissive_Slash_Speed", Float) = 1
		_Emissive_Intensity("Emissive_Intensity", Float) = 3
		[Space(13)][Header(Emissive Dissolve)][Space(13)]_Emissive_Dissolve_Texture("Emissive_Dissolve_Texture", 2D) = "white" {}
		_Emissive_Dissolve_Scale("Emissive_Dissolve_Scale", Vector) = (1,1,0,0)
		_Emissive_Dissolve_Speed("Emissive_Dissolve_Speed", Vector) = (1,1,0,0)
		[Space(13)][Header(Distortion)][Space(13)]_Distortion_Noise_Texture("Distortion_Noise_Texture", 2D) = "white" {}
		_Distortion_Noise_Scale("Distortion_Noise_Scale", Vector) = (1,1,0,0)
		_Distortion_Noise_Speed("Distortion_Noise_Speed", Vector) = (1,1,0,0)
		_Distortion_Intensity("Distortion_Intensity", Float) = 1
		[Space(13)][Header(Color Noise)][Space(13)]_Color_Noise_Texture("Color_Noise_Texture", 2D) = "white" {}
		_ColorNoise_Scale("ColorNoise_Scale", Vector) = (1,1,0,0)
		_ColorNoise_Speed("ColorNoise_Speed", Vector) = (1,1,0,0)
		_Color_Boost("Color_Boost", Float) = 1
		[Space(13)][Header(Opacity)][Space(13)]_Mask("Mask", 2D) = "white" {}
		_Opacity_Boost("Opacity_Boost", Float) = 1
		[Space(13)][Header(Colors)][Space(13)]_Color_1("Color_1", Color) = (1,0,0.6261435,0)
		_Color_2("Color_2", Color) = (0.06587124,0,1,0)
		_Emissive_Color("Emissive_Color", Color) = (1,0,0.6261435,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _texcoord2( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		CGINCLUDE
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 3.0
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
			float4 uv2_texcoord2;
		};

		uniform float4 _Color_1;
		uniform float4 _Color_2;
		uniform sampler2D _Color_Noise_Texture;
		uniform float2 _ColorNoise_Scale;
		uniform float2 _ColorNoise_Speed;
		uniform float _Color_Boost;
		uniform sampler2D _Mask;
		uniform float4 _Mask_ST;
		uniform sampler2D _Slash_Texture;
		uniform float _Slash_Scale;
		uniform float _Slash_Speed;
		uniform sampler2D _Distortion_Noise_Texture;
		uniform float2 _Distortion_Noise_Scale;
		uniform float2 _Distortion_Noise_Speed;
		uniform float _Distortion_Intensity;
		uniform float _Slash_Noise_Intensity;
		uniform sampler2D _Slash_Noise_Texture;
		uniform float2 _Slash_Noise_Scale;
		uniform float2 _Slash_Noise_Speed;
		uniform sampler2D _Emissive_Slash_Texture;
		uniform float _Emissive_Slash_Scale;
		uniform float _Emissive_Slash_Speed;
		uniform sampler2D _Emissive_Dissolve_Texture;
		uniform float2 _Emissive_Dissolve_Scale;
		uniform float2 _Emissive_Dissolve_Speed;
		uniform float4 _Emissive_Color;
		uniform float _Emissive_Intensity;
		uniform float _Opacity_Boost;

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_TexCoord86 = i.uv_texcoord * _ColorNoise_Scale + ( _Time.y * _ColorNoise_Speed );
			float3 lerpResult91 = lerp( (_Color_1).rgb , (_Color_2).rgb , tex2D( _Color_Noise_Texture, uv_TexCoord86 ).r);
			float2 uv_Mask = i.uv_texcoord * _Mask_ST.xy + _Mask_ST.zw;
			float4 tex2DNode82 = tex2D( _Mask, uv_Mask );
			float2 appendResult103 = (float2(_Slash_Scale , 1.0));
			float2 appendResult105 = (float2(_Slash_Speed , 0.0));
			float2 uv_TexCoord107 = i.uv_texcoord * appendResult103 + ( _Time.y * appendResult105 );
			float2 uv_TexCoord144 = i.uv_texcoord * _Distortion_Noise_Scale + ( _Time.y * _Distortion_Noise_Speed );
			float Distortion152 = ( ( tex2D( _Distortion_Noise_Texture, uv_TexCoord144 ).r * 0.1 ) * _Distortion_Intensity );
			float2 uv_TexCoord109 = i.uv_texcoord * _Slash_Noise_Scale + ( _Time.y * _Slash_Noise_Speed );
			float clampResult117 = clamp( ( ( tex2D( _Slash_Texture, ( uv_TexCoord107 + Distortion152 ) ).r * _Slash_Noise_Intensity ) + tex2D( _Slash_Noise_Texture, uv_TexCoord109 ).g ) , 0.0 , 1.0 );
			float temp_output_118_0 = ( tex2DNode82.r * clampResult117 );
			o.Albedo = ( ( lerpResult91 * _Color_Boost ) * temp_output_118_0 );
			float2 appendResult62 = (float2(_Emissive_Slash_Scale , 1.0));
			float2 appendResult56 = (float2(_Emissive_Slash_Speed , 0.0));
			float2 uv_TexCoord60 = i.uv_texcoord * appendResult62 + ( _Time.y * appendResult56 );
			float2 uv_TexCoord76 = i.uv_texcoord * _Emissive_Dissolve_Scale + ( _Time.y * _Emissive_Dissolve_Speed );
			o.Emission = ( (i.vertexColor).rgb * ( ( ( saturate( ( (-1.0 + (( 1.0 - i.uv2_texcoord2.w ) - 0.0) * (0.0 - -1.0) / (1.0 - 0.0)) + saturate( ( tex2D( _Emissive_Slash_Texture, ( uv_TexCoord60 + Distortion152 ) ).g * tex2D( _Emissive_Dissolve_Texture, uv_TexCoord76 ).r ) ) ) ) * tex2DNode82.r ) * (_Emissive_Color).rgb ) * _Emissive_Intensity ) );
			o.Alpha = ( i.vertexColor.a * saturate( ( (0.0 + (( 1.0 - i.uv2_texcoord2.z ) - 0.0) * (2.0 - 0.0) / (1.0 - 0.0)) + saturate( ( temp_output_118_0 * _Opacity_Boost ) ) ) ) );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf Lambert alpha:fade keepalpha fullforwardshadows 

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
			#include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
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
				float4 customPack2 : TEXCOORD2;
				float3 worldPos : TEXCOORD3;
				half4 color : COLOR0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				Input customInputData;
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				half3 worldNormal = UnityObjectToWorldNormal( v.normal );
				o.customPack1.xy = customInputData.uv_texcoord;
				o.customPack1.xy = v.texcoord;
				o.customPack2.xyzw = customInputData.uv2_texcoord2;
				o.customPack2.xyzw = v.texcoord1;
				o.worldPos = worldPos;
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.color = v.color;
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
				surfIN.uv2_texcoord2 = IN.customPack2.xyzw;
				float3 worldPos = IN.worldPos;
				half3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.vertexColor = IN.color;
				SurfaceOutput o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutput, o )
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
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18921
0;-1;2560;1380;7452.161;2753.147;2.434129;True;False
Node;AmplifyShaderEditor.SimpleTimeNode;141;-7422.465,-2484.566;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;140;-7471.262,-2368.772;Inherit;False;Property;_Distortion_Noise_Speed;Distortion_Noise_Speed;16;0;Create;True;0;0;0;False;0;False;1,1;0.4,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;143;-7178.463,-2468.566;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;142;-7250.264,-2659.773;Inherit;False;Property;_Distortion_Noise_Scale;Distortion_Noise_Scale;15;0;Create;True;0;0;0;False;0;False;1,1;1.8,1.2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;144;-6974.164,-2587.766;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;145;-6702.371,-2614.453;Inherit;True;Property;_Distortion_Noise_Texture;Distortion_Noise_Texture;14;0;Create;True;0;0;0;False;3;Space(13);Header(Distortion);Space(13);False;-1;57e9e92fd03983b41bdbda66dc81e6b2;2c5f648e215789a4599c67843b5f7e9a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;150;-6474.357,-2368.219;Inherit;False;Property;_Distortion_Intensity;Distortion_Intensity;17;0;Create;True;0;0;0;False;0;False;1;1.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;66;-7102.625,-1696.139;Inherit;False;Property;_Emissive_Slash_Speed;Emissive_Slash_Speed;9;0;Create;True;0;0;0;False;0;False;1;0.8;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;148;-6357.158,-2586.39;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;104;-6853.868,-66.69758;Inherit;False;Property;_Slash_Speed;Slash_Speed;2;0;Create;True;0;0;0;False;0;False;1;1.3;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;106;-6676.101,-385.9376;Inherit;False;Property;_Slash_Scale;Slash_Scale;1;0;Create;True;0;0;0;False;0;False;1;4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;149;-6170.158,-2588.39;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;57;-6924.858,-2015.379;Inherit;False;Property;_Emissive_Slash_Scale;Emissive_Slash_Scale;8;0;Create;True;0;0;0;False;0;False;1;6;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;56;-6848.487,-1695.701;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;55;-6878.487,-1812.701;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;105;-6599.73,-66.25959;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;102;-6629.73,-183.2596;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;101;-6413.729,-187.2596;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;80;-7108.749,-1123.492;Inherit;False;Property;_Emissive_Dissolve_Speed;Emissive_Dissolve_Speed;13;0;Create;True;0;0;0;False;0;False;1,1;0.3,0.4;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;58;-6662.486,-1816.701;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;103;-6410.796,-381.0565;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;152;-5947.141,-2592.95;Inherit;False;Distortion;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;62;-6659.553,-2010.498;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;1;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;73;-7059.951,-1239.286;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;156;-6093.283,-187.6541;Inherit;False;152;Distortion;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;157;-6458.495,-1797.566;Inherit;False;152;Distortion;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;112;-6046.208,321.6166;Inherit;False;Property;_Slash_Noise_Speed;Slash_Noise_Speed;5;0;Create;True;0;0;0;False;0;False;-1,0.5;0.4,-0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;79;-6887.749,-1414.493;Inherit;False;Property;_Emissive_Dissolve_Scale;Emissive_Dissolve_Scale;12;0;Create;True;0;0;0;False;0;False;1,1;3.8,1.8;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;107;-6225.427,-323.4596;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;60;-6474.185,-1952.901;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;-6815.95,-1223.286;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleTimeNode;108;-5997.411,205.8223;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;111;-5753.41,221.8223;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;76;-6611.65,-1342.486;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;158;-6238.148,-1953.003;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;113;-5825.208,30.61556;Inherit;False;Property;_Slash_Noise_Scale;Slash_Noise_Scale;4;0;Create;True;0;0;0;False;0;False;1,1;2.8,1.3;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleAddOpNode;155;-5879.441,-316.2423;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;129;-5957.653,-1760.133;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;53;-6078.206,-1983.662;Inherit;True;Property;_Emissive_Slash_Texture;Emissive_Slash_Texture;7;0;Create;True;0;0;0;False;3;Space(13);Header(Emissive);Space(13);False;-1;61327fc8f347d034a914bf43bc9d74ea;0ff8e5dfd92c3a84499fb0aec5d3431a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;109;-5549.109,102.6224;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;100;-5631.304,-343.7232;Inherit;True;Property;_Slash_Texture;Slash_Texture;0;0;Create;True;0;0;0;False;3;Space(13);Header(Slash);Space(13);False;-1;61327fc8f347d034a914bf43bc9d74ea;0ff8e5dfd92c3a84499fb0aec5d3431a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;116;-5450.834,-103.6839;Inherit;False;Property;_Slash_Noise_Intensity;Slash_Noise_Intensity;6;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;74;-6122.302,-1354.806;Inherit;True;Property;_Emissive_Dissolve_Texture;Emissive_Dissolve_Texture;11;0;Create;True;0;0;0;False;3;Space(13);Header(Emissive Dissolve);Space(13);False;-1;57e9e92fd03983b41bdbda66dc81e6b2;2c5f648e215789a4599c67843b5f7e9a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;-5718.896,-1398.141;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.OneMinusNode;138;-5721.327,-1662.71;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;115;-5210.246,-311.4575;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;110;-5165.381,56.46943;Inherit;True;Property;_Slash_Noise_Texture;Slash_Noise_Texture;3;0;Create;True;0;0;0;False;3;Space(13);Header(Slash Noise);Space(13);False;-1;57e9e92fd03983b41bdbda66dc81e6b2;57e9e92fd03983b41bdbda66dc81e6b2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TFHCRemapNode;132;-5517.003,-1661.071;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;133;-5468.749,-1400.28;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;114;-4960.211,-308.9519;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;85;-4866.076,-2041.382;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;89;-4914.874,-1925.588;Inherit;False;Property;_ColorNoise_Speed;ColorNoise_Speed;20;0;Create;True;0;0;0;False;0;False;1,1;-0.4,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;82;-5281.997,-682.3408;Inherit;True;Property;_Mask;Mask;22;0;Create;True;0;0;0;False;3;Space(13);Header(Opacity);Space(13);False;-1;1e0097c086800a64b90526548667a51e;1e0097c086800a64b90526548667a51e;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;90;-4693.874,-2216.588;Inherit;False;Property;_ColorNoise_Scale;ColorNoise_Scale;19;0;Create;True;0;0;0;False;0;False;1,1;1.8,0.8;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;88;-4622.074,-2025.382;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;137;-5266.125,-1423.71;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;117;-4740.023,-309.2157;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;86;-4417.773,-2144.582;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;94;-4393.89,-2433.822;Inherit;False;Property;_Color_2;Color_2;25;0;Create;True;0;0;0;False;0;False;0.06587124,0,1,0;0.7,0.2176563,0.21,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;118;-4507.248,-644.7629;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;121;-3950.756,-426.261;Inherit;False;Property;_Opacity_Boost;Opacity_Boost;23;0;Create;True;0;0;0;False;0;False;1;1.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;96;-5287.146,-1089.914;Inherit;False;Property;_Emissive_Color;Emissive_Color;26;0;Create;True;0;0;0;False;0;False;1,0,0.6261435,0;1,0.2610294,0.25,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SaturateNode;139;-5101.304,-1418.768;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;24;-3938.916,-918.5826;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;92;-4295.588,-2696.925;Inherit;False;Property;_Color_1;Color_1;24;0;Create;True;0;0;0;False;3;Space(13);Header(Colors);Space(13);False;1,0,0.6261435,0;0.3,0.07967214,0.06,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;87;-4146.978,-2171.269;Inherit;True;Property;_Color_Noise_Texture;Color_Noise_Texture;18;0;Create;True;0;0;0;False;3;Space(13);Header(Color Noise);Space(13);False;-1;57e9e92fd03983b41bdbda66dc81e6b2;57e9e92fd03983b41bdbda66dc81e6b2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;93;-3983.049,-2698.632;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;-4912.898,-1378.883;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;120;-3714.509,-641.2752;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;98;-4877.577,-1085.931;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;95;-4085.89,-2434.822;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;49;-3214.29,-799.5125;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;122;-3342.77,-640.023;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;128;-3787.143,-2013.669;Inherit;False;Property;_Color_Boost;Color_Boost;21;0;Create;True;0;0;0;False;0;False;1;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;50;-2976.766,-804.4653;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;97;-4622.678,-1380.053;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.LerpOp;91;-3805.281,-2251.365;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-4546.065,-1134.978;Inherit;False;Property;_Emissive_Intensity;Emissive_Intensity;10;0;Create;True;0;0;0;False;0;False;3;25;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;48;-2714.821,-658.2591;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;27;-2759.456,-1005.683;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;127;-3573.327,-2105.484;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;2;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;99;-4312.41,-1377.976;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;119;-3524.111,-1557.458;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;30;-2486.832,-1162.47;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WireNode;126;-4033.474,-1071.71;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;47;-2539.417,-813.2694;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;124;-1937.906,-1407.705;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-2149.872,-1044.618;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-2370.939,-904.4759;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;6;-1796.883,-1091.369;Float;False;True;-1;2;ASEMaterialInspector;0;0;Lambert;SH_VFX_Veffects_Slash;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;7;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;143;0;141;0
WireConnection;143;1;140;0
WireConnection;144;0;142;0
WireConnection;144;1;143;0
WireConnection;145;1;144;0
WireConnection;148;0;145;1
WireConnection;149;0;148;0
WireConnection;149;1;150;0
WireConnection;56;0;66;0
WireConnection;105;0;104;0
WireConnection;101;0;102;0
WireConnection;101;1;105;0
WireConnection;58;0;55;0
WireConnection;58;1;56;0
WireConnection;103;0;106;0
WireConnection;152;0;149;0
WireConnection;62;0;57;0
WireConnection;107;0;103;0
WireConnection;107;1;101;0
WireConnection;60;0;62;0
WireConnection;60;1;58;0
WireConnection;71;0;73;0
WireConnection;71;1;80;0
WireConnection;111;0;108;0
WireConnection;111;1;112;0
WireConnection;76;0;79;0
WireConnection;76;1;71;0
WireConnection;158;0;60;0
WireConnection;158;1;157;0
WireConnection;155;0;107;0
WireConnection;155;1;156;0
WireConnection;53;1;158;0
WireConnection;109;0;113;0
WireConnection;109;1;111;0
WireConnection;100;1;155;0
WireConnection;74;1;76;0
WireConnection;81;0;53;2
WireConnection;81;1;74;1
WireConnection;138;0;129;4
WireConnection;115;0;100;1
WireConnection;115;1;116;0
WireConnection;110;1;109;0
WireConnection;132;0;138;0
WireConnection;133;0;81;0
WireConnection;114;0;115;0
WireConnection;114;1;110;2
WireConnection;88;0;85;0
WireConnection;88;1;89;0
WireConnection;137;0;132;0
WireConnection;137;1;133;0
WireConnection;117;0;114;0
WireConnection;86;0;90;0
WireConnection;86;1;88;0
WireConnection;118;0;82;1
WireConnection;118;1;117;0
WireConnection;139;0;137;0
WireConnection;87;1;86;0
WireConnection;93;0;92;0
WireConnection;84;0;139;0
WireConnection;84;1;82;1
WireConnection;120;0;118;0
WireConnection;120;1;121;0
WireConnection;98;0;96;0
WireConnection;95;0;94;0
WireConnection;49;0;24;3
WireConnection;122;0;120;0
WireConnection;50;0;49;0
WireConnection;97;0;84;0
WireConnection;97;1;98;0
WireConnection;91;0;93;0
WireConnection;91;1;95;0
WireConnection;91;2;87;1
WireConnection;48;0;50;0
WireConnection;48;1;122;0
WireConnection;127;0;91;0
WireConnection;127;1;128;0
WireConnection;99;0;97;0
WireConnection;99;1;29;0
WireConnection;119;0;127;0
WireConnection;119;1;118;0
WireConnection;30;0;27;0
WireConnection;126;0;99;0
WireConnection;47;0;48;0
WireConnection;124;0;119;0
WireConnection;31;0;30;0
WireConnection;31;1;126;0
WireConnection;43;0;27;4
WireConnection;43;1;47;0
WireConnection;6;0;124;0
WireConnection;6;2;31;0
WireConnection;6;9;43;0
ASEEND*/
//CHKSM=3B7D463D96CBD33827E662AC81A09C8B78B078F2