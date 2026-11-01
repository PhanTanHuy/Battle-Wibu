using UnityEngine;

public class NhanSatThuong : MonoBehaviour
{
    private ControlAnimator ctra;
    private ChiSoNhanVat csnv;
    private TrangThai trangThai;

    private readonly Vector2 right = new Vector2(1f, 1f);
    private readonly Vector2 left = new Vector2(-1f, 1f);

    private void Start()
    {
        ctra = GetComponent<ControlAnimator>();
        csnv = GetComponent<ChiSoNhanVat>();
        trangThai = GetComponent<TrangThai>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (csnv.MauHienTai <= 0f) return;

        if (collision.CompareTag("boxAttack"))
        {
            HandleCharacterAttack(collision);
        }
        else if (collision.CompareTag("boxObjectAttack"))
        {
            HandleObjectAttack(collision);
        }
    }

    private void HandleCharacterAttack(Collider2D collision)
    {
        Transform attacker = collision.transform.parent;

        if (attacker == null || attacker == transform) return;
        if (collision.transform.root.CompareTag(tag)) return;

        ChiSoNhanVat attackerStats = attacker.GetComponentInParent<ChiSoNhanVat>();
        TrangThai attackerState = attacker.GetComponentInParent<TrangThai>();

        if (attackerStats == null || attackerState == null) return;

        Vector2 direction = attacker.localScale.x < 0f ? left : right;

        float damage = attackerStats.Dame;
        float knockOut = attackerState.LucKnockOut;
        float hatTung = attackerState.LucHatTung;

        PoolVfx.instance.CreateCollideEffect(
            transform.position,
            attackerStats.TenNhanVat,
            direction
        );

        trangThai.BatLui(direction.x, knockOut, 0.35f, hatTung);

        if (!trangThai.IsDefense)
        {
            trangThai.StartHurt();
            csnv.MauHienTai -= damage;
        }
        else
        {
            HandleDefense(damage);
        }

        attackerState.Hitting();

        CheckDeath(direction);
    }

    private void HandleObjectAttack(Collider2D collision)
    {
        ChiSo attackStats = collision.GetComponentInParent<ChiSo>();

        if (attackStats == null) return;
        if (attackStats.tenNguoiSoHuu == csnv.TenNhanVat) return;

        Vector2 direction = attackStats.transform.localScale.x < 0f
            ? left
            : right;

        float damage = attackStats.Dame;
        float knockOut = attackStats.BatLui > 0f ? attackStats.BatLui : 3f;
        float hatTung = attackStats.HatTung > 0f ? attackStats.HatTung : 0f;

        QuanLiAmThanh.Instance.PlayExPolison();

        trangThai.BatLui(direction.x, knockOut, 0.35f, hatTung);

        if (!trangThai.IsDefense)
        {
            trangThai.StartHurt();
            csnv.MauHienTai -= damage;

            PoolVfx.instance.CreateCollideEffect(
                transform.position,
                attackStats.tenNguoiSoHuu,
                direction
            );
        }
        else
        {
            HandleDefense(damage);
        }

        CheckDeath(direction);
    }

    private void HandleDefense(float damage)
    {
        RungCameraSingleton.Instance.Shake(0.1f, 4f, 1f);
        QuanLiAmThanh.Instance.PlayHit();

        csnv.PlusNangLuong(0.0075f);
        csnv.MauHienTai -= damage * 0.2f;
    }

    private void CheckDeath(Vector2 direction)
    {
        if (csnv.MauHienTai <= 0f)
        {
            trangThai.Death(direction.x);
        }
    }
}