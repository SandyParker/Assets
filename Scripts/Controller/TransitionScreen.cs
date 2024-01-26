using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class TransitionScreen : MonoBehaviour
{
    public GameObject Loading;
    public Animator transition;
    public Slider progressbar;
    private int Scene;
    private bool loadscene = false;
    private AsyncOperation operation;
    public GameObject progress;

    public void Load(int scene)
    {
        StartCoroutine(screentransition(1));    
        Scene = scene;
    }

    IEnumerator screentransition(float time)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(time);
        loadscene = true;
    }

    public void reload()
    {
        Load(SceneManager.GetActiveScene().buildIndex);
    }

    private void Update()
    {
        if (loadscene)
        {
            loadscene = false;
            //SceneManager.LoadScene(Scene);
            //StartCoroutine(LoadAsync(Scene));
            operation = SceneManager.LoadSceneAsync(Scene);
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }

        /*if (operation != null) 
        {
            while (operation.isDone == false)
            {
                progressbar.value = operation.progress;
            }
        }*/
            
    }

    public void ExitApp()
    {
        Application.Quit();
    }

    IEnumerator LoadAsync(int scene)
    {
        //AsyncOperation operation = SceneManager.LoadSceneAsync(scene);

        while (operation.isDone == false) 
        {
            //progressbar.value = operation.progress;
            yield return null;
        }
    }
}
