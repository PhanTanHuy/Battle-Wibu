using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChiSo : MonoBehaviour
{
    [SerializeField] private float dame;
    [HideInInspector] public string tenNguoiSoHuu;

    public float Dame
    {
        get { return dame; } set { dame = value; }
    }
}
