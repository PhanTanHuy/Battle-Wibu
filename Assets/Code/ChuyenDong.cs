using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChuyenDong : MonoBehaviour
{
    [SerializeField] private float speed, giaToc;
    private float _speed;
    private Vector3 huongDiChuyen;
    private void Start()
    {
        _speed = giaToc == 0f ? speed : 0f;
    }
    // Update is called once per frame
    void Update()
    {
        if (_speed < speed && giaToc != 0f) _speed += giaToc * Time.deltaTime;
        transform.position += huongDiChuyen.normalized * _speed * Time.deltaTime;
    }
    public void DatHuongDiChuyen(Vector3 _huongDiChuyen)
    {
        huongDiChuyen = _huongDiChuyen;
    }

    public void SetSpeed(float value)
    {
        _speed = value;
    }
    public void ResetSpeed()
    {
        _speed = speed;
    }
}
