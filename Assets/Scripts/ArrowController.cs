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

        // �q�b�g���̃p�[�e�B�N��
        if (hitParticlePrefab != null)
        {
            GameObject particle = Instantiate(hitParticlePrefab, transform.position, Quaternion.identity);
            Destroy(particle, 1f);
        }

        // �C���^�[�t�F�[�X
        if (collision.gameObject.TryGetComponent<IHittable>(out var hittable))
        {
            hittable.OnHit();
        }

        // �h�����Ď~�܂�
        StickArrow(collision);

        // ��莞�Ԍ�ɖ���폜
        Destroy(gameObject, destroyDelay);
    }

    private void StickArrow(Collision collision)
    {
        // Rigidbody ���~
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        // ������������Ɏh����iTransform�̐e�Ɂj
        transform.parent = collision.transform;

        // �ڐG�ʂ̊p�x�ɍ��킹�Ďh����i���R�Ȍ����j
        ContactPoint contact = collision.contacts[0];
        transform.position = contact.point;
        transform.rotation = Quaternion.LookRotation(-contact.normal);
    }
}
