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
    public void LenCapTiepTheo()
    {
        if (currentCap >= runtimeAnims.Length) return;
        currentAnimator.runtimeAnimatorController = runtimeAnims[currentCap];
        currentCap++;
    }
}
