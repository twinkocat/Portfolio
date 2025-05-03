Shader "Custom/IndicatorRadial"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint Color", Color) = (1,1,1,1)
        _AngleDeg ("Angle (Degrees)", Range(0, 360)) = 90
        _Radius ("Radius", Range(0,1)) = 0.4
        _ForwardDir ("Forward Direction (XY)", Vector) = (0,0,1,0)
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float4 _ForwardDir;
            float _AngleDeg;
            float _Radius;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 screenUV : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.screenUV = v.uv * 2.0 - 1.0;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 coord = i.screenUV;
                float dist = length(coord);

                if (dist > _Radius)
                    discard;

                float angle = atan2(coord.y, coord.x);
                if (angle < 0) angle += 6.2831;

                // Розрахунок кута forward-вектора
                float2 fwd = normalize(_ForwardDir.xy);
                float forwardAngle = atan2(fwd.y, fwd.x);
                if (forwardAngle < 0) forwardAngle += 6.2831;

                // Конвертація кута сектора у радіани
                float angleRad = _AngleDeg * 0.0174533; // (π / 180)
                float halfAngle = angleRad * 0.5;

                // Межі сектора
                float minAngle = forwardAngle - halfAngle;
                float maxAngle = forwardAngle + halfAngle;

                // Врахування wrap-around (кутів понад 2π або < 0)
                bool inside = false;
                if (minAngle < 0.0) {
                    inside = (angle >= (6.2831 + minAngle)) || (angle <= maxAngle);
                }
                else if (maxAngle > 6.2831) {
                    inside = (angle >= minAngle) || (angle <= (maxAngle - 6.2831));
                }
                else {
                    inside = (angle >= minAngle) && (angle <= maxAngle);
                }
                float4 texColor = tex2D(_MainTex, i.uv);
                
                return inside ?  texColor * _Color : float4(0, 0, 0, 0);
            }
            ENDCG
        }
    }
}
