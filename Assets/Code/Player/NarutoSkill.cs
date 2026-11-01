
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class NarutoSkill : MonoBehaviour, ICharacterSpecial
{
    private ChiSoNhanVat selfChiSoNhanVat;
    private NarutoSkill ownerSkill;
    public GameObject phanThan, khoiPhanThan;
    public bool isPlayer = false;

    [HideInInspector] public bool isMain = true;

    private TrangThai tt;

    [Header("Object Pooling - Khói Phân Thân")]
    private Queue<GameObject> khoiPhanThanPool = new Queue<GameObject>();

    private HashSet<GameObject> allKhoiPhanThan = new HashSet<GameObject>();

    private int poolSize = 5;

    // Các phân thân do chính object này tạo ra
    private List<GameObject> activePhanThans = new List<GameObject>();

    private bool isDead = false;

    private void Awake()
    {
        selfChiSoNhanVat = GetComponent<ChiSoNhanVat>();
        tt = GetComponent<TrangThai>();

        InitializeKhoiPhanThanPool();
    }

    private void OnEnable()
    {
        if (selfChiSoNhanVat == null) return;

        selfChiSoNhanVat.OnNangLuongDay += OnNangLuongDay;
        selfChiSoNhanVat.OnNangLuongThieu += OnNangLuongThieu;
    }

    private void OnDisable()
    {
        if (selfChiSoNhanVat == null) return;

        selfChiSoNhanVat.OnNangLuongDay -= OnNangLuongDay;
        selfChiSoNhanVat.OnNangLuongThieu -= OnNangLuongThieu;
    }

    public void HandleDungIm(float time)
    {
        foreach (GameObject clone in activePhanThans)
        {
            TrangThai tt = clone.GetComponent<TrangThai>();
            tt.DungIm(time);
        }
    }

    public void HandleDeath()
    {
        if (isDead) return;

        isDead = true;

        if (isMain)
        {
            HandleMainDeath();
        }
        else
        {
            HandleCloneDeath();
        }
    }

    // =========================================
    // MAIN DEATH
    // =========================================

    private void HandleMainDeath()
    {
        List<GameObject> allClones = new List<GameObject>();
        List<Vector3> clonePositions = new List<Vector3>();

        HashSet<GameObject> visited = new HashSet<GameObject>();

        // Thu thập toàn bộ phân thân theo cây
        CollectAllClones(this, allClones, clonePositions, visited);

        // Xóa toàn bộ khói cũ của Naruto chính và các phân thân
        ClearSmokeOfHierarchy(allClones);

        // Tạo khói mới tại vị trí từng phân thân
        foreach (Vector3 pos in clonePositions)
        {
            CreateDeathSmoke(pos);
        }

        // Hủy toàn bộ phân thân
        foreach (GameObject clone in allClones)
        {
            if (clone != null)
            {
                Destroy(clone);
            }
        }

        activePhanThans.Clear();
    }

    // Duyệt đệ quy tất cả phân thân con
    private void CollectAllClones(
        NarutoSkill owner,
        List<GameObject> allClones,
        List<Vector3> positions,
        HashSet<GameObject> visited)
    {
        foreach (GameObject clone in owner.activePhanThans)
        {
            if (clone == null || !visited.Add(clone))
                continue;

            allClones.Add(clone);
            positions.Add(clone.transform.position);

            NarutoSkill cloneSkill = clone.GetComponent<NarutoSkill>();

            if (cloneSkill != null)
            {
                CollectAllClones(
                    cloneSkill,
                    allClones,
                    positions,
                    visited
                );
            }
        }
    }

    // =========================================
    // CLONE DEATH
    // =========================================

    private void HandleCloneDeath()
    {
        Vector3 deathPosition = transform.position;

        // Xóa phân thân khỏi danh sách của owner
        if (ownerSkill != null)
        {
            ownerSkill.activePhanThans.Remove(gameObject);
        }

        // Xóa khói cũ của chính phân thân này
        ClearOwnSmoke();

        // Tạo khói tại vị trí phân thân chết
        CreateDeathSmoke(deathPosition);

        // Hủy phân thân
        Destroy(gameObject);
    }

    // =========================================
    // SMOKE MANAGEMENT
    // =========================================

    private void InitializeKhoiPhanThanPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject khoi = Instantiate(
                khoiPhanThan,
                transform.position,
                Quaternion.identity
            );

            khoi.SetActive(false);

            khoiPhanThanPool.Enqueue(khoi);
            allKhoiPhanThan.Add(khoi);
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
            khoi = Instantiate(khoiPhanThan);
            allKhoiPhanThan.Add(khoi);
        }

        khoi.transform.position = position;
        khoi.SetActive(true);

        return khoi;
    }

    private IEnumerator ReturnToPoolAfterDelay(GameObject khoi, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (khoi == null) yield break;

        khoi.SetActive(false);

        if (!isDead)
        {
            khoiPhanThanPool.Enqueue(khoi);
        }
        else
        {
            Destroy(khoi);
            allKhoiPhanThan.Remove(khoi);
        }
    }

    // Khói khi chết: tồn tại 3 giây rồi tự hủy
    private void CreateDeathSmoke(Vector3 position)
    {
        GameObject khoi = Instantiate(
            khoiPhanThan,
            position,
            Quaternion.identity
        );

        Destroy(khoi, 3f);
    }

    // Xóa toàn bộ khói thuộc object hiện tại
    private void ClearOwnSmoke()
    {
        StopAllCoroutines();

        foreach (GameObject khoi in allKhoiPhanThan)
        {
            if (khoi != null)
            {
                Destroy(khoi);
            }
        }

        allKhoiPhanThan.Clear();
        khoiPhanThanPool.Clear();
    }

    // Xóa khói của Naruto chính và toàn bộ phân thân
    private void ClearSmokeOfHierarchy(List<GameObject> allClones)
    {
        ClearOwnSmoke();

        foreach (GameObject clone in allClones)
        {
            if (clone == null) continue;

            NarutoSkill skill = clone.GetComponent<NarutoSkill>();

            if (skill != null)
            {
                skill.ClearOwnSmoke();
                skill.activePhanThans.Clear();
            }
        }
    }

    // =========================================
    // SKILL
    // =========================================

    private void OnNangLuongDay()
    {
        if (isDead) return;

        if (isPlayer)
        {
            MobileUIManager.Instance.ShowNutPhanThan(true);
        }
        else
        {
            if (tt.CanAttack())
                tt.Attack("phanThan");
        }
    }

    public void AnhPhanThanChiThuat()
    {
        if (isDead || selfChiSoNhanVat.NangLuongHienTai == 0f)
            return;

        selfChiSoNhanVat.ReSetNangLuong();

        Vector2 spawnPos = new Vector2(
            transform.position.x + 2f * transform.localScale.x,
            transform.position.y
        );

        GameObject phanThanClone = Instantiate(
            phanThan,
            spawnPos,
            Quaternion.identity
        );

        // Đăng ký phân thân con vào object đã tạo ra nó
        activePhanThans.Add(phanThanClone);

        // Khói xuất hiện khi phân thân
        GameObject khoiPhan = GetKhoiPhanThan(spawnPos);

        StartCoroutine(ReturnToPoolAfterDelay(khoiPhan, 3f));

        PlayerControl playerControl =
            phanThanClone.GetComponent<PlayerControl>();

        EnemyAi enemyAi =
            phanThanClone.GetComponent<EnemyAi>();

        NarutoSkill cloneSkill =
            phanThanClone.GetComponent<NarutoSkill>();

        if (playerControl != null)
            playerControl.enabled = false;

        if (enemyAi != null)
        {
            enemyAi.enabled = true;

            enemyAi.player =
                QuanLiCharacter.Instance.GetTransFormEnemy(
                    selfChiSoNhanVat.TenNhanVat
                );
        }

        phanThanClone.tag = tag;

        if (cloneSkill != null)
        {
            cloneSkill.isMain = false;
            cloneSkill.ownerSkill = this;
        }

        ChiSoNhanVat cs =
            phanThanClone.GetComponent<ChiSoNhanVat>();

        if (cs != null)
        {
            cs.updateUI = false;
            cs.Mau *= 0.05f;
            cs.MauHienTai = cs.Mau;
        }
    }

    private void OnNangLuongThieu()
    {
        if (isPlayer)
            MobileUIManager.Instance.ShowNutPhanThan(false);
    }
    private void OnDestroy()
    {
        // Chỉ Naruto chính mới chịu trách nhiệm hủy toàn bộ phân thân
        if (!isMain) return;

        DestroyAllClones(this);
    }

    private void DestroyAllClones(NarutoSkill owner)
    {
        foreach (GameObject clone in owner.activePhanThans)
        {
            if (clone == null) continue;

            NarutoSkill cloneSkill = clone.GetComponent<NarutoSkill>();

            // Hủy tiếp các phân thân con trước
            if (cloneSkill != null)
            {
                DestroyAllClones(cloneSkill);
            }

            // Hủy phân thân
            Destroy(clone);
        }

        owner.activePhanThans.Clear();
    }
}
