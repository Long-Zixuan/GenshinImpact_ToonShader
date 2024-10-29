using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILogic : MonoBehaviour
{
    public GameObject textKeyboard;
    public GameObject textJoystick;

    public UnityEngine.UI.Text VersionMSG;
    public UnityEngine.UI.Text VersionMSGForShot;

    // Start is called before the first frame update
    void Start()
    {
        VersionMSG.text = "Version: " + Application.version + "( Windows )";
        VersionMSGForShot.text = "LZX Toonshader Test Demo\nVersion: " + Application.version + "( Windows )";
       
        if (CameraController.InputMode == "Keyboard" )
        {
            textKeyboard.SetActive(true);
            textJoystick.SetActive(false);

        }
        else if (CameraController.InputMode == "Joystick" )
        {
            textKeyboard.SetActive(false);
            textJoystick.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {


        if(CameraController.InputMode == "Keyboard" )
        {
            textKeyboard.SetActive(true);
            textJoystick.SetActive(false);

        }
        else if(CameraController.InputMode == "Joystick" )
        {
            textKeyboard.SetActive(false);
            textJoystick.SetActive(true);
        }
    }
}
