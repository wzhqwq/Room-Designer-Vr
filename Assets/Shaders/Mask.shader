Shader "Unlit/Mask" {
    Properties {
        _Color("Color", Color) = (0, 0, 0, 1)
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

            fixed4 _Color;

            float4 vert(float4 v:POSITION) : SV_POSITION {
                return UnityObjectToClipPos (v);
            }

            fixed4 frag() : SV_Target {
                return _Color;
            }
            ENDCG
        }
    }
}
