using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Mana"))
        {
            GetComponent<Player>().mana += collision.GetComponent<ManaCount>().mana;
            Destroy(collision.gameObject);
        }
    }
}
