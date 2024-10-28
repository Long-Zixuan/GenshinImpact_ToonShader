using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class ScreenDepthOffest : MonoBehaviour
{
    public Camera cam;
    public Material mat;
    [SerializeField]
    DepthTextureMode depthTextureMode;

    void Start()
    { }
    private void Awake()
    {
        //自己获取摄像机组件
        if (cam == null)   //如果摄像机为空，则获取所挂载的物体的Camer组件
        {
            cam = this.GetComponent<Camera>();
        }
        if (mat == null)
        {
            mat = new Material(Shader.Find("Texture/ScreenDepthOffest"));  //自己定义的材质代码首行名字还是路径
        }
    }
    void Update()
    {
        SetCameraDepthTextureMode();
    }
    private void OnPreRender()
    {
        //传递相机的逆矩阵，重构世界坐标
        Shader.SetGlobalMatrix(Shader.PropertyToID("UNITY_MATRIX_IV"), cam.cameraToWorldMatrix);
    }
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        //入门精要的内容
        Graphics.Blit(src, dest, mat);
    }
    private void SetCameraDepthTextureMode()
    {
        cam.depthTextureMode = depthTextureMode;
    }
}