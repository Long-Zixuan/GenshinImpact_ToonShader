using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUILogic : MonoBehaviour
{
    public GameObject Cav;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Cav != null)
        {
            if (Input.GetKeyDown(KeyCode.F2))
            {
                HideOrShowUI();
            }
        }
    }

    public void HideOrShowUI()
    {
        Cav.SetActive(!Cav.activeInHierarchy);
    }

}
