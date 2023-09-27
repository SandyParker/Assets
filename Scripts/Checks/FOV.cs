using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour
{
    [Header("View")]
    public float visionAngle = 90f;
    public float visionRange = 5f;
    public int visionSegments = 10;
    public LayerMask targetLayer;
	public LayerMask obstacleLayer;
    public float mod;
    public RotateView rv;
    private float Direction;
    public Rigidbody2D rb;

    private Mesh mesh;
    public MeshFilter mf;
    public MeshRenderer mr;
    public Renderer rend;
    private bool found;
    private Color color;
    [SerializeField, Range(0, 1f)] private float alpha;

    [Header("AI")]
    private Patrol patrol;
    private EnemyAI AI;
    public float chasetime;
    public float chasing;
    void Start()
    {
        mesh = new Mesh();
        mf.mesh = mesh;
        mr.sortingLayerName = "Default"; // Set the sorting layer of the mesh renderer
        rend.material.color=Color.red;
        found = false;
        Direction = -1;
        patrol = GetComponent<Patrol>();
        AI = GetComponent<EnemyAI>();
        chasing = chasetime;
        color.a = alpha;

        // Generate the initial mesh
        UpdateMesh();
    }

    void Update()
    {

        if (found)
        {
            color = Color.red;
            patrol.enabled = false;
            AI.enabled = true;
            chasing = 0;
        }
        else
        {
            color = Color.cyan;
        }
        color.a = alpha;
        if (chasing < chasetime)
        {
            chasing += Time.deltaTime;
        }
        else
        {
            patrol.enabled = true;
            AI.enabled = false;
        }

        if (rb.velocity.x>0)
        {
            Direction = 1;
        }
        else if (rb.velocity.x<0)
        {
            Direction = -1;
        }

        // Update the mesh each frame
        UpdateMesh();
        rend.material.color = color;

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
        found = false;

        // Iterate through each vision segment and calculate the position
        for (int i = 0; i <= visionSegments; i++)
        {
            float angle = startAngle + i * angleStep;

            // Calculate the direction vector based on the angle
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad) * Direction, Mathf.Sin(angle * Mathf.Deg2Rad) * -1);

            // Raycast to check for obstacles
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, visionRange, obstacleLayer);

            // Calculate the position of the vision cone point
            if (Physics2D.Raycast(transform.position, direction, visionRange, obstacleLayer))
            {
                vertices[i + 1] = new Vector2((hit.point - (Vector2)transform.position).x / mod * Direction * -1, (hit.point - (Vector2)transform.position).y / mod);
                Debug.DrawLine(transform.position, (Vector2)hit.point, Color.green);
                if (Physics2D.Raycast(transform.position, direction, (hit.point - (Vector2)transform.position).magnitude, targetLayer) && !Physics2D.Raycast(transform.position, direction, (hit.point - (Vector2)transform.position).magnitude, obstacleLayer))
                {
                    Debug.Log("Target Found");
                    found = true;
                }
                // Debug.Log((hit.point - (Vector2)transform.position).magnitude);
            }
            else
            {
                vertices[i + 1] = new Vector2((direction * visionRange).x / mod * Direction*-1, (direction * visionRange).y / mod);
                Debug.DrawLine(transform.position, (Vector2)transform.position + direction * visionRange, Color.red);
                if (Physics2D.Raycast(transform.position, direction, visionRange, targetLayer) && !Physics2D.Raycast(transform.position, direction, visionRange, obstacleLayer))
                {
                    found = true;
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