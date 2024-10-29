using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[ExecuteInEditMode]
public class GetMaterialAndSetFront : MonoBehaviour
{
    private Renderer[] rendArray;
    private List<Material> materials = new List<Material>();


    public void UpdateMaterial()
    {
        GetModelAllMaterialsAndChange(gameObject);
    }

    private void GetModelAllMaterialsAndChange(GameObject gameObject)
    {
        materials.Clear();
        rendArray = gameObject.transform.GetComponentsInChildren<Renderer>(true);
        for (int i = 0; i < rendArray.Length; i++)
        {
            Material[] mats = rendArray[i].materials;
            for (int j = 0; j < mats.Length; j++)
            {
                materials.Add(mats[j]);
            }
        }

        
    }
    void SetVal()
    {
        for (int i = 0; i < materials.Count; i++)
        {
            try
            {
                materials[i].SetVector("_Front", gameObject.transform.forward);
                materials[i].SetVector("_UP", gameObject.transform.up);
                materials[i].SetVector("_LeftDir", -gameObject.transform.right);
            }
            catch 
            {
                Debug.Log(materials[i].name+"Haven't those val");
            }
        }
    }
    private void Awake()
    {
        GetModelAllMaterialsAndChange(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        GetModelAllMaterialsAndChange(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        SetVal();
    }
}
