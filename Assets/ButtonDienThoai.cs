using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonDienThoai : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum Type
    {
        Xuong,
        Len,
        Trai,
        Phai,
        TanCong,
        PhongThu,
        Gong,
        Nhay

    }
    public Type buttonType; 

    public void OnPointerDown(PointerEventData eventData)
    {
        NhanNut(buttonType);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        ThaNut(buttonType);
    }
    void ThaNut(Type type)
    {
        if (type == Type.Trai)
        {
            InputDienThoai.Instance.LeftButtonPressed = false;
            InputDienThoai.Instance.LeftButtonUp = true;
            StartCoroutine(FrameSauLaFalse("LeftUp"));
        }

        if (type == Type.Phai)
        {
            InputDienThoai.Instance.RightButtonPressed = false;
            InputDienThoai.Instance.RightButtonUp = true;
            StartCoroutine(FrameSauLaFalse("RightUp"));
        }

        if (type == Type.Len)
        {
            InputDienThoai.Instance.UpButtonPressed = false;
        }

        if (type == Type.Xuong)
        {
            InputDienThoai.Instance.DownButtonPressed = false;
        }

        if (type == Type.TanCong)
        {
            InputDienThoai.Instance.TanCongPressed = false;
        }

        if (type == Type.PhongThu)
        {
            InputDienThoai.Instance.PhongThuPressed = false;
        }

        if (type == Type.Gong)
        {
            InputDienThoai.Instance.GongPressed = false;
        }

        if (type == Type.Nhay)
        {
            InputDienThoai.Instance.NhayPressed = false;
        }
    }

    void NhanNut(Type type)
    {
        if (type == Type.Trai)
        {
            InputDienThoai.Instance.LeftButtonPressed = true;
            InputDienThoai.Instance.LeftButtonDown = true;
            StartCoroutine(FrameSauLaFalse("LeftDown"));
        }

        if (type == Type.Phai)
        {
            InputDienThoai.Instance.RightButtonPressed = true;
            InputDienThoai.Instance.RightButtonDown = true;
            StartCoroutine(FrameSauLaFalse("RightDown"));
        }

        if (type == Type.Len)
        {
            InputDienThoai.Instance.UpButtonPressed = true;
        }

        if (type == Type.Xuong)
        {
            InputDienThoai.Instance.DownButtonPressed = true;
        }

        if (type == Type.TanCong)
        {
            InputDienThoai.Instance.TanCongPressed = true;
        }

        if (type == Type.PhongThu)
        {
            InputDienThoai.Instance.PhongThuPressed = true;
        }

        if (type == Type.Gong)
        {
            InputDienThoai.Instance.GongPressed = true;
        }

        if (type == Type.Nhay)
        {
            InputDienThoai.Instance.NhayPressed = true;
        }
    }
    IEnumerator FrameSauLaFalse(string buttonName)
    {
        yield return new WaitForEndOfFrame();
        if (buttonName == "LeftDown") InputDienThoai.Instance.LeftButtonDown = false;
        if (buttonName == "RightDown") InputDienThoai.Instance.RightButtonDown = false;
        if (buttonName == "LeftUp") InputDienThoai.Instance.LeftButtonUp = false;
        if (buttonName == "RightUp") InputDienThoai.Instance.RightButtonUp = false;
    }

}
