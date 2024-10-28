using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SetNormalsInVertColor : MonoBehaviour
{
    public string NewMeshPath = "Assets/";
    void Awake()
    {
        //��ȡMesh
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

        //����һ��Vector3���飬������mesh.normalsһ�������ڴ��
        //��mesh.vertices�ж���һһ��Ӧ�Ĺ⻬�����ķ���ֵ
        Vector3[] meshNormals = new Vector3[mesh.normals.Length];

        //��ʼһ��ѭ����ѭ���Ĵ��� = mesh.normals.Length = mesh.vertices.Length = meshNormals.Length
        for (int i = 0; i < meshNormals.Length; i++)
        {
            //����һ����ֵ����
            Vector3 Normal = new Vector3(0, 0, 0);
            //����mesh.vertices���飬�����������ֵ�뵱ǰ��Ŷ���ֵ��ͬ�������Ӧ�ķ�����Normal���
            for (int j = 0; j < meshNormals.Length; j++)
            {
                if (mesh.vertices[j] == mesh.vertices[i])
                {
                    Normal += mesh.normals[j];
                }
            }
            //��һ��Normal����meshNormals���ж�Ӧλ�ø�ֵΪNormal,�������Ϊi�Ķ���Ķ�Ӧ���߹⻬�������
            //��ʱ��õķ���Ϊģ�Ϳռ��µķ���
            Normal.Normalize();
            meshNormals[i] = Normal;
        }

        //����ģ�Ϳռ�����߿ռ��ת������
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

        //��meshNormals�����еķ���ֵһһ�������ˣ�������߿ռ��µķ���ֵ
        for (int i = 0; i < meshNormals.Length; i++)
        {
            Vector3 tNormal;
            tNormal = Vector3.zero;
            tNormal.x = Vector3.Dot(((Vector3[])OtoTMatrixs[i])[0], meshNormals[i]);
            tNormal.y = Vector3.Dot(((Vector3[])OtoTMatrixs[i])[1], meshNormals[i]);
            tNormal.z = Vector3.Dot(((Vector3[])OtoTMatrixs[i])[2], meshNormals[i]);
            meshNormals[i] = tNormal;
        }

        //�½�һ����ɫ����ѹ⻬�����ķ���ֵ��������
        Color[] meshColors = new Color[mesh.colors.Length];
        for (int i = 0; i < meshColors.Length; i++)
        {
            meshColors[i].r = meshNormals[i].x * 0.5f + 0.5f;
            meshColors[i].g = meshNormals[i].y * 0.5f + 0.5f;
            meshColors[i].b = meshNormals[i].z * 0.5f + 0.5f;
            meshColors[i].a = mesh.colors[i].a;
        }

        //�½�һ��mesh����֮ǰmesh��������Ϣcopy��ȥ
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
        //����ģ�͵���ɫ��ֵΪ����õ���ɫ
        newMesh.colors = meshColors;
        newMesh.colors32 = mesh.colors32;
        newMesh.bounds = mesh.bounds;
        newMesh.indexFormat = mesh.indexFormat;
        newMesh.bindposes = mesh.bindposes;
        newMesh.boneWeights = mesh.boneWeights;
        //����mesh����Ϊ.asset�ļ���·��������"Assets/Character/Shader/VertexColorTest/TestMesh2.asset"                          
        AssetDatabase.CreateAsset(newMesh, NewMeshPath);
        AssetDatabase.SaveAssets();
        Debug.Log("Done");
    }
}


