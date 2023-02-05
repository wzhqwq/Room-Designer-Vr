// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Indicator" {
    Properties {
        _MainColor("Main Color", Color) = (1, 1, 1, 1)
        _Color("Inner Color", Color) = (1, 1, 1, 1)
        _Thickness("Line Thickness", Range(0.01, 0.1)) = 0.01
        _Radius("Circle Radius", Range(0.1, 0.5)) = 0.1
    }
    SubShader {
        Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
        LOD 200

        Pass {
            ZWrite Off
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile ANIMATION_ON

            #include "UnityCG.cginc"

            fixed4 _MainColor;
            fixed4 _Color;
            float _Thickness;
            float _Radius;

            struct vertexInput {
                float4 vertex : POSITION;
                float4 texcoord0 : TEXCOORD0;
            };

            struct fragmentInput{
                float4 position : SV_POSITION;
                float4 texcoord0 : TEXCOORD0;
            };

            fragmentInput vert(vertexInput i){
                fragmentInput o;
                o.position = UnityObjectToClipPos (i.vertex);
                o.texcoord0 = i.texcoord0;
                return o;
            }

            fixed4 frag(fragmentInput i) : SV_Target {
                fixed4 color;
                color = _MainColor;
                color.a = 0;
                float d = length(i.texcoord0.xy - float2(0.5, 0.5));
                if (d < _Radius)
                    color.a = lerp(0, _MainColor.a, (_Radius - d) / 0.002f);
                if (d < _Radius - _Thickness)
                    color = lerp(color, _Color, min(1, (_Radius - _Thickness - d) / 0.002f));
                return color;
            }
            ENDCG
        }
    }
		
    FallBack "Diffuse"
}
