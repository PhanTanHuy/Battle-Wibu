using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectMap : MonoBehaviour
{
    public static SelectMap Instance;
    public GameObject[] prefabsMap;
    public Image anhMap;
    [HideInInspector] public int currentIndexMap;
    private void Awake()
    {
        if (Instance != null) Destroy(Instance.gameObject);
        Instance = this;
        SelectCharacter.Instance._khungSelect.SetActive(false);
        QuanLiChuyenScene.instance.BatDauSceneMoi();
        currentIndexMap = 0;
        ShowCurrentMap();
        DontDestroyOnLoad(gameObject);
    }
    public GameObject MapDaDuocChon()
    {
        return prefabsMap[currentIndexMap];
    }
    public void ShowCurrentMap()
    {
        SpriteRenderer spr = prefabsMap[currentIndexMap].transform.GetChild(4).GetComponent<SpriteRenderer>();
        anhMap.sprite = spr.sprite;
        anhMap.color = spr.color;
    }
    public void NextMap()
    {
        currentIndexMap++;
        if (currentIndexMap == prefabsMap.Length) currentIndexMap = 0;
        ShowCurrentMap();
    }
    public void PrevMap()
    {
        currentIndexMap--;
        if (currentIndexMap == -1) currentIndexMap = prefabsMap.Length - 1;
        ShowCurrentMap();
    }
    public void VoGame()
    {
        QuanLiChuyenScene.instance.LoadSceneKhac("Battle");
    }
    public void Back()
    {
        QuanLiChuyenScene.instance.LoadSceneKhac("SelectCharacter");
    }
}
