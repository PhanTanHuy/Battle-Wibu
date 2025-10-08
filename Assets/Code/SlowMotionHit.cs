using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionHit : MonoBehaviour
{
    public static SlowMotionHit Instance;
    private Coroutine resetTimeCor;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        resetTimeCor = null;
    }
    public void SlowKO()
    {
        Time.timeScale = 0.15f;
        QuanLiCharacter.Instance.CorouDenMap(0.3f);
        if (resetTimeCor != null)
        {
            StopCoroutine(resetTimeCor);
        }
        resetTimeCor = StartCoroutine(ResetTimeScale(0.3f));
    }
    public void SlowHitNang()
    {
        Time.timeScale = 0.01f;
        if (resetTimeCor != null )
        {
            StopCoroutine(resetTimeCor);
        }
        resetTimeCor = StartCoroutine(ResetTimeScale(0.0014f));

    }
    public void SlowHitNhe()
    {
        Time.timeScale = 0.01f;
        if (resetTimeCor != null)
        {
            StopCoroutine(resetTimeCor);
        }
        resetTimeCor = StartCoroutine(ResetTimeScale(0.0002f));

    }
    public void SlowHitVua()
    {
        Time.timeScale = 0.01f;
        if (resetTimeCor != null)
        {
            StopCoroutine(resetTimeCor);
        }
        resetTimeCor = StartCoroutine(ResetTimeScale(0.0006f));

    }
    IEnumerator ResetTimeScale(float time)
    {
        yield return new WaitForSeconds(time);
        Time.timeScale = 1f;
    }
}
