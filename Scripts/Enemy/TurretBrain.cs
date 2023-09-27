using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBrain : MonoBehaviour
{
    public Transform target;
    public float start;
    public float visangle;
    private float visionAngle;
    public float shootangle;
    public int visionSegments;
    public float closingspeeb;
    public LayerMask targetLayer;
    public LayerMask obstacleLayer;
    public float visionRange = 5f;
    private RotateFollow follow;
    private RotateScan scan;
    private TurretShoot shoot;
    public float lockontime;
    public float lockingin;
    private bool HIT;
    private bool minused=false;



    [Header("Mesh")]
    private Mesh mesh;
    public MeshFilter mf;
    public MeshRenderer mr;
    public Renderer rend;
    private bool found;
    public Color colorc;
    public Color colorr;
    public float mod;
    [SerializeField, Range(0, 1f)] private float alpha;
    void Start()
    {
        HIT = false;
        scan = GetComponent<RotateScan>();
        follow = GetComponent<RotateFollow>();
        shoot = GetComponent<TurretShoot>();
        scan.enabled = true;
        follow.enabled = false;
        lockingin = 0;

        mesh = new Mesh();
        mf.mesh = mesh;
        mr.sortingLayerName = "Default"; // Set the sorting layer of the mesh renderer
        rend.material.color = colorr;
        found = false;
        visionAngle = visangle;
        // Generate the initial mesh
        visionarea();
    }

    // Update is called once per frame
    void Update()
    {
        if (found)
        {
            rend.material.color = colorr;
        }
        else
        {
            rend.material.color = colorc;
        }

        visionarea();
        HIT = false;
    }
    void visionarea()
    {
        float startAngle = transform.eulerAngles.z - visionAngle / 2f - start / 2f;
        float angleStep = visionAngle / visionSegments;

        Vector3[] vertices = new Vector3[visionSegments + 2];
        int[] triangles = new int[visionSegments * 3];

        // Set the origin point as the first vertex
        vertices[0] = Vector3.zero;

        for (int i = 0; i <= visionSegments; i++)
        {
            float angle = startAngle + i * angleStep;


            // Calculate the direction vector based on the angle
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad) * -1);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, visionRange, obstacleLayer);


           if (Physics2D.Raycast(transform.position, direction, visionRange, obstacleLayer) && (!shoot.enabled))
            {
                vertices[i + 1] = new Vector2(((Vector2)transform.position - hit.point).x / mod * -1, ((Vector2)transform.position - hit.point).y / mod);
                Debug.DrawLine(transform.position, (Vector2)hit.point, Color.green);
                if (Physics2D.Raycast(transform.position, direction, (hit.point - (Vector2)transform.position).magnitude, targetLayer) && !Physics2D.Raycast(transform.position, direction, (hit.point - (Vector2)transform.position).magnitude, obstacleLayer))
                {
                    
                    HIT = true;
                    Debug.DrawLine(transform.position, (Vector2)hit.point, Color.red);
                    if (scan.enabled == true)
                    {
                        scan.enabled = false;
                        follow.enabled = true;
                    }
                    found = true;

                    /*if (!minused && visionAngle>1)
                    {
                        visionAngle -= Time.deltaTime * closingspeeb;
                        minused=true;
                    }*/
                }
                // Debug.Log((hit.point - (Vector2)transform.position).magnitude);
            }
            else
            {
                vertices[i + 1] = new Vector2((direction * visionRange).x / mod, (direction * -1 * visionRange).y / mod);
                Debug.DrawLine(transform.position, (Vector2)transform.position + direction * visionRange, Color.green);
            }


            if (Vector2.Distance(transform.position, target.position) > visionRange)
            {
                visionAngle = visangle;
                scan.enabled = true;
                follow.enabled = false;
                found = false;
            }

            if (i < visionSegments)
            {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        if (shoot.enabled)
        {
            shoot.enabled = false;
            visionAngle = visangle;
            scan.enabled = true;
            follow.enabled = false;
            found = false;
            lockingin = 0;
        }

        if (visionAngle<shootangle)
        {
            shoot.enabled = true;
            shoot.shoot();
        }

        if (lockingin > lockontime)
        {
            scan.enabled = false;
            follow.enabled = false;
            visionAngle -= Time.deltaTime * closingspeeb;
        }

        minused = false;

        if (!HIT && lockingin <= lockontime)
        {
            lockingin = 0;
        }

        else
        {
            lockingin += Time.deltaTime;
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
