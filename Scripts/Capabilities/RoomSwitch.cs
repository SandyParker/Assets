using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSwitch : MonoBehaviour
{
    public GameObject virtualcam;
    public GameObject universal;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            universal.SetActive(false);
            virtualcam.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            universal.SetActive(true);
            virtualcam.SetActive(false);
        }
    }
}
