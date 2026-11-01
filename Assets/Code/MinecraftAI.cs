
using System.Collections.Generic;
using UnityEngine;

public class MinecraftAI : MonoBehaviour
{
    [Header("AI Settings")]
    [SerializeField] private LayerMask lop;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private float stopDistance = 2.75f;
    [SerializeField] private float attackCooldown = 0.8f;
    [SerializeField] private float comboResetTime = 1.5f;

    [HideInInspector] public Transform player;

    private ChiSoNhanVat csnv;
    private TrangThai trangThai;
    private Rigidbody2D rb;

    private readonly List<string> combos = new List<string>();

    private int currentCombo;
    private float attackTimer;
    private float comboTimer;

    private void Start()
    {
        csnv = GetComponent<ChiSoNhanVat>();
        trangThai = GetComponent<TrangThai>();
        rb = GetComponent<Rigidbody2D>();
        //player = QuanLiCharacter.Instance.GetTransFormEnemy(csnv.TenNhanVat);

        // Chỉ thêm các đòn AttackJ được nhân vật hỗ trợ.
        for (int i = 0; i < csnv.ChuoiCombo; i++)
        {
            combos.Add(stringKey.Attack.AttackJ + (i + 1));
        }

        currentCombo = 0;
        attackTimer = 0f;
        comboTimer = 0f;

        if (combos.Count == 0)
        {
            Debug.LogWarning($"{name}: Nhân vật không có combo AttackJ!");
        }
    }
    private void Update()
    {
        if (!trangThai.IsAlive || trangThai.Performance)
            return;

        if (player == null)
            return;

        Vector2 distance = player.position - transform.position;


        // AI chỉ di chuyển và tấn công.
        Move(distance.x);
        Attack(distance.x);

        attackTimer -= Time.deltaTime;
        comboTimer += Time.deltaTime;

        // Quá lâu không tiếp tục combo thì quay về đòn đầu.
        if (comboTimer > comboResetTime)
        {
            currentCombo = 0;
        }
    }

    private void Move(float distanceX)
    {
        float velocityX = 0f;

        bool canMove = trangThai.CanMove();

        if (canMove && Mathf.Abs(distanceX) > stopDistance)
        {
            velocityX = Mathf.Sign(distanceX);
        }

        // Không tự ý thay đổi vận tốc Y, để hệ thống vật lý xử lý.
        rb.linearVelocity = new Vector2(
            velocityX * csnv.TocDo,
            rb.linearVelocity.y
        );

        trangThai.Run(velocityX != 0f && rb.linearVelocity.y == 0f);
        Flip(distanceX);
    }

    private void Attack(float distanceX)
    {
        // Không tấn công nếu ngoài tầm hoặc đang trong thời gian hồi.
        if (Mathf.Abs(distanceX) > attackRange)
            return;

        if (attackTimer > 0f)
            return;

        if (trangThai.IsAttack || !trangThai.CanAttack())
            return;

        if (combos.Count == 0)
            return;

        // Chọn đòn tiếp theo trong chuỗi combo hợp lệ.
        string attackName = combos[currentCombo];

        // Chỉ tăng combo nếu TrangThai chấp nhận đòn đánh.
        if (trangThai.Attack(attackName))
        {
            currentCombo++;

            if (currentCombo >= combos.Count)
            {
                currentCombo = 0;
            }

            attackTimer = attackCooldown;
            comboTimer = 0f;
        }
    }

    private void Flip(float distanceX)
    {
        Vector3 scale = transform.localScale;

        scale.x = Mathf.Abs(scale.x) * (distanceX >= 0f ? 1f : -1f);

        transform.localScale = scale;
    }

    private void OnDisable()
    {
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }
    }
}
