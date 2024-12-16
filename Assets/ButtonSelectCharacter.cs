using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelectCharacter : MonoBehaviour
{
    [HideInInspector] public int index;
    public void NhanVatDangDuocChon()
    {
        SelectCharacter.Instance.indexNhanVatDcChon = index;
        SelectCharacter.Instance._khungSelect.SetActive(true);
        SelectCharacter.Instance._khungSelect.transform.SetParent(transform);
        RectTransform rectTransform = SelectCharacter.Instance._khungSelect.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        // Đặt vị trí về giữa
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.transform.localScale = Vector3.one;
    }
}
