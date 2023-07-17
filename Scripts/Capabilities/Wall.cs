using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{

    [SerializeField] private Transform wallcheck;
    [SerializeField] private LayerMask walllayer;
    public bool PLEASE;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PLEASE = iswalled();
    }

    public bool iswalled()
    {
        return Physics2D.OverlapCircle(wallcheck.position, 0.2f, walllayer);
    }


}
