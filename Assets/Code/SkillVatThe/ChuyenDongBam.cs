
using UnityEngine;

public class ChuyenDongBam: ChuyenDong
{
    private ChiSo chiSoChuSoHuu;

    private void Awake()
    {
        // ChiSo đã có sẵn trên object cha
        chiSoChuSoHuu = GetComponentInParent<ChiSo>();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        transform.SetParent(null);
    }
    public override void DatHuongDiChuyen(Vector3 direction)
    {
        // Gọi hàm của class cha để thiết lập hướng di chuyển
        base.DatHuongDiChuyen(direction);

        // Xoay projectile theo hướng di chuyển trong mặt phẳng XY
        if (direction.sqrMagnitude > 0f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ChiSoNhanVat nhanVat = collision.GetComponent<ChiSoNhanVat>();

        // Không va chạm với object không phải nhân vật
        if (nhanVat == null)
            return;

        // Không bám lên nhân vật cùng chủ sở hữu
        if (chiSoChuSoHuu != null &&
            nhanVat.TenNhanVat == chiSoChuSoHuu.tenNguoiSoHuu)
        {
            return;
        }

        // Bám projectile vào mục tiêu
        transform.SetParent(collision.transform, true);

        // Dừng di chuyển sau khi bám
        enabled = false;
    }
}
