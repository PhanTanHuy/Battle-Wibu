using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarutoSkill : MonoBehaviour
{
    private ChiSoNhanVat selfChiSoNhanVat;
    public GameObject phanThan, khoiPhanThan;
    public bool isPlayer = false;
    private TrangThai tt;
    [Header("Object Pooling - Khói Phân Thân")]
    private Queue<GameObject> khoiPhanThanPool = new Queue<GameObject>();
    private int poolSize = 5;

    // Danh sách lưu trữ các phân thân đang hoạt động để hủy khi OnDisable
    private List<GameObject> activePhanThans = new List<GameObject>();

    private void Awake()
    {
        selfChiSoNhanVat = GetComponent<ChiSoNhanVat>();
        tt = GetComponent<TrangThai>();
        InitializeKhoiPhanThanPool();
    }

    private void InitializeKhoiPhanThanPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject khói = Instantiate(khoiPhanThan, transform.position, Quaternion.identity);
            khói.SetActive(false);
            khoiPhanThanPool.Enqueue(khói);
        }
    }

    private GameObject GetKhoiPhanThan(Vector3 position)
    {
        GameObject khoi;
        if (khoiPhanThanPool.Count > 0)
        {
            khoi = khoiPhanThanPool.Dequeue();
        }
        else
        {
            // Nếu hết pool (trường hợp dùng quá 5 cái cùng lúc), tạo mới
            khoi = Instantiate(khoiPhanThan);
        }

        khoi.transform.position = position;
        khoi.SetActive(true);
        return khoi;
    }

    private IEnumerator ReturnToPoolAfterDelay(GameObject khoi, float delay)
    {
        yield return new WaitForSeconds(delay);
        khoi.SetActive(false);
        khoiPhanThanPool.Enqueue(khoi);
    }

    private void OnEnable()
    {
        selfChiSoNhanVat.OnNangLuongDay += OnNangLuongDay;
        selfChiSoNhanVat.OnNangLuongThieu += OnNangLuongThieu;
    }

    private void OnDisable()
    {
        selfChiSoNhanVat.OnNangLuongDay -= OnNangLuongDay;
        selfChiSoNhanVat.OnNangLuongThieu -= OnNangLuongThieu;

        // Destroy toàn bộ các phân thân hiện có trong danh sách
        foreach (var pt in activePhanThans)
        {
            if (pt != null)
            {
                Destroy(pt);
            }
        }
        activePhanThans.Clear();
    }

    private void OnNangLuongDay()
    {
        if (isPlayer) MobileUIManager.Instance.ShowNutPhanThan(true);
        else
        {
           if (tt.CanAttack()) tt.Attack("phanThan");
        }
    }

    public void AnhPhanThanChiThuat()
    {
        if (selfChiSoNhanVat.NangLuongHienTai == 0f) return;
        selfChiSoNhanVat.ReSetNangLuong();
        Vector2 spawnPos = new Vector2(transform.position.x + 2f * transform.localScale.x, transform.position.y);
        GameObject phanThanClone = Instantiate(phanThan, spawnPos, Quaternion.identity);

        // Thêm phân thân vào danh sách quản lý
        activePhanThans.Add(phanThanClone);

        // Lấy khói từ pool thay vì Instantiate trực tiếp
        GameObject khoiPhan = GetKhoiPhanThan(spawnPos);
        StartCoroutine(ReturnToPoolAfterDelay(khoiPhan, 3f));

        phanThanClone.GetComponent<PlayerControl>().enabled = false;
        phanThanClone.GetComponent<EnemyAi>().enabled = true;
        phanThanClone.GetComponent<EnemyAi>().player = QuanLiCharacter.Instance.GetTransFormEnemy(selfChiSoNhanVat.TenNhanVat);
        phanThanClone.tag = tag;
        phanThanClone.GetComponent<NarutoSkill>().enabled = false;
        phanThanClone.GetComponent<ChiSoNhanVat>().updateUI = false;
        ChiSoNhanVat cs = phanThanClone.GetComponent<ChiSoNhanVat>();
        cs.Mau *= 0.05f;
        cs.MauHienTai *= cs.Mau;

    }

    private void OnNangLuongThieu()
    {
        if (isPlayer) MobileUIManager.Instance.ShowNutPhanThan(false);
    }
}