using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrangThai : MonoBehaviour
{
    private bool isAttack;
    private bool performance;
    private bool isDefense;
    private bool isHurt;
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
    private Coroutine batLuiCorou;
    private float currentComboTimer;
    int mapLayer;

    private void Start()
    {
        performance = true;
        
        boostAttack = lucKnockOut = 0f;
        controlAnimator = GetComponent<ControlAnimator>();
        thu = GetComponent<TaoHieuUng>();
        csnv = GetComponent<ChiSoNhanVat>();
        rb = GetComponent<Rigidbody2D>();
        viTriHitting = Vector2.zero;
        mapLayer = LayerMask.GetMask("Map");
        doiPhuong = QuanLiCharacter.Instance.TFDoiThuGanNhat(csnv.TenNhanVat);
    }
    private void OnEnable()
    {
        isAttack = false;
        isDefense = false;
        isHurt = false;
        isAura = false;
        isDash = false;
        justHitting = 0f;
        dungYenTrenKhong = false;
        isWin = false;
        isAlive = true;
    }
    public void KetThucManXuatHien()
    {
        if (!QuanLiCharacter.Instance.XuatHienXongRoi()) performance = false;
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
        QuanLiCharacter.Instance.SkillDacBiet(csnv.TenNhanVat, time);
    }
    public void DungIm(float time)
    {
        performance = true;
        viTriHitting = transform.position;
        EndAttack();
        rb.linearVelocity = Vector2.zero;
        StartCoroutine(HetDungIm(time));
        GetComponent<Animator>().speed = 0f;
        if (TryGetComponent<ICharacterSpecial>(out ICharacterSpecial characterSpecial))
        {
            characterSpecial.HandleDungIm(time);
        }
    }
    IEnumerator HetDungIm(float time)
    {
        yield return new WaitForSeconds(time);
        rb.linearVelocity = Vector2.zero;
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
                BoostAttacking(csnv.DashBoost);
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
        if (performance) return;

        if (_isDefense && !isDefense)
        {
            isDefense = true;
            controlAnimator.Defense(isDefense);

            if (isHurt)
            {
                bool hitLeft = Physics.Raycast(
                    transform.position,
                    Vector3.left,
                    2f,
                    mapLayer
                );

                bool hitRight = Physics.Raycast(
                    transform.position,
                    Vector3.right,
                    2f,
                    mapLayer
                );

                if (hitLeft || hitRight)
                    DichChuyenToiDoiPhuong();

                EndHurt();
            }

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
            controlAnimator.Aura(isAura);
            QuanLiAmThanh.Instance.StopAura();
        }
        if (isAura)
        {
            csnv.PlusNangLuong();
            rb.linearVelocity = Vector2.zero;
        }

    }
    public void DichChuyenToiDoiPhuong()
    {
        QuanLiAmThanh.Instance.PlayTele();
        StartCoroutine(DichChuyenIE());
    }
    public void DichChuyenToiDoiPhuongHeight(float y)
    {
        QuanLiAmThanh.Instance.PlayTele();
        StartCoroutine(DichChuyenHeightIE(y));
    }
    IEnumerator DichChuyenIE()
    {
        float khoangCach = doiPhuong.position.x - transform.position.x;
        thu.Tele();
        transform.position = doiPhuong.position + new Vector3(Mathf.Sign(khoangCach) * 1f, 0f, 0f);
        yield return null;
        XoayVeDoiPhuong();
        thu.Tele();
    }
    IEnumerator DichChuyenHeightIE(float y)
    {
        thu.Tele();
        transform.position += new Vector3(0f, y, 0f);
        yield return null;
        thu.Tele();
    }
    public void LuotToiDoiPhuong(float time)
    {
        float khoangCach = doiPhuong.position.y - transform.position.y;

        if (Mathf.Abs(khoangCach) > 1f)
            DichChuyenToiDoiPhuongHeight(khoangCach);

        float tocDoLuot = Mathf.Abs(XoayVeDoiPhuong() / time);

        boostAttack = XoayVeDoiPhuong() / time;

        StartCoroutine(SetBoosAttackIE(0f, time));
        StartCoroutine(GhostEffectIE(tocDoLuot, time));
    }
    private IEnumerator GhostEffectIE(float tocDoLuot, float time)
    {
        // Tốc độ càng cao thì khoảng cách giữa các Ghost càng nhỏ
        float interval = Mathf.Lerp(
            0.15f,
            0.02f,
            Mathf.InverseLerp(2f, 20f, tocDoLuot)
        );

        float elapsed = 0f;

        while (elapsed < time)
        {
            thu.GhostEffect();

            yield return new WaitForSeconds(interval);

            elapsed += interval;
        }
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
    public bool Attack(string nameAttackAnim, ref int currentCombo)
    {
        if (isDash || isAttack) return false;
        currentCombo++;
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
        if (csnv.NangLuongHienTai >= nangLuongTieuHao)
        {
            if (nangLuongTieuHao != 0) isNotSetHurt = true;
            StartAttack();
            controlAnimator.AttackWithName(nameAttackAnim);
            csnv.NangLuongHienTai -= nangLuongTieuHao;
            Battle.Instance.CapNhatThanhNangLuong(csnv.TenNhanVat, csnv.NangLuongHienTai / 100f);
            return true;
        }
        else return false;
    }
    public bool Attack(string nameAttackAnim)
    {
        if (isDash || isAttack) return false;
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
        if (csnv.NangLuongHienTai >= nangLuongTieuHao)
        {
            if (nangLuongTieuHao != 0) isNotSetHurt = true;
            StartAttack();
            controlAnimator.AttackWithName(nameAttackAnim);
            csnv.DungNangLuong(nangLuongTieuHao);
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
    public void BatLui(float flip, float bl, float time, float _lucHatTung)
    {
        if (_lucHatTung != 0f)
        {
            justHitting = -1f;
        }
        if (bl == 0f) bl = 3f;
        else if (bl > 20f)
        {
            QuanLiAmThanh.Instance.PlayTelePunch();
            PoolVfx.instance.KhoiKnockOut(transform);
            RungCameraSingleton.Instance.Shake(0.3f, 20f, 2f);
        }
        else
        {
            RungCameraSingleton.Instance.Shake(0.15f, 5f, 1f);
        }
        float maxForce = Mathf.Max(Mathf.Abs(_lucHatTung), bl);

        if (maxForce > 20f)
            SlowMotionHit.Instance.SlowHitNang();
        else if (maxForce > 10f)
            SlowMotionHit.Instance.SlowHitVua();
        else
            SlowMotionHit.Instance.SlowHitNhe();
        boostBatLui = bl * flip;

        if (_lucHatTung != 0f)
        {
            rb.AddForce(new Vector2(0f, 25f), ForceMode2D.Impulse);
        }
        if (batLuiCorou != null) StopCoroutine(batLuiCorou);
        batLuiCorou = StartCoroutine(NgungBatLui(time, boostBatLui));
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
        bd.linearVelocity = Vector2.zero;
    }
    public void StartAttack()
    {
        if (currentComboTimer != 0) currentComboTimer = 0f;
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
        currentComboTimer = 0f;
        dungYenTrenKhong = false;
        if (isWin)
        {
            EndHurt();
            controlAnimator.Win();
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            performance = true;
            viTriHitting = Vector2.zero;
        }
    }
    public void StartHurt()
    {
        if (!isNotSetHurt)
        {
            Hurt();
            controlAnimator.Hurt();
        }
        if (rb.linearVelocity.y < -1f)
        {
            justHitting = 0.35f;
            viTriHitting = transform.position;
        }
    }
    public bool CapNhatCurrentComboTimerForPlayer()
    {
        if (isAttack) return false;
        currentComboTimer += Time.deltaTime;
        if (currentComboTimer > 1f) return true;
        else return false;
               
    }
    public void Hurt()
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
        csnv.PlusNangLuong(0.0075f);
    }
    public void EndHurt()
    {
        isHurt = false;
    }
    public void Death(float flip)
    {
        if (isAlive)
        {
            isAlive = false;
            controlAnimator.Death();
            rb.linearVelocity = Vector2.zero;
            if (TryGetComponent<ICharacterSpecial>(out ICharacterSpecial characterSpecial))
            {
                characterSpecial.HandleDeath();
            }
            //bd.AddForce(new Vector2(10f * flip, 30f), ForceMode2D.Impulse);
            //StartCoroutine(BodyVelocityZero(bd, 0.75f));
        }
    }
    public bool CanMove()
    {
        return !isAttack && !isAura && !isHurt && !isDefense && !dungYenTrenKhong;
    }
    public bool CanAura()
    {
        return !isAttack && !isHurt && !isDefense;
    }
    public bool CanDash()
    {
        return !isAttack && !isAura && !isHurt && !isDefense;
    }
    public bool CanJump()
    {
        return !isAttack && !isAura && !isHurt && !isDefense;
    }
    public bool CanAttack()
    {
        return !isAttack && !isHurt && !isDefense;
    }
    public bool CanDefense()
    {
        return !isAttack && !isAura;
    }
}
public interface ICharacterSpecial
{
    public void HandleDungIm(float time);
    public void HandleDeath();
}
