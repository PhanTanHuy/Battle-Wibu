using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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

        // Khởi tạo các biến riêng biệt
        float time = 0f;
        float doManh = 0f;
        float tanSo = 0f;

        // Dùng CultureInfo.InvariantCulture để ép kiểu chính xác trên mọi nền tảng
        bool isValidTime = float.TryParse(values[0], NumberStyles.Float, CultureInfo.InvariantCulture, out time);
        bool isValidDoManh = float.TryParse(values[1], NumberStyles.Float, CultureInfo.InvariantCulture, out doManh);
        bool isValidTanSo = float.TryParse(values[2], NumberStyles.Float, CultureInfo.InvariantCulture, out tanSo);

        // Kiểm tra nếu tất cả các giá trị hợp lệ
        if (isValidTime && isValidDoManh && isValidTanSo)
        {
            // Truyền các biến đã được kiểm tra vào phương thức Shake
            RungCameraSingleton.Instance.Shake(time, doManh, tanSo);
        }
        else
        {
            Debug.LogWarning("Giá trị chuỗi không hợp lệ");
        }
    }

}
