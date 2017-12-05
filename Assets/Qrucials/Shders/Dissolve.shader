Shader "Custom/Dissolve3D"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
 
        [Header(Dissolution)]
        _DisVal ("Threshold", Range(0., 1.01)) = 0.
 
        [Header(Ambient)]
        _Ambient ("Intensity", Range(0., 1.)) = 0.1
        _AmbColor ("Color", color) = (1., 1., 1., 1.)
 
        [Header(Diffuse)]
        _Diffuse ("Val", Range(0., 1.)) = 1.
        _DifColor ("Color", color) = (1., 1., 1., 1.)
 
        [Header(Specular)]
        [Toggle] _Spec("Enabled?", Float) = 0.
        _Shininess ("Shininess", Range(0.1, 10)) = 1.
        _SpecColor ("Specular color", color) = (1., 1., 1., 1.)
 
        [Header(Emission)]
        _EmissionTex ("Emission texture", 2D) = "gray" {}
        _EmiVal ("Intensity", float) = 0.
        [HDR]_EmiColor ("Color", color) = (1., 1., 1., 1.)
    }
 
    SubShader
    {
        Pass
        {
            Tags { "RenderType"="Transparent" "Queue"="Transparent" "LightMode"="ForwardBase" }
 
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
 
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
 
            #include "UnityCG.cginc"
            // Change path if needed

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 worldNormal : TEXCOORD2;
            };
 
            v2f vert(appdata_base v)
            {
                v2f o;
                // World position
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
 
                // Clip position
                o.pos = mul(UNITY_MATRIX_VP, float4(o.worldPos, 1.));
 
                // Normal in WorldSpace
                o.worldNormal = normalize(mul(v.normal, (float3x3)unity_WorldToObject));
 
                o.uv = v.texcoord;
 
                return o;
            }
 
			float hash( float n )
			{
				return frac(sin(n)*43758.5453);
			}
 
			float noise( float3 x )
			{
				// The noise function returns a value in the range -1.0f -> 1.0f
 
				float3 p = floor(x);
				float3 f = frac(x);
 
				f       = f*f*(3.0-2.0*f);
				float n = p.x + p.y*57.0 + 113.0*p.z;
 
				return lerp(lerp(lerp( hash(n+0.0), hash(n+1.0),f.x),
							   lerp( hash(n+57.0), hash(n+58.0),f.x),f.y),
						   lerp(lerp( hash(n+113.0), hash(n+114.0),f.x),
							   lerp( hash(n+170.0), hash(n+171.0),f.x),f.y),f.z);
			}

            sampler2D _MainTex;
 
            fixed4 _LightColor0;
           
            // Diffuse
            fixed _Diffuse;
            fixed4 _DifColor;
 
            //Specular
            fixed _Shininess;
            fixed4 _SpecColor;
           
            //Ambient
            fixed _Ambient;
            fixed4 _AmbColor;
 
            // Emission
            sampler2D _EmissionTex;
            fixed4 _EmiColor;
            fixed _EmiVal;
 
            // Dissolution
            fixed _DisVal;
 
            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 c = tex2D(_MainTex, i.uv);
 
                // Light direction
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
 
                // Camera direction
                float3 viewDir = normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz);
 
                float3 worldNormal = normalize(i.worldNormal);
 
                // Compute ambient lighting
                fixed4 amb = _Ambient * _AmbColor;
 
                // Compute the diffuse lighting
                fixed4 NdotL = max(0., dot(worldNormal, lightDir) * _LightColor0);
                fixed4 dif = NdotL * _Diffuse * _LightColor0 * _DifColor;
 
                fixed4 light = dif + amb;
 
                c.rgb *= light.rgb;
 
                // Compute emission
                fixed4 emi = tex2D(_EmissionTex, i.uv).r * _EmiColor * _EmiVal;
                c.rgb += emi.rgb;
 
                if(noise(i.worldPos) < _DisVal)
                    discard;
 
                return c;
            }
 
            ENDCG
        }
    }
}