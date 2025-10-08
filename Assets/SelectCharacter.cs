using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Tournament
{
    public Queue<int> danhSachNguoiThamGia;
    public Tournament()
    {
        danhSachNguoiThamGia = new Queue<int>();
    }
    public int TraVeNguoiTiepTheo()
    {
        return danhSachNguoiThamGia.Dequeue();
    }
    public void SetWinner(int winner)
    {
        danhSachNguoiThamGia.Enqueue(winner);
    }
}


public class SelectCharacter : MonoBehaviour
{
    public static SelectCharacter Instance;
    public GameObject Tournament;
    [HideInInspector] public int[] soTranDanh;
    public GameObject[] Characters;
    public GameObject container;
    public GameObject prefabsSelectMau, khungSelect;
    public Image[] imageChar;
    public TextMeshProUGUI cpuModeText, tournamentMatchsText;
    [SerializeField] private TextMeshProUGUI t1char, t2char;
    private string[] modes = { "Cpu mode: Easy", "Cpu mode: Medium", "Cpu mode: Hard" };
    [HideInInspector] public int modeIndex = 0, indexSoTranDanh;
    [HideInInspector] public Tournament tournament;
    [HideInInspector] public List<GameObject> buttonSelect;
    [HideInInspector] public int indexNhanVatDcChon;
    [HideInInspector] public int soLanChon, nguoiChoi;
    [HideInInspector] public int[] iD2chars;
    [HideInInspector] public GameObject _khungSelect;

    private void Awake()
    {
        if (Instance != null) Destroy(Instance.gameObject);
        QuanLiChuyenScene.instance.BatDauSceneMoi();
        if (SelectMap.Instance != null) Destroy(SelectMap.Instance.gameObject);
        Instance = this;
        //
        soTranDanh = new int[3];
        indexSoTranDanh = 0;
        int soTranDanhFirst = 2;
        for (int i=0; i<soTranDanh.Length; i++)
        {
            soTranDanh[i] = soTranDanhFirst;
            soTranDanhFirst *= 2;
        }
        //
        soLanChon = 0;
        buttonSelect = new List<GameObject>();
        iD2chars = new int[2];
        indexNhanVatDcChon = -1;
        for (int i = 0; i < Characters.Length; i++)
        {
            GameObject _prefSelectNew = Instantiate(prefabsSelectMau);
            _prefSelectNew.transform.GetChild(0).GetComponent<Image>().sprite = Characters[i].GetComponent<ChiSoNhanVat>().anhDanhDien;
            _prefSelectNew.GetComponent<ButtonSelectCharacter>().index = i;
            _prefSelectNew.transform.SetParent(container.transform);
            buttonSelect.Add(_prefSelectNew);
            _prefSelectNew.transform.localScale = Vector3.one;
        }
        Tournament.SetActive(false);
        t2char.transform.parent.gameObject.SetActive(true);
        if (QuanLiCheDoChoi.Instance.fightMode == QuanLiCheDoChoi.FightMode.PVE)
        {
            t1char.text = "YOU";
            t2char.text = "CPU";
        }
        else if (QuanLiCheDoChoi.Instance.fightMode == QuanLiCheDoChoi.FightMode.EVE)
        {
            t1char.text = "CPU 1";
            t2char.text = "CPU 2";
        }
        else if (QuanLiCheDoChoi.Instance.fightMode == QuanLiCheDoChoi.FightMode.Tournament)
        {
            t2char.transform.parent.gameObject.SetActive(false);
            Tournament.SetActive(true);
            tournamentMatchsText.text = soTranDanh[0].ToString() + " Matchs";
            tournament = new Tournament();
        }
        _khungSelect = Instantiate(khungSelect);
        _khungSelect.transform.SetParent(buttonSelect[0].transform);
        _khungSelect.SetActive(false);
        DontDestroyOnLoad(gameObject);
    }
    public void SpriteToUIImage()
    {
        imageChar[soLanChon].enabled = true;
        // Lấy Sprite từ SpriteRenderer
        Sprite sprite = Characters[indexNhanVatDcChon].GetComponent<SpriteRenderer>().sprite;
        imageChar[soLanChon].sprite = sprite;

        // Tính tỷ lệ aspect ratio của sprite
        RectTransform rt = imageChar[soLanChon].rectTransform;
        rt.sizeDelta = new Vector2(sprite.bounds.size.x * 50, sprite.bounds.size.y * 50);
    }
    public void Chon()
    {
        if (indexNhanVatDcChon == -1) return;
        if (QuanLiCheDoChoi.Instance.fightMode == QuanLiCheDoChoi.FightMode.Tournament)
        {
            List<int> inDexCacNhanVatConLai = new List<int>();
            List<int> inDexCacNhanVatConLai1 = new List<int>();

            for (int i=0; i<Characters.Length; i++)
            {
                inDexCacNhanVatConLai.Add(i);
            }
            iD2chars[0] = indexNhanVatDcChon;
            inDexCacNhanVatConLai.Remove(iD2chars[0]);
            int soNguoiChoi = soTranDanh[indexSoTranDanh] * 2;
            tournament.danhSachNguoiThamGia.Enqueue(iD2chars[0]);
            inDexCacNhanVatConLai1.Add(iD2chars[0]);

            for (int i=0; i<soNguoiChoi - 1; i++)
            {
                int nhanVatNgauNhien = inDexCacNhanVatConLai[Random.Range(0, inDexCacNhanVatConLai.Count)];
                tournament.danhSachNguoiThamGia.Enqueue(nhanVatNgauNhien);
                inDexCacNhanVatConLai1.Add(nhanVatNgauNhien);
                inDexCacNhanVatConLai.Remove(nhanVatNgauNhien);
            }
            for (int i = 0; i < soNguoiChoi; i++)
            {
                Debug.Log(Characters[inDexCacNhanVatConLai1[i]].GetComponent<ChiSoNhanVat>().TenNhanVat + " " + inDexCacNhanVatConLai1[i]);
            }
            nguoiChoi = indexNhanVatDcChon;
            QuanLiChuyenScene.instance.LoadSceneKhac("SelectMap");

        }
        else
        {
            iD2chars[soLanChon] = indexNhanVatDcChon;
            if (soLanChon == 1)
            {
                QuanLiChuyenScene.instance.LoadSceneKhac("SelectMap");
                return;
            }
            soLanChon++;
            if (soLanChon != 2) indexNhanVatDcChon = -1;
        }
    }
    public void Back()
    {
        QuanLiChuyenScene.instance.LoadSceneKhac("MainMenu");
    }
    public void ChangeCpuMode()
    {
        modeIndex = (modeIndex + 1) % modes.Length;
        cpuModeText.text = modes[modeIndex];
    }
    public void TangSoTranDauTournament()
    {
        indexSoTranDanh++;
        if (indexSoTranDanh >= soTranDanh.Length) indexSoTranDanh = soTranDanh.Length - 1;
        tournamentMatchsText.text = soTranDanh[indexSoTranDanh].ToString() + " Matchs";
    }
    public void GiamSoTranDauTournament()
    {
        indexSoTranDanh--;
        if (indexSoTranDanh < 0) indexSoTranDanh = 0;
        tournamentMatchsText.text = soTranDanh[indexSoTranDanh].ToString() + " Matchs";
    }
}
