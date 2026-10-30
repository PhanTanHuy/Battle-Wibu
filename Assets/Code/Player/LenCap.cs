using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LenCap : MonoBehaviour
{
    [System.Serializable]
    public struct LenCapData
    {
        public RuntimeAnimatorController animatorController;

        [Header("--- CẤU HÌNH KỸ NĂNG MỚI KHI LÊN CẤP ---")]
        // Thêm danh sách kỹ năng vật thể mới cho cấp độ này
        [SerializeField] public PoolObject.SkillVatThe[] danhSachSkillMoi;

        [Header("Tỷ lệ cộng thêm (0.2 = 20%)")]
        public float phanTramMau;
        public float phanTramDame;
        public float phanTramTocDo;
        public float phanTramLucNhay;
        public float phanTramTocDoHoiNangLuong;
        public float phanTramDashBoost;
    }

    [Header("Cấu hình danh sách các cấp độ")]
    [SerializeField] private LenCapData[] danhSachCapDo;

    private int currentCap = 0;
    private Animator currentAnimator;
    private ChiSoNhanVat selfChiSoNhanVat;
    private TrangThai tt;
    private PoolObject selfPoolObject; // Thêm tham chiếu tới PoolObject

    [HideInInspector] public bool isPlayer = false;

    private void Awake()
    {
        currentAnimator = GetComponent<Animator>();
        selfChiSoNhanVat = GetComponent<ChiSoNhanVat>();
        selfPoolObject = GetComponent<PoolObject>(); // Lấy component PoolObjecta
        tt = GetComponent<TrangThai>();
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
    }

    private void OnNangLuongDay()
    {
        if (currentCap >= danhSachCapDo.Length)
        {
            Debug.LogWarning($"{gameObject.name} đã đạt cấp độ cấu hình tối đa!");
            return;
        }
        if (isPlayer) MobileUIManager.Instance.ShowNutLenCap(true);
        else
        {
            if (tt.CanAttack()) tt.Attack("lenCap");
        }
    }

    private void OnNangLuongThieu()
    {
        if (isPlayer) MobileUIManager.Instance.ShowNutLenCap(false);
    }

    public void LenCapTiepTheo()
    {
        if (currentCap >= danhSachCapDo.Length)
        {
            Debug.LogWarning($"{gameObject.name} đã đạt cấp độ cấu hình tối đa!");
            return;
        }

        Debug.Log($"Tiến hành lên cấp tiếp theo cho: {gameObject.name}");

        LenCapData dataCapMoi = danhSachCapDo[currentCap];

        if (dataCapMoi.animatorController != null)
        {
            currentAnimator.runtimeAnimatorController = dataCapMoi.animatorController;
        }

        // --- CẬP NHẬT SKILL MỚI VÀO POOL OBJECT ---
        if (selfPoolObject != null && dataCapMoi.danhSachSkillMoi != null && dataCapMoi.danhSachSkillMoi.Length > 0)
        {
            selfPoolObject.CapNhatSkillMoi(dataCapMoi.danhSachSkillMoi);
        }

        if (selfChiSoNhanVat != null)
        {
            selfChiSoNhanVat.PlusChiSoTheoPhanTram(
                dataCapMoi.phanTramMau,
                dataCapMoi.phanTramDame,
                dataCapMoi.phanTramTocDo,
                dataCapMoi.phanTramLucNhay,
                dataCapMoi.phanTramTocDoHoiNangLuong,
                dataCapMoi.phanTramDashBoost
            );

            selfChiSoNhanVat.ReSetNangLuong();
        }

        currentCap++;
        GetComponent<TrangThai>().EndAttack();
        RungCameraSingleton.Instance.Shake(0.2f, 20f, 1f);
    }
}