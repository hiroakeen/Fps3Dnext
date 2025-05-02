using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{

    private void Update()
    {
        StartCoroutine(LoadSceneCountDown());
    }

    IEnumerator LoadSceneCountDown()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MainScene");
    }
}
