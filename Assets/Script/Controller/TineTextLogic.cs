using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TineTextLogic : MonoBehaviour
{
    public Text text;

    private void Awake()
    {
        if (text == null)
        { 
            text = GetComponent<Text>(); 
        }
        string dateAndTime = string.Format("{0}/{1}/{2}  {3}:{4}:{5}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        text.text = dateAndTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (text == null)
        {
            text = GetComponent<Text>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        string dateAndTime = string.Format("{0}/{1}/{2}  {3}:{4}:{5}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        text.text = dateAndTime;
    }
}
