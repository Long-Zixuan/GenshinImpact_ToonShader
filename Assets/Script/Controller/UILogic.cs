using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILogic : MonoBehaviour
{

    public UnityEngine.UI.Text VersionMSG;
    public UnityEngine.UI.Text VersionMSGForShot;

    public GameObject textKeyboard;
    public GameObject textJoystick;


    public GameObject androidUI;
    // Start is called before the first frame update
    void Start()
    {
        VersionMSG.text = "Version: " + Application.version + "(" + MainLogic.GetOS + ")";
        VersionMSGForShot.text = "LZX Toonshader Test Demo\nVersion: " + Application.version + "(" + MainLogic.GetOS + ")";
        if (MainLogic.GetOS == "Android")
        {
            textKeyboard.SetActive(false);
            textJoystick.SetActive(false);
            androidUI.SetActive(true);
        }
        else if (CameraController.InputMode == "Keyboard" && MainLogic.GetOS == "Windows")
        {
            textKeyboard.SetActive(true);
            textJoystick.SetActive(false);
            androidUI.SetActive(false);

        }
        else if (CameraController.InputMode == "Joystick" && MainLogic.GetOS == "Windows")
        {
            textKeyboard.SetActive(false);
            textJoystick.SetActive(true);
            androidUI.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {


        if(CameraController.InputMode == "Keyboard" && MainLogic.GetOS == "Windows")
        {
            textKeyboard.SetActive(true);
            textJoystick.SetActive(false);

        }
        else if(CameraController.InputMode == "Joystick" && MainLogic.GetOS == "Windows")
        {
            textKeyboard.SetActive(false);
            textJoystick.SetActive(true);
        }
    }
}
