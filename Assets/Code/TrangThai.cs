using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrangThai : MonoBehaviour
{
    private bool isAttack;
    private bool performance;
    private bool isDefense;
    private bool isHurt;
    private bool isKnockout;
    private bool isAura;
    private bool isDash;
    private bool isWin;
    private bool isAlive;
    private bool dungYenTrenKhong;
    private float boostAttack, boostBatLui, lucKnockOut, lucHatTung, justHitting;
    private bool isNotSetHurt;
    [HideInInspector] public Vector2 viTriHitting;
    [HideInInspector] public float nangLuongTieuHao;
    ControlAnimator controlAnimator;
    private TaoHieuUng thu;
    private ChiSoNhanVat csnv;
    private Rigidbody2D rb;
    private Transform doiPhuong;
    private void Start()
    {
        performance = true;
        isAttack = false;
        isDefense = false;
        isHurt = false;
        isKnockout = false;
        isAura = false;
        isDash = false;
        justHitting = 0f;
        dungYenTrenKhong = false;
        isWin = false;
        isAlive = true;
        boostAttack = lucKnockOut = 0f;
        controlAnimator = GetComponent<ControlAnimator>();
        thu = GetComponent<TaoHieuUng>();
        csnv = GetComponent<ChiSoNhanVat>();
        rb = GetComponent<Rigidbody2D>();
        viTriHitting = Vector2.zero;
        doiPhuong = QuanLiCharacter.Instance.TFDoiThuGanNhat(csnv.TenNhanVat);
    }
    public void KetThucManXuatHien()
    {
        QuanLiCharacter.Instance.XuatHienXongRoi();
    }
    public void SetDungYenTrenKhong(int value)
    {
        viTriHitting = transform.position;
        dungYenTrenKhong = value == 1 ? true : false;
    }
    public void SetBatTu(int value)
    {
        isNotSetHurt = value == 1 ? true : false;
    }
    public void SetLucHatTung(float value)
    {
        lucHatTung = value;
    }
    public bool IsNotSertHurt
    {
        get { return isNotSetHurt; }
    }
    public float LucKnockOut
    {
        get { return lucKnockOut; }
    }
    public float BoostBatLui
    {
        get { return boostBatLui; }
        set { boostBatLui = value; }
    }
    public bool DungYenTrenKhong
    {
        get { return dungYenTrenKhong; }
        set { dungYenTrenKhong = value; }
    }
    public bool Performance
    {
        get { return performance; }
        set { performance = value; }
    }
    public bool IsAlive
    {
        get { return isAlive; }
        set { isAlive = value; }
    }
    public float JustHitting
    {
        get { return justHitting; }
        set {  justHitting = value; }
    }
    public float BoostAttack
    {
        get { return boostAttack; }
        set
        {
            boostAttack = value;
        }
    }
    public void Win()
    {
        isWin = true;
    }
    public void SkillDacBiet(float time)
    {
        QuanLiCharacter.Instance.SkillDacBiet(GetComponent<ChiSoNhanVat>().TenNhanVat, time);
    }
    public void DungIm(float time)
    {
        performance = true;
        viTriHitting = transform.position;
        EndAttack();
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        StartCoroutine(HetDungIm(time));
        GetComponent<Animator>().speed = 0f;
    }
    IEnumerator HetDungIm(float time)
    {
        yield return new WaitForSeconds(time);
        performance = false;
        EndAttack();
        GetComponent<Animator>().speed = 1f;

    }
    public void BoostAttacking(float value)
    {
        boostAttack = value * transform.localScale.x;
    }
    public void BoostJump(float value)
    {
        rb.AddForce(new Vector2(0f, value), ForceMode2D.Impulse);
        justHitting = -1f;
    }
    public float LucHatTung
    {
        get { return lucHatTung; }
    }
    public bool IsAttack
    {
        get { return isAttack; }
        set { isAttack = value; }
    }
    public bool IsDash
    {
        get { return isDash; }
        set { isDash = value; }
    }
    public bool IsDefense
    {
        get { return isDefense; }
        set { isDefense = value; }
    }
    public bool IsHurt
    {
        get { return isHurt; }
        set { isHurt = value; }
    }
    public bool IsKnockout
    {
        get { return isKnockout; }
        set { isKnockout = value; }
    }
    public bool IsAura
    {
        get { return isAura; }
        set { isAura = value; }
    }
    public void SetLucKnockOut(float value)
    {
        lucKnockOut = value;
    }
    public void Hitting()
    {
        justHitting = 0.45f;
        viTriHitting = transform.position;
    }
    public void Dash(bool doYouWantDash)
    {
        if (doYouWantDash)
        {
             if (!isDash)
             {
                isDash = true;
                EndAttack();
                controlAnimator.Dash(isDash);
                BoostAttacking(30f);
                thu.DashSmoke();
                SetDungYenTrenKhong(1);
            }
        }
        else
        {
            if (isDash)
            {
                isDash = false;
                EndAttack();
                controlAnimator.Dash(isDash);
                boostAttack = 0f;
                SetDungYenTrenKhong(0);
            }
        }
      
    }
    public void Run(bool isrun)
    {
        controlAnimator.Run(isrun);
    }
    public void Jump()
    {
        justHitting = -1f;
        controlAnimator.Jump(true);
    }
    public void Fall()
    {
        controlAnimator.Jump(false);
        controlAnimator.Fall(true);
    }
    public void Defense(bool _isDefense)
    {
        if (_isDefense && !isDefense)
        {
            isDefense = true;
            controlAnimator.Defense(isDefense);
            EndAttack();
        }
        else if (!_isDefense && isDefense)
        {
            isDefense = false;
            controlAnimator.Defense(isDefense);
        }
    }
    public void Aura(bool _isAura)
    {
        if (_isAura && !isAura)
        {
            isAura = true;
            controlAnimator.Aura(isAura);
            EndAttack();
            QuanLiAmThanh.Instance.PlayAura();
        }
        else if (!_isAura && isAura)
        {
            isAura = false;
            RungCameraSingleton.Instance.StopShake();
            controlAnimator.Aura(isAura);
            QuanLiAmThanh.Instance.StopAura();
        }
        if (isAura)
        {
            csnv.PlusNangLuong();
            rb.velocity = Vector2.zero;
        }

    }
    public void DichChuyenToiDoiPhuong()
    {
        QuanLiAmThanh.Instance.PlayTele();
        StartCoroutine(DichChuyenIE());
    }
    IEnumerator DichChuyenIE()
    {
        float x = transform.position.x;
        thu.Tele();
        transform.position = doiPhuong.position;
        yield return null;
        XoayVeDoiPhuong();
        thu.Tele();
    }
    public void LuotToiDoiPhuong(float time)
    {
        boostAttack = XoayVeDoiPhuong() / time;
        StartCoroutine(SetBoosAttackIE(0f, time));
    }
    public float XoayVeDoiPhuong()
    {
        float khoangCach = doiPhuong.position.x - transform.position.x;
        transform.localScale = khoangCach >= 0f ? new Vector2(1f, 1f) : new Vector2(-1f, 1f);
        return khoangCach;
    }
    IEnumerator SetBoosAttackIE(float boost, float time)
    {
        yield return new WaitForSeconds(time);
        boostAttack = boost;
    }
    public bool Attack(string nameAttackAnim)
    {
        if (isDash) return false;
        nangLuongTieuHao = 0f;
        switch (nameAttackAnim)
        {
            case stringKey.Attack.AttackU:
                nangLuongTieuHao = csnv.nangLuongU;
                break;
            case stringKey.Attack.AttackI:
                nangLuongTieuHao = csnv.nangLuongI;
                break;
            case stringKey.Attack.AttackWU:
                nangLuongTieuHao = csnv.nangLuongWU;
                break;
            case stringKey.Attack.AttackWI:
                nangLuongTieuHao = csnv.nangLuongWI;
                break;
        }
        if (nangLuongTieuHao != 0f) isNotSetHurt = true;
        if (csnv.NangLuongHienTai >= nangLuongTieuHao)
        {
            StartAttack();
            controlAnimator.AttackWithName(nameAttackAnim);
            csnv.NangLuongHienTai -= nangLuongTieuHao;
            Battle.Instance.CapNhatThanhNangLuong(csnv.TenNhanVat, csnv.NangLuongHienTai / 100f);
            return true;
        }
        else return false;
    }
    public void OnGround()
    {
        controlAnimator.Jump(false);
        controlAnimator.Fall(false);
    }
    public void BatLui(float flip, float bl, float time, bool hatTung)
    {
        boostBatLui = bl * flip;

        if (hatTung)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(new Vector2(0f, 25f), ForceMode2D.Impulse);
        }
        StartCoroutine(NgungBatLui(time, boostBatLui));
    }
    IEnumerator NgungBatLui(float time, float doLon)
    {
        float timeWait = 0.1f;
        float a = Mathf.Abs(doLon / time) * timeWait;
        if (boostBatLui > 0f) a *= -1f;
        while (boostBatLui != 0f)
        {
            yield return new WaitForSeconds(timeWait);
            boostBatLui += a;
            if (a * boostBatLui > 0f) boostBatLui = 0f;
        }
        
    }
    IEnumerator BodyVelocityZero(Rigidbody2D bd, float time)
    {
        yield return new WaitForSeconds(time);
        bd.velocity = Vector2.zero;
    }
    public void StartAttack()
    {
        XoayVeDoiPhuong();
        isAttack = true;
        boostAttack = 0f;
        if (isAura) Aura(false);
        if (isDefense) Defense(false);
        if (isDash) Dash(false);
    }
    public void EndAttack()
    {
        csnv.ResetDame();
        isAttack = false;
        isNotSetHurt = false;
        boostAttack = 0f;
        lucHatTung = 0f;
        lucKnockOut = 0f;
        dungYenTrenKhong = false;
        if (isWin)
        {
            EndHurt();
            controlAnimator.Win();
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            performance = true;
            viTriHitting = Vector2.zero;
        }
    }
    public void StartHurt(float huongTanCong, float _lucKnockOut, float _lucHatTung, bool nmat)
    {
        if (!isNotSetHurt)
        {
            if (_lucKnockOut == 0f) _lucKnockOut = 8f;
            Hurt(_lucHatTung, nmat);
            controlAnimator.Hurt();
            if (_lucKnockOut >= 50f)
            {
                QuanLiAmThanh.Instance.PlayTelePunch();
                PoolVfx.instance.KhoiKnockOut(transform);
                RungCameraSingleton.Instance.Shake(0.35f, 13f, 1f);
            }
            else
            {
                RungCameraSingleton.Instance.Shake(0.15f, 5f, 1f);
            }
            BatLui(huongTanCong, _lucKnockOut, 0.5f, false);
        }
    }
    public void Hurt(float _lucHatTung, bool nmat)
    {
        dungYenTrenKhong = false;
        isHurt = true;
        performance = false;
        /***/
        isAura = false;
        //RungCameraSingleton.Instance.StopShake();
        QuanLiAmThanh.Instance.StopAura();
        /*****/
        Defense(false);
        Aura(false);
        Dash(false);
        EndAttack();
        OnGround();
        QuanLiAmThanh.Instance.PlayHit();
        if (nmat) SlowMotionHit.Instance.SlowHit();
        csnv.PlusNangLuong(0.0075f);
        if (_lucHatTung != 0f)
        {
            rb.AddForce(new Vector2(0f, _lucHatTung), ForceMode2D.Impulse);
        }
    }
    public void EndHurt()
    {
        isHurt = false;
    }
    //public void StartKnockOut(float huongTanCong, float lucBatLui, float _lucHatTung, bool nmat)
    //{
    //    if (!isNotSetHurt)
    //    {
    //        RungCameraSingleton.Instance.Shake(0.35f, 10f, 1f);
    //        isKnockout = true;
    //        Hurt(_lucHatTung, nmat);
    //        //controlAnimator.KnockOut();
    //        controlAnimator.Hurt();
    //        BatLui(huongTanCong, lucBatLui, 1f, true);
    //    }
    //}
    public void EndKnockOut()
    {
        isKnockout = false;
        EndHurt();
    }
    public void Death(float flip)
    {
        if (isAlive)
        {
            isAlive = false;
            controlAnimator.Death();
            SlowMotionHit.Instance.SlowKO();
            rb.velocity = Vector2.zero;
            //bd.AddForce(new Vector2(10f * flip, 30f), ForceMode2D.Impulse);
            //StartCoroutine(BodyVelocityZero(bd, 0.75f));
        }
    }
    public bool CanMove()
    {
        return !isAttack && !isAura && !isHurt && !isDefense && !isKnockout && !dungYenTrenKhong;
    }
    public bool CanAura()
    {
        return !isAttack && !isHurt && !isDefense && !isKnockout;
    }
    public bool CanDash()
    {
        return !isAttack && !isAura && !isHurt && !isDefense && !isKnockout;
    }
    public bool CanJump()
    {
        return !isAttack && !isAura && !isHurt && !isDefense && !isKnockout;
    }
    public bool CanAttack()
    {
        return !isAttack && !isHurt && !isKnockout && !isDefense;
    }
    public bool CanDefense()
    {
        return !isAttack && !isAura && !isHurt && !isKnockout;
    }
}
