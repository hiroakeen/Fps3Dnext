using UnityEngine;

public class GameOverBGMPlayer : MonoBehaviour
{
    [Header("�Q�[���I�[�o�[�pBGM")]
    [SerializeField] private AudioClip gameOverBGM;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = gameOverBGM;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.volume = 0.6f;

        PlayBGM();
    }

    private void PlayBGM()
    {
        if (gameOverBGM != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("GameOverBGM ���ݒ肳��Ă��܂���B");
        }
    }
}
