using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretShoot : MonoBehaviour
{
    public Transform firepoint;
    public Transform rotate;
    public GameObject Laser;

    private void Update()
    {
        
    }

    public void shoot()
    {
        Instantiate(Laser, firepoint.position, rotate.rotation);

    }
}
/*public float start;
    public LayerMask targetLayer;
    [SerializeField] private float DistanceRay = 100f;
    public Transform laserPoint;
    public LineRenderer line;
    public float visionRange = 5f;
    Transform trans;
    void Start()
    {
        trans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        float angle = transform.eulerAngles.z - start / 2f;

        Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad) * -1);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, visionRange, targetLayer);
        if (Physics2D.Raycast(trans.position,direction))
        {
            Draw2DRay(laserPoint.position, hit.point);
        }
        else
        {
            Draw2DRay(laserPoint.position,  hit.point);
        }
    }

    void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        line.SetPosition(0, startPos);
        line.SetPosition(1, endPos);
    }*/
