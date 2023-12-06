Shader"ImageEffect/RadialBlurImageEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurSize ("Blur Size", Range(0, 1)) = 0.1
        _BlurCenterPos ("Blur Center Position", Vector) = (0, 0, 0, 0)
        _Samples ("Samples", Range(1, 48)) = 5
    }
    SubShader
    {
        Pass
        {
Cull Off

ZWrite Off

ZTest Always
            Fog {
Mode off}
    
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
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

v2f vert(appdata v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = v.uv;
    return o;
}

sampler2D _MainTex;
half4 _MainTex_TexelSize;
float2 _BlurCenterPos;
float _BlurSize;
float _Samples;

half4 frag(v2f IN) : SV_Target
{
    half4 col = half4(0, 0, 0, 1);
    float2 movedTexcoord = IN.uv - _BlurCenterPos;
    
    for (int i = 0; i < _Samples; i++)
    {
        half Scale = 1.0f - _BlurSize * _MainTex_TexelSize.x * i;
        col.rgb += tex2D(_MainTex, movedTexcoord * Scale + _BlurCenterPos).rgb;
    }
    col.rgb *= 1 / _Samples;
    
    
    //float2 movedTexcoord = IN.uv - _BlurCenterPos * _MainTex_TexelSize.xy;

    //if (_Samples > 0)
    //{
    //    for (int i = 0; i < _Samples; i++)
    //    {
    //        float scale = 1.0 - _BlurSize * i;
    //        col.rgb += tex2D(_MainTex, movedTexcoord * scale + _BlurCenterPos).rgb;
    //    }
    //    col.rgb /= _Samples;
    //}

    return col;
}
            ENDCG
        }
    }
}