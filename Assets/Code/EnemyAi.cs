using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] LayerMask lop;
    private Transform player;
    public bool canAttack;
    private List<string> combos;
    private string[] skills = stringKey.Attack.Skills;
    //
    private ChiSoNhanVat csnv;
    private Rigidbody2D rb;
    private TrangThai trangThai;
    private CapsuleCollider2D cs;
    private TaoHieuUng thu;
    private Vector2 right, left, direction, distanceFromPlayer;
    private int currentCombo;
    private bool isGrounded;
    private float currentComboTimer, jumpTimer, moveTimer, _moveTimer, attackTimer, auraTimer, timeAura, dashTimer, timeDash, defenseTimer, timeDefense;
    void Start()
    {
        csnv = GetComponent<ChiSoNhanVat>();
        rb = GetComponent<Rigidbody2D>();
        trangThai = GetComponent<TrangThai>();
        cs = GetComponent<CapsuleCollider2D>();
        thu = GetComponent<TaoHieuUng>();
        player = QuanLiCharacter.Instance.GetTransFormEnemy(csnv.TenNhanVat);
        combos = new List<string>();
        for (int i = 0; i < csnv.ChuoiCombo; i++)
        {
            combos.Add(stringKey.Attack.AttackJ + (i + 1));
        }
        for (int i=0; i < skills.Length; i++)
        {
            combos.Add(skills[i]);
        }
        right = new Vector2(1f, 1f);
        left = new Vector2(-1f, 1f);
        currentCombo = 0;
        currentComboTimer = Random.Range(0f, 5f);
        attackTimer = 0f;
        auraTimer = Random.Range(5f, 15f);
        dashTimer = Random.Range(0f, 5f);
        defenseTimer = Random.Range(5f, 15f);
        timeDefense = 0f;
        jumpTimer = Random.Range(0f, 5f);

        //
     
        Flip();
    }

    // Update is called once per frame
    void Update()
    {
        if (trangThai.IsAlive && !trangThai.Performance)
        {
            if (!canAttack && !trangThai.IsHurt) csnv.MauHienTai += csnv.Mau * 0.5f * Time.deltaTime;
            distanceFromPlayer = player.position - transform.position;
            Move(trangThai.CanMove() && timeAura < 0f);
            if (trangThai.IsHurt)
            {
                jumpTimer = Random.Range(2f, 4f);
                auraTimer = Random.Range(1f, 2f);
                dashTimer = Random.Range(1f, 2f);
                attackTimer = 0.3f;
            }
            if (trangThai.CanJump()) Jump();
            Defense();
            if (trangThai.CanDash() && canAttack) dash();
            if (trangThai.CanAura() && canAttack) Aura();
            if (trangThai.CanAttack() && timeAura < 0f && canAttack && (Mathf.Abs(distanceFromPlayer.x) < 5f || currentCombo != 0)) Attack();

            if (trangThai.JustHitting > 0f || trangThai.DungYenTrenKhong)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                transform.position = new Vector2(transform.position.x, trangThai.viTriHitting.y);
            }

            currentComboTimer += Time.deltaTime;
            if (Mathf.Abs(distanceFromPlayer.x) >= 3.5f) moveTimer -= Time.deltaTime;
            attackTimer -= Time.deltaTime;
            auraTimer -= Time.deltaTime;
            timeAura -= Time.deltaTime;
            dashTimer -= Time.deltaTime;
            defenseTimer -= Time.deltaTime;
            timeDefense -= Time.deltaTime;
            if (timeDefense <= 0f) attackTimer = -1f;
            jumpTimer -= Time.deltaTime;
            timeDash -= Time.deltaTime;
            trangThai.JustHitting -= Time.deltaTime;
        }
        else if (trangThai.Performance && trangThai.viTriHitting != Vector2.zero)
        {
            transform.position = trangThai.viTriHitting;
        }
    }
    private void dash()
    {
        if (dashTimer < 0f || (!isGrounded && dashTimer < 0.2f))
        {
            if (Mathf.Abs(distanceFromPlayer.x) < 3f) transform.localScale = new Vector2(-transform.localScale.x, 1f);
            trangThai.Dash(true);
            timeDash = Random.Range(0.25f, 1f);
            dashTimer = Random.Range(1.5f, 2.5f);
            rb.AddForce(new Vector2(0f, csnv.LucNhay), ForceMode2D.Impulse);
        }
        if (timeDash < 0f)
        {
            trangThai.Dash(false);
        }
    }
    private void Flip()
    {
        if (trangThai.IsDash) return;
        direction = player.position.x - transform.position.x > 0f ? right : left;
        transform.localScale = direction;
    }
    private void Move(bool canMove)
    {
        float velo = 0f;
        if (canMove)
        {
            if (moveTimer < _moveTimer)
            {
                moveTimer = Random.Range(0.75f, 2f);
                _moveTimer = Random.Range(-2.25f, -1.5f);
            }
            else if (moveTimer > 0f) velo = direction.x;
            if (!(trangThai.BoostAttack > 0f || !isGrounded))
            Flip();
        }
        else velo = 0f;
        if (Mathf.Abs(distanceFromPlayer.x) < 2.75f)
        {
            velo = 0f;
            moveTimer = 0f;
        }
        if (!isGrounded && !trangThai.IsAttack && !trangThai.IsDefense && !trangThai.IsHurt) velo = transform.localScale.x;
        rb.velocity = new Vector2(velo * csnv.TocDo + trangThai.BoostAttack + trangThai.BoostBatLui, rb.velocity.y);
        trangThai.Run(velo != 0 && rb.velocity.y == 0f);
    }
    private void Jump()
    {
        bool trenDat = IsOnGround();
        if (jumpTimer < 0f && trenDat && canAttack && distanceFromPlayer.y > 1.5f)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(new Vector2(0f, csnv.LucNhay), ForceMode2D.Impulse);
            trangThai.Jump();
            jumpTimer = Random.Range(1f, 2f);
            QuanLiAmThanh.Instance.PlayJump();
            isGrounded = false;
        }
        if (!trenDat && rb.velocity.y < 0f)
        {
            trangThai.Fall();
            isGrounded = false;
        }
        if (trenDat && rb.velocity.y == 0f && !isGrounded)
        {
            trangThai.OnGround();
            isGrounded = true;
            QuanLiAmThanh.Instance.PlayOnGround();
        }
    }
    private bool IsOnGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(cs.bounds.center, Vector2.down, cs.bounds.extents.y + 0.1f, lop);
        return hit.collider != null;
    }
    private string AttackString()
    {
        if (trangThai.IsHurt || trangThai.IsAura)
        {
            attackTimer = 0.3f;
        }
        if (attackTimer < 0f)
        {
            Flip();
            jumpTimer = Random.Range(0.5f, 1f);
            attackTimer = Random.Range(0f, 1.25f);
            int rd;
            if (Random.Range(0f, 1f) < 0.3f) rd = Random.Range(0, combos.Count);
            else rd = Random.Range(csnv.ChuoiCombo, combos.Count);
            string s = combos[rd];
            if (rd < csnv.ChuoiCombo || currentCombo != 0)
            {
                rd = currentCombo;
                s = combos[rd];
                currentCombo++;
                dashTimer = Random.Range(0.5f, 1.5f);
                currentComboTimer = 0f;
                if (currentCombo >= csnv.ChuoiCombo) currentCombo = 0;
                if (rd == csnv.ChuoiCombo - 1)
                {
                    attackTimer = Random.Range(0.75f, 1.25f);
                    return s;
                }
                
                attackTimer = -1f;
            }
            return s;
        }
        return "";
    }
    private void Attack()
    {
        string nameAttackAnimaiton = AttackString();
        if (nameAttackAnimaiton != "") if(!trangThai.IsAttack) if (!trangThai.Attack(nameAttackAnimaiton)) attackTimer = -1f;
        if (currentComboTimer > 0.75f) currentCombo = 0;
    }
    private bool canAura()
    {
        if (currentCombo != 0) return false;

        if (timeAura > 0f && isGrounded)
        {
            return true;
        }
        else
        {
            if (auraTimer < 0f)
            {
                float energyRatio = csnv.NangLuongHienTai / csnv.NangLuongToiDa;
                energyRatio = Mathf.Clamp01(energyRatio);
                float plusTimeAura = Mathf.Lerp(1f, 3f, energyRatio);
                timeAura = Random.Range(1f, 2f) / plusTimeAura;
                auraTimer = Random.Range(4f, 6f) * plusTimeAura;
            }
            return false;
        }
    }
    private bool canDefense()
    {
        if (timeDefense > 0f && Mathf.Abs(distanceFromPlayer.x) < 3f)
        {
            return true;
        }
        else
        {
            if (defenseTimer < 0f)
            {
                timeDefense = Random.Range(0.5f, 1.5f);
                defenseTimer = Random.Range(5f, 10f);
            }
            return false;
        }
    }
    private void Aura()
    {
        trangThai.Aura(canAura());
    }
    private void Defense()
    {
        //if (trangThai.IsHurt)
        //{
        //    timeDefense = Random.Range(0.5f, 1f);
        //}
        if (trangThai.CanDefense()) trangThai.Defense(canDefense());
    }
}
