using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HienThiTournament : MonoBehaviour
{
    private void TatTournament()
    {
        gameObject.SetActive(false);
        QuanLiCharacter.Instance.ActiveCharacter();
    }
    public void AnNutTatTournament()
    {
        GetComponent<Animator>().Play("EndTournament");
    }
}
