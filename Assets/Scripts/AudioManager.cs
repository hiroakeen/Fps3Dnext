using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("BGM ê›íË")]
    [SerializeField] private AudioClip bgmClip;
    private AudioSource bgmSource;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.clip = bgmClip;
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
        bgmSource.volume = 0.5f;
        bgmSource.Play();
    }
    public void PlayBGM()
    {
        if (!bgmSource.isPlaying)
        {
            bgmSource.Play();
        }
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void SetVolume(float volume)
    {
        bgmSource.volume = volume;
    }
}
