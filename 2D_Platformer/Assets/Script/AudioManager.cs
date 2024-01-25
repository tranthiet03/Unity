using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Sound[] sfxSounds;

    public AudioSource musicSource, sfxSource, waitSource;

    private void Awake()
    {
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


    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, s => s.name == name);
        if (s == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }
    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
    }
    public void ToggleWait()
    {
        waitSource.mute = !waitSource.mute;
    }
}