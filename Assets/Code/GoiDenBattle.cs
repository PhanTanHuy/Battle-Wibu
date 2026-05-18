using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoiDenBattle : MonoBehaviour
{
   public void nextRound()
    {
        Battle.Instance.SetCharacterNextGround();
    }
}
