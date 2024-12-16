using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class PoolVfx : MonoBehaviour
{
    struct CollideEffect
    {
        public GameObject collide;
        public string tenNhanVat;
        public void KhoiTao(GameObject cl, string n)
        {
            collide = cl;
            tenNhanVat = n;
        }
    }
    public static PoolVfx instance;
    private CollideEffect[] Collides;
    [HideInInspector] public Queue<GameObject>[] CollideQueues;
    public GameObject dashSmoke, tele, datVo, ghostEffect;
    private Queue<GameObject> SmokeDashQueue, TeleQueue, DatVoQueue, GhostEffectQueue;
    [Header("Particle")]
    public ParticleSystem KhoiKnockOutParticlePrefabs;
    private ParticleSystem KhoiKnockOutParticle;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else Destroy(gameObject);
    }
    private void Start()
    {
        KhoiKnockOutParticle = Instantiate(KhoiKnockOutParticlePrefabs);
        Collides = new CollideEffect[QuanLiCharacter.Instance.csnv.Length];
        for (int i=0; i<Collides.Length; i++)
        {
            Collides[i].KhoiTao(QuanLiCharacter.Instance.csnv[i].hieuUngHit, QuanLiCharacter.Instance.csnv[i].TenNhanVat);
        }
        CollideQueues = new Queue<GameObject>[Collides.Length];
        for (int i = 0; i < Collides.Length; i++)
        {
            AddQueue(ref CollideQueues[i], 20, Collides[i].collide);
        }
        //
        AddQueue(ref SmokeDashQueue, 15, dashSmoke);
        AddQueue(ref TeleQueue, 10, tele);
        AddQueue(ref DatVoQueue, 10, datVo);
        AddQueue(ref GhostEffectQueue, 30, ghostEffect);

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
            gobj.transform.position = new Vector2(pos.x + Random.Range(-0.1f, 0.1f), pos.y + Random.Range(-0.2f, 0f));
            gobj.transform.localScale = scale;
            StartCoroutine(ReturnToPoolAfterDelay(CollideQueues[i], gobj, 1f));
        }
    }

    private IEnumerator ReturnToPoolAfterDelay(Queue<GameObject> q, GameObject gobj, float delay)
    {
        yield return new WaitForSeconds(delay);

        gobj.SetActive(false);
        q.Enqueue(gobj); 
    }
    /** particle **/
    public void KhoiKnockOut(Transform target)
    {
        if (KhoiKnockOutParticle == null) KhoiKnockOutParticle = Instantiate(KhoiKnockOutParticlePrefabs);
        KhoiKnockOutParticle.transform.SetParent(target);
        KhoiKnockOutParticle.transform.localPosition = Vector3.zero;
        KhoiKnockOutParticle.Play();
    }
}
