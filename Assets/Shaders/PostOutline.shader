Shader "Custom/PostOutline"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "black" {}
        _SceneTex ("Scene Texture", 2D) = "black" {}
        _Kernel ("Gaussian Kernel", Vector) = (0, 0, 0, 0)
        _KernelWidth ("Gaussian Kernel Width", Float) = 1
        _OutlineColor ("Outline Color", Color) = (0, 1, 1, 1)
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _SceneTex;

            // 'TexelSize' is a float2 that says how much screen space a texel occupies
            float2 _MainTex_TexelSize;
            float _Kernel[21];
            float _KernelWidth;

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uvs : TEXCOORD0;
            };

            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uvs = o.pos.xy / 2 + 0.5;
                return o;
            }

            half4 frag (v2f i) : COLOR 
            {
                int numberOfIterations = _KernelWidth;

                float TX_x = _MainTex_TexelSize.x;
                float TX_y = _MainTex_TexelSize.y;
                float colorIntensityRadius = 0;

                // Horizontal rendering
                for(int k = 0; k < numberOfIterations; k++)
                {
                    colorIntensityRadius += _Kernel[k] * tex2D(
                        _MainTex,
                        float2(
                            i.uvs.x + (k - numberOfIterations / 2) * TX_x,
                            i.uvs.y
                        )
                    ).r;
                }

                return colorIntensityRadius;
            }
            ENDCG
        }

        GrabPass{}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _SceneTex;
            sampler2D _GrabTexture;
            float2 _GrabTexture_TexelSize;
            float _Kernel[21];
            float _KernelWidth;
            half4 _OutlineColor;

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uvs : TEXCOORD0;
            };

            v2f vert (appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uvs = o.pos.xy / 2 + 0.5;
                return o;
            }

            float4 frag(v2f i) : COLOR
            {
                float TX_x = _GrabTexture_TexelSize.x;
                float TX_y = _GrabTexture_TexelSize.y;

                // If something exists underneath fragment, draw scene instead
                if (tex2D(_MainTex, i.uvs.xy).r > 0)
                {
                    return tex2D(_SceneTex, i.uvs.xy);
                }

                int numberOfIterations = _KernelWidth;
                float4 colorIntensityRadius = 0;

                for (int k = 0; k < numberOfIterations; k++)
                {
                    colorIntensityRadius += _Kernel[k] * tex2D(
                        _GrabTexture,
                        float2(
                            i.uvs.x,
                            1 - i.uvs.y + (k - numberOfIterations / 2) * TX_y
                        )
                    );
                }

                // Output the scene's color and create outline
                half4 color = tex2D(_SceneTex, i.uvs.xy) + colorIntensityRadius * _OutlineColor;
                return color;
            }
            ENDCG
        }
    }
}
