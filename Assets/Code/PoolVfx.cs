
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolVfx : MonoBehaviour
{
    private struct CollideEffect
    {
        public GameObject collide;
        public string tenNhanVat;
        public Color m;

        public void KhoiTao(GameObject cl, string n, Color mau)
        {
            collide = cl;
            tenNhanVat = n;
            m = mau;
        }
    }

    public static PoolVfx instance;

    private CollideEffect[] Collides;

    [HideInInspector]
    public Queue<GameObject>[] CollideQueues;

    public GameObject dashSmoke;
    public GameObject tele;
    public GameObject datVo;
    public GameObject ghostEffect;
    public GameObject bloodEffect;
    public GameObject supperCollide;

    private Queue<GameObject> SmokeDashQueue;
    private Queue<GameObject> TeleQueue;
    private Queue<GameObject> DatVoQueue;
    private Queue<GameObject> GhostEffectQueue;
    private Queue<GameObject> BloodQueue;
    private Queue<GameObject> SupperCollideQueue;

    [Header("Particle")]
    public ParticleSystem KhoiKnockOutParticlePrefabs;
    private ParticleSystem KhoiKnockOutParticle;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void KhoiTaoPoolVfx()
    {
        if (KhoiKnockOutParticle == null)
        {
            KhoiKnockOutParticle = Instantiate(KhoiKnockOutParticlePrefabs);
        }

        Collides = new CollideEffect[2];

        AddQueue(ref SmokeDashQueue, 15, dashSmoke);
        AddQueue(ref TeleQueue, 25, tele);
        AddQueue(ref DatVoQueue, 10, datVo);
        AddQueue(ref GhostEffectQueue, 40, ghostEffect);
        AddQueue(ref BloodQueue, 30, bloodEffect);
        AddQueue(ref SupperCollideQueue, 10, supperCollide);
    }

    // =========================================
    // INITIALIZE COLLIDE
    // =========================================

    public void KhoiTaoCollideVFX()
    {
        XoaCollideVFX();

        for (int i = 0; i < Collides.Length; i++)
        {
            ChiSoNhanVat csnv = QuanLiCharacter.Instance.csnv[i];

            Collides[i].KhoiTao(
                csnv.hieuUngHit,
                csnv.TenNhanVat,
                csnv.mauHit
            );
        }

        CollideQueues = new Queue<GameObject>[Collides.Length];

        for (int i = 0; i < Collides.Length; i++)
        {
            AddQueue(
                ref CollideQueues[i],
                20,
                Collides[i].collide,
                Collides[i].m
            );
        }
    }

    public void XoaCollideVFX()
    {
        if (CollideQueues == null) return;

        for (int i = 0; i < CollideQueues.Length; i++)
        {
            if (CollideQueues[i] == null) continue;

            while (CollideQueues[i].Count > 0)
            {
                GameObject gobj = CollideQueues[i].Dequeue();

                if (gobj != null)
                    Destroy(gobj);
            }
        }
    }

    // =========================================
    // ADD OBJECT TO POOL
    // =========================================

    private void AddQueue(ref Queue<GameObject> q, int soLuong, GameObject prefab)
    {
        q = new Queue<GameObject>();

        for (int i = 0; i < soLuong; i++)
        {
            GameObject g = Instantiate(prefab);

            g.SetActive(false);

            q.Enqueue(g);
        }
    }

    private void AddQueue(
        ref Queue<GameObject> q,
        int soLuong,
        GameObject prefab,
        Color mau)
    {
        q = new Queue<GameObject>();

        for (int i = 0; i < soLuong; i++)
        {
            GameObject g = Instantiate(prefab);

            SpriteRenderer sr = g.GetComponent<SpriteRenderer>();

            if (sr != null)
                sr.color = mau;

            g.SetActive(false);

            q.Enqueue(g);
        }
    }

    // =========================================
    // GET OBJECT - DYNAMIC POOLING
    // =========================================

    private GameObject GetObject(
        Queue<GameObject> q,
        GameObject prefab)
    {
        GameObject gobj;

        if (q.Count > 0)
        {
            gobj = q.Dequeue();
        }
        else
        {
            // Pool hết -> Instantiate object mới
            gobj = Instantiate(prefab);
        }

        return gobj;
    }

    // =========================================
    // RETURN OBJECT TO POOL
    // =========================================

    private IEnumerator ReturnToPoolAfterDelay(
        Queue<GameObject> q,
        GameObject gobj,
        float delay)
    {
        yield return new WaitForSeconds(delay);

        if (gobj == null) yield break;

        gobj.SetActive(false);

        if (q != null)
        {
            q.Enqueue(gobj);
        }
        else
        {
            Destroy(gobj);
        }
    }

    // =========================================
    // CREATE VFX
    // =========================================

    public void CreatSupperCollide(Vector2 pos)
    {
        GameObject gobj = GetObject(SupperCollideQueue, supperCollide);

        gobj.transform.position = pos;
        gobj.SetActive(true);

        StartCoroutine(
            ReturnToPoolAfterDelay(SupperCollideQueue, gobj, 2f)
        );
    }

    public void CreateGhostEffect(
        Vector2 pos,
        Sprite sprite,
        Color cl,
        Vector3 scal)
    {
        GameObject gobj = GetObject(GhostEffectQueue, ghostEffect);

        gobj.transform.position = pos;
        gobj.transform.localScale = scal;

        SpriteRenderer sr = gobj.GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            sr.sprite = sprite;
            sr.color = cl;
        }

        gobj.SetActive(true);

        StartCoroutine(
            ReturnToPoolAfterDelay(GhostEffectQueue, gobj, 0.75f)
        );
    }

    public void CreateDatVo(Vector2 pos)
    {
        GameObject gobj = GetObject(DatVoQueue, datVo);

        gobj.transform.position = pos;
        gobj.SetActive(true);

        StartCoroutine(
            ReturnToPoolAfterDelay(DatVoQueue, gobj, 4f)
        );
    }

    public void CreateDashSmoke(Vector2 pos, Vector2 scale)
    {
        GameObject gobj = GetObject(SmokeDashQueue, dashSmoke);

        gobj.transform.position = pos;
        gobj.transform.localScale = scale;
        gobj.SetActive(true);

        StartCoroutine(
            ReturnToPoolAfterDelay(SmokeDashQueue, gobj, 1f)
        );
    }

    public void CreatBlood(Vector2 pos, Vector2 scale)
    {
        GameObject gobj = GetObject(BloodQueue, bloodEffect);

        gobj.transform.position = pos;
        gobj.transform.localScale = scale;
        gobj.SetActive(true);

        StartCoroutine(
            ReturnToPoolAfterDelay(BloodQueue, gobj, 2f)
        );
    }

    public void CreateTele(Vector2 pos)
    {
        GameObject gobj = GetObject(TeleQueue, tele);

        gobj.transform.position = pos;
        gobj.SetActive(true);

        StartCoroutine(
            ReturnToPoolAfterDelay(TeleQueue, gobj, 0.5f)
        );
    }

    public void CreateCollideEffect(
        Vector2 pos,
        string nameNhanVat,
        Vector2 scale)
    {
        int index = -1;

        for (int j = 0; j < Collides.Length; j++)
        {
            if (nameNhanVat == Collides[j].tenNhanVat)
            {
                index = j;
                break;
            }
        }

        if (index == -1) return;

        Queue<GameObject> q = CollideQueues[index];

        GameObject gobj = GetObject(q, Collides[index].collide);

        gobj.transform.position = new Vector2(
            pos.x + Random.Range(-0.1f, 0.1f),
            pos.y + Random.Range(-0.5f, 0.5f)
        );

        gobj.transform.localScale = scale;
        gobj.SetActive(true);

        CreatBlood(pos, scale);

        StartCoroutine(
            ReturnToPoolAfterDelay(q, gobj, 1f)
        );
    }

    // =========================================
    // PARTICLE
    // =========================================

    public void KhoiKnockOut(Transform target)
    {
        if (KhoiKnockOutParticle == null)
        {
            KhoiKnockOutParticle = Instantiate(
                KhoiKnockOutParticlePrefabs
            );
        }

        KhoiKnockOutParticle.transform.SetParent(target);

        KhoiKnockOutParticle.transform.localPosition = Vector3.zero;

        KhoiKnockOutParticle.Play();

        CreatSupperCollide(target.position);
    }
}
