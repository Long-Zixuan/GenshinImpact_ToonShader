using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialChange : MonoBehaviour
{
    public Material[] materialsL;

    public Material[] materialsS;

    //private List<Material> materials = new List<Material>();

    private Renderer[] rendArray;

    //public Button bt;
    void Awake()
    {
        GetModelAllMaterialsAndChange(gameObject);
    }

    void Start()
    {
       
        GetModelAllMaterialsAndChange(gameObject);

        //bt.onClick.AddListener(ChangeShaderTwo);
    }



    private void GetModelAllMaterialsAndChange(GameObject gameObject)
    {
       // materials.Clear();
        rendArray = gameObject.GetComponentsInChildren<Renderer>(true);
      /*  for (int i = 0; i < rendArray.Length; i++)
        {
            Material[] mats = rendArray[i].materials;
            for (int j = 0; j < mats.Length; j++)
            {
                materials.Add(mats[j]);
            }
        }*/


    }

    public void Use_N_M()
    {
        /*int dir = 0;
        for(int i = 0; i < materials.Count; )
        {

            for (int j = 0; j < rendArray[dir].materials.Length; j++)
            {
                rendArray[dir].materials[j] = materialsS[i];
                i++;
            }
            dir++;
        }*/
        //rendArray = gameObject.transform.GetComponentsInChildren<Renderer>(true);
        int dir = 0;
        for (int i = 0; i < rendArray.Length; i++)
        {
            int j;
            Material[] mats = new Material[rendArray[i].materials.Length];
            for (j = 0; j < rendArray[i].materials.Length; j++)
            {

                mats[j] = materialsS[dir];

                // rendArray[i].materials[j] = materialsL[dir];
                print(rendArray[i].materials[j].name + " to " + materialsS[dir].name + "(S)");
                dir++;
            }

            rendArray[i].materials = mats;
        }
        print("dir =" + dir);
    }

    public void Use_L_M()
    {
        /* int dir = 0;
         for (int i = 0; i < materials.Count;)
         {

             for (int j = 0; j < rendArray[dir].materials.Length; j++)
             {
                 rendArray[dir].materials[j] = materialsL[i];
                 i++;
             }
             dir++;
         }*/
        //rendArray = gameObject.transform.GetComponentsInChildren<Renderer>(true);
        int dir = 0;
        for (int i = 0; i < rendArray.Length; i++)
        {
            int j;
            Material[] mats = new Material[rendArray[i].materials.Length];
            for (j = 0; j < rendArray[i].materials.Length; j++)
            {
                
                mats[j] = materialsL[dir];

               // rendArray[i].materials[j] = materialsL[dir];
                print(rendArray[i].materials[j].name + " " + materialsL[dir].name+"(L)");
                dir++;
            }

            rendArray[i].materials = mats;
        }
        print("dir =" + dir);
    }
}
