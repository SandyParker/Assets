using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamExplosion : MonoBehaviour
{
    public Transform explosionPos;
    public float force;
    public float speed;
    public bool active;
    public float timer = 0;
    public float explodetime;
    public bool exploded;
    public float endtime;
    private float allactive;
    public KnifeThrow kt;
    public AdenturerAnimation anim;
    private void Update()
    {
        if (active)
        {
            allactive = 0;
            timer += Time.unscaledDeltaTime;
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.active)
                {
                    allactive++;
                }
                if (allactive == transform.childCount)
                {
                    kt.slow = false;
                }
                if (timer > transform.GetChild(i).GetComponent<PieceAppearTime>().time)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
                if (timer > explodetime && !exploded)
                {
                    transform.GetChild(i).GetComponent<Rigidbody>().useGravity = true;
                    transform.GetChild(i).GetComponent<Rigidbody>().AddExplosionForce(force, explosionPos.position, speed);
                    anim.anim.SetBool("IsUltEnd", true);
                    if (i == transform.childCount-1)
                        exploded = true;
                }
                if (timer > endtime)
                {
                    active = false;
                    anim.anim.SetBool("IsUltEnd", false);
                }
            }
        }

        else
        {
            exploded = false;
            timer = 0;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Rigidbody>().velocity = new Vector3 (0,0, 0);
                transform.GetChild(i).GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
                transform.GetChild(i).GetComponent<Rigidbody>().useGravity = false;
                transform.GetChild(i).GetComponent<Rigidbody>().transform.position = transform.GetChild(i).GetComponent<PieceAppearTime>().pos;
                transform.GetChild(i).GetComponent<Rigidbody>().transform.rotation = transform.GetChild(i).GetComponent<PieceAppearTime>().rot;
                //transform.GetChild(i).GetComponent<Rigidbody>().AddExplosionForce(0, explosionPos.position, speed);
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        
        /*foreach (Transform child in transform)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childrb))
            {
                childrb.useGravity = true; 
                childrb.AddExplosionForce(force, explosionPos.position, speed);
            }
        }*/
    }
}
