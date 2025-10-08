using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuanLiChuyenScene : MonoBehaviour
{
    public static QuanLiChuyenScene instance;
    public Animator animator;
    private string tenSceneLoad;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void BatDauSceneMoi()
    {
        animator.Play("BatDauSceneMoi");
    }
    public void LoadSceneKhac(string tenScene)
    {
        animator.Play("KetThucSceneHienTai");
        tenSceneLoad = tenScene;
    }
    public void LoadSceneTrongEvent()
    {
        SceneManager.LoadScene(tenSceneLoad);
    }
}
