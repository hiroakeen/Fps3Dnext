using System.Collections.Generic;
using UnityEngine;

public class BloodEffectController : MonoBehaviour
{
    [Header("Blood Settings")]
    [SerializeField] private Sprite[] bloodSprites;
    [SerializeField] private float zOffset = 1.5f;
    [SerializeField] private int sortOrder = 10;
    [SerializeField] private float alpha = 0.8f;
    [SerializeField] private float scaleMin = 0.8f;
    [SerializeField] private float scaleMax = 1.2f;

    private List<GameObject> activeBlood = new();

    private int maxHealth = 3;

    /// <summary>
    /// 最大HPをセット（ゲーム開始時などに呼び出す）
    /// </summary>
    public void SetMaxHealth(int value)
    {
        maxHealth = value;
    }

    /// <summary>
    /// 現在のHPに応じて血エフェクトを調整
    /// </summary>
    public void SyncBloodWithHealth(int currentHealth)
    {
        int targetCount = Mathf.Clamp(maxHealth - currentHealth, 0, maxHealth);
        int currentCount = activeBlood.Count;

        if (targetCount > currentCount)
        {
            AddBlood(targetCount - currentCount);
        }
        else if (targetCount < currentCount)
        {
            RemoveBlood(currentCount - targetCount);
        }
    }

    private void AddBlood(int count)
    {
        Camera cam = Camera.main;
        if (cam == null) return;

        for (int i = 0; i < count; i++)
        {
            GameObject go = new GameObject("BloodSprite");
            go.transform.SetParent(cam.transform);

            SpriteRenderer sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = bloodSprites[Random.Range(0, bloodSprites.Length)];
            sr.sortingOrder = sortOrder;
            sr.color = new Color(1f, 1f, 1f, alpha);

            Vector3 localOffset = new Vector3(
                Random.Range(-0.5f, 0.5f),
                Random.Range(-0.3f, 0.3f),
                zOffset
            );

            go.transform.localPosition = localOffset;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one * Random.Range(scaleMin, scaleMax);

            activeBlood.Add(go);
        }
    }

    private void RemoveBlood(int count)
    {
        for (int i = 0; i < count && activeBlood.Count > 0; i++)
        {
            GameObject go = activeBlood[activeBlood.Count - 1];
            activeBlood.RemoveAt(activeBlood.Count - 1);
            Destroy(go);
        }
    }

    public void ClearAll()
    {
        foreach (GameObject go in activeBlood)
        {
            Destroy(go);
        }
        activeBlood.Clear();
    }
}
