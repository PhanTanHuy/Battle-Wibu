using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChiSoNhanVat : MonoBehaviour
{
    public float nangLuongU, nangLuongI, nangLuongWU, nangLuongWI;
    [SerializeField] private float tocDo, lucNhay, mau, dame;
    [SerializeField] private string tenNhanVat;
    public string tenUi;
    [SerializeField] private int chuoiCombo;
    public Sprite anhDanhDien;
    public GameObject hieuUngHit;
    private float mauHienTai;
    public float NangLuongHienTai, TocDoHoiNangLuong, NangLuongToiDa;
    private float _dame;
    private void Start()
    {
        mauHienTai = mau;
        NangLuongHienTai = 0f;
        _dame = dame;
    }
    public void PlusNangLuong()
    {
        if (NangLuongHienTai + TocDoHoiNangLuong * Time.deltaTime < NangLuongToiDa) NangLuongHienTai += TocDoHoiNangLuong * Time.deltaTime;
        if (NangLuongHienTai > NangLuongToiDa) NangLuongHienTai = NangLuongToiDa * 0.99f;
        if (Battle.Instance.CapNhatThanhNangLuong(TenNhanVat, NangLuongHienTai / 100f))
        {
            RungCameraSingleton.Instance.AuraShake();
            QuanLiAmThanh.Instance.PlayStartAura();
        };
    }
    public void PlusNangLuong(float value)
    {
        if (NangLuongHienTai + NangLuongToiDa * value < NangLuongToiDa) NangLuongHienTai += NangLuongToiDa * value;
        if (NangLuongHienTai > NangLuongToiDa) NangLuongHienTai = NangLuongToiDa * 0.99f;
        Battle.Instance.CapNhatThanhNangLuong(TenNhanVat, NangLuongHienTai / 100f);
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
