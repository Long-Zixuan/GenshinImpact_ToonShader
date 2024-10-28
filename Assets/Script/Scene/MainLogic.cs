using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLogic : MonoBehaviour
{
    [SerializeField]
    private static string OS = "Windows";
    [SerializeField]
    private string os;
    public static string Version
    {
        get { return version; }
    }
    public static string version;

    public static string GetOS
    {
        get { return OS; }
    }

    public static string ModeName = "修复了法相的模型";

    public void SetModeName()
    {
        if (ModeName == "修复了法相的模型")
        {
            ModeName = "未修复法相的模型";
        }
        else if(ModeName == "未修复法相的模型")
        {
            ModeName = "修复了法相的模型";
        }
    }

    public void SetModeName_修复了法相的模型() 
    {
        ModeName = "修复了法相的模型";
    }

    public void SetModeName_未修复法相的模型()
    {
        ModeName = "未修复法相的模型";
    }

    public static string OutlineName = "法相偏移";

    public void SetOutlineName_法相偏移() 
    {
        OutlineName = "法相偏移";
    }

    public void SetOutlineName_屏幕空间() 
    {
        OutlineName = "屏幕空间";
    }

    public static string InputMode = "Keyboard";

    static List<GameObject> yunjinList = new List<GameObject>();

    public void UpdateModeMaterial_屏幕空间()
    {
        SetOutlineName_屏幕空间();
        GameObject[] yunjin = GameObject.FindGameObjectsWithTag("Yunjin");
        for (int i = 0; i < yunjin.Length; i++)
        {
            yunjin[i].GetComponent<MaterialChange>().Use_N_M(); // <--->
            yunjinList.Add(yunjin[i]);

        }
    }

    public void UpdateModeMaterial_法相偏移() 
    {
        SetOutlineName_法相偏移();
        GameObject[] yunjin = GameObject.FindGameObjectsWithTag("Yunjin");
        for (int i = 0; i < yunjin.Length; i++)
        {
            yunjin[i].GetComponent<MaterialChange>().Use_L_M(); // <--->
            print("Yunjin:"+yunjin[i].name);
            yunjinList.Add(yunjin[i]);

        }
    }

    private void Awake()
    {
        OS = SystemInfo.operatingSystem;
    }

    // Start is called before the first frame update
    void Start()
    {
        OS = os;
       // if (version == "0")
       // {
            version = Application.version;
       // }
       // else
       // {
       //     Version = version;
       // }
        //OS = SystemInfo.operatingSystem;
    }

    // Update is called once per frame
    void Update()
    {
        //print(version);
        //print(OS);
    }
}
