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
        if (Instance == null)
        {
            Instance = this;
            currentIndexMap = 0;
            ShowCurrentMap();
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
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
        SelectMap.Instance.gameObject.SetActive(false);
        SceneManager.LoadScene("Battle");
    }
}
