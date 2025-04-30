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

            Destroy(this.gameObject); // –î‚ğíœ
        }
        else if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(this.gameObject); // ’n–Ê‚É“–‚½‚Á‚Ä‚à–î‚ğÁ‚·
        }
    }
}
