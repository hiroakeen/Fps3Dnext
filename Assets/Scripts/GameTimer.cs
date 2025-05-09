using UnityEngine;

public class GameTimer : MonoBehaviour
{
    private float timer;
    private bool isRunning;

    public string GetFormattedTime() => FormatTime(timer);

    public void StartTimer() => isRunning = true;
    public void StopTimer() => isRunning = false;
    public float GetTime() => timer;

    void Update()
    {
        if (isRunning)
            timer += Time.deltaTime;
    }

    public void ResetTimer()
    {
        timer = 0f;
    }

    private string FormatTime(float time)
    {
        //�ЂȌ`
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60F);
        int milliseconds = Mathf.FloorToInt((time * 100F) % 100F);
        return string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
    }
}
