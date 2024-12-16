using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnPool : MonoBehaviour
{
    public void DeactiveObject()
    {
        gameObject.SetActive(false);
    }
    public void DeactiveObjectParent()
    {
        transform.parent.gameObject.SetActive(false);
    }
}
