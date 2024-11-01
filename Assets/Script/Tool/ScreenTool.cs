using UnityEngine;
using System.Collections;
using System.IO;
using System;

public delegate void CallBack();//利用委托回调可以先关闭UI，截取到没有UI的画面
/// <summary>
/// 截图工具类
/// </summary>
public class ScreenTool
{
    private static ScreenTool _instance;
    public static ScreenTool Instance
    {
        get
        {
            if (_instance == null)
                _instance = new ScreenTool();
            return _instance;
        }
    }
    public string UrlRelativeToAbsolute(string relative)
    {
        string absolutePath = System.IO.Path.GetFullPath(relative);
        return absolutePath;
    }
    /// <summary>
    /// UnityEngine自带截屏Api，只能截全屏,传入相对路径和绝对路径都可以
    /// </summary>
    /// <param name="fileName">文件地址以及名（包括后缀名）</param>
    public void ScreenShotFile(string fileName)
    {
        UnityEngine.ScreenCapture.CaptureScreenshot(UrlRelativeToAbsolute(fileName));//截图并保存截图文件
        Debug.Log(string.Format("截取了一张图片: {0}", fileName));

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();//刷新Unity的资产目录
#endif
    }
    /// <summary>
    /// UnityEngine自带截屏Api，只能截全屏
    /// </summary>
    /// <param name="fileName">文件名</param>
    /// <param name="callBack">截图完成回调</param>
    /// <returns>协程</returns>
    public IEnumerator ScreenShotTex(string fileName, CallBack callBack = null)
    {
        yield return new WaitForEndOfFrame();//等到帧结束，不然会报错
        Texture2D tex = UnityEngine.ScreenCapture.CaptureScreenshotAsTexture();//截图返回Texture2D对象
        byte[] bytes = tex.EncodeToPNG();//将纹理数据，转化成一个png图片
        System.IO.File.WriteAllBytes(fileName, bytes);//写入数据
        Debug.Log(string.Format("截取了一张图片: {0}", fileName));

        callBack?.Invoke();
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();//刷新Unity的资产目录
#endif
    }
    /// <summary>
    /// 截取游戏屏幕内的像素
    /// </summary>
    /// <param name="rect">截取区域：屏幕左下角为0点</param>
    /// <param name="fileName">文件名</param>
    /// <param name="callBack">截图完成回调</param>
    /// <returns></returns>
    public IEnumerator ScreenCapture(Rect rect, string fileName, CallBack callBack = null)
    {
        yield return new WaitForEndOfFrame();//等到帧结束，不然会报错
        Texture2D tex = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.ARGB32, false);//新建一个Texture2D对象
        tex.ReadPixels(rect, 0, 0);//读取像素，屏幕左下角为0点
        tex.Apply();//保存像素信息

        byte[] bytes = tex.EncodeToPNG();//将纹理数据，转化成一个png图片
        System.IO.File.WriteAllBytes(fileName, bytes);//写入数据
        Debug.Log(string.Format("截取了一张图片: {0}", fileName));

        callBack?.Invoke();
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();//刷新Unity的资产目录
#endif
    }
    /// <summary>
    /// 对相机拍摄区域进行截图，如果需要多个相机，可类比添加，可截取多个相机的叠加画面
    /// </summary>
    /// <param name="camera">待截图的相机</param>
    /// <param name="width">截取的图片宽度</param>
    /// <param name="height">截取的图片高度</param>
    /// <param name="fileName">文件名</param>
    /// <returns>返回Texture2D对象</returns>
    public Texture2D CameraCapture(Camera camera, Rect rect, string fileName)
    {
        RenderTexture render = new RenderTexture((int)rect.width, (int)rect.height, -1);//创建一个RenderTexture对象 

        camera.gameObject.SetActive(true);//启用截图相机
        camera.targetTexture = render;//设置截图相机的targetTexture为render
        camera.Render();//手动开启截图相机的渲染

        RenderTexture.active = render;//激活RenderTexture
        Texture2D tex = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.ARGB32, false);//新建一个Texture2D对象
        tex.ReadPixels(rect, 0, 0);//读取像素
        tex.Apply();//保存像素信息

        camera.targetTexture = null;//重置截图相机的targetTexture
        RenderTexture.active = null;//关闭RenderTexture的激活状态
        UnityEngine.Object.Destroy(render);//删除RenderTexture对象

        byte[] bytes = tex.EncodeToPNG();//将纹理数据，转化成一个png图片
        System.IO.File.WriteAllBytes(fileName, bytes);//写入数据
        Debug.Log(string.Format("截取了一张图片: {0}", fileName));

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();//刷新Unity的资产目录
#endif

        return tex;//返回Texture2D对象，方便游戏内展示和使用
    }



    private void CheckFold(string path = "Assets/../ScreenShot/")
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public void _SaveCamTexture(Camera cam,string path)
    {
        RenderTexture rt;
        rt = cam.targetTexture;
        if (rt != null)
        {
            _SaveRenderTexture(rt,path);
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
            _SaveRenderTexture(rt,path);
            MonoBehaviour.Destroy(camGo);
            //rt.Release();
            RenderTexture.ReleaseTemporary(rt);
            //Destroy(rt);
            rt = null;
        }

    }
    private void _SaveRenderTexture(RenderTexture rt,string fold)
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

        /*if (!Directory.Exists("Assets/../ScreenShot/"))
        {
            Directory.CreateDirectory("Assets/../ScreenShot/");
        }*/
        CheckFold(fold);

        string pathPNG = string.Format(fold+"LZX_ToonShaderTest_rt_{0}_{1}_{2}_{3}_{4}_{5}.png", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

        string pathJPG = string.Format(fold+"LZX_ToonShaderTest_rt_{0}_{1}_{2}_{3}_{4}_{5}.jpg", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

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
        MonoBehaviour.Destroy(png);
        png = null;
        MonoBehaviour.Destroy(jpg);
        jpg = null;
        Debug.Log("保存成功！" + pathPNG);
        Debug.Log("保存成功！" + pathJPG);
    }
}
