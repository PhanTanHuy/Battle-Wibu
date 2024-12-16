using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMove : MonoBehaviour
{
    [HideInInspector] public Transform target, sanDau;
    private SpriteRenderer spriteShadow, spriteMain;
    CapsuleCollider2D cs;
    // Update is called once per frame
    private void Start()
    {
        spriteMain = target.GetComponent<SpriteRenderer>();
        spriteShadow = GetComponent<SpriteRenderer>();
        transform.position = target.position;
        cs = target.GetComponent<CapsuleCollider2D>();
    }
    public void SetTargetShadow(Transform tg, Transform sd)
    {
        target = tg;
        sanDau = sd;
    }
    void Update()
    {
        if (target != null)
        {
            spriteShadow.sprite = spriteMain.sprite;
            transform.position = new Vector2(target.position.x, sanDau.position.y - (target.transform.position - (cs.bounds.center - new Vector3(0f, cs.bounds.extents.y, 0f))).y);
            transform.localScale = new Vector2(target.transform.localScale.x, -1f);
        }
        else Destroy(gameObject);
    }
}
