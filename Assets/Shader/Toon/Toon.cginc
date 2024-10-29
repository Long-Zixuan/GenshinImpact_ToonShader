//LZX completed this header file in 2024/10/12   16:37
//LZX-TC-2024-10-12-001
#pragma shader_feature _IS_FACE_ON
#pragma shader_feature _SCREAM_SPACE_OUTLINE_ON 
#pragma shader_feature _NORMAL_OUTLINE_ON 
#pragma shader_feature _IS_DEBUG_ON
#pragma shader_feature _IS_RIMLIGHT_DEBUG_ON
           
#include "UnityCG.cginc"
#include "Lighting.cginc"
float3 _Skylight;
sampler2D _MainTex;
sampler2D _IlmTex;
sampler2D _GradationMap;

float _RimWidth;
float _RimIntensity;
float _HighlightsIntensity;
float _ExcessiveWidth;
float _GradationMin;
float _GradationMax;
float _shininess;
fixed3 _specColor;
float _GradationRampValue;

float _RimOffect;
float _Threshold;
sampler2D _CameraDepthTexture; 
float _RimLightStrength;

sampler2D _MatCapTex;
float _MatCapIntensity;
float _MatCapPow;
float _MatCapUVScale;
       
half _OutlineWidthMax;
half _OutlineWidthMin;
half _OutLineWigth;
    float _OutlineThreshold;
half4 _OutLineColor;


float4 _XRayCol;
float _XRayFresPow;
sampler2D _EyesMask;

float _LerpMax;
float3 _Front;
float3 _UP;
float3 _LeftDir;


struct c2v
{
    float4 vertex:POSITION;
    float3 normal:NORMAL;
    float2 uv:TEXCOORD0;

    float4 vertColor : COLOR;
    float4 tangent : TANGENT;
};

struct v2f
{
    float4 pos:SV_POSITION;
    float3 worldPos :TEXCOORD2;
    float3 worldNormal:NORMAL;
    float2 uv:TEXCOORD;
    float3 objViewDir:COLOR1;
    float3 normal:NORMAL2;
    float clipW:TEXCOORD1;

    float3 wNor : TEXCOORD3;
    float3 wPos : TEXCOORD4;
};


float GetBlinnPhongSpec(float3 worldPos, float3 worldNormal)
{
    float3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos));
    float3 halfDir = normalize((viewDir + _WorldSpaceLightPos0.xyz));
    float specDir = max(dot(normalize(worldNormal), halfDir), 0);
    float specVal = pow(specDir, _shininess);
    return specVal;
}


float2 GetMatCapUV(float3 normalWorld)
{
	float3 normalView = mul(UNITY_MATRIX_IT_MV, normalWorld);
	return normalView.xy*0.5 + 0.5;
}



v2f vert(c2v input)
{
    v2f output;
    output.pos = UnityObjectToClipPos(input.vertex);
    output.worldNormal = normalize( mul((float3x3)unity_ObjectToWorld,input.normal) );
    output.uv = input.uv;

    output.worldPos = mul(unity_ObjectToWorld, input.vertex).xyz;

    float3 ObjViewDir = normalize(ObjSpaceViewDir(input.vertex));
    output.objViewDir = ObjViewDir;
    output.normal = normalize(input.normal);
    output.clipW = output.pos.w;

    output.wNor = UnityObjectToWorldNormal(input.normal);
    output.wPos = mul(unity_ObjectToWorld, input.vertex).xyz;

    return output;
}


v2f vertOutline (c2v v) 
{
    #ifdef _NORMAL_OUTLINE_ON
    v2f o;
               

    UNITY_INITIALIZE_OUTPUT(v2f, o);
    float4 pos = UnityObjectToClipPos(v.vertex);
    float3 viewNormal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal.xyz);
    float3 ndcNormal = normalize(TransformViewToProjection(viewNormal.xyz)) * pos.w;//将法线变换到NDC空间
    float4 nearUpperRight = mul(unity_CameraInvProjection, float4(1, 1, UNITY_NEAR_CLIP_VALUE, _ProjectionParams.y));//将近裁剪面右上角位置的顶点变换到观察空间
    float aspect = abs(nearUpperRight.y / nearUpperRight.x);//求得屏幕宽高比
    ndcNormal.x *= aspect;

    half width = sqrt(pow(ndcNormal.x,2)+pow(ndcNormal.y,2)) * _OutLineWigth;


    half OutlineWidth;
    if(width>_OutlineWidthMax)
    {
        OutlineWidth = _OutlineWidthMax;
    }
    else if(_OutlineWidthMax >= width && width >= _OutlineWidthMin)
    {
        OutlineWidth = width;
    }
    else if(width < _OutlineWidthMin)
    {
        OutlineWidth = _OutlineWidthMin;
    }

                
                
    //pos.xy += 0.01 * _OutlineWidthMax * ndcNormal.xy;
    pos.xy += 0.01 * OutlineWidth * normalize( ndcNormal.xy );
    o.pos = pos;
    return o;

	#endif
    return vert(v);
}


fixed4 fragEyes (v2f i) : SV_Target
{
    #ifdef _IS_FACE_ON
				
    float3 vDir = normalize(_WorldSpaceCameraPos.xyz - i.wPos);
    float flag = tex2D(_EyesMask, i.uv).r;
    if(flag < 0.1)
    {
        return (1,0,0,0);
    }
    return _XRayCol;

    #endif
    return fixed4(0,0,0,0);
				
}

fixed4 fragOutline(v2f i) : SV_TARGET
{
	#ifdef _NORMAL_OUTLINE_ON
    return _OutLineColor;
	#endif
	return fixed4(0,0,0,0);

}


           

fixed4 fragFaceLight(in v2f input):SV_TARGET0
{
    fixed3 originCol = tex2D(_MainTex, input.uv).rgb;
    #ifdef _IS_DEBUG_ON
	originCol = fixed3(1,1,1);
	#endif
    fixed3 diffuseColor = _LightColor0.rgb * originCol;
    //fixed3 diffuseColor =  tex2D(_MainTex, input.uv).rgb ;
    // float isSahdow = 0;
    //这张阈值图代表的是阴影在灯光从正前方移动到左后方的变化
    half4 ilmTex = tex2D(_IlmTex, input.uv);
    //这张阈值图代表的是阴影在灯光从正前方移动到右后方的变化
    half4 r_ilmTex = tex2D(_IlmTex, float2(1 - input.uv.x, input.uv.y));
         
    float3 RightDir = -_LeftDir;
    float2 Left = normalize(float2(_LeftDir.x,_LeftDir.z));	//世界空间角色正左侧方向向量
    float2 Front = normalize(float2(_Front.x,_Front.z));	//世界空间角色正前方向向量


    float2 LightDir = normalize(float2(_WorldSpaceLightPos0.x,_WorldSpaceLightPos0.z));

    float ctrl = (acos(dot(Front, LightDir))/3.1415926);

     

    float ilm = dot(LightDir, Left) > 0 ? ilmTex.r : r_ilmTex.r;//确定采样的贴图

                
    //isSahdow = step(ilm, ctrl);//if(ctrl>=ilm) isSahdow = 1 


    float bias = smoothstep(0, _LerpMax, ctrl - ilm);

    fixed3 col = UNITY_LIGHTMODEL_AMBIENT.xyz + diffuseColor;
    fixed3 sssCol = originCol * _Skylight;




    float2 objViewDirXZ = float2(input.objViewDir.x,input.objViewDir.z);

    fixed4 finalCol = fixed4(col,1);
    fixed4 finalsssCol = fixed4(sssCol,1);
    return lerp(finalCol , finalsssCol ,bias);
}

fixed4 fragSkinLight(in v2f input):SV_TARGET0
{
    //return fixed3(tex2D(_IlmTex, input.uv).g,tex2D(_IlmTex, input.uv).g,tex2D(_IlmTex, input.uv).g);
    fixed4 originColor;

    #ifdef _IS_DEBUG_ON
    originColor = fixed4(1,1,1,1);
    #else
    originColor = tex2D(_MainTex, input.uv);
    #endif

    fixed4 diffuseColor = _LightColor0.rgba *  originColor.rgba ;

    fixed4 col = diffuseColor;//UNITY_LIGHTMODEL_AMBIENT.xyz + diffuseColor;


    fixed3 worldNormal = normalize(input.worldNormal);
	fixed3 worldLight = normalize(_WorldSpaceLightPos0.xyz);
    fixed3 worldLightXZ = normalize(fixed3(_WorldSpaceLightPos0.x,0,_WorldSpaceLightPos0.z));
	fixed3 diffuse = _LightColor0.rgb * (dot(worldNormal, worldLightXZ) * 0.5 + 0.5);
    half4 GradationCol = tex2D(_GradationMap, float2(diffuse.z, _GradationRampValue));
    fixed3 sssCol = originColor.rgb * _Skylight;
    fixed3 ilmCol = tex2D(_IlmTex, input.uv);

    if(ilmCol.g<0.1)
    {
        return fixed4(col * _Skylight,1);
    }

    if(ilmCol.g == 1)
    {
        return col; //* toonVal * GradationCol * (1-ilmCol.b)+MatCapCol.rgb*ilmCol.b;
    }
               
    half toonVal = smoothstep(_GradationMin, _GradationMax, diffuse);
	//高光
	half specVal = GetBlinnPhongSpec(input.worldPos, input.worldNormal);	

    float2 MatCapUV = GetMatCapUV(input.worldNormal)*_MatCapUVScale;
	float4 MatCapCol = tex2D(_MatCapTex, MatCapUV)*_MatCapIntensity;
	MatCapCol = pow(MatCapCol, _MatCapPow);
                
	half3 finalRGB = col.rgb * toonVal * GradationCol+ half3(sssCol.r,sssCol.g,sssCol.b) * (1 - toonVal) * (GradationCol);// + _specColor * specVal;
    finalRGB = finalRGB * (1-ilmCol.b)+MatCapCol.rgb*ilmCol.b;


    float2 screenParams01 = float2(input.pos.x/_ScreenParams.x,input.pos.y/_ScreenParams.y);
                
	half alpha = sssCol.r;
    return half4(finalRGB,1);
    //return half4(finalRGB,alpha);
  
}
fixed4 fragLight(in v2f input):SV_TARGET0
{
    #ifdef _IS_RIMLIGHT_DEBUG_ON
    return fixed4(0,0,0,1);
	#endif

	#ifdef _IS_FACE_ON
	return fragFaceLight(input);
	#else
	return fragSkinLight(input);
	#endif
}
            

fixed4 fragFaceRimLight(in v2f input)// : SV_Target
{
    fixed3 originCol;

    #ifdef _IS_DEBUG_ON
    originCol = fixed3(1,1,1);
    #else
    originCol = tex2D(_MainTex, input.uv).rgb;
    #endif

    float2 objViewDirXZ = float2(input.objViewDir.x,input.objViewDir.z);
    float2 Left = normalize(float2(_LeftDir.x,_LeftDir.z));	//世界空间角色正左侧方向向量
    float isLeft = step(0,dot(objViewDirXZ,Left));//if(0 > dot(objViewDirXZ,Left)) return 0;

    float coefficient = 1 - 2 * isLeft;

    //只有一边有边缘光
    float2 screenParams01 = float2(input.pos.x/_ScreenParams.x,input.pos.y/_ScreenParams.y);
    float2 offectSamplePos = screenParams01-float2(_RimOffect * coefficient/input.clipW,0);
    float offcetDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, offectSamplePos);
    float trueDepth   = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, screenParams01);
    float linear01EyeOffectDepth = Linear01Depth(offcetDepth);
    float linear01EyeTrueDepth = Linear01Depth(trueDepth);
    float depthDiffer = linear01EyeOffectDepth-linear01EyeTrueDepth;
    float rimIntensity = step(_Threshold,depthDiffer);

    return fixed4(originCol * _RimLightStrength,rimIntensity);
}


fixed4 fragSkinRimLight(in v2f input)// : SV_Target
{
    fixed3 originCol;

    #ifdef _IS_DEBUG_ON
	originCol = fixed3(1,1,1);
	#else
    originCol = tex2D(_MainTex, input.uv).rgb;
	#endif

    /*fixed3 diffuseColor = _LightColor0.rgb * originCol;
    fixed3 col = UNITY_LIGHTMODEL_AMBIENT.xyz + diffuseColor;*/

	float2 screenParams01 = float2(input.pos.x/_ScreenParams.x,input.pos.y/_ScreenParams.y);
    //只有一边有边缘光
    /*float2 offectSamplePos = screenParams01-float2(_RimOffect/input.clipW,0);
    float offcetDepth = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, offectSamplePos);
    float trueDepth   = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, screenParams01);
    float linear01EyeOffectDepth = Linear01Depth(offcetDepth);
    float linear01EyeTrueDepth = Linear01Depth(trueDepth);
    float depthDiffer = linear01EyeOffectDepth-linear01EyeTrueDepth;
    float rimIntensity = step(_Threshold,depthDiffer);*/
    //两边都有边缘光
    float2 offectSamplePosLeft = screenParams01-float2(_RimOffect/input.clipW,0);
    float2 offectSamplePosRight = screenParams01+float2(_RimOffect/input.clipW,0);
    float offcetDepthLeft = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, offectSamplePosLeft);
	float offcetDepthRight = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, offectSamplePosRight);
    float trueDepth  = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, screenParams01);
    float linear01EyeOffectDepthLeft = Linear01Depth(offcetDepthLeft);
	float linear01EyeOffectDepthRight = Linear01Depth(offcetDepthRight);
    float linear01EyeTrueDepth = Linear01Depth(trueDepth);
    float depthDifferLeft = linear01EyeOffectDepthLeft-linear01EyeTrueDepth;
	float depthDifferRight = linear01EyeOffectDepthRight-linear01EyeTrueDepth;
    float rimIntensityLeft = step(_Threshold,depthDifferLeft);
    float rimIntensityRight = step(_Threshold,depthDifferRight);
    float rimIntensity = max(rimIntensityLeft,rimIntensityRight);
               
    return fixed4(originCol * _RimLightStrength,rimIntensity);
}

fixed4 fragScreamSpaceOutline(in v2f input) : SV_Target
{
#ifdef _SCREAM_SPACE_OUTLINE_ON
	float2 screenParams01 = float2(input.pos.x/_ScreenParams.x,input.pos.y/_ScreenParams.y);

    //四边采样
    float2 offectSamplePosLeft = screenParams01-float2(_OutLineWigth/input.clipW,0);
    float2 offectSamplePosRight = screenParams01+float2(_OutLineWigth/input.clipW,0);
    float2 offectSamplePosUp = screenParams01+float2(0,_OutLineWigth/input.clipW);
    float2 offectSamplePosDown = screenParams01-float2(0,_OutLineWigth/input.clipW);

    float offcetDepthLeft = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, offectSamplePosLeft);
	float offcetDepthRight = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, offectSamplePosRight);
    float offcetDepthUp = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, offectSamplePosUp);
    float offcetDepthDown = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, offectSamplePosDown);

    float trueDepth  = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, screenParams01);
    float linear01EyeOffectDepthLeft = Linear01Depth(offcetDepthLeft);
	float linear01EyeOffectDepthRight = Linear01Depth(offcetDepthRight);
    float linear01EyeOffectDepthUp = Linear01Depth(offcetDepthUp);
    float linear01EyeOffectDepthDown = Linear01Depth(offcetDepthDown);

    float linear01EyeTrueDepth = Linear01Depth(trueDepth);
    float depthDifferLeft = linear01EyeOffectDepthLeft-linear01EyeTrueDepth;
	float depthDifferRight = linear01EyeOffectDepthRight-linear01EyeTrueDepth;
	float depthDifferUp = linear01EyeOffectDepthUp-linear01EyeTrueDepth;
	float depthDifferDown = linear01EyeOffectDepthDown-linear01EyeTrueDepth;

    float rimIntensityLeft = step(_OutlineThreshold,depthDifferLeft);
    float rimIntensityRight = step(_OutlineThreshold,depthDifferRight);
	float rimIntensityUp = step(_OutlineThreshold,depthDifferUp);
	float rimIntensityDown = step(_OutlineThreshold,depthDifferDown);

    float rimIntensityH = max(rimIntensityLeft,rimIntensityRight);
    float rimIntensityV = max(rimIntensityDown,rimIntensityUp);
    float rimIntensity = max(rimIntensityH,rimIntensityV);
    //rimIntensity = 1 - rimIntensity;
    //return fixed4(rimIntensity,rimIntensity,rimIntensity,1);
    return fixed4(_OutLineColor.rgb,rimIntensity);
#endif
    return fixed4(0,0,0,0);
}


fixed4 fragRimLight(in v2f input) : SV_Target
{
    #ifdef _IS_FACE_ON
		return fragFaceRimLight(input);
	#else
		return fragSkinRimLight(input);
	#endif
}
  
  


//||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
//||||--==||||||||||||||||------==||||||||||----------==||||
//||||--==||||||||||||||------------==||||||------------==||
//||||--==|||||||||||||---==||||----==||||||--==||||----==||
//||||--==|||||||||||||---==||||||||||||||||--------------||
//||||--==|||||||||||||---==||||----==||||||----------==||||
//||||------------==||||------------==||||||--==||||||||||||
//||||------------==||||||------==||||||||||--==||||||||||||
//||||||||||||||||||||||||||||||||||||||||||||||||||||||||||
//|||||||||||||||||LZX.Celluloid.Project||||||||||||||||||||

                                                       
                                                          
                                                          