using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanExplosionActivate : MonoBehaviour
{
    [SerializeField] private Transform shatter;
    public CamExplosion explosion;
    public void Explosion()
    {
        explosion.active = true;
    }
}
