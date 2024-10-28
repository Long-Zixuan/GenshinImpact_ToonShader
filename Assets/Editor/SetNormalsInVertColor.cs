using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SetNormalsInVertColor : MonoBehaviour
{
    public string NewMeshPath = "Assets/";
    void Awake()
    {
        //获取Mesh
        Mesh mesh = new Mesh();
        if (GetComponent<SkinnedMeshRenderer>())
        {
            mesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;
        }
        if (GetComponent<MeshFilter>())
        {
            mesh = GetComponent<MeshFilter>().sharedMesh;
        }
        Debug.Log(mesh.name);

        //声明一个Vector3数组，长度与mesh.normals一样，用于存放
        //与mesh.vertices中顶点一一对应的光滑处理后的法线值
        Vector3[] meshNormals = new Vector3[mesh.normals.Length];

        //开始一个循环，循环的次数 = mesh.normals.Length = mesh.vertices.Length = meshNormals.Length
        for (int i = 0; i < meshNormals.Length; i++)
        {
            //定义一个零值法线
            Vector3 Normal = new Vector3(0, 0, 0);
            //遍历mesh.vertices数组，如果遍历到的值与当前序号顶点值相同，则将其对应的法线与Normal相加
            for (int j = 0; j < meshNormals.Length; j++)
            {
                if (mesh.vertices[j] == mesh.vertices[i])
                {
                    Normal += mesh.normals[j];
                }
            }
            //归一化Normal并将meshNormals数列对应位置赋值为Normal,到此序号为i的顶点的对应法线光滑处理完成
            //此时求得的法线为模型空间下的法线
            Normal.Normalize();
            meshNormals[i] = Normal;
        }

        //构建模型空间→切线空间的转换矩阵
        ArrayList OtoTMatrixs = new ArrayList();
        for (int i = 0; i < mesh.normals.Length; i++)
        {
            Vector3[] OtoTMatrix = new Vector3[3];
            OtoTMatrix[0] = new Vector3(mesh.tangents[i].x, mesh.tangents[i].y, mesh.tangents[i].z);
            OtoTMatrix[1] = Vector3.Cross(mesh.normals[i], OtoTMatrix[0]);
            OtoTMatrix[1] = new Vector3(OtoTMatrix[1].x * mesh.tangents[i].w, OtoTMatrix[1].y * mesh.tangents[i].w, OtoTMatrix[1].z * mesh.tangents[i].w);
            OtoTMatrix[2] = mesh.normals[i];
            OtoTMatrixs.Add(OtoTMatrix);
        }

        //将meshNormals数组中的法线值一一与矩阵相乘，求得切线空间下的法线值
        for (int i = 0; i < meshNormals.Length; i++)
        {
            Vector3 tNormal;
            tNormal = Vector3.zero;
            tNormal.x = Vector3.Dot(((Vector3[])OtoTMatrixs[i])[0], meshNormals[i]);
            tNormal.y = Vector3.Dot(((Vector3[])OtoTMatrixs[i])[1], meshNormals[i]);
            tNormal.z = Vector3.Dot(((Vector3[])OtoTMatrixs[i])[2], meshNormals[i]);
            meshNormals[i] = tNormal;
        }

        //新建一个颜色数组把光滑处理后的法线值存入其中
        Color[] meshColors = new Color[mesh.colors.Length];
        for (int i = 0; i < meshColors.Length; i++)
        {
            meshColors[i].r = meshNormals[i].x * 0.5f + 0.5f;
            meshColors[i].g = meshNormals[i].y * 0.5f + 0.5f;
            meshColors[i].b = meshNormals[i].z * 0.5f + 0.5f;
            meshColors[i].a = mesh.colors[i].a;
        }

        //新建一个mesh，将之前mesh的所有信息copy过去
        Mesh newMesh = new Mesh();
        newMesh.vertices = mesh.vertices;
        newMesh.triangles = mesh.triangles;
        newMesh.normals = mesh.normals;
        newMesh.tangents = mesh.tangents;
        newMesh.uv = mesh.uv;
        newMesh.uv2 = mesh.uv2;
        newMesh.uv3 = mesh.uv3;
        newMesh.uv4 = mesh.uv4;
        newMesh.uv5 = mesh.uv5;
        newMesh.uv6 = mesh.uv6;
        newMesh.uv7 = mesh.uv7;
        newMesh.uv8 = mesh.uv8;
        //将新模型的颜色赋值为计算好的颜色
        newMesh.colors = meshColors;
        newMesh.colors32 = mesh.colors32;
        newMesh.bounds = mesh.bounds;
        newMesh.indexFormat = mesh.indexFormat;
        newMesh.bindposes = mesh.bindposes;
        newMesh.boneWeights = mesh.boneWeights;
        //将新mesh保存为.asset文件，路径可以是"Assets/Character/Shader/VertexColorTest/TestMesh2.asset"                          
        AssetDatabase.CreateAsset(newMesh, NewMeshPath);
        AssetDatabase.SaveAssets();
        Debug.Log("Done");
    }
}


