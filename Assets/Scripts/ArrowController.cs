using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class ArrowController : MonoBehaviour
{
    [SerializeField] private GameObject hitParticlePrefab;
    [SerializeField] private float destroyDelay = 2f;

    private bool hasHit = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;
        hasHit = true;

        // パーティクルを1回だけ再生
        if (hitParticlePrefab != null)
        {
            Instantiate(hitParticlePrefab, transform.position, Quaternion.identity);
        }

        int damage = 1;

        // ヒット部位によるダメージ判定
        if (collision.collider.GetComponent<HitboxPart>() is HitboxPart hitPart)
        {
            damage = hitPart.GetDamage();
            Debug.Log($"Hit {hitPart.part}, damage = {damage}");
        }

        // ダメージ処理（優先順位：IDamageable > IHittable）
        if (collision.collider.GetComponentInParent<IDamageable>() is IDamageable dmg)
        {
            dmg.OnHit(damage);
        }
        else if (collision.collider.GetComponentInParent<IHittable>() is IHittable hit)
        {
            hit.OnHit();
        }

        // 矢を刺す処理
        StickArrow(collision);

        // 一定時間後に削除
        Destroy(gameObject, destroyDelay);
    }

    private void StickArrow(Collision collision)
    {
        // コライダーを無効化して物理挙動停止
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        // 接触相手に矢を親としてくっつける
        transform.parent = collision.transform;

        // 接触面の法線方向に矢の向きを合わせて刺す
        ContactPoint contact = collision.contacts[0];
        transform.position = contact.point;
        transform.rotation = Quaternion.LookRotation(-contact.normal);
    }


}
