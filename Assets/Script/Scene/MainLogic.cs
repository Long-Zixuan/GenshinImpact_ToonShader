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

    public static string ModeName = "�޸��˷����ģ��";

    public void SetModeName()
    {
        if (ModeName == "�޸��˷����ģ��")
        {
            ModeName = "δ�޸������ģ��";
        }
        else if(ModeName == "δ�޸������ģ��")
        {
            ModeName = "�޸��˷����ģ��";
        }
    }

    public void SetModeName_�޸��˷����ģ��() 
    {
        ModeName = "�޸��˷����ģ��";
    }

    public void SetModeName_δ�޸������ģ��()
    {
        ModeName = "δ�޸������ģ��";
    }

    public static string OutlineName = "����ƫ��";

    public void SetOutlineName_����ƫ��() 
    {
        OutlineName = "����ƫ��";
    }

    public void SetOutlineName_��Ļ�ռ�() 
    {
        OutlineName = "��Ļ�ռ�";
    }

    public static string InputMode = "Keyboard";

    static List<GameObject> yunjinList = new List<GameObject>();

    public void UpdateModeMaterial_��Ļ�ռ�()
    {
        SetOutlineName_��Ļ�ռ�();
        GameObject[] yunjin = GameObject.FindGameObjectsWithTag("Yunjin");
        for (int i = 0; i < yunjin.Length; i++)
        {
            yunjin[i].GetComponent<MaterialChange>().Use_N_M(); // <--->
            yunjinList.Add(yunjin[i]);

        }
    }

    public void UpdateModeMaterial_����ƫ��() 
    {
        SetOutlineName_����ƫ��();
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
