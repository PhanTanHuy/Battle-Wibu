using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuanLiAmThanh : MonoBehaviour
{
    public AudioSource audioSource, auraSource;

    public static QuanLiAmThanh Instance;
    public AudioClip dash, jump, onground, walk, startAura, aura, tele, telePunch, kiBlast, explosion;
    public AudioClip[] hits, punchs;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        auraSource.loop = true;
        auraSource.clip = aura;
    }
    public void PlayKiBlast()
    {
        audioSource.PlayOneShot(kiBlast);
    }
    public void PlayAura()
    {
        auraSource.Play();
    }
    public void StopAura()
    {
        auraSource.Stop();
    }
    public void PlayStartAura()
    {
        audioSource.PlayOneShot(startAura);
    }
    public void PlayExPolison()
    {
        audioSource.PlayOneShot(explosion);
    }
    public void PlayTelePunch()
    {
        audioSource.PlayOneShot(telePunch);
    }
    public void PlayTele()
    {
        audioSource.PlayOneShot(tele);
    }
    public void PlayDash()
    {
        audioSource.PlayOneShot(dash);
    }
    public void PlayJump()
    {
        audioSource.PlayOneShot(jump);
    }
    public void PlayWalk() 
    { 
        audioSource.PlayOneShot(walk);
    }
    public void PlayOnGround()
    {
        audioSource.PlayOneShot(onground);
    }
    public void PlayHit()
    {
        audioSource.PlayOneShot(hits[Random.Range(0, hits.Length)]);
    }
    public void PlayPunch()
    {
        audioSource.PlayOneShot(punchs[Random.Range(0, punchs.Length)]);
    }
    public void PlaySfx(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
