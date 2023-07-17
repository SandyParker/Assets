using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateView : MonoBehaviour
{
    // Start is called before the first frame update
    public void RotateRight()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
            
    public void RotateLeft()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
           
}
