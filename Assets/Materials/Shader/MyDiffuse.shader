Shader "Custom/MyDiffuse"
{
    Properties
    {
        [HDR] _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _NoiseTex("Noise", 2D) = "white" {}
        _Cutoff("Cut off", Range(0, 1)) = 0.25
        [HDR] _EmissionColor("Emmision Color", Color) = (0,0,0)
        _EmitTex("Emission Texture", 2D) = "white" {}
        _EdgeWidth("Edge Width", Range(0, 1)) = 0.05
        [HDR] _EdgeColor("Edge Color", Color) = (1,1,1,1)
        _SpecColor("Specular Color", Color) = (1,1,1,1)
        _Shininess("Shininess", Float) = 10
    }
    SubShader
    {
        Pass
        {
            Tags { "LightMode" = "ForwardBase" }
            CGPROGRAM
    
            //pragmas
            #pragma vertex vert
            #pragma fragment frag
            #pragma exclude_renderers flash
    
            //user defined variables
            fixed4 _Color;
            fixed4 _EdgeColor;     
            fixed4 _EmissionColor;

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _EmitTex;
            float4 _EmitTex_ST;
            
            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;
            
            half _Cutoff;
            half _EdgeWidth;
            
            float4 _SpecColor;
            float _Shininess;
    
            //unity defined variables
            float4 _LightColor0;
    
            //base input structs
            struct vertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord : TEXCOORD0;
                float4 noisecoord : TEXCOORD1;
                float4 emissioncoord : TEXCOORD2;
            };
    
            struct vertexOutput {
                float4 pos : SV_POSITION;
                float4 tex : TEXCOORD0;
                float4 noise : TEXCOORD1;
                float4 emission: TEXCOORD2;
                float4 posWorld : TEXCOORD3;
                float3 normalDir : TEXCOORD4;
            };
    
            //vertex function
            vertexOutput vert(vertexInput v)
            {
                vertexOutput o;
    
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.normalDir = normalize(mul(float4(v.normal, 0.0f), unity_WorldToObject).xyz);
                o.pos = UnityObjectToClipPos(v.vertex);
                o.tex = v.texcoord;
                o.noise = v.noisecoord;
                o.emission = v.emissioncoord;
    
                return o;
            }
    
            //fragment function
            float4 frag(vertexOutput i) : COLOR
            {
                float3 normalDirection = i.normalDir;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 lightDirection;
                float atten;
    
                if (_WorldSpaceLightPos0.w == 0.0f) //Directional lights
                {
                    atten = 1.0f;
                    lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                }
                else
                {
                    float3 fragmentToLightSource = _WorldSpaceLightPos0.xyz - i.posWorld.xyz;
                    float distance = length(fragmentToLightSource);
                    atten = 1.0f / distance;
                    lightDirection = normalize(fragmentToLightSource);
                }
    
                //Lighting
                float3 diffuseReflection = atten * _LightColor0.xyz * saturate(dot(normalDirection, lightDirection));
                float3 specularReflection = diffuseReflection * _SpecColor.xyz * pow(saturate(dot(reflect(-lightDirection, normalDirection), viewDirection)), max(1, _Shininess));
    
                float3 lightFinal = diffuseReflection + specularReflection + UNITY_LIGHTMODEL_AMBIENT.xyz;
    
                //TextureMaps
                float4 tex = tex2D(_MainTex, i.tex.xy * _MainTex_ST.xy + _MainTex_ST.zw);
                float4 emmision = tex2D(_EmitTex, i.emission.xy * _EmitTex_ST.xy + _EmitTex_ST.zw);

                //Dissolve
                float4 noise = tex2D(_NoiseTex, i.noise.xy * _NoiseTex_ST.xy + _NoiseTex_ST.zw);
                clip(noise.r >= _Cutoff ? 1 : -1);

                if (noise.r >= (_Cutoff * (_EdgeWidth + 1.0)))
                {
                    if (emmision.r > 0.5f)
                    {
                        return float4(tex.xyz * _EmissionColor.rgb, emmision.r);
                    }
                    else
                    {
                        return float4(tex.xyz * lightFinal * _Color.rgb, noise.r);
                    }
                }
                else
                {
                    return _EdgeColor;
                }
            }
            ENDCG
        }
    }
    //FallBack "Specular"
}