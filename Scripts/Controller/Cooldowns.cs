using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cooldowns : MonoBehaviour
{

    public Image blink;
    public float cooldown1;
    public bool iscooldown = false;
    // Start is called before the first frame update
    void Awake()
    {
        blink.fillAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (iscooldown)
        {
            blink.fillAmount += 1 / cooldown1 * Time.deltaTime;

            if (blink.fillAmount>=1)
            {
                blink.fillAmount = 1;
                iscooldown = false;
            }
        }
    }
}
