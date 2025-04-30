using UnityEngine;

public class WorldWarpZone : MonoBehaviour
{
    private BoxCollider warpBounds;
    private Vector3 center;
    private Vector3 size;

    void Awake()
    {
        warpBounds = GetComponent<BoxCollider>();
        warpBounds.isTrigger = true;

        center = warpBounds.center + transform.position;
        size = warpBounds.size;
    }

    void OnTriggerExit(Collider other)
    {
        var t = other.transform;
        Vector3 pos = t.position;
        Vector3 local = pos - center;

        bool warped = false;

        if (Mathf.Abs(local.x) > size.x / 2f)
        {
            pos.x = center.x - local.x;
            warped = true;
        }

        if (Mathf.Abs(local.z) > size.z / 2f)
        {
            pos.z = center.z - local.z;
            warped = true;
        }

        if (!warped) return;

        // 特別対応：CharacterController対応（プレイヤー）
        if (other.TryGetComponent<CharacterController>(out var controller))
        {
            controller.enabled = false;
            t.position = pos;
            controller.enabled = true;
        }
        else
        {
            // 通常のTransform移動（動物など）
            t.position = pos;
        }
    }
}
