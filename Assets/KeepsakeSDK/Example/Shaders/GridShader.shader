Shader "Custom/Grid" {
    Properties
    {
      _Magnitude("Magnitude", Float) = 8
    }
        SubShader{
            Pass {
                CGPROGRAM
                #pragma vertex vert_img
                #pragma fragment frag
                float _Magnitude;

                #include "UnityCG.cginc"

                fixed4 frag(v2f_img i) : SV_Target {
                    bool p = fmod(i.uv.x * _Magnitude,2.0) < 1.0;
                    bool q = fmod(i.uv.y * _Magnitude,2.0) > 1.0;
                    bool c = p ^ q;

                    return fixed4(c, c, c, 1.0);
                }
                ENDCG
            }
    }
}