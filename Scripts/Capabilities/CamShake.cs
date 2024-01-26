using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamShake : MonoBehaviour
{
    public CinemachineVirtualCamera vm;
    private float startintensity;
    private float totalshaketime;
    private float shaketime;

    public void ShakeCam(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin perlin = vm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        perlin.m_AmplitudeGain = intensity;
        startintensity = intensity;
        totalshaketime = time;
        shaketime = time;
    }

    private void Update()
    {
        if (shaketime > 0)
        {
            shaketime -= Time.deltaTime;
            if (shaketime <= 0)
            {
                CinemachineBasicMultiChannelPerlin perlin = vm.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                var lerp = Mathf.Lerp(startintensity, 0f, 1 - (shaketime / totalshaketime));
                perlin.m_AmplitudeGain = lerp;
            }
        }
    }
}
