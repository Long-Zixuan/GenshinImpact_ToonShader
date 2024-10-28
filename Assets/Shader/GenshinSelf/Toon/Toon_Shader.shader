// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
//This Shader just for study,not for project  LZX-VS2022-04-02-003
Shader "Toon/Toon_Shader"
{
    Properties
    {
        [Header(Debug)]
        [Toggle] _IS_RIMLIGHT_DEBUG("- IS RIMLIGHT DEBUG", float) = 0
        [Toggle] _IS_DEBUG("- IS DEBUG(Mainamp Off)", float) = 0
        [Header(Main)]
        _Skylight("Sky Light",Color) = (0.1,0.1,0.1,1)
        _MainTex("Main Tex",2D) = "white"{}
        _IlmTex("Ilm Tex",2D) = "white"{}

        [Space(10)]
        [Header(IS FACE)]
        [Toggle] _IS_FACE("- Is Face", float) = 0


         [Header(Vector)]
        _LerpMax("Lerp Max",Range(0,1)) = 0.5
        _Front("Front",Vector) = (0,0,0)
        _UP("UP",Vector) = (0,0,0)
        _LeftDir("LeftDir",Vector) = (0,0,0)
        [Header(Eyes)]
        [HDR]_XRayCol ("XRay Color", Color) = (1, 1, 1, 1)
        _XRayFresPow ("XRay Fresnel Pow", Float) = 5
        _EyesMask("Eyes Mask",2D) = "black"{}


        [Header(Outline)]
        [Toggle] _SCREAM_SPACE_OUTLINE("- Enable Scream Space Outline", float) = 0
        [Toggle] _NORMAL_OUTLINE("- Enable Normal Outline", float) = 0

        _OutLineWigth("OutLine Width",Range(-1,10)) = 0.001

         _OutLineColor("Out Line Color",Color) = (0,0,0,0)
         [Header(Outline(Normal))]
         _OutlineWidthMax("Outline Width Max",Range(0,10)) = 6
		_OutlineWidthMin("Outline Width Min",Range(0,10)) = 2
        [Header(Outline(ScreamSpace))]
        _OutlineThreshold("OutlineThreshold",range(-1,1)) = 0.5

       

        [Header(Gradation)]
        _GradationMin("Gradation Min",Range(0,1)) = 0.5
		_GradationMax("Gradation Max",Range(0,1)) = 0.5
        _GradationRampValue("Gradation Ramp Value",Range(0,1)) = 0.5
        _shininess("Shininess",Range(0,1)) = 0.5
        //_specColor("Spec Color",Color) = (0.5,0.5,0.5,1)
        _GradationMap("Gradation Map",2D) = "white"{}

		[Header(MatCap)]
        _MatCapTex("MatCapTex", 2D) = "white" {}
		_MatCapIntensity("MatCapIntensity",Range(0,2)) = 1
		_MatCapPow("MatCapPow",Range(0,5)) = 1
		_MatCapUVScale("MatCapUVScale",Range(0,1)) = 1

        [Header(Rim)]
        _RimOffect("RimOffect",range(0,1)) = 0.5
        _Threshold("RimThreshold",range(-1,1)) = 0.5
        _RimLightStrength("Rim Light Strength",Range(1,10)) = 2

    }   


    SubShader
    {
        Pass//Lighting Pass
        {
            Tags{"LightMode" = "ForwardBase"}
            CGPROGRAM
           /* #pragma shader_feature _IS_DEBUG_ON
#pragma shader_feature _IS_RIMLIGHT_DEBUG_ON

            #pragma multi_compile _ _IS_FACE_ON*/
            #pragma multi_compile_fog 
            #pragma vertex vert

            #pragma fragment fragLight
            #include "Toon.cginc"

            ENDCG
        }

        Pass//Rim Light Pass
        {
			Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "IgnoreProjector" = "True"}
            ZWrite Off
           
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
          /*  #pragma shader_feature _IS_DEBUG_ON
#pragma shader_feature _IS_RIMLIGHT_DEBUG_ON

            #pragma shader_feature _IS_FACE_ON*/
            #pragma multi_compile_fog
            #pragma vertex vert
			
			#pragma fragment fragRimLight
            #include "Toon.cginc"
            ENDCG
        }


        Pass//Outline Pass
	    {
	    Tags {"LightMode"="ForwardBase"}
			 
            Cull Front

            CGPROGRAM
          /*  #pragma shader_feature _IS_DEBUG_ON
#pragma shader_feature _IS_RIMLIGHT_DEBUG_ON
*/

            #pragma shader_feature _SCREAM_SPACE_OUTLINE_ON 
			#pragma shader_feature _NORMAL_OUTLINE_ON 
            #pragma vertex vertOutline
            #pragma fragment fragOutline
            #include "Toon.cginc"
            ENDCG
        }


        Pass//Scream Space Outline Pass
        {
            Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "IgnoreProjector" = "True"}
            ZWrite Off
           
            Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
          /*  #pragma shader_feature _IS_DEBUG_ON
#pragma shader_feature _IS_RIMLIGHT_DEBUG_ON


            #pragma shader_feature _SREAMSPACE_OUTLINE_ON */
            #pragma multi_compile_fog
			#pragma vertex vert
			#pragma fragment fragScreamSpaceOutline
            #include "Toon.cginc"
            ENDCG
        }

                
        Pass  //x ray pass
        {
            Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "IgnoreProjector" = "True"}
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back
            ZTest Greater

            Stencil 
            {
                Ref 0
                Comp Equal
                Pass Keep
                //pass Replace
            }

            CGPROGRAM
          /*  #pragma shader_feature _IS_DEBUG_ON
#pragma shader_feature _IS_RIMLIGHT_DEBUG_ON


            #pragma shader_feature _IS_FACE_ON*/
            

            #pragma vertex vert
            #pragma fragment fragEyes
            #include "Toon.cginc"

            ENDCG
        }


    }
    FallBack "Diffuse"
}
