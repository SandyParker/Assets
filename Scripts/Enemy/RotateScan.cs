using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateScan : MonoBehaviour
{
    public float target;
    public float speeb;
    public float rotation;
    public float start;
    float r;
    bool left;
    void Start()
    {
        int test = Random.Range(0, 2);
        Debug.Log(test);
        left = test==0?true:false;
    }

    private void Awake()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }
    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }
    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.Gameplay;
    }

    void Update()
    {
        float Angle = Mathf.SmoothDampAngle(transform.eulerAngles.z, rotation + start, ref r, 0.01f);

        transform.rotation = Quaternion.Euler(0, 180, Angle);

        if (rotation <= target && left)
        {
            rotation += speeb * Time.deltaTime;
        }
        else if (rotation >= -target && !left)
        {
            rotation -= speeb * Time.deltaTime;
        }

        if (rotation > target)
        {
            left = false;
        }

        if(rotation < -target)
        {
            left = true;
        }

    }

}
