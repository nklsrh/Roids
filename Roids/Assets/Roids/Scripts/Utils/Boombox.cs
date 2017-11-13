using UnityEngine;
using System.Collections.Generic;

public class Boombox : MonoBehaviour
{
    [SerializeField]
    List<AudioSource> musicMenu;

    [SerializeField]
    List<AudioSource> musicGameplay;

    [SerializeField]
    List<AudioSource> musicGameplayAmbience;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayMenuMusic()
    {
        StopAllGameplayMusic();
        StopAllGameplayAmbienceMusic();
        musicMenu[Random.Range(0, musicMenu.Count)].Play();
    }

    public void PlayGameplayMusic()
    {
        StopAllMenuMusic();
        musicGameplay[Random.Range(0, musicGameplay.Count)].Play();
    }

    public void PlayGameplayAmbience()
    {
        for (int i = 0; i < musicGameplayAmbience.Count; i++)
        {
            musicGameplayAmbience[i].Play();
        }
    }

    private void StopAllMenuMusic()
    {
        for (int i = 0; i < musicMenu.Count; i++)
        {
            musicMenu[i].Stop();
        }
    }

    private void StopAllGameplayMusic()
    {
        for (int i = 0; i < musicGameplay.Count; i++)
        {
            musicGameplay[i].Stop();
        }
    }

    private void StopAllGameplayAmbienceMusic()
    {
        for (int i = 0; i < musicGameplayAmbience.Count; i++)
        {
            musicGameplayAmbience[i].Stop();
        }
    }

    public static Boombox Instance;
}
