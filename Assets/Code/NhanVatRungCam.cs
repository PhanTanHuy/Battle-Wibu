using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NhanVatRungCam : MonoBehaviour
{
    public void AuraShakeCam()
    {
        if (GetComponent<TrangThai>().IsAura)
        {
            RungCameraSingleton.Instance.AuraShake();
            QuanLiAmThanh.Instance.PlayStartAura();
        }
    }
    public void Shake(string chuoiFloat)
    {
        string[] values = chuoiFloat.Split(',');
        if (float.TryParse(values[0], out float time) &&
            float.TryParse(values[1], out float doManh) &&
            float.TryParse(values[2], out float tanSo))
        {
            RungCameraSingleton.Instance.Shake(time, doManh, tanSo);
        }
    }
}
