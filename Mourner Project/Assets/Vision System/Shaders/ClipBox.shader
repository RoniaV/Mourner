Shader "Custom/ClipSpace"
{
    Properties {
        //_radius ("Radius", float) = 3
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
        float4 _worldCenter;
        float _radius;
        float _toggle;

        struct Input {
            float2 uv_MainTex;
            float3 worldPos;
        };

        void surf (Input IN, inout SurfaceOutputStandard o) {
            //float3 boxPosition = mul(_WorldToBox, float4(IN.worldPos, 1));
            // clip(boxPosition + 0.5);
            // clip(0.5 - boxPosition);
            if(_toggle < 1) discard;

            clip (
                _radius * _radius - (
                    (_worldCenter.x - IN.worldPos.x) * 
                    (_worldCenter.x - IN.worldPos.x) + 
                    (_worldCenter.z - IN.worldPos.z) * 
                    (_worldCenter.z - IN.worldPos.z)
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
