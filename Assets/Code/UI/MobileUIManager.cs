using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileUIManager : MonoBehaviour
{
    public static MobileUIManager Instance;
    [SerializeField] private GameObject ButtonLenCap;
    private void Awake()
    {
        Instance = this;
    }
    public void ShowNutLenCap(bool show)
    {
        if (show)
        {
            ButtonLenCap.SetActive(true);
        }
        else
        {
            ButtonLenCap.SetActive(false);
        }
    }
}
