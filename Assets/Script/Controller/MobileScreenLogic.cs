using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileScreenLogic : MonoBehaviour
{
    public Material[] materials;
    public UnityEngine.UI.Text text;

    public UnityEngine.UI.Text resolutionRatio;


    private enum Device { Smartphone , pad}
    private enum Orientation { Portrait, PortraitUpsideDown, LandscapeLeft, LandscapeRight }

    private Device device = Device.Smartphone;

    // Start is called before the first frame update
    void Start()
    {
        resolutionRatio.text = "Height"+Screen.height.ToString() + " Width" + Screen.width.ToString();

        if( Screen.height > Screen.width)
        {
            device = Device.Smartphone;
            //Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
        else
        {
            device = Device.pad;
            
            //Screen.orientation = ScreenOrientation.Portrait;
        }
        /*foreach (var material in materials)
            material.SetInt("_Orientation", 2);*/
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Device.Smartphone == device )
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            foreach (var material in materials)
                material.SetInt("_Orientation", 2);
        }
        else
        {
            Screen.orientation = ScreenOrientation.Portrait;
            foreach (var material in materials)
            { material.SetInt("_Orientation", 0); }
        }*/

        resolutionRatio.text = "Height" + Screen.height.ToString() + " Width" + Screen.width.ToString();
        ScreenLogic();
    }

    private void ScreenLogic()
    {
        if (Screen.orientation == ScreenOrientation.Portrait)
        {
            if(Device.Smartphone == device)
            {
                //material.SetInt("_Orientation", Convert.ToInt32(Orientation.Portrait));
                foreach (var material in materials)
                { material.SetInt("_Orientation", 0); }
                text.text = "Portrait";
            }
            else
            {
                //material.SetInt("_Orientation", Convert.ToInt32(Orientation.LandscapeLeft));
                foreach (var material in materials)
                    material.SetInt("_Orientation", 2);
                text.text = "LandscapeLeft";
            }
        }       
        else if (Screen.orientation == ScreenOrientation.PortraitUpsideDown)
        {
            if (Device.Smartphone == device)
            {
                //material.SetInt("_Orientation", Convert.ToInt32(Orientation.PortraitUpsideDown));
                foreach (var material in materials)
                    material.SetInt("_Orientation", 1);
                text.text = "PortraitUpsideDown";
            }
            else
            {
                //material.SetInt("_Orientation", Convert.ToInt32(Orientation.LandscapeRight));
                foreach (var material in materials)
                    material.SetInt("_Orientation", 3);
                text.text = "LandscapeRight";
            }
        }       
        else if (Screen.orientation == ScreenOrientation.LandscapeLeft)
        {
            if (Device.Smartphone == device)
            {
                //material.SetInt("_Orientation", Convert.ToInt32(Orientation.LandscapeLeft));
                foreach (var material in materials)
                    material.SetInt("_Orientation", 2);
                text.text = "LandscapeLeft";
            }
            else
            {
                //material.SetInt("_Orientation", Convert.ToInt32(Orientation.Portrait));
                foreach (var material in materials)
                    material.SetInt("_Orientation", 0);
                text.text = "Portrait";
            }
        }
        else if (Screen.orientation == ScreenOrientation.LandscapeRight)
        {
            if (Device.Smartphone == device)
            {
                //material.SetInt("_Orientation", Convert.ToInt32(Orientation.LandscapeRight));
                foreach (var material in materials)
                    material.SetInt("_Orientation", 3);
                text.text = "LandscapeRight";
            }
            else
            {
                //material.SetInt("_Orientation", Convert.ToInt32(Orientation.PortraitUpsideDown));
                foreach (var material in materials)
                    material.SetInt("_Orientation", 1);
                text.text = "PortraitUpsideDown";
            }
        }
            
    }

}
