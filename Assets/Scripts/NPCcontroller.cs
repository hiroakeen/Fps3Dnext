
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class NPCcontroller : MonoBehaviour, IHittable
{
    private NavMeshAgent agent;
    private Animator animator;
    private Vector3 randomPos;
    private VillagerController villagerMonsterPosition;
    private bool _dead = false;
    private Camera playerCamera;
    public float interval = 2f;  // 一定時間（秒）
    private float elapsedTime = 0f;   // 経過時間を保持する変数


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        villagerMonsterPosition = GetComponent<VillagerController>();
        playerCamera = Camera.main;
    }



    void Update()
    {
        if (!_dead)
        {
            TimerNPC();
            // 移動しているかどうかでアニメーションを切り替え
            if (agent.velocity.magnitude > 0.1f)
            {
                animator.SetBool("Move", true);
            }
            else
            {
                animator.SetBool("Move", false);
            }
        }
        else
        {
            return;
        }
    }


    void TimerNPC()
    {
        
        elapsedTime += Time.deltaTime;   // 毎フレームの経過時間を加算

        // 経過時間が設定した間隔を超えたら処理を実行
        if (elapsedTime >= interval)
        {
            RandomSwitchNPC();
            elapsedTime = 0f;   // 経過時間をリセット（超過分を保持する場合は elapsedTime -= interval; とする）
        }
    }
    void RandomSwitchNPC()
    {
        {
            var onOffRandom = Random.Range(0, 5);
            if (onOffRandom >= 2)
            {
                nextGoal();
            }
            else
            {
                return;
            }
        }
    }
    void nextGoal()
    {
        randomPos = new Vector3(Random.Range(0, 30), 0, Random.Range(0, 30)); //プレイヤーから見て、の距離に変更したい
        agent.destination = randomPos;
    }

    public void OnHit()
    {
        if (_dead) return;

        _dead = true;

        if (GetComponent<VillagerController>() != null)
        {
            GameManager.Instance.EnemyDefeated();
        }
        else
        {
            GameManager.Instance.VillagerDefeated();
        }

        if (animator != null && agent != null)
        {
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            agent.destination = this.transform.position;
            animator.SetTrigger("GetHit");
            Destroy(gameObject, 1.5f);
        }
    }

}


