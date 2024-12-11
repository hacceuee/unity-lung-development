/*******************************************************
* A Square's PBR Shader with Transparency fixes.
*
* Per feedback and research into shaders, we created a PBR shader with access to
* all maps and properties and combined it with the transparency fixes from jhocking's
* post.
*
* https://discussions.unity.com/t/737870 page-4
*
*******************************************************/


Shader "Custom/TransparencyFix" {
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _MetallicGlossMap ("Metallic (B) Gloss (G)", 2D) = "white" {}
        _Roughness ("Roughness", Range(0,1)) = 1
        _Metallic ("Metallic", Range(0,1)) = 1
        [Normal] _BumpMap ("Normal", 2D) = "bump" {}
        _BumpScale("NormalScale", Float) = 1.0
        _OcclusionMap ("Occlusion (R)", 2D) = "white" {}
        _EmissionMap ("Emission", 2D) = "black" {}
        _EmissionColor ("Emission Color", Color) = (0,0,0,0)
    } // END Properties

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 200

        // ---------- Start Pass 1 ----------
        Pass
        {
            ZWrite On
            //Cull Off
            ColorMask 0

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

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                clip(col.a - .97);
                return col;
            }

            ENDCG

        } // ---------- END Pass 1----------

        // ---------- Start Pass 2 ----------
            ZWrite Off
            //Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            // Physically based Standard lighting model, and enable shadows on all light types
            #pragma surface surf Standard fullforwardshadows alpha:fade
            // Use shader model 3.0 target, to get nicer looking lighting
            #pragma target 3.0

            sampler2D _MainTex;
            sampler2D _MetallicGlossMap;
            sampler2D _BumpMap;
            sampler2D _OcclusionMap;
            sampler2D _EmissionMap;

            struct Input
            {
                float2 uv_MainTex;
                float2 uv_BumpMap;
                float2 uv_MetallicGlossMap;
                float2 uv_OcclusionMap;
                float2 uv_EmissionMap;
                float4 color : COLOR;
            };

            half _Roughness;
            half _Metallic;
            half _AlphaCutoff;
            half _BumpScale;
            fixed4 _Color;
            fixed4 _EmissionColor;

            // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
            // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
            // #pragma instancing_options assumeuniformscaling
            UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
            UNITY_INSTANCING_BUFFER_END(Props)

            void surf (Input IN, inout SurfaceOutputStandard o)
            {
                // Albedo comes from a texture tinted by color
                fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
                o.Albedo = c.rgb * IN.color;
                o.Alpha = c.a;
                // Metallic comes from blue channel tinted by slider variables
                fixed4 m = tex2D (_MetallicGlossMap, IN.uv_MetallicGlossMap);
                o.Metallic = m.b * _Metallic;
                // Smoothness comes from blue channel tinted by slider variables
                o.Smoothness = 1 - (m.g * _Roughness);
                // Normal comes from a bump map
                o.Normal = UnpackScaleNormal(tex2D (_BumpMap, IN.uv_BumpMap), _BumpScale);
                // Ambient Occlusion comes from red channel
                o.Occlusion = tex2D (_OcclusionMap, IN.uv_OcclusionMap).r;
                // Emission comes from a texture tinted by color
                o.Emission = tex2D (_EmissionMap, IN.uv_EmissionMap) * _EmissionColor;
            }

            ENDCG
        // ---------- END Pass 2 ----------
    }

    FallBack "Diffuse"
}