using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class SetLight : MonoBehaviour
{
    float euAnY ;
    public Joystick joystick;
    public float roundSpeed = 10f;
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
        //float euAnY = transform.localEulerAngles.y;
        if(joystick != null )
        {
            float herzion = joystick.Horizontal;


            float tmp = Time.deltaTime * roundSpeed;
            transform.localRotation = Quaternion.Euler(euAnX, euAnY += tmp * herzion, euAnZ);
            /*if (herzion > 0)
            {
                float tmp = Time.deltaTime * roundSpeed;
                Debug.Log("DeltaTime = " + tmp);
                transform.localRotation = Quaternion.Euler(euAnX, euAnY += tmp, euAnZ);
                //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + roundSpeed * Time.deltaTime, transform.localEulerAngles.z);
            }
            if (herzion < 0)
            {
                float tmp = Time.deltaTime * roundSpeed;
                Debug.Log("DeltaTime = " + tmp);
                transform.localRotation = Quaternion.Euler(euAnX, euAnY -= tmp, euAnZ);
                //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + roundSpeed * Time.deltaTime, transform.localEulerAngles.z);
            }*/
        }
        


        if (Input.GetKey(KeyCode.RightArrow))
        {
            float tmp = Time.deltaTime * roundSpeed;
            Debug.Log("DeltaTime = "+tmp);
            transform.localRotation = Quaternion.Euler(euAnX, euAnY+=tmp,euAnZ);
            //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + roundSpeed * Time.deltaTime, transform.localEulerAngles.z);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            float tmp = Time.deltaTime * roundSpeed;
            Debug.Log("DeltaTime = " + tmp);
            transform.localRotation = Quaternion.Euler(euAnX, euAnY-=tmp, euAnZ);
            //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y + roundSpeed * Time.deltaTime, transform.localEulerAngles.z);
        }
    }
}
