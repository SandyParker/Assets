using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursordefault;
    [SerializeField] private Texture2D cursorpressed;
    private Texture2D discursor;
    [SerializeField ]private bool ispressed;

    private Vector2 cursorHotsport;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ispressed |= Input.GetButtonDown("Fire2");
        if (ispressed)
        {
            discursor = cursorpressed;
        }
        else
        {
            discursor = cursordefault;
        }

        if (ispressed && Input.GetButtonUp("Fire2"))
        {
            ispressed = false;
        }
        cursorHotsport = new Vector2(discursor.width / 2, discursor.height / 2);
        Cursor.SetCursor(discursor, cursorHotsport, CursorMode.Auto);
    }
}
