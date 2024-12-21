using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Battle : MonoBehaviour
{
    [System.Serializable]
    public class HubStruct
    {
        public Image Avartar;
        public Image ThanhMau, ThanhNangLuong;
        public TextMeshProUGUI soLanTichLuyThanhNangLuong, tenNhanVat;
        private int soLanTichLuyNl, maxTichLuy;
        private float mau, nangLuong;
        private string tenNhanVatString;
        public void KhoiTao(Sprite avt, string t, string tLogic, float maxTl)
        {
            Avartar.sprite = avt;
            ThanhMau.fillAmount = 1f;
            ThanhNangLuong.fillAmount = 0f;
            soLanTichLuyThanhNangLuong.text = soLanTichLuyNl.ToString();
            tenNhanVat.text = t;
            tenNhanVatString = tLogic;
            soLanTichLuyNl = 0;
            maxTichLuy = (int)maxTl;
            mau = 1f;
            nangLuong = 0f;
            CapNhatThanhNangLuong(0f);
        }
        public void CapNhatThanhMau(float value)
        {
            ThanhMau.fillAmount = value;
        }
        public bool CapNhatThanhNangLuong(float value)
        {
            if (value > soLanTichLuyNl + 1 && soLanTichLuyNl < maxTichLuy - 2)
            {
                soLanTichLuyNl++;
                soLanTichLuyThanhNangLuong.text = soLanTichLuyNl.ToString();
                return true;
            }
            else if (value < soLanTichLuyNl)
            {
                soLanTichLuyNl = (int)value;
                soLanTichLuyThanhNangLuong.text = soLanTichLuyNl.ToString();
            }
            ThanhNangLuong.fillAmount = value - soLanTichLuyNl;
            return false;
        }
        public string TenNhanVatString
        {
            get { return tenNhanVatString; }
        }
    }
    public HubStruct Hub1, Hub2;
    public static Battle Instance;
    public GameObject Shadows;
    public Transform sanDau;
    public CinemachineTargetGroup targetGroup;
    public Animator textUiTranDau;
    private int currentRound;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            KhoiTaoMap();
            QuanLiCharacter.Instance.KhoiTaoNhanVat();
            KhoiTaoBattle();
        }
        else Destroy(gameObject);
    }
    //void Start()
    //{
    //    QuanLiCharacter.Instance.KhoiTaoNhanVat();
    //    KhoiTaoBattle();
    //}
    public void KhoiTaoMap()
    {
        GameObject map = Instantiate(SelectMap.Instance.MapDaDuocChon());
        map.transform.SetParent(sanDau);
        map.transform.localPosition = Vector3.zero;
    }
    public void KhoiTaoBattle()
    {
        currentRound = 0;
        Hub1.KhoiTao(QuanLiCharacter.Instance.csnv[0].anhDanhDien, QuanLiCharacter.Instance.csnv[0].tenUi, QuanLiCharacter.Instance.csnv[0].TenNhanVat, QuanLiCharacter.Instance.csnv[0].NangLuongToiDa / 100f);
        Hub2.KhoiTao(QuanLiCharacter.Instance.csnv[1].anhDanhDien, QuanLiCharacter.Instance.csnv[1].tenUi, QuanLiCharacter.Instance.csnv[1].TenNhanVat, QuanLiCharacter.Instance.csnv[1].NangLuongToiDa / 100f);
        for (int i = 0; i < QuanLiCharacter.Instance.characters.Length; i++)
        {
            Transform tf = QuanLiCharacter.Instance.characters[i].transform;
            tf.position = sanDau.position;
            if (i == 0)
            {
                tf.position += new Vector3(-3f, 0f, 0f);
                tf.localScale = new Vector2(1f, 1f);
            }
            else
            {
                tf.position += new Vector3(3f, 0f, 0f);
                tf.localScale = new Vector2(-1f, 1f);
            }
            GameObject sd = Instantiate(Shadows);
            sd.GetComponent<ShadowMove>().SetTargetShadow(tf, sanDau);
        }
        var existingTargets = targetGroup.m_Targets;

        // Tạo một mảng mới với kích thước lớn hơn
        var newTargets = new CinemachineTargetGroup.Target[existingTargets.Length + 2];

        // Sao chép các target cũ vào mảng mới
        for (int i = 0; i < existingTargets.Length; i++)
        {
            newTargets[i] = existingTargets[i];
        }

        // Thêm target mới
        newTargets[existingTargets.Length] = new CinemachineTargetGroup.Target
        {
            target = QuanLiCharacter.Instance.characters[0].transform,
            weight = 1f,
            radius = 0f
        };

        newTargets[existingTargets.Length + 1] = new CinemachineTargetGroup.Target
        {
            target = QuanLiCharacter.Instance.characters[1].transform,
            weight = 1f,
            radius = 0f
        };

        // Gán lại mảng target mới cho Target Group
        targetGroup.m_Targets = newTargets;
    }
    public void CapNhatThanhMau(string tenNhanVat, float value)
    {
        if (tenNhanVat == Hub1.TenNhanVatString) Hub1.CapNhatThanhMau(value);
        else Hub2.CapNhatThanhMau(value);
        if (KiemTraCoAiChetChua())
        {
            int winner = Hub2.ThanhMau.fillAmount == 0f ? 0 : 1;
            QuanLiCharacter.Instance.SetWinner(winner);
        }
    }
    public bool CapNhatThanhNangLuong(string tenNhanVat, float value)
    {
        if (tenNhanVat == Hub1.TenNhanVatString) return Hub1.CapNhatThanhNangLuong(value);
        else return Hub2.CapNhatThanhNangLuong(value);
    }
    private bool KiemTraCoAiChetChua()
    {
        return Hub1.ThanhMau.fillAmount == 0f || Hub2.ThanhMau.fillAmount == 0f;
    }
    public void StartNextRound()
    {
        textUiTranDau.Play("startDen");
    }
    public void EndDen()
    {
        textUiTranDau.Play("endDen");
    }
    public void SetCharacterNextGround()
    {
        QuanLiCharacter.Instance.KhoiTaoNhanVat();
        KhoiTaoBattle();
        EndDen();
    }
    public void NextRound()
    {
        if (currentRound == SelectCharacter.Instance.maxRound) return;
        Invoke("StartNextRound", 2f);
        currentRound++;
    }
}
