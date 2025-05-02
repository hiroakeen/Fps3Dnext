using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Bee : MonoBehaviour
{
    public float detectionRange = 8f;
    public float attackRange = 2f;
    public float moveRadius = 5f;
    public float moveInterval = 3f;
    public float attackCooldown = 1.5f;

    private Transform player;
    private Animator animator;
    private NavMeshAgent agent;

    private float moveTimer;
    private float attackTimer;
    private bool isDead = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; // �n�`�͉�]�s�v�ȃP�[�X����
        agent.updateUpAxis = false;
    }

    void Update()
    {
        if (isDead || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        moveTimer += Time.deltaTime;
        attackTimer += Time.deltaTime;

        if (distance <= attackRange)
        {
            if (attackTimer >= attackCooldown)
            {
                Attack();
                attackTimer = 0f;
            }
        }
        else if (distance <= detectionRange)
        {
            ChasePlayer();
        }
        else
        {
            RandomMove();
        }

        // �A�j���[�V�����X�V
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    void RandomMove()
    {
        if (moveTimer < moveInterval) return;

        Vector3 randomPoint = transform.position + Random.insideUnitSphere * moveRadius;
        randomPoint.y = transform.position.y; // �����ړ��̂�
        agent.SetDestination(randomPoint);
        animator.Play("Move");

        moveTimer = 0f;
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);
        animator.Play("Move");
    }

    void Attack()
    {
        agent.SetDestination(transform.position); // �ꎞ��~
        animator.SetTrigger("Attack");
        ;

        // �U������Ȃǂ���΂����Œǉ�
        Debug.Log("Bee attacks the player!");
    }

    public void OnHit()
    {
        if (isDead) return;

        isDead = true;
        agent.isStopped = true;
        animator.SetTrigger("Die");
        Destroy(gameObject, 2f);
    }
}
