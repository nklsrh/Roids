using UnityEngine;
using System.Collections;

public class GameDirector : MonoBehaviour
{
    public PlayerController player;

    void Start()
    {
        player.Setup();
    }

    void Update()
    {
        player.Logic();

        if (ProjectilePoolManager.Instance != null)
        {
            ProjectilePoolManager.Instance.Logic();
        }
    }
}
