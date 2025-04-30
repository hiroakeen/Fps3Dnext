using UnityEngine;

public class VillagerController : MonoBehaviour, IHittable
{
    private bool _transformed = false;

    public GameObject monsterPrefab;
    public GameObject smokeEffectPrefab;

    public void OnHit()
    {
        if (_transformed) return;

        _transformed = true;

        Vector3 smokePosition = transform.position + Vector3.up * 2f;
        GameObject smoke = Instantiate(smokeEffectPrefab, smokePosition, Quaternion.identity);
        GameObject monster = Instantiate(monsterPrefab, transform.position, transform.rotation);

        Animator anim = monster.GetComponent<Animator>();
        if (anim != null)
        {
            anim.updateMode = AnimatorUpdateMode.UnscaledTime;
            anim.SetTrigger("GetHit");
        }

        Destroy(this.gameObject);
        GameManager.Instance.EnemyDefeated();

        if (GameManager.Instance.GetEnemiesRemaining() == 0)
        {
            GameManager.Instance.EndGame();
        }

        Destroy(smoke, 1f);
        Destroy(monster, 2f);
    }
}
