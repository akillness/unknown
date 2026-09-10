Shader "Tide/Candidate Roughness" {
 Properties { _BaseMap("Authored sRGB base color",2D)="white"{} _RoughnessMap("Authored linear roughness",2D)="white"{} _Metallic("Authored metallic factor",Range(0,1))=0 }
 SubShader { Tags {"RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" "Queue"="Geometry"}
  Pass { Name "ForwardLit" Tags {"LightMode"="UniversalForward"}
   HLSLPROGRAM
   #pragma vertex Vert
   #pragma fragment Frag
   #pragma target 3.0
   #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
   #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
   TEXTURE2D(_BaseMap);SAMPLER(sampler_BaseMap);TEXTURE2D(_RoughnessMap);SAMPLER(sampler_RoughnessMap);
   CBUFFER_START(UnityPerMaterial) float4 _BaseMap_ST;half _Metallic; CBUFFER_END
   struct Attributes {float4 positionOS:POSITION;float3 normalOS:NORMAL;float2 uv:TEXCOORD0;};
   struct Varyings {float4 positionCS:SV_POSITION;float3 positionWS:TEXCOORD0;half3 normalWS:TEXCOORD1;float2 uv:TEXCOORD2;};
   Varyings Vert(Attributes v){Varyings o;VertexPositionInputs p=GetVertexPositionInputs(v.positionOS.xyz);o.positionCS=p.positionCS;o.positionWS=p.positionWS;o.normalWS=TransformObjectToWorldNormal(v.normalOS);o.uv=TRANSFORM_TEX(v.uv,_BaseMap);return o;}
   half4 Frag(Varyings v):SV_Target {InputData input=(InputData)0;input.positionWS=v.positionWS;input.normalWS=normalize(v.normalWS);input.viewDirectionWS=GetWorldSpaceNormalizeViewDir(v.positionWS);input.bakedGI=SampleSH(input.normalWS);input.shadowCoord=TransformWorldToShadowCoord(v.positionWS);input.normalizedScreenSpaceUV=GetNormalizedScreenSpaceUV(v.positionCS);input.shadowMask=half4(1,1,1,1);
    SurfaceData surface=(SurfaceData)0;surface.albedo=SAMPLE_TEXTURE2D(_BaseMap,sampler_BaseMap,v.uv).rgb;surface.alpha=1;surface.metallic=_Metallic;surface.smoothness=1-SAMPLE_TEXTURE2D(_RoughnessMap,sampler_RoughnessMap,v.uv).r;surface.normalTS=half3(0,0,1);surface.occlusion=1;return UniversalFragmentPBR(input,surface);}
   ENDHLSL
  }
 }
}
