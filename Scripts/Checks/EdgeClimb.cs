using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdgeClimb : MonoBehaviour
{

    [Header("Edge Grab")]
    public bool mantleready;
    public bool greenBox, Redbox, OnEdge;
    public float RedXOffset, RedYOffset, RedXSize, RedYSize, GreenXOffset, GreenYOffset, GreenXSize, GreenYSize;
    public LayerMask groundmask;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        greenBox = Physics2D.OverlapBox(new Vector2(transform.position.x + (GreenXOffset * transform.localScale.x * transform.right.x), transform.position.y + GreenYOffset), new Vector2(GreenXSize, GreenYSize), 0f, groundmask);
        Redbox = Physics2D.OverlapBox(new Vector2(transform.position.x + (RedXOffset * transform.localScale.x * transform.right.x), transform.position.y + RedYOffset), new Vector2(RedXSize, RedYSize), 0f, groundmask);
        mantleready = OnEdge;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + (GreenXOffset * transform.localScale.x * transform.right.x), transform.position.y + GreenYOffset), new Vector2(GreenXSize, GreenYSize));
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + (RedXOffset * transform.localScale.x * transform.right.x), transform.position.y + RedYOffset), new Vector2(RedXSize, RedYSize));
    }

    public bool IsOnEdge()
    {
        if (greenBox && !Redbox)
        {
            OnEdge = true;
        }
        else
        {
            OnEdge = false;
        }
        return OnEdge;
    }
}
