using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static SelectCharacter;

public class QuanLiCharacter : MonoBehaviour
{
    public static QuanLiCharacter Instance;

    public GameObject[] characters;
    [HideInInspector] public ChiSoNhanVat[] csnv;
    private int soNhanVatDaXuatHienXong;
    private Animator animator;
    private int[] id2charWin, soManThang2Nv;
    private int id, id2;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            id2charWin = new int[2];
            soManThang2Nv = new int[2];
            id = id2 = -1;
        }
        else Destroy(gameObject);
    }
    public void KhoiTaoNhanVat()
    {
        for (int i = 0; i < 2; i++)
        {
            if (characters[i] != null)
            {
                Destroy(characters[i]);
            }
            if (QuanLiCheDoChoi.Instance.fightMode == QuanLiCheDoChoi.FightMode.Tournament)
            {
                if (Battle.Instance.currentRound == 0)
                {
                    soManThang2Nv[i] = 0;
                    Battle.Instance.KhoiTaoSoTranThangUI();
                    id = SelectCharacter.Instance.tournament.TraVeNguoiTiepTheo();
                    if (i == 0 && id != SelectCharacter.Instance.nguoiChoi)
                    {
                        id2 = SelectCharacter.Instance.tournament.TraVeNguoiTiepTheo();
                        int w = Random.Range(0f, 1f) > 0.5f ? id2 : id;
                        SelectCharacter.Instance.tournament.SetWinner(w);
                        while (true)
                        {
                            id = SelectCharacter.Instance.tournament.TraVeNguoiTiepTheo();
                            if (id == SelectCharacter.Instance.nguoiChoi)
                            {
                                break;
                            }
                            else
                            {
                                id2 = SelectCharacter.Instance.tournament.TraVeNguoiTiepTheo();
                                w = Random.Range(0f, 1f) > 0.5f ? id2 : id;
                                SelectCharacter.Instance.tournament.SetWinner(w);
                            }
                        }
                    }

                    characters[i] = Instantiate(SelectCharacter.Instance.Characters[id]);
                    characters[i].SetActive(false);
                    characters[i].GetComponent<ChiSoNhanVat>().TenNhanVat += i.ToString();
                    id2charWin[i] = id;
                }
                else
                {
                    characters[i] = Instantiate(SelectCharacter.Instance.Characters[id2charWin[i]]);
                    characters[i].GetComponent<ChiSoNhanVat>().TenNhanVat += i.ToString();
                }
                
            }
            else
            {
                characters[i] = Instantiate(SelectCharacter.Instance.Characters[SelectCharacter.Instance.iD2chars[i]]);
                characters[i].GetComponent<ChiSoNhanVat>().TenNhanVat += i.ToString();
                
            }
        }
        
        if (QuanLiCheDoChoi.Instance.fightMode == QuanLiCheDoChoi.FightMode.Tournament && Battle.Instance.currentRound == 0)
        {
            Battle.Instance.HienThiTournamentUI(id2charWin[0], id2charWin[1], SelectCharacter.Instance.tournament.danhSachNguoiThamGia);
        }

        if (QuanLiCheDoChoi.Instance.fightMode == QuanLiCheDoChoi.FightMode.PVP)
        {
            characters[0].GetComponent<EnemyAi>().enabled = false;
            characters[1].GetComponent<EnemyAi>().enabled = false;
        }
        else if (QuanLiCheDoChoi.Instance.fightMode == QuanLiCheDoChoi.FightMode.PVE || QuanLiCheDoChoi.Instance.fightMode == QuanLiCheDoChoi.FightMode.Tournament)
        {
            characters[0].GetComponent<EnemyAi>().enabled = false;
            /**/
            characters[1].GetComponent<EnemyAi>().enabled = true;
            characters[1].GetComponent<PlayerControl>().enabled = false;
        }
        else if (QuanLiCheDoChoi.Instance.fightMode == QuanLiCheDoChoi.FightMode.EVE)
        {
            characters[0].GetComponent<EnemyAi>().enabled = true;
            characters[0].GetComponent<PlayerControl>().enabled = false;

            characters[1].GetComponent<EnemyAi>().enabled = true;
            characters[1].GetComponent<PlayerControl>().enabled = false;

        }
        else
        {
            characters[0].GetComponent<EnemyAi>().enabled = false;
            characters[0].GetComponent<PlayerControl>().enabled = true;

            characters[1].GetComponent<EnemyAi>().enabled = true;
            characters[1].GetComponent<EnemyAi>().canAttack = false;
            characters[1].GetComponent<PlayerControl>().enabled = false;
        }

        soNhanVatDaXuatHienXong = 0;
        animator = GetComponent<Animator>();
        csnv = new ChiSoNhanVat[characters.Length];
        for (int i = 0; i < characters.Length; i++)
        {
            csnv[i] = characters[i].GetComponent<ChiSoNhanVat>();
        }
    }
    public void ActiveCharacter()
    {
        for (int i=0; i<2; i++)
        {
            characters[i].SetActive(true);
        }
        Battle.Instance.KhoiTaoBattle();

    }
    public void StartDenMap()
    {
        animator.Play("startDen");
    }
    public void EndDenMap()
    {
        animator.Play("endDen");
    }
    public void CorouDenMap(float time)
    {
        StartCoroutine(DenMap(time));   
    }
    IEnumerator DenMap(float time)
    {
        animator.Play("startDen");
        yield return new WaitForSeconds(time);
        animator.Play("endDen");
    }
    public void SkillDacBiet(string tenNhanVatDungSkillDacBiet, float time)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            if (tenNhanVatDungSkillDacBiet != csnv[i].TenNhanVat)
            {
                characters[i].GetComponent<TrangThai>().DungIm(time);
            }
        }
        StartCoroutine(DenMap(time));
    }
    public void XuatHienXongRoi()
    {
        soNhanVatDaXuatHienXong++;
        if (soNhanVatDaXuatHienXong == characters.Length)
        {
            for (int i = 0; i < characters.Length; i++)
            {
                characters[i].GetComponent<TrangThai>().Performance = false;
            }
            Battle.Instance.textUiTranDau.Play("Fight");

        }
    }
    public Transform GetTransFormEnemy(string tenEnemyAi)
    {
        foreach (ChiSoNhanVat cs in csnv)
        {
            if (tenEnemyAi != cs.TenNhanVat) return cs.transform;
        }
        return null;
    }
    public void SetWinner(int winner)
    {
        if (!Battle.Instance.vuaXongTran)
        {
            Battle.Instance.currentRound++;
            characters[winner].GetComponent<TrangThai>().Win();
            Battle.Instance.textUiTranDau.Play("KO");
            /*********/
            //if (QuanLiCheDoChoi.Instance.fightMode == QuanLiCheDoChoi.FightMode.Tournament)
            //{
                soManThang2Nv[winner]++;
                Battle.Instance.ThemSoTranThangUI(winner);
                if (soManThang2Nv[0] > QuanLiCheDoChoi.Instance.soRounds / 2 || soManThang2Nv[1] > QuanLiCheDoChoi.Instance.soRounds / 2)
                {
                    Battle.Instance.currentRound = QuanLiCheDoChoi.Instance.soRounds;
                }
                // * vấn đề nằm ở đây
                if (Battle.Instance.currentRound == QuanLiCheDoChoi.Instance.soRounds)
                {
                    int w = soManThang2Nv[0] > soManThang2Nv[1] ? 0 : 1;
                    if (QuanLiCheDoChoi.Instance.fightMode == QuanLiCheDoChoi.FightMode.Tournament) SelectCharacter.Instance.tournament.SetWinner(id2charWin[w]);
                    if (id2charWin[w] != SelectCharacter.Instance.nguoiChoi) Battle.Instance.playerDaThua = true;
                }
            //}
            Battle.Instance.NextRound();
        }
    }
    public Vector3 ViTriDoiThuGanNhat(string tenNguoiDung)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            if (tenNguoiDung != csnv[i].TenNhanVat)
            {
                return characters[i].transform.position;
            }
        }
        return Vector3.zero;
    }
    public Transform TFDoiThuGanNhat(string tenNguoiDung)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            if (tenNguoiDung != csnv[i].TenNhanVat)
            {
                return characters[i].transform;
            }
        }
        return null;
    }
}
