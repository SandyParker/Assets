using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour
{
    public float visionAngle = 90f;
    public float visionRange = 5f;
    public int visionSegments = 10;
    public LayerMask targetLayer;
	public LayerMask obstacleLayer;
    public float mod;
    public RotateView rv;

    private Mesh mesh;
    public MeshFilter mf;
    public MeshRenderer mr;

    void Start()
    {
        mesh = new Mesh();
        mf.mesh = mesh;
        mr.sortingLayerName = "Default"; // Set the sorting layer of the mesh renderer

        // Generate the initial mesh
        UpdateMesh();
    }

    void Update()
    {
        if (transform.right.x==1)
        {
            rv.RotateRight();
        }
        else
        {
            rv.RotateLeft();
        }

        // Update the mesh each frame
        UpdateMesh();
    }

    void UpdateMesh()
    {
        // Calculate the angle range for the vision cone
        float startAngle = transform.eulerAngles.z - visionAngle / 2f;
        float endAngle = transform.eulerAngles.z + visionAngle / 2f;

        // Calculate the angle between each vision segment
        float angleStep = visionAngle / visionSegments;

        // Generate the vertices and triangles for the mesh
        Vector3[] vertices = new Vector3[visionSegments + 2];
        int[] triangles = new int[visionSegments * 3];

        // Set the origin point as the first vertex
        vertices[0] = Vector3.zero;

        // Iterate through each vision segment and calculate the position
        for (int i = 0; i <= visionSegments; i++)
        {
            float angle = startAngle + i * angleStep;

            // Calculate the direction vector based on the angle
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * transform.right.x, Mathf.Sin(angle * Mathf.Deg2Rad) * -1);

            // Raycast to check for obstacles
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, visionRange, obstacleLayer);

            // Calculate the position of the vision cone point
            if (Physics2D.Raycast(transform.position, direction, visionRange, obstacleLayer))
            {
                vertices[i + 1] = (hit.point - (Vector2)transform.position) / mod;
                Debug.DrawLine(transform.position, (Vector2)hit.point, Color.green);
               // Debug.Log((hit.point - (Vector2)transform.position).magnitude);
                if (Physics2D.Raycast(transform.position, direction, (hit.point - (Vector2)transform.position).magnitude, targetLayer) && !Physics2D.Raycast(transform.position, direction, (hit.point - (Vector2)transform.position).magnitude, obstacleLayer))
                {
                    Debug.Log("Target Found");
                }
            }
            else
            {
                vertices[i + 1] = (direction * visionRange) / mod;
                Debug.DrawLine(transform.position, (Vector2)transform.position + direction * visionRange, Color.red);
                if (Physics2D.Raycast(transform.position, direction, visionRange, targetLayer) && !Physics2D.Raycast(transform.position, direction, visionRange, obstacleLayer))
                {
                    Debug.Log("Target Found");
                }
            }

            // Set the triangles for each segment
            if (i < visionSegments)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        // Assign the vertices and triangles to the mesh
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}