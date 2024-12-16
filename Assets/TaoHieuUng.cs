using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaoHieuUng : MonoBehaviour
{
    private CapsuleCollider2D cs;
    public Color mauGhostEffect;
    private SpriteRenderer spriteRenderer;
    private void Start()
    {
        cs = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void DashSmoke()
    {
        QuanLiAmThanh.Instance.PlayDash();
        PoolVfx.instance.CreateDashSmoke(new Vector2(cs.bounds.center.x, cs.bounds.center.y - cs.bounds.extents.y), transform.localScale);
    }
    public void GhostEffect()
    {
        PoolVfx.instance.CreateGhostEffect(transform.position, spriteRenderer.sprite, mauGhostEffect, transform.localScale);
    }
    public void Tele()
    {
        PoolVfx.instance.CreateTele(cs.bounds.center);
    }
    public void DatVo()
    {
        PoolVfx.instance.CreateDatVo(new Vector2(cs.bounds.center.x, cs.bounds.center.y - cs.bounds.extents.y));
    }
}
