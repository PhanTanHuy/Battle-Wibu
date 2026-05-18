using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDienThoai : MonoBehaviour
{
    public static InputDienThoai Instance;
    [HideInInspector] public bool RightButtonPressed, LeftButtonPressed, RightButtonDown, RightButtonUp, LeftButtonDown, LeftButtonUp, UpButtonPressed, DownButtonPressed, TanCongPressed, PhongThuPressed, NhayPressed, GongPressed;
    private void Awake()
    {
        if (Instance != null) Destroy(Instance.gameObject);
        Instance = this;
        RightButtonPressed = LeftButtonPressed = UpButtonPressed = DownButtonPressed = TanCongPressed = PhongThuPressed = NhayPressed = GongPressed = RightButtonDown = LeftButtonDown = RightButtonUp = LeftButtonUp = false;
        DontDestroyOnLoad(gameObject);
    }
    

}
