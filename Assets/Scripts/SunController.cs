using UnityEngine;

public class SunController : MonoBehaviour
{
    public float rotationDuration = 15f; // ’‹–éØ‚è‘Ö‚¦•b”

    void Update()
    {
        float rotationSpeed = 360f / rotationDuration; // 1ü15•b
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }
}
