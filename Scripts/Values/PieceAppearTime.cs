using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceAppearTime : MonoBehaviour
{
    public float time = 0f;
    public Vector3 pos;
    public Quaternion rot;

    private void Awake()
    {
        pos = transform.position;
        rot = transform.rotation;
    }
}
