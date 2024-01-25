using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class NextLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        AudioManager.instance.PlaySFX("Next_Level");
        StartCoroutine(LoadNextLevel());
    }

    IEnumerator LoadNextLevel()

    {
        Time.timeScale = 0.5f;
        yield return new WaitForSecondsRealtime(1f);//su dung de tao thoi gian cho
        Time.timeScale = 1f;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        
        SceneManager.LoadScene(nextSceneIndex);
    }
}
