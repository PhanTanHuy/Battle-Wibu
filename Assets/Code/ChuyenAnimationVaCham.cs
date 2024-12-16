using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChuyenAnimationVaCham : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<ChiSoNhanVat>() == null)
        {
            GetComponentInParent<Animator>().Play("no");
            return;
        }
        if (collision.GetComponent<ChiSoNhanVat>().TenNhanVat != GetComponentInParent<ChiSo>().tenNguoiSoHuu) GetComponentInParent<Animator>().Play("no");
    }
}
