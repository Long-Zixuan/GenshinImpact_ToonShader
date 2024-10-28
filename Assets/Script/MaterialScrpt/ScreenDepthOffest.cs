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
        //�Լ���ȡ��������
        if (cam == null)   //��������Ϊ�գ����ȡ�����ص������Camer���
        {
            cam = this.GetComponent<Camera>();
        }
        if (mat == null)
        {
            mat = new Material(Shader.Find("Texture/ScreenDepthOffest"));  //�Լ�����Ĳ��ʴ����������ֻ���·��
        }
    }
    void Update()
    {
        SetCameraDepthTextureMode();
    }
    private void OnPreRender()
    {
        //���������������ع���������
        Shader.SetGlobalMatrix(Shader.PropertyToID("UNITY_MATRIX_IV"), cam.cameraToWorldMatrix);
    }
    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        //���ž�Ҫ������
        Graphics.Blit(src, dest, mat);
    }
    private void SetCameraDepthTextureMode()
    {
        cam.depthTextureMode = depthTextureMode;
    }
}