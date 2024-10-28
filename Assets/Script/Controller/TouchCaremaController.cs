using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class TouchCaremaController : MonoBehaviour
{
    public float Speed = 1;//�����ٶ�

    Transform m_Camera;//���
    Vector3 m_transfrom;//��¼camera�ĳ�ʼλ��
    Vector3 m_eulerAngles;//��¼camera�ĳ�ʼ�Ƕ�
    Vector3 m_RayHitPoint;//��¼���ߵ�
    Touch m_touchLeft;//��¼��ߵĴ�����
    Touch m_touchRight;//��¼�ұߵĴ�����
    int m_isforward;//����������ǰ���ƶ�����
    //�����ж��Ƿ�Ŵ�
    float m_leng0 = 0;

    public Joystick[] joysticks;

    bool CanMoveOrTurn()
    {
        foreach (var joystick in joysticks) 
        {
            if (joystick.Horizontal != 0 || joystick.Vertical != 0)
            {
                return false;
            }
        }
        return true;
    }


    int IsEnlarge(Vector2 P1, Vector2 P2)
    {
        float leng1 = Vector2.Distance(P1, P2);
        if (m_leng0 == 0)
        {
            m_leng0 = leng1;
        }
        if (m_leng0 < leng1)
        {
            //�Ŵ�����
            m_leng0 = leng1;
            return 1;
        }
        else if (m_leng0 > leng1)
        {
            //��С����
            m_leng0 = leng1;
            return -1;
        }
        else
        {
            m_leng0 = leng1;
            return 0;
        }
    }
    void Start()
    {
        m_Camera = this.transform;
        m_RayHitPoint = Vector3.zero;
        m_transfrom = m_Camera.position;
        m_eulerAngles = m_Camera.eulerAngles;
    }
    //�õ���λ����
    Vector2 GetDirection(Vector2 vector)
    {
        vector.Normalize();
        return vector;
    }
    void Update()
    {
        if (!CanMoveOrTurn())
        {
            return;
        }
        if (Input.touchCount <= 0)
            return;
        if (Input.touchCount == 1) //���㴥���ƶ������
        {
            if (Input.touches[0].phase == TouchPhase.Began)
                RayPoint();
            if (Input.touches[0].phase == TouchPhase.Moved) //��ָ����Ļ���ƶ����ƶ������
            {
                Translation(-GetDirection(Input.touches[0].deltaPosition));
            }
        }
        else if (Input.touchCount == 2)
        {
            //�ж����Ҵ�����
            if (Input.touches[0].position.x > Input.touches[1].position.x)
            {
                m_touchLeft = Input.touches[1];
                m_touchRight = Input.touches[0];
            }
            else
            {
                m_touchLeft = Input.touches[0];
                m_touchRight = Input.touches[1];
            }
            RayPoint();
            if (m_touchRight.deltaPosition != Vector2.zero && m_touchLeft.deltaPosition != Vector2.zero)
            {
                //�ж����������Ӷ����������ǰ���ƶ���������Ч��
                m_isforward = IsEnlarge(m_touchLeft.position, m_touchRight.position);
                //FrontMove(m_isforward);
            }
            else if (m_touchRight.deltaPosition == Vector2.zero && m_touchLeft.deltaPosition != Vector2.zero)
            {
                transform.Translate(new Vector3(0, 1 * Speed * 5 * Time.deltaTime * m_touchLeft.deltaPosition.y,0));
                transform.Rotate(new Vector3(0, 1 * Speed * 20 * Time.deltaTime * m_touchLeft.deltaPosition.x, 0));
                //RotatePoint(-GetDirection(m_touchLeft.deltaPosition));//������ת
                //transform.Rotate(new Vector3(0, 1 * Speed * Time.deltaTime, 0));
            }
            else if (m_touchRight.deltaPosition != Vector2.zero && m_touchLeft.deltaPosition == Vector2.zero)
            {
                transform.Translate(new Vector3(0, 1 * Speed * 5 * Time.deltaTime * m_touchRight.deltaPosition.y, 0));
                transform.Rotate(new Vector3(0, 1 * Speed * 20 * Time.deltaTime * m_touchRight.deltaPosition.x, 0));
                //RotatePoint(-GetDirection(m_touchRight.deltaPosition));//������ת
                //transform.Rotate(new Vector3(0, -1 * Speed * Time.deltaTime, 0));
            }
            else
            {
                return;
            }
        }
    }

    Vector3 m_VecOffet = Vector3.zero;
    /// <summary>
    /// ˮƽƽ��
    /// </summary>
    /// <param name="direction"></param>
    void Translation(Vector2 direction)
    {
        m_VecOffet = m_RayHitPoint - m_Camera.position;
        float ftCamerDis = GetDis();
        if (ftCamerDis == 0)
        {
            ftCamerDis = 1;
        }
        float tranY = direction.y * (float)Math.Sin(Math.Round(m_Camera.localRotation.eulerAngles.x, 2) * Math.PI / 180.0);
        float tranZ = direction.y * (float)Math.Cos(Math.Round(m_Camera.localRotation.eulerAngles.x, 2) * Math.PI / 180.0);
        m_Camera.Translate(new Vector3(-direction.x, -tranY, -tranZ) * ftCamerDis * Time.deltaTime * Speed, Space.Self);
        m_RayHitPoint = m_Camera.position + m_VecOffet;
    }
    /// <summary>
    /// �õ�������ײ��
    /// </summary>
    void RayPoint()
    {
        Ray ray;
        ray = new Ray(m_Camera.position, m_Camera.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            m_RayHitPoint = hit.point;
        }
        else
        {
            m_RayHitPoint = transform.forward * 800 + transform.position;//�����ǰ�� 800 ��                                            
        }
    }
    /// <summary>
    /// �Ƶ���ת
    /// </summary>
    void RotatePoint(Vector2 rotate)
    {
        Vector3 eulerAngles = m_Camera.eulerAngles;
        float eulerAngles_x = eulerAngles.y;
        float eulerAngles_y = eulerAngles.x;

        float ftCamerDis = GetDis();
        eulerAngles_x += (rotate.x) * Time.deltaTime * 60;
        eulerAngles_y -= (rotate.y) * Time.deltaTime * 60;
        if (eulerAngles_y > 80)
        {
            eulerAngles_y = 80;
        }
        else if (eulerAngles_y < 1)
        {
            eulerAngles_y = 1;
        }
        Quaternion quaternion = Quaternion.Euler(eulerAngles_y, eulerAngles_x, (float)0);
        Vector3 vector = ((Vector3)(quaternion * new Vector3((float)0, (float)0, -ftCamerDis))) + m_RayHitPoint;
        m_Camera.rotation = quaternion;
        m_Camera.position = vector;

    }
    /// <summary>
    /// ��ǰ�ƶ�
    /// Direction[����]
    /// </summary>
    /// <param name="intDirection">��д������1��ǰ�ƶ���2����ƶ�</param>
    void FrontMove(int intDirection)
    {
        float ftCamerDis = GetDis();
        if (ftCamerDis < 1)
        {
            ftCamerDis = 1;
        }
        m_Camera.Translate(Vector3.forward * ftCamerDis * Time.deltaTime * Speed * intDirection);
    }
    float GetDis()
    {
        float ftCamerDis = Vector3.Distance(m_Camera.position, m_RayHitPoint);

        return ftCamerDis;
    }
    //�����λ
    public void Reset()
    {
        m_Camera.position = m_transfrom;
        m_Camera.eulerAngles = m_eulerAngles;
    }
}

