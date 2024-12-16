using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlAnimator : MonoBehaviour
{
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("xuatHien");
    }
    public void Death()
    {
        animator.Play("death");
        Debug.Log("death");
    }
    public void Run(bool value)
    {
        animator.SetBool("isRun", value);
    }
    public void Jump(bool value)
    {
        animator.SetBool("isJump", value);
    }
    public void Fall(bool value)
    {
        animator.SetBool("isFall", value);
    }    
    //public void AttackJ(ref int i)
    //{
    //    if (i > attackName.Length - 1) i = 0;
    //    animator.Play(attackName[i]);
    //}
    public void Win()
    {
        animator.Play("Win");
    }
    public void AttackWithName(string name)
    {
        animator.Play(name);
    }
    public void AttackWU()
    {
        animator.Play("atwu");
    }
    public void AttackU()
    {
        animator.Play("atu");
    }
    public void AttackI()
    {
        animator.Play("ati");
    }
    public void Hurt()
    {
        animator.Play("hurt");
    }
    public void KnockOut()
    {
        animator.Play("knockOut");
    }
    public void Defense(bool value)
    {
        if (value)
        {
            animator.Play("defense");
            animator.SetBool("isDefense", value);
        }
        else animator.SetBool("isDefense", value);
    }
    public void Aura(bool value)
    {
        if (value)
        {
            animator.Play("startAura");
            animator.SetBool("isAura", value);
        }
        else animator.SetBool("isAura", value);
    }
    public void Dash(bool isDash)
    {
        if (isDash) animator.Play("Dash");
        animator.SetBool("isDash", isDash);
    }
}
