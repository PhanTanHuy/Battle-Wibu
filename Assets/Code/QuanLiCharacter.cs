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
    private bool xong1Tran;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else Destroy(gameObject);
    }
    public void KhoiTaoNhanVat()
    {
        xong1Tran = false;
        for (int i = 0; i < SelectCharacter.Instance.maxChar; i++)
        {
            if (characters[i] != null)
            {
                Destroy(characters[i]);
            }
            characters[i] = Instantiate(SelectCharacter.Instance.chars[i]);
            characters[i].GetComponent<ChiSoNhanVat>().TenNhanVat += i.ToString();
        }
        if (SelectCharacter.Instance.fightMode == FightMode.PVP)
        {
            characters[0].GetComponent<EnemyAi>().enabled = false;
            characters[1].GetComponent<EnemyAi>().enabled = false;
        }
        else if (SelectCharacter.Instance.fightMode == FightMode.PVE)
        {
            characters[0].GetComponent<EnemyAi>().enabled = false;
            /**/
            characters[1].GetComponent<EnemyAi>().enabled = true;
            characters[1].GetComponent<PlayerControl>().enabled = false;
        }
        else if (SelectCharacter.Instance.fightMode == FightMode.EVE)
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
        RungCameraSingleton.Instance.StopShake();
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
        if (!xong1Tran)
        {
            characters[winner].GetComponent<TrangThai>().Win();
            Battle.Instance.textUiTranDau.Play("KO");
            /*********/
            Battle.Instance.NextRound();
            xong1Tran = true;
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
