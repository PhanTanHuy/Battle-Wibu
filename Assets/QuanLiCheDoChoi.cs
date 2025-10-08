using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuanLiCheDoChoi : MonoBehaviour
{
    public static QuanLiCheDoChoi Instance;
    [HideInInspector] public int soRounds;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            soRounds = 1;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    public enum FightMode
    {
        PVP,
        PVE,
        EVE,
        Training,
        Tournament
    }
    public void TangGiamRounds(bool tang)
    {
        if (tang)
        {
            soRounds += 2;
            if (soRounds > 5) soRounds = 5;
        }
        else
        {
            soRounds -= 2;
            if (soRounds < 1) soRounds = 1;
        }
    }
    public FightMode fightMode;
}
