using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Outline : MonoBehaviour
{
    private MeshFilter meshFilter;
    private Vector3[] nList;
    private Vector3[] vList;
    private Color[] cList;
    private Color[] ori_cList;

    private void OnEnable()
    {
        InitData();
        SmoothData();
    }
    private void OnDisable()
    {
        if (meshFilter == null) return;
        ResetData();
    }

    private void InitData()
    {
        meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            enabled = false;
            return;
        }
        nList = meshFilter.sharedMesh.normals;
        vList = meshFilter.sharedMesh.vertices;
        ori_cList = new Color[vList.Length];
        cList = new Color[vList.Length];

        for (int i = 0; i < ori_cList.Length; i++)
        {
            ori_cList[i] = new Color(0.5f, 0.5f, 0.5f);
        }
    }

    private void SmoothData()
    {
        for (int i = 0; i < nList.Length; i++)
        {
            Vector3 nor = Vector3.zero;
            for (int j = 0; j < nList.Length; j++)
            {
                if (vList[i] == vList[j])
                {
                    nor += nList[j];
                }
            }
            //[-1, 1] -> [0, 1]
            //obj -> tangent
            Vector3 nCol = Obj2Tangent(nor.normalized, i) * 0.5f + Vector3.one * 0.5f;
            cList[i] = new Color(nCol.x, nCol.y, nCol.z);
        }
        meshFilter.sharedMesh.SetColors(cList);//set data
    }

    private Vector3 Obj2Tangent(Vector3 ori, int id)
    {
        Vector4 t4 = meshFilter.sharedMesh.tangents[id];
        //tbn
        Vector3 t = new Vector3(t4.x, t4.y, t4.z);
        Vector3 n = meshFilter.sharedMesh.normals[id];
        Vector3 b = Vector3.Cross(n, t) * t4.w;

        Vector3 tNor = Vector3.zero;
        tNor.x = t.x * ori.x + t.y * ori.y + t.z * ori.z;
        tNor.y = b.x * ori.x + b.y * ori.y + b.z * ori.z;
        tNor.z = n.x * ori.x + n.y * ori.y + n.z * ori.z;

        return tNor;
    }

    private void ResetData()
    {
        meshFilter.sharedMesh.SetColors(ori_cList);
    }
}