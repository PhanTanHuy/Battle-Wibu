using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSpaceScreenCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public enum sortingLayer
    {
        ImageSkill,
        CutScene
    }
    public sortingLayer sortLayer;
    void Awake()
    {
        Canvas cv = GetComponent<Canvas>();
        cv.worldCamera = Camera.main;
        cv.sortingLayerName = sortLayer.ToString();
        RectTransform rectTransform = cv.GetComponent<RectTransform>();
        rectTransform.transform.position = Vector3.zero;
        rectTransform.localScale = Vector3.one;
        rectTransform.sizeDelta = new Vector2(1920, 1080);
        cv.planeDistance = 0;
    }
}
