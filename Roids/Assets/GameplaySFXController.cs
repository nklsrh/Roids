using UnityEngine;
using System.Collections;

public class GameplaySFXController : MonoBehaviour
{
    public AudioSource sfxAchievement;

    public void PlayAchievement()
    {
        sfxAchievement.Play();
    }
}
