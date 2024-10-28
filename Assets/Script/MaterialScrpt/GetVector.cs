using UnityEngine;

[ExecuteInEditMode]
public class GetVector : MonoBehaviour
{
    public Material material;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        material.SetVector("_Front", gameObject.transform.forward);
        material.SetVector("_UP", gameObject.transform.up);
        material.SetVector("_LeftDir", -gameObject.transform.right);
        Debug.Log("Forward = " + gameObject.transform.forward);
        Debug.Log("Up = " + gameObject.transform.up);
        Debug.Log("Left = " + -gameObject.transform.right);
        if (Vector3.Dot(new Vector2(gameObject.transform.right.x, gameObject.transform.right.z), new Vector2(0, 1)) < 0)
        {
            Debug.Log("ilmTex.r");
        }
        else
        {
            Debug.Log("r_ilmTex.r");
        }
    }
}