using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Image Sound, MuteSound;
    private void Start()
    {
        AudioManager.instance.waitSource.Play();
        AudioManager.instance.musicSource.Stop();
    }
    private void Update()
    {
        
        UpdateButtonSound();
    }
    public void PlayGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadSceneAsync(1); 
        AudioManager.instance.waitSource.Stop();
        AudioManager.instance.musicSource.Play();
    }
    public void ButtonSound()
    {
        PauseGame.isSoundOn = !PauseGame.isSoundOn;
        AudioManager.instance.ToggleWait();
        AudioManager.instance.ToggleMusic();
        AudioManager.instance.ToggleSFX();
        UpdateButtonSound();
    }

    public void Quit()
    {
        Application.Quit();
    }
    private void UpdateButtonSound()
    {
        if (PauseGame.isSoundOn)
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
