using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLogic : MonoBehaviour
{
    public Texture2D[] texs;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void setCursorTexture()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//定义鼠标在game窗口所指的射线
        RaycastHit hitInfo;	//射线碰撞的信息
        if (Physics.Raycast(ray, out hitInfo))//判断是否碰到物体
        {
            //切换指针
            switch (hitInfo.collider.gameObject.tag)
            {
                case "Yunjin":
                    Cursor.SetCursor(texs[1], new Vector2(16, 16), CursorMode.Auto);
                    break;
                default:
                    Cursor.SetCursor(texs[0], new Vector2(16, 16), CursorMode.Auto);
                    break;
            }
        }

        if (Input.GetMouseButton(1))
        {
            Cursor.SetCursor(texs[2], Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(texs[0], Vector2.zero, CursorMode.Auto);
        }

    }

    // Update is called once per frame
    void Update()
    {
        //Cursor.SetCursor(texs[0], Vector2.zero, CursorMode.Auto);
        setCursorTexture();
    }
}
