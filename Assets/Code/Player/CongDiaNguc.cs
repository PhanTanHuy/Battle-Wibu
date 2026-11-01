
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CongDiaNguc : MonoBehaviour
{
    [Header("Owner")]
    public string tenNguoiSoHuu;

    [Header("Prefabs")]
    [SerializeField] private List<GameObject> prefabs = new List<GameObject>();

    [Header("Spawn Settings")]
    [SerializeField] private Transform vitriSpawn;
    [SerializeField] private int poolSizePerPrefab = 3;
    [SerializeField] private int maxQuaiVatActive = 3;

    [Header("Random Spawn Time")]
    [SerializeField] private float minSpawnTime = 2f;
    [SerializeField] private float maxSpawnTime = 5f;

    [Header("Lifetime")]
    [SerializeField] private float thoiGianTonTai = 5f;

    private List<GameObject> pool = new List<GameObject>();

    private Coroutine spawnCoroutine;
    private Coroutine lifetimeCoroutine;

    private void Awake()
    {
        InitializePool();
        ShufflePool();
    }

    public void OnCreateCongDiaNguc()
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);

        if (lifetimeCoroutine != null)
            StopCoroutine(lifetimeCoroutine);

        // Tìm lại mục tiêu khi cổng được kích hoạt
        spawnCoroutine = StartCoroutine(SpawnRoutine());
        lifetimeCoroutine = StartCoroutine(LifetimeRoutine());
    }

    private IEnumerator LifetimeRoutine()
    {
        yield return new WaitForSeconds(thoiGianTonTai);

        gameObject.SetActive(false);
    }

    private void InitializePool()
    {
        pool.Clear();

        foreach (GameObject prefab in prefabs)
        {
            if (prefab == null)
                continue;

            for (int i = 0; i < poolSizePerPrefab; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                obj.tag = tag;

                pool.Add(obj);
            }
        }
    }

    private void ShufflePool()
    {
        for (int i = pool.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            GameObject temp = pool[i];
            pool[i] = pool[randomIndex];
            pool[randomIndex] = temp;
        }
    }
    public void DungImTatCaQuaiVat(float time)
    {
        foreach (GameObject monster in pool)
        {
            if (monster == null || !monster.activeInHierarchy)
                continue;

            if (monster.TryGetComponent<TrangThai>(out TrangThai trangThai))
            {
                trangThai.DungIm(time);
            }
        }
    }
    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            if (GetActiveMonsterCount() >= maxQuaiVatActive)
                continue;

            SpawnMonster();
        }
    }

    private int GetActiveMonsterCount()
    {
        int count = 0;

        foreach (GameObject obj in pool)
        {
            if (obj != null && obj.activeSelf)
                count++;
        }

        return count;
    }

    private void SpawnMonster()
    {
        if (GetActiveMonsterCount() >= maxQuaiVatActive)
            return;

        Transform player = QuanLiCharacter.Instance.GetTransFormEnemy(tenNguoiSoHuu);

        if (player == null)
            return;

        foreach (GameObject monster in pool)
        {
            if (monster == null || monster.activeSelf)
                continue;

            monster.transform.position = vitriSpawn != null
                ? vitriSpawn.position
                : transform.position;

            if (monster.TryGetComponent<MinecraftAI>(out MinecraftAI ai))
            {
                ai.player = player;

                if (ai.TryGetComponent<ChiSoNhanVat>(out ChiSoNhanVat chiSo))
                {
                    chiSo.TenNhanVat = tenNguoiSoHuu;
                    chiSo.updateUI = false;

                }
            }
            monster.GetComponent<SpriteRenderer>().enabled = true;
            monster.SetActive(true);
            return;
        }
    }

    public void OnSteveDie()
    {
        // Dừng tất cả Coroutine
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }

        if (lifetimeCoroutine != null)
        {
            StopCoroutine(lifetimeCoroutine);
            lifetimeCoroutine = null;
        }

        // Hủy toàn bộ quái vật do cổng spawn
        foreach (GameObject monster in pool)
        {
            if (monster != null)
                Destroy(monster);
        }

        pool.Clear();
    }
}
