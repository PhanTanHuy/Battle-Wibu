using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NhanSatThuong : MonoBehaviour
{
    private ControlAnimator ctra;
    private ChiSoNhanVat csnv;
    private TrangThai trangThai;
    private Vector2 right, left;
    private void Start()
    {
        ctra = GetComponent<ControlAnimator>();
        csnv = GetComponent<ChiSoNhanVat>();
        trangThai = GetComponent<TrangThai>();
        right = new Vector2(1f, 1f);
        left = new Vector2(-1f, 1f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("boxAttack") && collision.transform.parent != transform)
        {
            Vector2 directionOfAttack = collision.transform.parent.localScale.x < 0f ? left : right;
            Transform keGayDame = collision.transform.parent;
            PoolVfx.instance.CreateCollideEffect(transform.position, keGayDame.GetComponentInParent<ChiSoNhanVat>().TenNhanVat, directionOfAttack);
            float lucDayKnockOut = keGayDame.GetComponentInParent<TrangThai>().LucKnockOut;
            float lucHatTung = keGayDame.GetComponentInParent<TrangThai>().LucHatTung;
            bool normalAttack = keGayDame.GetComponentInParent<TrangThai>().nangLuongTieuHao == 0f;
            if (!trangThai.IsDefense)
            {
                if (csnv.MauHienTai > 0f)
                {
                    if (lucDayKnockOut == 0f)
                    {
                        trangThai.StartHurt(directionOfAttack.x, lucHatTung, normalAttack);
                    }
                    else
                    {
                        QuanLiAmThanh.Instance.PlayTelePunch();
                        PoolVfx.instance.KhoiKnockOut(transform);
                        trangThai.StartKnockOut(directionOfAttack.x, lucDayKnockOut, lucHatTung, normalAttack);
                    }
                }
                csnv.MauHienTai -= keGayDame.GetComponentInParent<ChiSoNhanVat>().Dame;
                Battle.Instance.CapNhatThanhMau(csnv.TenNhanVat, csnv.PhanTramMauHienTai);
                keGayDame.GetComponentInParent<TrangThai>().Hitting();
                if (csnv.MauHienTai <= 0f) trangThai.Death(directionOfAttack.x);
            }
            else
            {
                keGayDame.GetComponentInParent<TrangThai>().Hitting();
                RungCameraSingleton.Instance.Shake(0.15f, 4f, 1f);
                QuanLiAmThanh.Instance.PlayHit();
                csnv.PlusNangLuong(0.0075f);
                csnv.MauHienTai -= keGayDame.GetComponentInParent<ChiSoNhanVat>().Dame * 0.2f;
                Battle.Instance.CapNhatThanhMau(csnv.TenNhanVat, csnv.PhanTramMauHienTai);
            }
        }
        else if (collision.CompareTag("boxObjectAttack"))
        {
            ChiSo cs = collision.GetComponentInParent<ChiSo>();
            if (cs.tenNguoiSoHuu == csnv.TenNhanVat) return;
            Vector2 directionOfAttack = cs.transform.localScale.x < 0f ? left : right;
            QuanLiAmThanh.Instance.PlayExPolison();
            if (!trangThai.IsDefense)
            {
                if (csnv.MauHienTai > 0f)
                {
                    if (!trangThai.IsKnockout)
                    {
                        trangThai.StartHurt(directionOfAttack.x, 3f, false);
                    }
                }
                csnv.MauHienTai -= cs.Dame;
                Battle.Instance.CapNhatThanhMau(csnv.TenNhanVat, csnv.PhanTramMauHienTai);
                PoolVfx.instance.CreateCollideEffect(transform.position, cs.tenNguoiSoHuu, directionOfAttack);
                if (csnv.MauHienTai <= 0f) trangThai.Death(directionOfAttack.x);
            }
            else
            {
                RungCameraSingleton.Instance.Shake(0.15f, 4f, 1f);
                QuanLiAmThanh.Instance.PlayHit();
                csnv.PlusNangLuong(0.0075f);
                csnv.MauHienTai -= cs.Dame * 0.2f;
                Battle.Instance.CapNhatThanhMau(csnv.TenNhanVat, csnv.PhanTramMauHienTai);
            }

        }
    }
}
