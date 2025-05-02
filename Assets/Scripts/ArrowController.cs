using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class ArrowController : MonoBehaviour
{
    [SerializeField] private GameObject hitParticlePrefab;
    [SerializeField] private float destroyDelay = 3f;

    private bool hasHit = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;
        hasHit = true;

        if (hitParticlePrefab != null)
        {
            Instantiate(hitParticlePrefab, transform.position, Quaternion.identity);
        }

        int damage = 1;

        // ✅ ヘッド or ボディ判定
        if (collision.collider.GetComponent<HitboxPart>() is HitboxPart hitPart)
        {
            damage = hitPart.GetDamage();
            Debug.Log($"Hit {hitPart.part}, damage = {damage}");
        }

        // ✅ ダメージ対応（IDamageableなら優先）
        if (collision.collider.GetComponentInParent<IDamageable>() is IDamageable dmg)
        {
            dmg.OnHit(damage);
        }
        else if (collision.collider.GetComponentInParent<IHittable>() is IHittable hit)
        {
            hit.OnHit();
        }

        StickArrow(collision);
        Destroy(gameObject, destroyDelay);
    }




    private void StickArrow(Collision collision)
    {
        // Rigidbody を停止
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        // 当たった相手に刺さる（Transformの親に）
        transform.parent = collision.transform;

        // 接触面の角度に合わせて刺さる（自然な向き）
        ContactPoint contact = collision.contacts[0];
        transform.position = contact.point;
        transform.rotation = Quaternion.LookRotation(-contact.normal);
    }
}
