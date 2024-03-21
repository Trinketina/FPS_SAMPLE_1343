using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCam : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera vcam;
    CinemachineBasicMultiChannelPerlin perlin;

    float time = 0;

    private void Start()
    {
        perlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }


    // Update is called once per frame
    void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime * 10;
            perlin.m_FrequencyGain = time;
        }
        else
            perlin.m_AmplitudeGain = 0;
    }

    public void StartShake(float amp)
    {
        
        perlin.m_AmplitudeGain = amp;
        time = 1;
        perlin.m_FrequencyGain = time;
    }
}
