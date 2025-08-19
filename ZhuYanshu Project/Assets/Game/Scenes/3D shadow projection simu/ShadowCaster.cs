using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ShadowCaster : MonoBehaviour
{
    public Transform lightSource; // 光源

    private Mesh shadowMesh;
    public MeshFilter obstacleMesh;
    public float zOffset = -5;
    public LayerMask layerMask;
    public float maxDistance = 50;

    public MeshCollider meshCollider;
    public float thicknessOffset;
    private Mesh shadowMesh_3d;
    public MeshFilter Mesh_3d;

    void Start()
    {
        shadowMesh = new Mesh();
        shadowMesh_3d = new Mesh();
        GetComponent<MeshFilter>().mesh = shadowMesh;
        Mesh_3d.mesh = shadowMesh_3d;
    }

    void Update()
    {
        GenerateShadowMesh();
    }

    void GenerateShadowMesh()
    {
        // transform.position = obstacleMesh.transform.position;
        // 获取障碍物几何体的顶点
        Vector3[] vertices = obstacleMesh.GetComponent<MeshFilter>().mesh.vertices;
        int[] triangles = obstacleMesh.GetComponent<MeshFilter>().mesh.triangles;

        // 计算阴影顶点
        List<Vector3> shadowVertices = new List<Vector3>();
        List<int> shadowTriangles = new List<int>();

        Vector3 lightPos = lightSource.position;
        lightPos.z = obstacleMesh.transform.position.z + zOffset;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 v1 = obstacleMesh.transform.TransformPoint(vertices[triangles[i]]);
            Vector3 v2 = obstacleMesh.transform.TransformPoint(vertices[triangles[i + 1]]);
            Vector3 v3 = obstacleMesh.transform.TransformPoint(vertices[triangles[i + 2]]);


            //计算三条射线

            RaycastHit hitInfo1 = new RaycastHit();
            var res1 = Physics.Raycast(v1, v1 - lightPos, out hitInfo1, maxDistance, layerMask);
            if (!res1)
                Debug.LogError("出错了，这个游戏的每个点都应能投影到某个对象上");

            RaycastHit hitInfo2 = new RaycastHit();
            var res2 = Physics.Raycast(v2, v2 - lightPos, out hitInfo2, maxDistance, layerMask);
            if (!res2)
                Debug.LogError("出错了，这个游戏的每个点都应能投影到某个对象上");

            RaycastHit hitInfo3 = new RaycastHit();
            var res3 = Physics.Raycast(v3, v3 - lightPos, out hitInfo3, maxDistance, layerMask);
            if (!res3)
                Debug.LogError("出错了，这个游戏的每个点都应能投影到某个对象上");

            Vector3 v1Shadow = hitInfo1.point;
            Vector3 v2Shadow = hitInfo2.point;
            Vector3 v3Shadow = hitInfo3.point;
            // 将每个顶点投射到阴影平面
            // Vector3 v1Shadow = obstacleMesh.transform.position + (v1 - lightPos) * 100;
            // Vector3 v2Shadow = obstacleMesh.transform.position + (v2 - lightPos) * 100;
            // Vector3 v3Shadow = obstacleMesh.transform.position + (v3 - lightPos) * 100;

            int startIndex = shadowVertices.Count;

            // shadowVertices.Add(v1);
            //  shadowVertices.Add(v2);
            //  shadowVertices.Add(v3);
            shadowVertices.Add(v1Shadow);
            shadowVertices.Add(v2Shadow);
            shadowVertices.Add(v3Shadow);

            // 添加前面和后面的三角形
            shadowTriangles.Add(startIndex);
            shadowTriangles.Add(startIndex + 1);
            shadowTriangles.Add(startIndex + 2);

            //  shadowTriangles.Add(startIndex + 3);
            //   shadowTriangles.Add(startIndex + 4);
            //   shadowTriangles.Add(startIndex + 5);

            // 添加侧面三角形
            // shadowTriangles.Add(startIndex);
            // shadowTriangles.Add(startIndex + 1);
            // shadowTriangles.Add(startIndex + 4);
            // shadowTriangles.Add(startIndex);
            // shadowTriangles.Add(startIndex + 4);
            // shadowTriangles.Add(startIndex + 3);
            //
            // shadowTriangles.Add(startIndex + 1);
            // shadowTriangles.Add(startIndex + 2);
            // shadowTriangles.Add(startIndex + 5);
            // shadowTriangles.Add(startIndex + 1);
            // shadowTriangles.Add(startIndex + 5);
            // shadowTriangles.Add(startIndex + 4);
            //
            // shadowTriangles.Add(startIndex + 2);
            // shadowTriangles.Add(startIndex);
            // shadowTriangles.Add(startIndex + 3);
            // shadowTriangles.Add(startIndex + 2);
            // shadowTriangles.Add(startIndex + 3);
            // shadowTriangles.Add(startIndex + 5);
        }
        // 更新Mesh
        shadowMesh.Clear();
        shadowMesh.vertices = shadowVertices.ToArray();
        shadowMesh.triangles = shadowTriangles.ToArray();


        ///////////////////////////////////////////////-更新3d Mesh-//////////////////////////////
        shadowMesh_3d.Clear();
        var vertCount = shadowVertices.Count;
        for (int i = 0; i < vertCount; i++)
        {
            var p = shadowVertices[i] + new Vector3(0, 0, -thicknessOffset);
            shadowVertices.Add(p);
        }

        shadowMesh_3d.vertices = shadowVertices.ToArray();

        List<int> shadowTriangles3D = EarclippingTriangulation(shadowVertices);
        shadowMesh_3d.triangles = shadowTriangles3D.ToArray();
        shadowMesh.RecalculateNormals();
        meshCollider.sharedMesh = shadowMesh_3d;
    }

    // 一个简单的Earclipping算法的示例，用于生成凸多边形的三角形
    private List<int> EarclippingTriangulation(List<Vector3> vertices)
    {
        List<int> indices = new List<int>();
        int numVertices = vertices.Count;
        if (numVertices < 3)
            return indices;

        int[] vertexIndices = new int[numVertices];
        for (int i = 0; i < numVertices; i++)
            vertexIndices[i] = i;

        for (int i = 0; i < numVertices;)
        {
            // 找到耳朵的顶点
            int a = i;
            i = (i + 1) % numVertices;
            int b = i;
            i = (i + 1) % numVertices;
            int c = i;

            // 检查是否所有的点都在耳朵的同一侧
            bool isEar = true;
            for (int j = 0; j < numVertices; j++)
            {
                if (j == a || j == b || j == c)
                    continue;

                Vector3 p = vertices[j];
                if (Vector3.Cross(vertices[b] - vertices[a], vertices[c] - vertices[a]).magnitude > 0 &&
                    Vector3.Dot(p - vertices[a], Vector3.Cross(vertices[b] - vertices[a], vertices[c] - vertices[a])) < 0)
                {
                    isEar = false;
                    break;
                }
            }

            if (isEar)
            {
                indices.Add(a);
                indices.Add(b);
                indices.Add(c);

                numVertices--;
                for (int j = 0; j < numVertices; j++)
                {
                    if (j == (b + 1) % numVertices || j == (c + 1) % numVertices)
                        vertexIndices[j] = a;
                }
            }
            else if (i >= numVertices - 2)
            {
                // 无法形成三角形
                indices.Clear();
                break;
            }
        }

        return indices;
    }
}