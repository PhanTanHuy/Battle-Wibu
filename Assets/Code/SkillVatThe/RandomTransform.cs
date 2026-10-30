using UnityEngine;

public class RandomTransformOnEnable : MonoBehaviour
{
    private void OnEnable()
    {
        Vector3 currentScale = transform.localScale;

        currentScale.y = (Random.value > 0.5f) ? 1f : -1f;

        transform.localScale = currentScale;

        Vector3 currentRotation = transform.localEulerAngles;

        currentRotation.z = Random.Range(-25f, 25f);

        transform.localEulerAngles = currentRotation;
    }
}