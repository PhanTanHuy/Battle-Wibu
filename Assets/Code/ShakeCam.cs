using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCam : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin noise;
    private bool haveToShake, haveToStopShake;
    private float _tgr;

    void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
    void Start()
    {
        GetComponent<CinemachineConfiner>().m_BoundingShape2D = GameObject.Find("boundVirtualCamera").GetComponent<PolygonCollider2D>();
        noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        haveToShake = false;
        haveToStopShake = true;
    }
    private void Update()
    {
        if (!haveToShake && haveToStopShake && noise.m_AmplitudeGain > 0f && noise.m_FrequencyGain > 0f)
        {
            noise.m_AmplitudeGain = 0f;
            noise.m_FrequencyGain = 0f;
        }
        if (_tgr <= 0f && !haveToStopShake)
        {
            haveToStopShake = true;
        }
        if (_tgr > 0f) _tgr -= Time.deltaTime;
    }
    public void Shake(float thoiGianRung, float doManh, float tanSoRung, bool shakeIfDoManhLessThan)
    {
        if (doManh < noise.m_AmplitudeGain && !shakeIfDoManhLessThan) return;
        noise.m_AmplitudeGain = doManh;
        noise.m_FrequencyGain = tanSoRung;
        haveToStopShake = false;
        _tgr = thoiGianRung;
        //Invoke("StopShake", thoiGianRung);
    }
    public void Shake(float doManh, float tanSoRung, bool shakeIfDoManhLessThan)
    {
        if (doManh < noise.m_AmplitudeGain && !shakeIfDoManhLessThan) return;
        haveToStopShake = false;
        haveToShake = true;
        noise.m_AmplitudeGain = doManh;
        noise.m_FrequencyGain = tanSoRung;
    }
    public void StopShake2()
    {
        haveToStopShake = true;
        haveToShake = false;
    }
    public void StopShake()
    {
        //noise.m_AmplitudeGain = 0f;
        //noise.m_FrequencyGain = 0;
        haveToStopShake = true;
    }
}
