
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steve : MonoBehaviour, ICharacterSpecial
{
    [Header("Prefabs")]
    [SerializeField] private List<GameObject> prefabs = new List<GameObject>();

    private ChiSoNhanVat selfChiSoNhanVat;

    [SerializeField] private Transform vitrispawn;

    [Header("Pool Settings")]
    [SerializeField] private int poolSizePerPrefab = 3;

    private List<List<GameObject>> pools = new List<List<GameObject>>();

    [Header("Cong Dia Nguc")]
    [SerializeField] private GameObject congDiaNgucPrefab;

    private CongDiaNguc congDiaNgucInstance;

    private void Awake()
    {
        selfChiSoNhanVat = GetComponent<ChiSoNhanVat>();

        InitializePools();

        GameObject cdn = Instantiate(
            congDiaNgucPrefab,
            vitrispawn.position,
            Quaternion.identity
        );

        congDiaNgucInstance = cdn.GetComponent<CongDiaNguc>();
        congDiaNgucInstance.gameObject.SetActive(false);
    }

    private void InitializePools()
    {
        pools.Clear();

        for (int i = 0; i < prefabs.Count; i++)
        {
            List<GameObject> pool = new List<GameObject>();

            if (prefabs[i] != null)
            {
                for (int j = 0; j < poolSizePerPrefab; j++)
                {
                    GameObject obj = Instantiate(prefabs[i]);
                    obj.SetActive(false);
                    obj.tag = tag;
                    pool.Add(obj);
                }
            }

            pools.Add(pool);
        }
    }

    public void TrieuHoiCongDiaNguc()
    {
        if (congDiaNgucInstance == null || congDiaNgucInstance.gameObject.activeSelf)
            return;

        congDiaNgucInstance.tenNguoiSoHuu = selfChiSoNhanVat.TenNhanVat;
        congDiaNgucInstance.transform.position = vitrispawn.position;

        congDiaNgucInstance.gameObject.SetActive(true);
        congDiaNgucInstance.OnCreateCongDiaNguc();
    }

    public GameObject Spawn(int index)
    {
        if (index < 0 || index >= pools.Count)
        {
            Debug.LogWarning($"Steve: Prefab index {index} không hợp lệ!");
            return null;
        }

        List<GameObject> pool = pools[index];

        foreach (GameObject obj in pool)
        {
            if (obj != null && !obj.activeSelf)
            {
                obj.transform.position = vitrispawn.position;

                if (obj.TryGetComponent<MinecraftAI>(out MinecraftAI qv))
                {
                    qv.player = QuanLiCharacter.Instance.GetTransFormEnemy(
                        selfChiSoNhanVat.TenNhanVat
                    );

                    if (qv.TryGetComponent<ChiSoNhanVat>(out ChiSoNhanVat chiSo))
                    {
                        chiSo.updateUI = false;
                        chiSo.TenNhanVat = selfChiSoNhanVat.TenNhanVat;
                    }
                }
                obj.GetComponent<SpriteRenderer>().enabled = true;
                obj.SetActive(true);
                return obj;
            }
        }

        return null;
    }

    public void ReturnPool(GameObject obj)
    {
        if (obj == null)
            return;

        obj.SetActive(false);
    }

    private void OnDestroy()
    {
        // Hủy toàn bộ quái vật do Steve triệu hồi
        foreach (List<GameObject> pool in pools)
        {
            foreach (GameObject obj in pool)
            {
                if (obj != null)
                    Destroy(obj);
            }
        }

        pools.Clear();

        // Hủy cổng địa ngục và toàn bộ quái vật do cổng triệu hồi
        if (congDiaNgucInstance != null)
        {
            congDiaNgucInstance.OnSteveDie();
            Destroy(congDiaNgucInstance.gameObject);
            congDiaNgucInstance = null;
        }
    }

    public void HandleDungIm(float time)
    {
        // Quái vật do Steve trực tiếp spawn
        foreach (List<GameObject> pool in pools)
        {
            foreach (GameObject obj in pool)
            {
                if (obj == null || !obj.activeInHierarchy)
                    continue;

                if (obj.TryGetComponent<TrangThai>(out TrangThai trangThai))
                {
                    trangThai.DungIm(time);
                }
            }
        }

        // Quái vật do cổng địa ngục spawn
        if (congDiaNgucInstance != null)
        {
            congDiaNgucInstance.DungImTatCaQuaiVat(time);
        }
    }

    public void HandleDeath()
    {
        
    }
}
