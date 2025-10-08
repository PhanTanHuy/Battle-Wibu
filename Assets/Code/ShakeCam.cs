using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShakeCam : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noise;
    private Coroutine currentStopShakeCoroutine;

    void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        currentStopShakeCoroutine = null;
    }
    public void Shake(float thoiGianRung, float doManh, float tanSoRung)
    {
        if (doManh < noise.m_AmplitudeGain) return;
        noise.m_AmplitudeGain = doManh;
        noise.m_FrequencyGain = tanSoRung;
        if (currentStopShakeCoroutine != null)
        {
            StopCoroutine(currentStopShakeCoroutine);
        }
        currentStopShakeCoroutine = StartCoroutine(NgungShakeCam(thoiGianRung));
    }
    IEnumerator NgungShakeCam(float time)
    {
        yield return new WaitForSeconds(time);
        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;
    }
}
