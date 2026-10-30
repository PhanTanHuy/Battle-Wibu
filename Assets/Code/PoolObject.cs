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
    private Dictionary<int, Queue<GameObject>> weaponPools = new Dictionary<int, Queue<GameObject>>();

    public Transform throwPoint;
    private ChiSoNhanVat csnv;
    private TrangThai tt;

    private void Start()
    {
        csnv = GetComponent<ChiSoNhanVat>();
        tt = GetComponent<TrangThai>();

        KhoiTaoPool();
    }

    private void KhoiTaoPool()
    {
        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            if (objectPrefabs[i].vatTheBanRa == null) continue;

            if (!weaponPools.ContainsKey(i))
            {
                weaponPools[i] = new Queue<GameObject>();
            }

            while (weaponPools[i].Count < objectPrefabs[i].soLuong)
            {
                GameObject newWeapon = Instantiate(objectPrefabs[i].vatTheBanRa);
                newWeapon.GetComponent<ChiSo>().tenNguoiSoHuu = csnv.TenNhanVat;
                newWeapon.SetActive(false);
                weaponPools[i].Enqueue(newWeapon);
            }
        }
    }

    public void CapNhatSkillMoi(SkillVatThe[] skillMoi)
    {
        Debug.Log($"{gameObject.name} đang cập nhật Pool cho kỹ năng mới!");

        this.objectPrefabs = skillMoi;

        foreach (var pool in weaponPools.Values)
        {
            while (pool.Count > 0)
            {
                GameObject oldObj = pool.Dequeue();
                Destroy(oldObj); 
            }
        }
        weaponPools.Clear();

        KhoiTaoPool();
    }

    public void ThrowPrefabs(int index)
    {
        if (index < 0 || index >= objectPrefabs.Length)
        {
            Debug.LogWarning("Index ngoài phạm vi mảng vũ khí!");
            return;
        }

        GameObject obj = GetObjectFromPool(index);
        if (obj != null)
        {
            obj.transform.position = throwPoint.position;
            obj.transform.localScale = new Vector3(transform.localScale.x * Mathf.Abs(obj.transform.localScale.x), obj.transform.localScale.y);
            obj.SetActive(true);
            obj.GetComponent<ChuyenDong>().DatHuongDiChuyen(transform.localScale.x > 0 ? Vector3.right : Vector3.left);
            obj.GetComponent<ChiSo>().SetChiSo(csnv.Dame, tt.LucKnockOut, tt.LucHatTung);
            StartCoroutine(DeactivateAfterTime(obj, objectPrefabs[index].timeReturn, index));
        }
    }

    public void ThrowPrefabsTarget(int index)
    {
        if (index < 0 || index >= objectPrefabs.Length)
        {
            Debug.LogWarning("Index ngoài phạm vi mảng vũ khí!");
            return;
        }

        GameObject obj = GetObjectFromPool(index);
        if (obj != null)
        {
            obj.transform.position = throwPoint.position;
            obj.transform.localScale = new Vector3(transform.localScale.x * Mathf.Abs(obj.transform.localScale.x), obj.transform.localScale.y);
            obj.SetActive(true);
            Vector3 direction = QuanLiCharacter.Instance.GetTransFormEnemy(csnv.TenNhanVat).position - obj.transform.position;
            obj.GetComponent<ChuyenDong>().DatHuongDiChuyen(direction.normalized);
            obj.GetComponent<ChiSo>().Dame = csnv.Dame;
            StartCoroutine(DeactivateAfterTime(obj, objectPrefabs[index].timeReturn, index));
        }
    }
    public void SpawnPrefabsAtTarget(int index)
    {
        if (index < 0 || index >= objectPrefabs.Length)
        {
            Debug.LogWarning("Index ngoài phạm vi mảng vũ khí!");
            return;
        }

        GameObject obj = GetObjectFromPool(index);
        if (obj != null)
        {
            obj.transform.position = QuanLiCharacter.Instance.GetTransFormEnemy(csnv.TenNhanVat).position;
            obj.transform.localScale = new Vector3(transform.localScale.x * Mathf.Abs(obj.transform.localScale.x), obj.transform.localScale.y);
            obj.SetActive(true);
            obj.GetComponent<ChiSo>().Dame = csnv.Dame;
            StartCoroutine(DeactivateAfterTime(obj, objectPrefabs[index].timeReturn, index));
        }
    }
    private GameObject GetObjectFromPool(int index)
    {
        if (weaponPools.ContainsKey(index) && weaponPools[index].Count > 0)
        {
            return weaponPools[index].Dequeue();
        }
        else
        {
            //if (index >= 0 && index < objectPrefabs.Length)
            //{
            //    GameObject newWeapon = Instantiate(objectPrefabs[index].vatTheBanRa);
            //    newWeapon.GetComponent<ChiSo>().tenNguoiSoHuu = csnv.TenNhanVat;
            //    newWeapon.SetActive(false);
            //    return newWeapon;
            //}
            //Debug.Log("pool trong và không tìm thấy prefab hợp lệ");
            Debug.LogWarning($"Pool cho index {index} đang trống hoặc không tồn tại!");
            return null;
        }
    }

    public void ReturnObjectToPool(GameObject obj, int index)
    {
        if (!weaponPools.ContainsKey(index))
        {
            weaponPools[index] = new Queue<GameObject>();
        }

        obj.SetActive(false);
        weaponPools[index].Enqueue(obj);
    }

    private IEnumerator DeactivateAfterTime(GameObject obj, float delay, int index)
    {
        yield return new WaitForSeconds(delay);
        if (obj != null)
        {
            ReturnObjectToPool(obj, index);
        }
    }
}