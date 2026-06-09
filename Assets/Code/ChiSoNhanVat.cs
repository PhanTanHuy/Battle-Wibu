using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChiSoNhanVat : MonoBehaviour
{
    public event Action OnNangLuongDay, OnNangLuongThieu;
    private bool isMaxNangLuong = false;
    public float nangLuongU, nangLuongI, nangLuongWU, nangLuongWI;
    [SerializeField] private float tocDo,dashBoost, lucNhay, mau, dame;
    [SerializeField] private string tenNhanVat;
    public string tenUi;
    [SerializeField] private int chuoiCombo;
    public Sprite anhDanhDien;
    public GameObject hieuUngHit;
    public Color mauHit = Color.white;

    private float mauHienTai;
    public float NangLuongHienTai, TocDoHoiNangLuong, NangLuongToiDa;
    private float _dame;
    [HideInInspector] public bool batTu = false;
    private void Start()
    {
        mauHienTai = mau;
        NangLuongHienTai = 0f;
        _dame = dame;
    }
    public void PlusChiSoTheoPhanTram(float phanTramMau, float phanTramDame, float phanTramTocDo, float phanTramLucNhay, float phanTramTocDoHoiNangLuong, float phanTramDashBoost)
    {
        if (phanTramMau != 0f)
        {
            float luongMauCongThem = mau * phanTramMau;
            Mau += luongMauCongThem;
            MauHienTai += luongMauCongThem; 
        }
        if (phanTramDame != 0f)
        {
            dame += dame  * phanTramDame;
        }
        if (phanTramTocDo != 0f)
        {
            tocDo += tocDo * phanTramTocDo;
        }
        if (phanTramLucNhay != 0f)
        {
            lucNhay += lucNhay * phanTramLucNhay;
        }
        if (phanTramTocDoHoiNangLuong != 0f)
        {
            TocDoHoiNangLuong += TocDoHoiNangLuong * phanTramTocDoHoiNangLuong;
        }
        if (phanTramDashBoost != 0f)
        {
            dashBoost += dashBoost * phanTramDashBoost;
        }
    }
    public void DungNangLuong(float th)
    {
        NangLuongHienTai -= th;
        if (isMaxNangLuong)
        {
            isMaxNangLuong = false;
            OnNangLuongThieu?.Invoke();
            Debug.Log("duma thieu nang luong");
        }
    }
    public void PlusNangLuong()
    {
        NangLuongHienTai += TocDoHoiNangLuong * Time.deltaTime;
        KiemTraNangLuongDay();
        if (Battle.Instance.CapNhatThanhNangLuong(TenNhanVat, NangLuongHienTai / 100f))
        {
            RungCameraSingleton.Instance.AuraShake();
            QuanLiAmThanh.Instance.PlayStartAura();
        };
    }
    public void PlusNangLuong(float value)
    {
        NangLuongHienTai += NangLuongToiDa * value;
        KiemTraNangLuongDay();
        Battle.Instance.CapNhatThanhNangLuong(TenNhanVat, NangLuongHienTai / 100f);
    }
    private void KiemTraNangLuongDay()
    {
        if (NangLuongHienTai > NangLuongToiDa)
        {
            Debug.Log("Len cap tiep theo ChiSo");
            NangLuongHienTai = NangLuongToiDa;
            if (!isMaxNangLuong)
            {
                isMaxNangLuong = true;
                OnNangLuongDay?.Invoke();
            }
        }
        else if (isMaxNangLuong)
        {
            isMaxNangLuong = false;
            OnNangLuongThieu?.Invoke();
            Debug.Log("duma thieu nang luong");
        }
    }
    public int ChuoiCombo
    {
        get { return chuoiCombo; }
    }
    public string TenNhanVat
    {
        get { return tenNhanVat; }
        set { tenNhanVat = value; }
    }
    public float DashBoost
    {
        get { return dashBoost;  }
    }
    public float TocDo
    {
        get { return tocDo; }
        set
        {
            tocDo = value;
        }
    }

    public float LucNhay
    {
        get { return lucNhay; }
        set
        {
            lucNhay = value;
        }
    }
    public float Mau
    {
        get { return mau; }
        set
        {
            mau = value;
        }
    }
    public float PhanTramMauHienTai
    {
        get { return (mauHienTai / mau); }
    }
    public float MauHienTai
    {
        get { return mauHienTai; }
        set
        {
            if (batTu) return;
            mauHienTai = value;
        }
    }
    public float Dame
    {
        get { return _dame; }
        set { _dame = value; }
    }
    public void PlusDame(float value)
    {

        _dame = dame + dame * value;
    }
    public void ResetDame()
    {
        _dame = dame;
    }
}
