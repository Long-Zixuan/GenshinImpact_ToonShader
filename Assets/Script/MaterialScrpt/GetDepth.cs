using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetDepth : MonoBehaviour
{
    private Camera currentCamera = null;
    void Awake()
    {
        currentCamera = GetComponent<Camera>();
    }
    void Start()
    {
        currentCamera.depthTextureMode = DepthTextureMode.Depth;
    }

    void Update()
    {
        //currentCamera.depthTextureMode = DepthTextureMode.Depth;
    }
}
