using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LenCap : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController[] runtimeAnims;
    private int currentCap = 0;
    private Animator currentAnimator;
    private void Start()
    {
        currentAnimator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        ChiSoNhanVat.OnNangLuongDay += LenCapTiepTheo;
    }

    private void OnDisable()
    {
        ChiSoNhanVat.OnNangLuongDay -= LenCapTiepTheo;
    }
    public void LenCapTiepTheo(ChiSoNhanVat chiSo)
    {
        Debug.Log("Len cap tiep theo");
        if (currentCap >= runtimeAnims.Length) return;
        currentAnimator.runtimeAnimatorController = runtimeAnims[currentCap];
        currentCap++;
    }
}
