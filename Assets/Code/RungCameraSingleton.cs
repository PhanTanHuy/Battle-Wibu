using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RungCameraSingleton : MonoBehaviour
{
    public static RungCameraSingleton Instance;
    private ShakeCam sk;
    private Coroutine currentShakeCoroutine;
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
    // Start is called before the first frame update
    void Start()
    {
        sk = GameObject.Find("Virtual Camera").GetComponent<ShakeCam>();
    }
    public void Shake(float time, float doManh, float tanSo)
    {
        sk.Shake(time, doManh, tanSo);
    }
    //public void Shake(float doManh, float tanSo, bool s)
    //{
    //    sk.Shake(doManh, tanSo, s);
    //}
    public void AuraShake()
    {
        Shake(0.2f, 22, 0.75f);
    }
   
}
