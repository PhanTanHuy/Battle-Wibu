using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolVfx : MonoBehaviour
{
    struct CollideEffect
    {
        public GameObject collide;
        public string tenNhanVat;
        public Color m;
        public void KhoiTao(GameObject cl, string n, ref Color mau)
        {
            collide = cl;
            tenNhanVat = n;
            m = mau;
        }
    }
    public static PoolVfx instance;
    private CollideEffect[] Collides;
    [HideInInspector] public Queue<GameObject>[] CollideQueues;
    public GameObject dashSmoke, tele, datVo, ghostEffect, bloodEffect, supperCollide;
    private Queue<GameObject> SmokeDashQueue, TeleQueue, DatVoQueue, GhostEffectQueue, BloodQueue, SupperCollideQueue;
    [Header("Particle")]
    public ParticleSystem KhoiKnockOutParticlePrefabs;
    private ParticleSystem KhoiKnockOutParticle;
    //private Queue<ParticleSystem> BloodQueue;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }
    public void KhoiTaoPoolVfx()
    {
        KhoiKnockOutParticle = Instantiate(KhoiKnockOutParticlePrefabs);
        Collides = new CollideEffect[2];
        KhoiTaoCollideVFX();
        //
        AddQueue(ref SmokeDashQueue, 15, dashSmoke);
        AddQueue(ref TeleQueue, 10, tele);
        AddQueue(ref DatVoQueue, 10, datVo);
        AddQueue(ref GhostEffectQueue, 30, ghostEffect);
        AddQueue(ref BloodQueue, 20, bloodEffect);
        AddQueue(ref SupperCollideQueue, 20, supperCollide);


    }
    public void KhoiTaoCollideVFX()
    {
        XoaCollideVFX();
        for (int i = 0; i < Collides.Length; i++)
        {
            ChiSoNhanVat csnv = QuanLiCharacter.Instance.csnv[i];
            Collides[i].KhoiTao(csnv.hieuUngHit, csnv.TenNhanVat, ref csnv.mauHit);
        }
        CollideQueues = new Queue<GameObject>[Collides.Length];
        for (int i = 0; i < Collides.Length; i++)
        {
            AddQueue(ref CollideQueues[i], 20, Collides[i].collide, ref Collides[i].m);
        }
    }
    public void XoaCollideVFX()
    {
        if (CollideQueues != null)
        {
            for (int i = 0; i < CollideQueues.Length; i++)
            {
                if (CollideQueues[i] != null)
                {
                    while (CollideQueues[i].Count > 0)
                    {
                        GameObject gobj = CollideQueues[i].Dequeue();
                        if (gobj != null)
                        {
                            Destroy(gobj); // Xóa clone khỏi scene
                        }
                    }
                }
            }
        }
    }
    private void AddQueue(ref Queue<GameObject> q, int soLuong, GameObject gobj, ref Color mau)
    {
        q = new Queue<GameObject>();
        for (int i = 0; i < soLuong; i++)
        {
            GameObject g = Instantiate(gobj);
            if (g.GetComponent<SpriteRenderer>() != null) g.GetComponent<SpriteRenderer>().color = mau;
            g.SetActive(false);
            q.Enqueue(g);
        }
    }
    private void AddQueue(ref Queue<ParticleSystem> q, int soLuong, ParticleSystem gobj)
    {
        q = new Queue<ParticleSystem>();
        for (int i = 0; i < soLuong; i++)
        {
            ParticleSystem g = Instantiate(gobj);
            q.Enqueue(g);
        }
    }
    private void AddQueue(ref Queue<GameObject> q, int soLuong, GameObject gobj)
    {
        q = new Queue<GameObject>();
        for (int i = 0; i < soLuong; i++)
        {
            GameObject g = Instantiate(gobj);
            g.SetActive(false);
            q.Enqueue(g);
        }
    }
    public void CreatSupperCollide(Vector2 pos)
    {
        if (SupperCollideQueue.Count <= 0) return;
        GameObject gobj = SupperCollideQueue.Dequeue();
        gobj.SetActive(true);
        gobj.transform.position = pos;
        StartCoroutine(ReturnToPoolAfterDelay(SupperCollideQueue, gobj, 2f));
    }
    public void CreateGhostEffect(Vector2 pos, Sprite sprite, Color cl, Vector3 scal)
    {
        GameObject gobj = GhostEffectQueue.Dequeue();
        gobj.SetActive(true);
        gobj.transform.position = pos;
        gobj.transform.localScale = scal;
        gobj.GetComponent<SpriteRenderer>().sprite = sprite;
        gobj.GetComponent<SpriteRenderer>().color = cl;
        StartCoroutine(ReturnToPoolAfterDelay(GhostEffectQueue, gobj, 0.75f));
    }
    public void CreateDatVo(Vector2 pos)
    {
        GameObject gobj = DatVoQueue.Dequeue();
        gobj.SetActive(true);
        gobj.transform.position = pos;
        StartCoroutine(ReturnToPoolAfterDelay(DatVoQueue, gobj, 4f));
    }
    public void CreateDashSmoke(Vector2 pos, Vector2 scale)
    {
        GameObject gobj = SmokeDashQueue.Dequeue();
        gobj.SetActive(true);
        gobj.transform.position = pos;
        gobj.transform.localScale = scale;
        StartCoroutine(ReturnToPoolAfterDelay(SmokeDashQueue, gobj, 1f));
    }
    public void CreatBlood(Vector2 pos, Vector2 scale)
    {
        if (BloodQueue.Count > 0)
        {
            GameObject gobj = BloodQueue.Dequeue();
            gobj.SetActive(true);
            gobj.transform.position = pos;
            gobj.transform.localScale = scale;
            StartCoroutine(ReturnToPoolAfterDelay(BloodQueue, gobj, 2f));
        }
    }
    public void CreateTele(Vector2 pos)
    {
        GameObject gobj = TeleQueue.Dequeue();
        gobj.SetActive(true);
        gobj.transform.position = pos;
        StartCoroutine(ReturnToPoolAfterDelay(TeleQueue, gobj, 0.5f));
    }
    public void CreateCollideEffect(Vector2 pos, string nameNhanVat, Vector2 scale)
    {
        int i = 0;
        for (int j=0; j<Collides.Length; j++)
        {
            if (nameNhanVat == Collides[j].tenNhanVat) i = j;
        }
        
        if (CollideQueues[i].Count > 0)
        {
            GameObject gobj = CollideQueues[i].Dequeue();
            gobj.SetActive(true);
            gobj.transform.position = new Vector2(pos.x + Random.Range(-0.1f, 0.1f), pos.y + Random.Range(-0.5f, 0.5f));
            gobj.transform.localScale = scale;
            CreatBlood(pos, scale);
            StartCoroutine(ReturnToPoolAfterDelay(CollideQueues[i], gobj, 1f));
        }
    }

    private IEnumerator ReturnToPoolAfterDelay(Queue<GameObject> q, GameObject gobj, float delay)
    {
        yield return new WaitForSeconds(delay);

        gobj.SetActive(false);
        q.Enqueue(gobj); 
    }
    private IEnumerator ReturnToPoolAfterDelay(Queue<ParticleSystem> q, ParticleSystem gobj, float delay)
    {
        yield return new WaitForSeconds(delay);
        q.Enqueue(gobj);
    }
    /** particle **/
    public void KhoiKnockOut(Transform target)
    {
        if (KhoiKnockOutParticle == null) KhoiKnockOutParticle = Instantiate(KhoiKnockOutParticlePrefabs);
        KhoiKnockOutParticle.transform.SetParent(target);
        KhoiKnockOutParticle.transform.localPosition = Vector3.zero;
        KhoiKnockOutParticle.Play();
        CreatSupperCollide(target.position);
    }
}
