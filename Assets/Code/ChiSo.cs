using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChiSo : MonoBehaviour
{
    [SerializeField] private float dame, batLui, hatTung;
    [HideInInspector] public string tenNguoiSoHuu;

    public float Dame
    {
        get { return dame; } set { dame = value; }
    }
    public float BatLui
    {
        get { return batLui; }
        set { batLui = value; }
    }
    public float HatTung
    {
        get { return hatTung; }
        set { hatTung = value; }
    }
    public void SetChiSo(float d, float bl, float ht)
    {
        dame = d;
        batLui = bl;
        ht = hatTung;
    }
}
