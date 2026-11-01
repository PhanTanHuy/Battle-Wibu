
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolObject : MonoBehaviour
{
    [System.Serializable]
    public class SkillVatThe
    {
        public GameObject vatTheBanRa;
        public int soLuong = 15;
        public float timeReturn = 3f;
    }

    public SkillVatThe[] objectPrefabs;

    private Dictionary<int, Queue<GameObject>> weaponPools =
        new Dictionary<int, Queue<GameObject>>();

    // Quản lý toàn bộ object, kể cả đang Active hoặc Inactive.
    private HashSet<GameObject> allWeapons = new HashSet<GameObject>();

    public Transform throwPoint;

    private ChiSoNhanVat csnv;
    private TrangThai tt;

    private void Start()
    {
        csnv = GetComponent<ChiSoNhanVat>();
        tt = GetComponent<TrangThai>();
        KhoiTaoPool();

    }
    private void OnEnable()
    {
        if (csnv != null) KhoiTaoPool();
    }

    private void KhoiTaoPool()
    {
        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            if (objectPrefabs[i].vatTheBanRa == null)
                continue;

            if (!weaponPools.ContainsKey(i))
                weaponPools[i] = new Queue<GameObject>();

            while (weaponPools[i].Count < objectPrefabs[i].soLuong)
            {
                GameObject newWeapon = Instantiate(
                    objectPrefabs[i].vatTheBanRa
                );

                newWeapon.GetComponent<ChiSo>().tenNguoiSoHuu =
                    csnv.TenNhanVat;

                newWeapon.SetActive(false);

                weaponPools[i].Enqueue(newWeapon);
                allWeapons.Add(newWeapon);
            }
        }
    }

    public void CapNhatSkillMoi(SkillVatThe[] skillMoi)
    {
        Debug.Log($"{gameObject.name} đang cập nhật Pool cho kỹ năng mới!");

        // Hủy toàn bộ object cũ, bao gồm cả object đang hoạt động.
        XoaToanBoPool();

        objectPrefabs = skillMoi;

        KhoiTaoPool();
    }

    public void ThrowPrefabs(int index)
    {
        if (!IsValidIndex(index))
            return;

        GameObject obj = GetObjectFromPool(index);

        if (obj == null)
            return;

        obj.transform.position = throwPoint.position;

        obj.transform.localScale = new Vector3(
            transform.localScale.x * Mathf.Abs(obj.transform.localScale.x),
            obj.transform.localScale.y
        );

        obj.SetActive(true);

        obj.GetComponent<ChuyenDong>().DatHuongDiChuyen(
            transform.localScale.x > 0
                ? Vector3.right
                : Vector3.left
        );

        obj.GetComponent<ChiSo>().SetChiSo(
            csnv.Dame,
            tt.LucKnockOut,
            tt.LucHatTung
        );

        StartCoroutine(
            DeactivateAfterTime(obj, objectPrefabs[index].timeReturn, index)
        );
    }

    public void ThrowPrefabsTarget(int index)
    {
        if (!IsValidIndex(index))
            return;

        GameObject obj = GetObjectFromPool(index);

        if (obj == null)
            return;

        Transform target = QuanLiCharacter.Instance.GetTransFormEnemy(
            csnv.TenNhanVat
        );

        if (target == null)
        {
            ReturnObjectToPool(obj, index);
            return;
        }

        obj.transform.position = throwPoint.position;

        obj.transform.localScale = new Vector3(
            transform.localScale.x * Mathf.Abs(obj.transform.localScale.x),
            obj.transform.localScale.y
        );

        obj.SetActive(true);

        Vector3 direction = target.position - obj.transform.position;

        obj.GetComponent<ChuyenDong>().DatHuongDiChuyen(
            direction.normalized
        );

        obj.GetComponent<ChiSo>().Dame = csnv.Dame;

        StartCoroutine(
            DeactivateAfterTime(obj, objectPrefabs[index].timeReturn, index)
        );
    }

    public void SpawnPrefabsAtTarget(int index)
    {
        if (!IsValidIndex(index))
            return;

        GameObject obj = GetObjectFromPool(index);

        if (obj == null)
            return;

        Transform target = QuanLiCharacter.Instance.GetTransFormEnemy(
            csnv.TenNhanVat
        );

        if (target == null)
        {
            ReturnObjectToPool(obj, index);
            return;
        }

        obj.transform.position = target.position;

        obj.transform.localScale = new Vector3(
            transform.localScale.x * Mathf.Abs(obj.transform.localScale.x),
            obj.transform.localScale.y
        );

        obj.SetActive(true);

        obj.GetComponent<ChiSo>().Dame = csnv.Dame;

        StartCoroutine(
            DeactivateAfterTime(obj, objectPrefabs[index].timeReturn, index)
        );
    }

    private bool IsValidIndex(int index)
    {
        if (index < 0 || index >= objectPrefabs.Length)
        {
            Debug.LogWarning($"Index ngoài phạm vi mảng vũ khí: {index}");
            return false;
        }

        return true;
    }

    private GameObject GetObjectFromPool(int index)
    {
        if (weaponPools.TryGetValue(index, out Queue<GameObject> pool)
            && pool.Count > 0)
        {
            return pool.Dequeue();
        }

        Debug.LogWarning($"Pool cho index {index} đang trống!");
        return null;
    }

    public void ReturnObjectToPool(GameObject obj, int index)
    {
        if (obj == null || index < 0 || index >= objectPrefabs.Length)
            return;

        if (!weaponPools.TryGetValue(index, out Queue<GameObject> pool))
        {
            pool = new Queue<GameObject>();
            weaponPools[index] = pool;
        }

        if (!obj.activeSelf)
            return;

        obj.SetActive(false);
        pool.Enqueue(obj);
    }

    private IEnumerator DeactivateAfterTime(
        GameObject obj,
        float delay,
        int index
    )
    {
        yield return new WaitForSeconds(delay);

        if (obj != null)
            ReturnObjectToPool(obj, index);
    }

    private void OnDestroy()
    {
        XoaToanBoPool();
    }

    private void XoaToanBoPool()
    {
        // Hủy tất cả object, bao gồm cả object đang được sử dụng.
        foreach (GameObject obj in allWeapons)
        {
            if (obj != null)
                Destroy(obj);
        }

        allWeapons.Clear();
        weaponPools.Clear();

        // Dừng các coroutine đang chờ trả object về pool.
        StopAllCoroutines();
    }
}
