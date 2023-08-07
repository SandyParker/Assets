using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFollow : MonoBehaviour
{
    public Transform target;
    public float speeb;
    private float rotation;
    public float start;
    float r;
    bool left;
    public float visionAngle;
    public float visionSegments;
    public LayerMask targetLayer;
    public LayerMask obstacleLayer;
    public float visionRange = 5f;
    void Start()
    {
        left = true;
    }

    void Update()
    {
        Vector3 rotat = target.position - transform.position;
        float rotz = Mathf.Atan2(rotat.x, rotat.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 180, rotz);

        visionarea();

    }

    void visionarea()
    {
        float startAngle = transform.eulerAngles.z - visionAngle / 2f - start / 2f;
        float angleStep = visionAngle / visionSegments;

        for (int i = 0; i <= visionSegments; i++)
        {
            float angle = startAngle + i * angleStep;

            // Calculate the direction vector based on the angle
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad) * -1);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, visionRange, obstacleLayer);

            if (Physics2D.Raycast(transform.position, direction, visionRange, obstacleLayer))
            {
                Debug.DrawLine(transform.position, (Vector2)hit.point, Color.green);
                // Debug.Log((hit.point - (Vector2)transform.position).magnitude);
            }
            else
            {
                Debug.DrawLine(transform.position, (Vector2)transform.position + direction * visionRange, Color.red);
            }
        }
    }
}
