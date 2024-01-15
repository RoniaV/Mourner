Shader "Custom/ClipBox"
{
    Properties {
        _radius("Size",Range(0.1,100)) = 5
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }

    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows addshadow
        #pragma target 3.0

        sampler2D _MainTex;
        half _Glossiness;
        half _Metallic;
        float4 _WorldToBox;
        float _radius;

        struct Input {
            float2 uv_MainTex;
            float3 worldPos;
        };

        void surf (Input IN, inout SurfaceOutputStandard o) {
            //float3 boxPosition = mul(_WorldToBox, float4(IN.worldPos, 1));
            // clip(boxPosition + 0.5);
            // clip(0.5 - boxPosition);
            clip (
                _radius * _radius - (
                    (_WorldToBox.x - IN.worldPos.x) * 
                    (_WorldToBox.x - IN.worldPos.x) + 
                    (_WorldToBox.z - IN.worldPos.z) * 
                    (_WorldToBox.z - IN.worldPos.z)
                    ));

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
