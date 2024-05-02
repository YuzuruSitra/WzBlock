// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SH_VFX_Veffects_Piercing"
{
	Properties
	{
		_Emissive_Noise_Texture("Emissive_Noise_Texture", 2D) = "white" {}
		_Distortion_Noise_Texture("Distortion_Noise_Texture", 2D) = "white" {}
		_Distortion_Mask("Distortion_Mask", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_Color_Noise_Texture("Color_Noise_Texture", 2D) = "white" {}
		_Piercing_Noise_Intesnity("Piercing_Noise_Intesnity", Float) = 3
		_Emissive_Intensity("Emissive_Intensity", Float) = 3
		_EmissiveDissolve_Scale("EmissiveDissolve_Scale", Vector) = (1,1,0,0)
		_Distortion_Noise_Scale("Distortion_Noise_Scale", Vector) = (1,1,0,0)
		_EmissiveDissolve_Speed("EmissiveDissolve_Speed", Vector) = (1,1,0,0)
		_Distortion_Noise_Speed("Distortion_Noise_Speed", Vector) = (1,1,0,0)
		_Piercing_Noise_Speed("Piercing_Noise_Speed", Vector) = (-1,0.5,0,0)
		_ColorNoise_Scale("ColorNoise_Scale", Vector) = (1,1,0,0)
		_ColorNoise_Speed("ColorNoise_Speed", Vector) = (1,1,0,0)
		_Piercing_Noise_Scale("Piercing_Noise_Scale", Vector) = (1,1,0,0)
		_Color_Boost("Color_Boost", Float) = 1
		_Opacity_Boost("Opacity_Boost", Float) = 1
		_Distortion_Intensity("Distortion_Intensity", Float) = 1
		_Color_1("Color_1", Color) = (1,0,0.6261435,0)
		_Color_2("Color_2", Color) = (0.06587124,0,1,0)
		_Emissive_Color("Emissive_Color", Color) = (1,0,0.6261435,0)
		_Piercing_Texture("Piercing_Texture", 2D) = "white" {}
		_Texture0("Texture 0", 2D) = "white" {}
		_Texture1("Texture 1", 2D) = "white" {}
		[HideInInspector] _texcoord2( "", 2D ) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
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
		uniform sampler2D _Piercing_Texture;
		uniform sampler2D _Distortion_Noise_Texture;
		uniform float2 _Distortion_Noise_Scale;
		uniform float2 _Distortion_Noise_Speed;
		uniform sampler2D _Distortion_Mask;
		uniform float4 _Distortion_Mask_ST;
		uniform float _Distortion_Intensity;
		uniform sampler2D _TextureSample1;
		uniform float2 _Piercing_Noise_Scale;
		uniform float2 _Piercing_Noise_Speed;
		uniform float _Piercing_Noise_Intesnity;
		uniform sampler2D _Texture0;
		uniform sampler2D _Emissive_Noise_Texture;
		uniform float2 _EmissiveDissolve_Scale;
		uniform float2 _EmissiveDissolve_Speed;
		uniform float4 _Emissive_Color;
		uniform float _Emissive_Intensity;
		uniform float _Opacity_Boost;
		uniform sampler2D _Texture1;
		uniform float4 _Texture1_ST;

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_TexCoord86 = i.uv_texcoord * _ColorNoise_Scale + ( _Time.y * _ColorNoise_Speed );
			float3 lerpResult91 = lerp( (_Color_1).rgb , (_Color_2).rgb , tex2D( _Color_Noise_Texture, uv_TexCoord86 ).r);
			float2 uv_TexCoord144 = i.uv_texcoord * _Distortion_Noise_Scale + ( _Time.y * _Distortion_Noise_Speed );
			float2 uv_Distortion_Mask = i.uv_texcoord * _Distortion_Mask_ST.xy + _Distortion_Mask_ST.zw;
			float Distortion152 = ( ( ( tex2D( _Distortion_Noise_Texture, uv_TexCoord144 ).r * ( 1.0 - tex2D( _Distortion_Mask, uv_Distortion_Mask ).r ) ) * 0.1 ) * _Distortion_Intensity );
			float4 tex2DNode100 = tex2D( _Piercing_Texture, ( i.uv_texcoord + Distortion152 ) );
			float2 uv_TexCoord109 = i.uv_texcoord * _Piercing_Noise_Scale + ( _Time.y * _Piercing_Noise_Speed );
			float clampResult117 = clamp( ( tex2DNode100.r + ( tex2DNode100.r * ( tex2D( _TextureSample1, uv_TexCoord109 ).r * _Piercing_Noise_Intesnity ) ) ) , 0.0 , 1.0 );
			o.Albedo = ( ( lerpResult91 * _Color_Boost ) * clampResult117 );
			float2 uv_TexCoord76 = i.uv_texcoord * _EmissiveDissolve_Scale + ( _Time.y * _EmissiveDissolve_Speed );
			o.Emission = ( (i.vertexColor).rgb * ( ( saturate( ( (-1.0 + (( 1.0 - i.uv2_texcoord2.w ) - 0.0) * (0.0 - -1.0) / (1.0 - 0.0)) + saturate( ( tex2D( _Texture0, ( i.uv_texcoord + Distortion152 ) ).g * tex2D( _Emissive_Noise_Texture, ( uv_TexCoord76 + Distortion152 ) ).r ) ) ) ) * (_Emissive_Color).rgb ) * _Emissive_Intensity ) );
			float2 uv_Texture1 = i.uv_texcoord * _Texture1_ST.xy + _Texture1_ST.zw;
			o.Alpha = ( i.vertexColor.a * saturate( ( (0.0 + (( 1.0 - i.uv2_texcoord2.z ) - 0.0) * (2.0 - 0.0) / (1.0 - 0.0)) + saturate( ( ( clampResult117 * _Opacity_Boost ) * tex2D( _Texture1, uv_Texture1 ).b ) ) ) ) );
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
7;6;2546;1373;9688.394;4987.941;3.716454;True;True
Node;AmplifyShaderEditor.CommentaryNode;164;-7522.986,-3314.869;Inherit;False;2179.121;577.0146;DISTORTION;13;141;140;143;142;144;152;150;148;149;145;161;163;171;;1,1,1,1;0;0
Node;AmplifyShaderEditor.Vector2Node;140;-7472.986,-2973.868;Inherit;False;Property;_Distortion_Noise_Speed;Distortion_Noise_Speed;10;0;Create;True;0;0;0;False;0;False;1,1;-0.8,-0.3;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;141;-7424.189,-3089.662;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;142;-7251.988,-3264.869;Inherit;False;Property;_Distortion_Noise_Scale;Distortion_Noise_Scale;8;0;Create;True;0;0;0;False;0;False;1,1;1.5,2.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;143;-7180.187,-3073.662;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;161;-6771.972,-2970.854;Inherit;True;Property;_Distortion_Mask;Distortion_Mask;2;0;Create;True;0;0;0;False;0;False;-1;fbe0d908e002ec84792dc44832c375c2;fbe0d908e002ec84792dc44832c375c2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;144;-6975.888,-3192.862;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;171;-6434.955,-2951.017;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;145;-6704.095,-3219.549;Inherit;True;Property;_Distortion_Noise_Texture;Distortion_Noise_Texture;1;0;Create;True;0;0;0;False;0;False;-1;57e9e92fd03983b41bdbda66dc81e6b2;2c5f648e215789a4599c67843b5f7e9a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;163;-6247.972,-3184.854;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;148;-5965.882,-3190.486;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;150;-6083.081,-2972.315;Inherit;False;Property;_Distortion_Intensity;Distortion_Intensity;17;0;Create;True;0;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;149;-5778.882,-3192.486;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;112;-6649.314,189.3876;Inherit;False;Property;_Piercing_Noise_Speed;Piercing_Noise_Speed;11;0;Create;True;0;0;0;False;0;False;-1,0.5;0.3,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleTimeNode;108;-6600.517,73.59329;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;113;-6428.314,-101.6135;Inherit;False;Property;_Piercing_Noise_Scale;Piercing_Noise_Scale;14;0;Create;True;0;0;0;False;0;False;1,1;1.8,3;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;111;-6356.516,89.59329;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;152;-5567.865,-3198.046;Inherit;False;Distortion;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;180;-5902.725,-198.3258;Inherit;False;152;Distortion;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;182;-5937.199,-376.4046;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleTimeNode;73;-7085.457,-1288.17;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;109;-6152.215,-29.60663;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;80;-7134.254,-1172.376;Inherit;False;Property;_EmissiveDissolve_Speed;EmissiveDissolve_Speed;9;0;Create;True;0;0;0;False;0;False;1,1;0.1,-0.6;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;110;-5768.487,-75.7596;Inherit;True;Property;_TextureSample1;Texture Sample 1;3;0;Create;True;0;0;0;False;0;False;-1;57e9e92fd03983b41bdbda66dc81e6b2;2c5f648e215789a4599c67843b5f7e9a;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;181;-5692.082,-292.1326;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;79;-6913.254,-1463.377;Inherit;False;Property;_EmissiveDissolve_Scale;EmissiveDissolve_Scale;7;0;Create;True;0;0;0;False;0;False;1,1;1.8,3.8;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;175;-5494.911,172.5657;Inherit;False;Property;_Piercing_Noise_Intesnity;Piercing_Noise_Intesnity;5;0;Create;True;0;0;0;False;0;False;3;-0.09;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;71;-6841.456,-1272.17;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TexturePropertyNode;166;-5610.896,-491.4941;Inherit;True;Property;_Piercing_Texture;Piercing_Texture;21;0;Create;True;0;0;0;False;0;False;3fd6b115a58e2cb4aae698542657e9fd;56dc720491a71e34a8ec34eeb5da8047;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TextureCoordinatesNode;76;-6637.155,-1391.37;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;179;-6836.168,-1887.148;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;157;-6573.72,-1191.315;Inherit;False;152;Distortion;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;176;-5218.471,-23.00305;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;100;-5326.191,-331.6793;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;3fd6b115a58e2cb4aae698542657e9fd;61327fc8f347d034a914bf43bc9d74ea;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.GetLocalVarNode;177;-6801.694,-1709.069;Inherit;False;152;Distortion;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;167;-6762.42,-2129.099;Inherit;True;Property;_Texture0;Texture 0;22;0;Create;True;0;0;0;False;0;False;3fd6b115a58e2cb4aae698542657e9fd;56dc720491a71e34a8ec34eeb5da8047;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.SimpleAddOpNode;178;-6591.051,-1802.876;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;168;-4996.195,-63.2281;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;158;-6327.435,-1389.37;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;53;-6468.206,-2131.662;Inherit;True;Property;_Slash_Texture;Slash_Texture;0;0;Create;True;0;0;0;False;0;False;-1;3fd6b115a58e2cb4aae698542657e9fd;61327fc8f347d034a914bf43bc9d74ea;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;74;-6147.807,-1403.69;Inherit;True;Property;_Emissive_Noise_Texture;Emissive_Noise_Texture;0;0;Create;True;0;0;0;False;0;False;-1;57e9e92fd03983b41bdbda66dc81e6b2;57e9e92fd03983b41bdbda66dc81e6b2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexCoordVertexDataNode;129;-5957.653,-1760.133;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;173;-4817.065,-292.4485;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;81;-5718.896,-1398.141;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;85;-5094.145,-2022.765;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.ClampOpNode;117;-4554.023,-306.2157;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;89;-5142.942,-1906.971;Inherit;False;Property;_ColorNoise_Speed;ColorNoise_Speed;13;0;Create;True;0;0;0;False;0;False;1,1;-0.6,0.2;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.OneMinusNode;138;-5721.327,-1662.71;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;121;-4509.067,-97.40008;Inherit;False;Property;_Opacity_Boost;Opacity_Boost;16;0;Create;True;0;0;0;False;0;False;1;1.11;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;170;-4022.425,-153.2463;Inherit;True;Property;_Texture1;Texture 1;23;0;Create;True;0;0;0;False;0;False;3fd6b115a58e2cb4aae698542657e9fd;56dc720491a71e34a8ec34eeb5da8047;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.TFHCRemapNode;132;-5517.003,-1661.071;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;133;-5468.749,-1400.28;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;90;-4921.942,-2197.969;Inherit;False;Property;_ColorNoise_Scale;ColorNoise_Scale;12;0;Create;True;0;0;0;False;0;False;1,1;2.5,4;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;120;-4272.825,-312.4145;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;88;-4850.143,-2006.765;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;169;-3712.721,-329.5557;Inherit;True;Property;_TextureSample2;Texture Sample 2;1;0;Create;True;0;0;0;False;0;False;-1;3fd6b115a58e2cb4aae698542657e9fd;61327fc8f347d034a914bf43bc9d74ea;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;137;-5266.125,-1423.71;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexCoordVertexDataNode;24;-3938.916,-918.5826;Inherit;False;1;4;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;94;-4621.958,-2415.203;Inherit;False;Property;_Color_2;Color_2;19;0;Create;True;0;0;0;False;0;False;0.06587124,0,1,0;0.6980392,0.2196078,0.2117647,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WireNode;172;-3964.004,-589.7512;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;86;-4645.841,-2125.963;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;96;-5287.146,-1089.914;Inherit;False;Property;_Emissive_Color;Emissive_Color;20;0;Create;True;0;0;0;False;0;False;1,0,0.6261435,0;1,0.2627451,0.2509804,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;92;-4523.656,-2678.306;Inherit;False;Property;_Color_1;Color_1;18;0;Create;True;0;0;0;False;0;False;1,0,0.6261435,0;0.2980392,0.07843138,0.05882353,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;165;-3428.137,-653.0266;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;139;-5101.304,-1418.768;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;87;-4374.046,-2152.65;Inherit;True;Property;_Color_Noise_Texture;Color_Noise_Texture;4;0;Create;True;0;0;0;False;0;False;-1;57e9e92fd03983b41bdbda66dc81e6b2;57e9e92fd03983b41bdbda66dc81e6b2;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;98;-4877.577,-1085.931;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;95;-4313.958,-2416.203;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.ComponentMaskNode;93;-4211.118,-2680.013;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.OneMinusNode;49;-3332.509,-849.1835;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;174;-2976.794,-842.2327;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;0;False;4;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;97;-4624.403,-1436.977;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;128;-4015.213,-1995.052;Inherit;False;Property;_Color_Boost;Color_Boost;15;0;Create;True;0;0;0;False;0;False;1;1.06;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;29;-4546.065,-1134.978;Inherit;False;Property;_Emissive_Intensity;Emissive_Intensity;6;0;Create;True;0;0;0;False;0;False;3;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;91;-4033.351,-2232.746;Inherit;False;3;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;122;-3155.18,-649.752;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;99;-4317.585,-1455.6;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;127;-3801.395,-2086.865;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;2;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleAddOpNode;48;-2714.821,-658.2591;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;27;-2759.456,-1005.683;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ComponentMaskNode;30;-2486.832,-1162.47;Inherit;False;True;True;True;False;1;0;COLOR;0,0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.WireNode;126;-4033.474,-1071.71;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;119;-3586.946,-1562.113;Inherit;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SaturateNode;47;-2539.417,-813.2694;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;-2370.939,-904.4759;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.WireNode;124;-1937.906,-1407.705;Inherit;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;-2149.872,-1044.618;Inherit;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;6;-1796.883,-1091.369;Float;False;True;-1;2;ASEMaterialInspector;0;0;Lambert;SH_VFX_Veffects_Piercing;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;7;False;-1;False;0;False;-1;0;False;-1;False;0;Transparent;0.5;True;True;0;False;Transparent;;Transparent;All;18;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;True;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;True;Relative;0;;-1;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;False;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;143;0;141;0
WireConnection;143;1;140;0
WireConnection;144;0;142;0
WireConnection;144;1;143;0
WireConnection;171;0;161;1
WireConnection;145;1;144;0
WireConnection;163;0;145;1
WireConnection;163;1;171;0
WireConnection;148;0;163;0
WireConnection;149;0;148;0
WireConnection;149;1;150;0
WireConnection;111;0;108;0
WireConnection;111;1;112;0
WireConnection;152;0;149;0
WireConnection;109;0;113;0
WireConnection;109;1;111;0
WireConnection;110;1;109;0
WireConnection;181;0;182;0
WireConnection;181;1;180;0
WireConnection;71;0;73;0
WireConnection;71;1;80;0
WireConnection;76;0;79;0
WireConnection;76;1;71;0
WireConnection;176;0;110;1
WireConnection;176;1;175;0
WireConnection;100;0;166;0
WireConnection;100;1;181;0
WireConnection;178;0;179;0
WireConnection;178;1;177;0
WireConnection;168;0;100;1
WireConnection;168;1;176;0
WireConnection;158;0;76;0
WireConnection;158;1;157;0
WireConnection;53;0;167;0
WireConnection;53;1;178;0
WireConnection;74;1;158;0
WireConnection;173;0;100;1
WireConnection;173;1;168;0
WireConnection;81;0;53;2
WireConnection;81;1;74;1
WireConnection;117;0;173;0
WireConnection;138;0;129;4
WireConnection;132;0;138;0
WireConnection;133;0;81;0
WireConnection;120;0;117;0
WireConnection;120;1;121;0
WireConnection;88;0;85;0
WireConnection;88;1;89;0
WireConnection;169;0;170;0
WireConnection;137;0;132;0
WireConnection;137;1;133;0
WireConnection;172;0;120;0
WireConnection;86;0;90;0
WireConnection;86;1;88;0
WireConnection;165;0;172;0
WireConnection;165;1;169;3
WireConnection;139;0;137;0
WireConnection;87;1;86;0
WireConnection;98;0;96;0
WireConnection;95;0;94;0
WireConnection;93;0;92;0
WireConnection;49;0;24;3
WireConnection;174;0;49;0
WireConnection;97;0;139;0
WireConnection;97;1;98;0
WireConnection;91;0;93;0
WireConnection;91;1;95;0
WireConnection;91;2;87;1
WireConnection;122;0;165;0
WireConnection;99;0;97;0
WireConnection;99;1;29;0
WireConnection;127;0;91;0
WireConnection;127;1;128;0
WireConnection;48;0;174;0
WireConnection;48;1;122;0
WireConnection;30;0;27;0
WireConnection;126;0;99;0
WireConnection;119;0;127;0
WireConnection;119;1;117;0
WireConnection;47;0;48;0
WireConnection;43;0;27;4
WireConnection;43;1;47;0
WireConnection;124;0;119;0
WireConnection;31;0;30;0
WireConnection;31;1;126;0
WireConnection;6;0;124;0
WireConnection;6;2;31;0
WireConnection;6;9;43;0
ASEEND*/
//CHKSM=A78B94FC0B7E455BEDE4733BB7ACD11DBE64D2AD