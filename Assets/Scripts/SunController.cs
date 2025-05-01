using UnityEngine;

public class SunController : MonoBehaviour
{
    public float rotationDuration = 15f; // ����؂�ւ��b��

    void Update()
    {
        float rotationSpeed = 360f / rotationDuration; // 1��15�b
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
    }
}
