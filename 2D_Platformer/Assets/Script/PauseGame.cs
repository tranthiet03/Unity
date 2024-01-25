using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Image Sound, MuteSound;
    [Space(10), Header("Game Over")]
    [SerializeField] GameObject GameOverPanel;
    [SerializeField] Text txthighScore;
    public static bool isSoundOn = true;
    private void Update()
    {
        GamePause();
        UpdateButtonSound();
    }
    public void GamePause()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void ButtonSound()
    {
        isSoundOn = !isSoundOn;
        AudioManager.instance.ToggleMusic();
        AudioManager.instance.ToggleWait();
        AudioManager.instance.ToggleSFX();
        UpdateButtonSound();
    }
    private void UpdateButtonSound()
    {
        if (Sound != null && MuteSound != null)
        {
            if (isSoundOn)
            {
                Sound.enabled = true;
                MuteSound.enabled = false;
            }
            else
            {
                Sound.enabled = false;
                MuteSound.enabled = true;
            }
        }
    }
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }
    public void Restart_Pause()
    {
        SceneManager.LoadScene("Level 1");
        Time.timeScale = 1;
        FindAnyObjectByType<GameManager>().DestroyGO();
    }

    //GameOver
    
    public void Restart()
    {
        GameOverPanel.SetActive(false);
        SceneManager.LoadScene("Level 1");
        Time.timeScale = 1;
        AudioManager.instance.musicSource.Play();
        FindAnyObjectByType<GameManager>().DestroyGO();
    }
    public void Home()
    {
        SceneManager.LoadScene(0);
    }
    public void GameOver()
    {
        GameOverPanel.SetActive(true);
        int highscore = FindObjectOfType<GameManager>().GetHighScore();
        txthighScore.text = string.Format($"HIGH SCORE: {highscore}");
    }
}
