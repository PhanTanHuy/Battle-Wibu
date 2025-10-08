using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject settingBoard;
    public Slider musicSlider;
    public Slider sfxSlider;
    public TextMeshProUGUI textRounds;
    public void VsCpu()
    {
        QuanLiCheDoChoi.Instance.fightMode = QuanLiCheDoChoi.FightMode.PVE;
        QuanLiChuyenScene.instance.LoadSceneKhac("SelectCharacter");
    }
    public void Trainning()
    {
        QuanLiCheDoChoi.Instance.fightMode = QuanLiCheDoChoi.FightMode.Training;
        QuanLiChuyenScene.instance.LoadSceneKhac("SelectCharacter");
    }
    public void Tournament()
    {
        QuanLiCheDoChoi.Instance.fightMode = QuanLiCheDoChoi.FightMode.Tournament;
        QuanLiChuyenScene.instance.LoadSceneKhac("SelectCharacter");
    }
    public void Watch()
    {
        QuanLiCheDoChoi.Instance.fightMode = QuanLiCheDoChoi.FightMode.EVE;
        QuanLiChuyenScene.instance.LoadSceneKhac("SelectCharacter");
    }
    public void Thoat()
    {
        Application.Quit();
    }
    private void Start()
    {
        if (SelectCharacter.Instance != null) Destroy(SelectCharacter.Instance.gameObject);
        if (SelectMap.Instance != null) Destroy(SelectMap.Instance.gameObject);
        QuanLiChuyenScene.instance.BatDauSceneMoi();
        musicSlider.value = sfxSlider.value = 1f;
        Application.targetFrameRate = 60;
        textRounds.text = QuanLiCheDoChoi.Instance.soRounds.ToString();
    }
    public void ChangeMusicSlider()
    {
        QuanLiAmThanh.Instance.ChangeMusicSourceVolume(musicSlider.value);
    }
    public void ChangeSFXSlider()
    {
        QuanLiAmThanh.Instance.ChangeSFXSourceVolume(sfxSlider.value);
    }
    public void OppenSettingBoard()
    {
        settingBoard.SetActive(true);
    }
    public void CloseSettingBoard()
    {
        settingBoard.SetActive(false);
    }
    public void TangRound()
    {
        QuanLiCheDoChoi.Instance.TangGiamRounds(true);
        textRounds.text = QuanLiCheDoChoi.Instance.soRounds.ToString();
    }
    public void GiamRound()
    {
        QuanLiCheDoChoi.Instance.TangGiamRounds(false);
        textRounds.text = QuanLiCheDoChoi.Instance.soRounds.ToString();
    }
}
