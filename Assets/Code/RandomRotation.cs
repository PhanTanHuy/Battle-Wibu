using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotation : MonoBehaviour
{
    [SerializeField] private Transform mucTieuXoay;
    // Start is called before the first frame update
    void Start()
    {
        if (mucTieuXoay == null)
        {
            mucTieuXoay = transform;
        }
    }

    public void XoayNgauNhien(string chuoiXoay)
    {
        string[] values = chuoiXoay.Split(',');
        if (float.TryParse(values[0], out float from) &&
            float.TryParse(values[1], out float to))
        {
            mucTieuXoay.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(from, to));
        }
    }
    public void ResetRotation()
    {
        mucTieuXoay.transform.rotation = Quaternion.identity;
    }
}
