
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
    public float interval = 2f;  // ��莞�ԁi�b�j
    private float elapsedTime = 0f;   // �o�ߎ��Ԃ�ێ�����ϐ�


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
            // �ړ����Ă��邩�ǂ����ŃA�j���[�V������؂�ւ�
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
        
        elapsedTime += Time.deltaTime;   // ���t���[���̌o�ߎ��Ԃ����Z

        // �o�ߎ��Ԃ��ݒ肵���Ԋu�𒴂����珈�������s
        if (elapsedTime >= interval)
        {
            RandomSwitchNPC();
            elapsedTime = 0f;   // �o�ߎ��Ԃ����Z�b�g�i���ߕ���ێ�����ꍇ�� elapsedTime -= interval; �Ƃ���j
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
        randomPos = new Vector3(Random.Range(0, 30), 0, Random.Range(0, 30)); //�v���C���[���猩�āA�̋����ɕύX������
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


