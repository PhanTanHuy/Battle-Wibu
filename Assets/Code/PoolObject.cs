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
    }
    public SkillVatThe[] objectPrefabs;
    private Dictionary<int, Queue<GameObject>> weaponPools = new Dictionary<int, Queue<GameObject>>();

    public Transform throwPoint;
    private ChiSoNhanVat csnv;
    private TrangThai tt;
    //public float throwForce = 10f; 

    private void Start()
    {
        csnv = GetComponent<ChiSoNhanVat>();
        tt = GetComponent<TrangThai>();
        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            if (!weaponPools.ContainsKey(i))
            {
                weaponPools[i] = new Queue<GameObject>();
            }
            for (int j = 0; j < objectPrefabs[i].soLuong; j++) 
            {
                GameObject newWeapon = Instantiate(objectPrefabs[i].vatTheBanRa);
                newWeapon.GetComponent<ChiSo>().tenNguoiSoHuu = csnv.TenNhanVat;
                newWeapon.SetActive(false); 
                weaponPools[i].Enqueue(newWeapon);
            }
        }
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
            StartCoroutine(DeactivateAfterTime(obj, 3f, index));
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
            StartCoroutine(DeactivateAfterTime(obj, 3f, index));
        }
    }

    private GameObject GetObjectFromPool(int index)
    {
        if (weaponPools[index].Count > 0)
        {
            return weaponPools[index].Dequeue();
        }
        else
        {
            Debug.Log("pool trong");
            return null;
        }
    }
    public void ReturnObjectToPool(GameObject obj, int index)
    {
        obj.SetActive(false);
        weaponPools[index].Enqueue(obj);
    }
    private IEnumerator DeactivateAfterTime(GameObject obj, float delay, int index)
    {
        yield return new WaitForSeconds(delay);
        ReturnObjectToPool(obj, index);
    }
}
