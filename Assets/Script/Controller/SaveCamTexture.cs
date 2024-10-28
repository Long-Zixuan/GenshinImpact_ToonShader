using UnityEngine;
using System;
using System.IO;
using System.Net.NetworkInformation;
using UnityEngine.XR;


public class SaveCamTexture : MonoBehaviour
{
    public Camera cam;
    public RenderTexture rt;

    public GameObject cav;
    public GameObject cavShot;

    public string path = "Assets/../ScreenShot/";

    public void Start()
    {
        if (cam == null)
        {
            cam = this.GetComponent<Camera>();
        }
    }

    bool hadShot = false;

    private void Update()
    {
        if (cam == null)
        { return; }

        if (hadShot)
        {
            UnityDefultShot2();
            hadShot = false;
            cavShot.SetActive(false);
            cav.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.F4) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            CheckPath();
            ScreenTool.Instance._SaveCamTexture(cam, path);
            //string pathPNG = string.Format("D:LZX_ToonShaderTest_camera_{0}_{1}_{2}_{3}_{4}_{5}.png", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            UnityDefultShot();
            hadShot = true;
            
            
        }
        if (!hadShot)
        {
           // cavShot.SetActive(false);
           // cav.SetActive(true);
        }
        
    }


    private void UnityDefultShot2()
    {
        string pathPNG = string.Format("Assets/../ScreenShot/LZX_ToonShaderTest_MSG_{0}_{1}_{2}_{3}_{4}_{5}.png", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        ScreenTool.Instance.ScreenShotFile(pathPNG);
        
    }

    private void UnityDefultShot()
    {
        string pathPNG = string.Format("Assets/../ScreenShot/LZX_ToonShaderTest_camera_{0}_{1}_{2}_{3}_{4}_{5}.png", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        ScreenTool.Instance.ScreenShotFile(pathPNG);
        cavShot.SetActive(true);
        cav.SetActive(false);
        
    }

    private void CheckPath(string savePath = "Assets/../ScreenShot/")
    {
        if (!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }
    }

    /*public void _SaveCamTexture()
    {
        rt = cam.targetTexture;
        if (rt != null)
        {
            _SaveRenderTexture(rt);
            rt = null;
        }
        else
        {
            GameObject camGo = new GameObject("camGO");
            Camera tmpCam = camGo.AddComponent<Camera>();
            tmpCam.CopyFrom(cam);
            // rt = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
            rt = RenderTexture.GetTemporary(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);

            tmpCam.targetTexture = rt;
            tmpCam.Render();
            _SaveRenderTexture(rt);
            Destroy(camGo);
            //rt.Release();
            RenderTexture.ReleaseTemporary(rt);
            //Destroy(rt);
            rt = null;
        }

    }
    private void _SaveRenderTexture(RenderTexture rt)
    {
        RenderTexture active = RenderTexture.active;
        RenderTexture.active = rt;
        
        Texture2D jpg = new Texture2D(rt.width, rt.height, TextureFormat.RGB24, false);
        jpg.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        jpg.Apply();

        Texture2D png = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
        png.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        png.Apply();
        RenderTexture.active = active;
        byte[] bytesPNG = png.EncodeToPNG();

        byte[] bytesJPG = jpg.EncodeToJPG();



        string pathPNG = string.Format("Assets/../ScreenShot/LZX_ToonShaderTest_rt_{0}_{1}_{2}_{3}_{4}_{5}.png",DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        string pathJPG = string.Format("Assets/../ScreenShot/LZX_ToonShaderTest_rt_{0}_{1}_{2}_{3}_{4}_{5}.jpg",DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        
        //string pathPNG = string.Format("/LZX_ToonShaderTest_rt_{0}_{1}_{2}_{3}_{4}_{5}.png", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        //string pathJPG = string.Format("/LZX_ToonShaderTest_rt_{0}_{1}_{2}_{3}_{4}_{5}.jpg", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        FileStream fsPNG = File.Open(pathPNG, FileMode.Create);
        FileStream fsJPG = File.Open(pathJPG, FileMode.Create);
        BinaryWriter writerPNG = new BinaryWriter(fsPNG);
        BinaryWriter writerJPG = new BinaryWriter(fsJPG);
        writerJPG.Write(bytesJPG);
        writerPNG.Write(bytesPNG);
        writerJPG.Flush();
        writerPNG.Close();
        fsPNG.Close();
        fsJPG.Close();
        Destroy(png);
        png = null;
        Destroy(jpg);
        jpg = null;
        Debug.Log("保存成功！" + pathPNG);
        Debug.Log("保存成功！" + pathJPG);
    }*/
}


