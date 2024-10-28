using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 1f;
    Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
    }

    // Update is called once per frame

    void Update()
    {
        float posX = transform.position.x;
        posX = posX+speed*Time.deltaTime;
        transform.position = new Vector3(posX,transform.position.y,transform.position.z);
        if(transform.position.x-pos.x>3|| transform.position.x - pos.x < -3)
        {
            speed = -speed;
        }
    }
}
