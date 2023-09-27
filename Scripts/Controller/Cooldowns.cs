using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cooldowns : MonoBehaviour
{

    public Image image;
    public float cooldown;
    public bool iscooldown = false;
    // Start is called before the first frame update
    void Awake()
    {
        image.fillAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (iscooldown)
        {
            image.fillAmount += 1 / cooldown * Time.deltaTime;

            if (image.fillAmount>=1)
            {
                image.fillAmount = 1;
                iscooldown = false;
            }
        }
    }
}
