using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class UnityroomRankingUploader : MonoBehaviour
{
    [Header("UnityroomのランキングURL")]
    [SerializeField] private string rankingUrl = "https://unityroom.com/games/hiroakeen_fps_1/rankings/DXLsUZPj7zeYGOcrIBvrp2V2zfHLsDndHnDsEDDCowLjG5SnyzVzG1wnmccugd/C+DZ0OuSsvtGCpqsbw6P4Xw==\r\n/score";

    private static UnityroomRankingUploader instance;

    void Awake()
    {
        // 重複防止＋常駐化
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// スコア送信（int値、例：Mathf.FloorToInt(survivalTime)）
    /// </summary>
    public void SendScore(int score)
    {
        StartCoroutine(PostScore(score));
    }

    private IEnumerator PostScore(int score)
    {
        WWWForm form = new WWWForm();
        form.AddField("score", score);

        UnityWebRequest www = UnityWebRequest.Post(rankingUrl, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"✅ ランキング送信成功：{score} 秒");
        }
        else
        {
            Debug.LogError($"❌ ランキング送信失敗: {www.error}");
        }
    }
}
