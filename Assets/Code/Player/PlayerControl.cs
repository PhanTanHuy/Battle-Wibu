using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] LayerMask lop;
    //
    private ChiSoNhanVat csnv;
    private Rigidbody2D rb;
    private CapsuleCollider2D cs;
    private TaoHieuUng thu;
    //private ControlAnimator ctra;
    private TrangThai trangThai;
    private Vector2 right, left;
    private int currentCombo;
    private float currentComboTimer;
    private bool isGrounded;
    private int doubleMove;
    private float doubleMoveTimer;
    private string[] nameAttackCombo;
    
    void Start()
    {
        csnv = GetComponent<ChiSoNhanVat>();
        rb = GetComponent<Rigidbody2D>();
        cs = GetComponent<CapsuleCollider2D>();
        //ctra = GetComponent<ControlAnimator>();
        trangThai = GetComponent<TrangThai>();
        thu = GetComponent<TaoHieuUng>();
        right = new Vector2(1f, 1f);
        left = new Vector2(-1f, 1f);
        currentCombo = 0;
        currentComboTimer = 0f;
        doubleMove = 0;
        doubleMoveTimer = 0f;
        nameAttackCombo = new string[csnv.ChuoiCombo];
        for (int i=0; i<csnv.ChuoiCombo; i++)
        {
            nameAttackCombo[i] = stringKey.Attack.AttackJ + (i + 1);
        }
        //
       
    }

    // Update is called once per frame
    void Update()
    {
        if (trangThai.IsAlive && !trangThai.Performance)
        {
            Move(trangThai.CanMove());
            if (trangThai.CanJump()) Jump();
            if (trangThai.CanAttack()) Attack();
            if (trangThai.CanDefense()) Defense();
            if (trangThai.CanDash()) dash();
            if (trangThai.CanAura()) Aura();
            if (trangThai.JustHitting > 0f || trangThai.DungYenTrenKhong)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f);
                transform.position = new Vector2(transform.position.x, trangThai.viTriHitting.y);
            }
            currentComboTimer += Time.deltaTime;
            trangThai.JustHitting -= Time.deltaTime;
            doubleMoveTimer += Time.deltaTime;
            if (doubleMoveTimer > 0.2f && doubleMove < 2) doubleMove = 0;
        }
        else if (trangThai.Performance && trangThai.viTriHitting != Vector2.zero)
        {
            transform.position = trangThai.viTriHitting;
        }
    }
    private void dash()
    {
        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A)) && doubleMove < 2)
        {
            doubleMove++;
            doubleMoveTimer = 0f;
        }
        if (doubleMove >= 2 && (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A)))
        {
            doubleMove = 0;
        }
        if (doubleMove >= 2)
        {
            trangThai.Dash(true);
        }
        else if (doubleMove == 0)
        {
            trangThai.Dash(false);
        }
    }
    private void Move(bool canMove)
    {
        float veloX = 0f;
        if (canMove)
        {
            if (Input.GetKey(KeyCode.D))
            {
                veloX = 1f;
                transform.localScale = right;
            }
            if (Input.GetKey(KeyCode.A))
            {
                veloX = -1f;
                transform.localScale = left;
            }
        }
        rb.velocity = new Vector2(veloX * csnv.TocDo + trangThai.BoostAttack + trangThai.BoostBatLui, rb.velocity.y);
        trangThai.Run(veloX != 0 && rb.velocity.y == 0f);
    }
    private void Jump()
    {
        bool trenDat = IsOnGround();
        if (Input.GetKeyDown(KeyCode.Space) && trenDat)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0f);
            rb.AddForce(new Vector2(0f, csnv.LucNhay), ForceMode2D.Impulse);
            trangThai.Jump();
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

    private void Attack()
    {
        if (Input.GetKey(KeyCode.J))
        {
            if (currentCombo > csnv.ChuoiCombo - 1) currentCombo = 0;
            trangThai.Attack(nameAttackCombo[currentCombo]);
            currentCombo++;
            currentComboTimer = 0f;
        }
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.U))
        {
            trangThai.Attack(stringKey.Attack.AttackWU);
        }
        else if (Input.GetKey(KeyCode.U))
        {
            trangThai.Attack(stringKey.Attack.AttackU);
        }
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.I))
        {
            trangThai.Attack(stringKey.Attack.AttackWI);
        }
        else if (Input.GetKey(KeyCode.I))
        {
            trangThai.Attack(stringKey.Attack.AttackI);
        }
        if (currentComboTimer > 0.75f) currentCombo = 0;
    }
    private void Defense()
    {
        trangThai.Defense(Input.GetKey(KeyCode.S));
    }
    private void Aura()
    {
        trangThai.Aura(Input.GetKey(KeyCode.K));
    }
}
