using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class DieuChinhMap : MonoBehaviour
{
    void Start()
    {
        // Xóa SpriteRenderer của chính GameObject
        SpriteRenderer selfRenderer = GetComponent<SpriteRenderer>();
        if (selfRenderer != null)
        {
            Destroy(selfRenderer);
        }

        // Xóa SpriteRenderer của các con (index 0, 1, 2, 3, 4)
        for (int i = 0; i < 4; i++)
        {
            Transform child = transform.GetChild(i); // Lấy con theo index
            SpriteRenderer childRenderer = child.GetComponent<SpriteRenderer>();
            if (childRenderer != null)
            {
                Destroy(childRenderer);
            }
        }
    }
}
