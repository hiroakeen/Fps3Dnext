using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [SerializeField] private AudioSource footStepSound;
    [SerializeField] private AudioSource bowShotSound;
    [SerializeField] private AudioSource stringSound;

    public void PlayFootstep()
    {
        if (footStepSound != null)
        {
            footStepSound.pitch = Random.Range(0.7f, 1.3f);
            footStepSound.Play();
        }
    }

    public void PlayBowShot()
    {
        if (bowShotSound != null)
        {
            bowShotSound.pitch = Random.Range(0.7f, 1.3f);
            bowShotSound.Play();
        }
    }

    public void PlayStringSound()
    {
        if (stringSound != null)
        {
            stringSound.pitch = Random.Range(0.7f, 1.3f);
            stringSound.Play();
        }
    }
}
