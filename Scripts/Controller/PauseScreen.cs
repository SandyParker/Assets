using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PauseScreen : MonoBehaviour
{
    public static bool paused = false;
    public GameObject PauseUI;

    private void Awake()
    {
        MoveUpdate.playercontrols.Controls.Pause.performed += _ => pause();
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            pause();
        }*/
    }

    public void pause()
    {
        if (!paused)
        {
            Time.timeScale = 0f;
            GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>().speed = 0f;
            MoveUpdate.allowed = false;
            paused = true;
            PauseUI.SetActive(true);
        }
        else
        {
            MoveUpdate.allowed = true;
            Time .timeScale = 1f;
            GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Animator>().speed = 1f;
            paused = false;
            PauseUI.SetActive(false);
        }
    }

    public void Menu()
    {
        paused = false;
    }

    public void quit()
    {
        Application.Quit();
    }
}
