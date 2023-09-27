using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGhost : MonoBehaviour
{
    public float ghostDelay;
    private float delay;
    public GameObject ghost;
    public bool makeghost = false;
    public SpriteRenderer renderer;
    void Start()
    {
        delay = ghostDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (makeghost)
        {
            if (delay > 0)
            {
                delay -= Time.unscaledDeltaTime;
            }
            else
            {
                GameObject current = Instantiate(ghost, transform.position, transform.rotation);
                Sprite currentSprite = renderer.sprite;
                current.GetComponent<SpriteRenderer>().sprite = currentSprite;
                delay = ghostDelay;
                Destroy(current,1f);
            }
        }
    }
}
