using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileUIManager : MonoBehaviour
{
    public static MobileUIManager Instance;
    [SerializeField] private GameObject ButtonLenCap, ButtonPhanThan;
    private void Awake()
    {
        Instance = this;
    }
    public void ShowNutLenCap(bool show)
    {
        ButtonLenCap.SetActive(show);
    }
    public void ShowNutPhanThan(bool show)
    {
        ButtonPhanThan.SetActive(show);
    }
}
