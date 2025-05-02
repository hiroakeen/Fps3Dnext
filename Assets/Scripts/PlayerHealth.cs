using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 2; // �����̓n�`�p��2�i�N�}�Ȃ�1�ő����j
    private int currentHealth;

    private bool isDead = false;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        Debug.Log($"Player took {amount} damage. HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("Player died. Game Over!");

        // ��F�V�[�����ēǂݍ���
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
