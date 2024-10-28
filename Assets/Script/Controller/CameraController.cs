using UnityEngine;
//using UnityEngine.Windows;

public class CameraController : MonoBehaviour
{
    /*private bool _isF;

    // [Header("所控制的摄像机")][SerializeField] private GameObject _camera;    //可有可无
    [Header("旋转中心目标物体")][SerializeField] private GameObject _target;
    [Header("拖动灵敏度")][SerializeField] private float _sensitivity = 2.0f;
    [Header("移动速度")][SerializeField] private float _speed = 0.1f;

    private void Update()
    {
        W_A_S_D();
        //通过键盘的F 控制切换是否锁定目标围绕着旋转
        if (Input.GetKeyUp(KeyCode.F))
        {
            _isF = !_isF;
            Debug.Log("isF=" + _isF);
        }

        if (!_isF)
        {
            if (Input.GetMouseButton(0))
            {
                Around();
            }
        }
        else
        {
            if (Input.GetMouseButton(0))
            {
                LookAround();
            }
        }
    }

    //通过键盘的W、A、S、D控制摄像机移动的方法函数
    private void W_A_S_D()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * _speed);
        }

        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * _speed);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * _speed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * _speed);
        }
    }

    //控制摄像机围绕物体旋转的方法函数
    private void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * _sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * _sensitivity;
        transform.RotateAround(_target.transform.position, Vector3.up, mouseX);
        transform.RotateAround(_target.transform.position, transform.right, -mouseY);
        transform.LookAt(_target.transform);
    }

    //控制摄像机自由旋转的方法函数
    private void Around()
    {
        float rotateX = 0;
        float rotateY = 0;
        rotateX = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * _sensitivity;
        rotateY = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * _sensitivity;

        transform.localEulerAngles = new Vector3(rotateX, rotateY, 0);
    }*/
    private new Transform transform;
    public Joystick joystick_moveCarema;
    public Joystick joystick_turnCarema;
    public float walkSpeed = 10f;
    public float runSpeed = 20f;
    public float rotateSpeed = 500f;
    private float moveSpeed;
    private bool can_move_mouse;
    public bool keyboard_input;
    public bool useThisInput = true;

    public static string InputMode { get { return inputMode; } }
    private static string inputMode = "Keyboard";

    private Vector3 oriPos;
    private Vector3 oriEuler;

    void Start()
    {
        if(MainLogic.GetOS == "Android")
        {
            useGUILayout = false;
        }
        transform = GetComponent<Transform>();
        can_move_mouse = false;
        //keyboard_input = true;
        oriPos = transform.position;
        oriEuler = transform.rotation.eulerAngles;
    }

    void Update()
    {
        /*if (!useThisInput)
        {
            return;
        }*/
        //float adValue = Input.GetAxis("Horizontal");
        //float wsValue = Input.GetAxis("Vertical");
        float adValue;
        float wsValue;
        float mxValue, myValue;
        setInputMode();
        if (joystick_moveCarema!=null&&joystick_turnCarema!=null)
        {
            rotateSpeed = 100;
            //keyboard_input = false;
            mxValue = joystick_turnCarema.Horizontal;
            myValue = joystick_turnCarema .Vertical;
            transform.Rotate(new Vector3(0, 1 * rotateSpeed * Time.deltaTime * mxValue, 0));
            transform.Rotate(new Vector3(-1 * rotateSpeed * Time.deltaTime * myValue, 0, 0));


            adValue = joystick_moveCarema.Horizontal;
            wsValue = joystick_moveCarema.Vertical;
            var moveDirectionMobile = (Vector3.forward * wsValue) + (Vector3.right * adValue);
            transform.Translate(moveDirectionMobile.normalized * moveSpeed * Time.deltaTime, Space.Self);
            Vector3 currentRotationMobile = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(currentRotationMobile.x, currentRotationMobile.y, 0f);
        }
        if (!keyboard_input) 
        {
            return;
        }
        rotateSpeed = 300;

        adValue = Input.GetAxis("Horizontal");
        wsValue = Input.GetAxis("Vertical");


        if (Input.GetMouseButtonDown(1))
        {
            inputMode = "Keyboard";
            can_move_mouse = true;
        }

        if (Input.GetMouseButtonUp(1)) 
        {
            can_move_mouse = false; 
        }
        /*if (Input.GetButtonDown("Fire3"))
        {
            can_move_mouse = !can_move_mouse;
        }*/

        if (can_move_mouse && useThisInput)
        {
            mxValue = Input.GetAxis("Mouse X");
            myValue = Input.GetAxis("Mouse Y");
            transform.Rotate(new Vector3(0, 1 * rotateSpeed * Time.deltaTime * mxValue, 0));
            transform.Rotate(new Vector3(-1 * rotateSpeed * Time.deltaTime * myValue, 0, 0));
        }

        float RightAnalogY = Input.GetAxis("JoyStick_Vertical_R");
        float RightAnalogX = Input.GetAxis("JoyStick_Horizontal_R");

        if(RightAnalogY != 0 || RightAnalogX != 0)
        {
            inputMode = "Joystick";
            transform.Rotate(new Vector3(0, 1 * rotateSpeed * Time.deltaTime * RightAnalogX * 0.4f, 0));
            transform.Rotate(new Vector3(1 * rotateSpeed * Time.deltaTime * RightAnalogY * 0.4f, 0, 0));
        }

        moveSpeed = walkSpeed;

        if (Input.GetKey(KeyCode.JoystickButton5))
        {
            moveSpeed = runSpeed;
            inputMode = "Joystick";
        }

        if (Input.GetButton("Fire3"))//if (Input.GetKey(KeyCode.LeftShift)||Input.GetKey(KeyCode.RightShift))
        {
            moveSpeed = runSpeed;
            inputMode = "Keyboard";
        }

        var moveDirection = (Vector3.forward * wsValue) + (Vector3.right * adValue);
        transform.Translate(moveDirection.normalized * moveSpeed * Time.deltaTime, Space.Self);
        Vector3 currentRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(currentRotation.x, currentRotation.y, 0f);
    }

    public void resetCamera()
    {
        transform.position = oriPos;
        transform.transform.eulerAngles = oriEuler;
    }

    void setInputMode()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D))
        {
            inputMode = "Keyboard";
        }

        if(Input.GetKeyDown(KeyCode.Joystick1Button0)|| Input.GetKeyDown(KeyCode.Joystick1Button1)|| Input.GetKeyDown(KeyCode.Joystick1Button2)|| Input.GetKeyDown(KeyCode.Joystick1Button3)|| Input.GetKeyDown(KeyCode.Joystick1Button4)|| Input.GetKeyDown(KeyCode.Joystick1Button5)|| Input.GetKeyDown(KeyCode.Joystick1Button6))
        {
                inputMode = "Joystick";
        }
    }
}