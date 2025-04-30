using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ArrowController : MonoBehaviour
{
    [SerializeField] private GameObject hitParticlePrefab;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<IHittable>(out var hittable))
        {
            hittable.OnHit();

            if (hitParticlePrefab != null)
            {
                GameObject particle = Instantiate(hitParticlePrefab, transform.position, Quaternion.identity);
                Destroy(particle, 1f);
            }

            Destroy(this.gameObject); // ����폜
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(this.gameObject); // �n�ʂɓ������Ă��������
        }
    }
}
