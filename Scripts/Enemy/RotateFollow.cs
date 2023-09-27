using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateFollow : MonoBehaviour
{
    public Transform target;
    public float speeb;
    public float start;
    void Start()
    {
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
        /*Vector3 rotat = target.position - transform.position;
        float rotz = Mathf.Atan2(rotat.x, rotat.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 180, rotz);*/

        float angle = Mathf.Atan2(target.position.x - transform.position.x, target.position.y - transform.position.y) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 180, angle));
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, speeb * Time.deltaTime);
    }

}
