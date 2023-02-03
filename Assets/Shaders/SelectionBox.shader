// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/SelectionBox" {
    Properties {
        _MainColor("Main Color", Color) = (1, 1, 1, 1)
        _Thickness("Lines Thickness", Range(0.01, 0.05)) = 0.01
        _Size("Corner Size", Range(0.01, 0.1)) = 0.02
    }
    SubShader {
        Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
        LOD 200

        Pass {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            fixed4 _MainColor;
            float _Thickness;
            float _Size;

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
                color.a = _MainColor.a * 0.1;
                if ((i.texcoord0.x < _Thickness || i.texcoord0.x > 1.0 - _Thickness)) {
                    if (i.texcoord0.y < _Size || i.texcoord0.y > 1.0 - _Size) {
                        color.a = _MainColor.a;
                    }
                }
                if ((i.texcoord0.y < _Thickness || i.texcoord0.y > 1.0 - _Thickness)) {
                    if (i.texcoord0.x < _Size || i.texcoord0.x > 1.0 - _Size) {
                        color.a = _MainColor.a;
                    }
                }
                return color;
            }
            ENDCG
        }
    }
		
    FallBack "Diffuse"
}
