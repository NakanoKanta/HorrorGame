using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class VisionMesh : MonoBehaviour
{
    public float viewDistance = 6f;
    public float viewAngle = 90f;
    public int segments = 10;

    public EnemyPatrol patrol; // EnemyPatrol ÇéQè∆

    MeshFilter mf;
    Mesh mesh;

    void Start()
    {
        mf = GetComponent<MeshFilter>();
        mesh = new Mesh();
        mf.mesh = mesh;
    }

    void LateUpdate()
    {
        if (patrol != null)
        {
            Vector3 forward = patrol.CurrentDirection != Vector2.zero ? (Vector3)patrol.CurrentDirection : Vector3.right;
            DrawVisionMesh(forward);
        }
    }

    void DrawVisionMesh(Vector3 forward)
    {
        mesh.Clear();

        Vector3[] vertices = new Vector3[segments + 2];
        int[] triangles = new int[segments * 3];

        vertices[0] = Vector3.zero; // íÜêS

        float halfAngle = viewAngle / 2f;
        for (int i = 0; i <= segments; i++)
        {
            float angle = -halfAngle + i * (viewAngle / segments);
            Vector3 dir = Quaternion.Euler(0, 0, angle) * forward;
            vertices[i + 1] = dir.normalized * viewDistance;
        }

        for (int i = 0; i < segments; i++)
        {
            triangles[i * 3 + 0] = 0;
            triangles[i * 3 + 1] = i + 1;
            triangles[i * 3 + 2] = i + 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
}
