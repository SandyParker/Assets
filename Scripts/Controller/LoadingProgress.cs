using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingProgress : MonoBehaviour
{
    public GameObject progress;

    public void startload()
    {
        progress.SetActive(true);
    }
}
