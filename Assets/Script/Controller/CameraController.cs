using UnityEngine;
//using UnityEngine.Windows;

public class CameraController : MonoBehaviour
{
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
        transform = GetComponent<Transform>();
        can_move_mouse = false;
        //keyboard_input = true;
        oriPos = transform.position;
        oriEuler = transform.rotation.eulerAngles;
    }

    void Update()
    {
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