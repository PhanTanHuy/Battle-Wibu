using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectCharacter : MonoBehaviour
{
    public static SelectCharacter Instance;
    public enum FightMode
    {
        PVP,
        PVE,
        EVE,
        Tranning
    }
    public GameObject[] Characters;
    public GameObject container;
    public GameObject prefabsSelectMau, khungSelect;
    public int maxChar, maxRound;
    public FightMode fightMode;
    [HideInInspector] public List<GameObject> buttonSelect;
    [HideInInspector] public int indexNhanVatDcChon;
    [HideInInspector] public int soLanChon;
    [HideInInspector] public GameObject[] chars;
    [HideInInspector] public GameObject _khungSelect;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            soLanChon = 0;
            buttonSelect = new List<GameObject>();
            chars = new GameObject[maxChar];
            indexNhanVatDcChon = -1;
            for (int i = 0; i < Characters.Length; i++)
            {
                GameObject _prefSelectNew = Instantiate(prefabsSelectMau);
                _prefSelectNew.transform.GetChild(0).GetComponent<Image>().sprite = Characters[i].GetComponent<ChiSoNhanVat>().anhDanhDien;
                _prefSelectNew.GetComponent<ButtonSelectCharacter>().index = i;
                _prefSelectNew.transform.SetParent(container.transform);
                buttonSelect.Add(_prefSelectNew);
                _prefSelectNew.transform.localScale = Vector3.one;
            }
            _khungSelect = Instantiate(khungSelect);
            _khungSelect.transform.SetParent(buttonSelect[0].transform);
            _khungSelect.SetActive(false);
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }
    public void Chon()
    {
        if (indexNhanVatDcChon == -1) return;
        if (soLanChon == maxChar)
        {
            this.gameObject.SetActive(false);
            SceneManager.LoadScene("SelectMap");
            return;
        }
        chars[soLanChon] = Characters[indexNhanVatDcChon];
        soLanChon++;
        SelectCharacter.Instance._khungSelect.SetActive(false);
    }
}
