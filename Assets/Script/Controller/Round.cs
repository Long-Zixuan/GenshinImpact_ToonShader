using System.Collections;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

public class Round : MonoBehaviour
{
    float euAnY;
    public float roundSpeed = 10f;
    public Joystick joystick;
    float euAnX;
    float euAnZ;
    // Start is called before the first frame update
    void Start()
    {
        euAnY = transform.localEulerAngles.y;
        euAnX = transform.localEulerAngles.x;
        euAnZ = transform.localEulerAngles.z;
    }

    // Update is called once per frame
    void Update()
    {
        euAnY = transform.localEulerAngles.y;
        euAnX = transform.localEulerAngles.x;
        euAnZ = transform.localEulerAngles.z;
        if(joystick != null )
        {
            float herizon = joystick.Horizontal;
            if (herizon < 0)
            {
                float tmp = Time.deltaTime * roundSpeed;
                Debug.Log("DeltaTime = " + tmp);
                transform.localRotation = Quaternion.Euler(euAnX, euAnY += tmp, euAnZ);
                //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + roundSpeed * Time.deltaTime, transform.localEulerAngles.z);
            }
            if (herizon > 0)
            {
                float tmp = Time.deltaTime * roundSpeed;
                Debug.Log("DeltaTime = " + tmp);
                transform.localRotation = Quaternion.Euler(euAnX, euAnY -= tmp, euAnZ);
                //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + roundSpeed * Time.deltaTime, transform.localEulerAngles.z);
            }
        }
        

        //float euAnY = transform.localEulerAngles.y;
        if (Input.GetKey(KeyCode.A))
        {
            float tmp = Time.deltaTime * roundSpeed;
            Debug.Log("DeltaTime = " + tmp);
            transform.localRotation = Quaternion.Euler(euAnX, euAnY += tmp, euAnZ);
            //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + roundSpeed * Time.deltaTime, transform.localEulerAngles.z);
        }
        if (Input.GetKey(KeyCode.D))
        {
            float tmp = Time.deltaTime * roundSpeed;
            Debug.Log("DeltaTime = " + tmp);
            transform.localRotation = Quaternion.Euler(euAnX, euAnY -= tmp, euAnZ);
            //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + roundSpeed * Time.deltaTime, transform.localEulerAngles.z);
        }
    }
}
