using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhatNhac : MonoBehaviour
{
    public AudioClip nhac;
    private void Start()
    {
        QuanLiAmThanh.Instance.musicSource.Stop();
        QuanLiAmThanh.Instance.musicSource.clip = nhac;
        QuanLiAmThanh.Instance.musicSource.loop = true;
        QuanLiAmThanh.Instance.musicSource.Play();
    }
}
