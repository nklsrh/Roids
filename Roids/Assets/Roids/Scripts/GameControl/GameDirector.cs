using UnityEngine;
using System.Collections;

public class GameDirector : MonoBehaviour
{
    public PlayerController player;
    public AsteroidManager asteroidManager;

    ProjectilePoolManager projectileManager;

    void Start()
    {
        player.Setup();
        asteroidManager.Setup();

        projectileManager = new ProjectilePoolManager();
        projectileManager.Setup();
    }

    void Update()
    {
        player.Logic();
        projectileManager.Logic();
        asteroidManager.Logic();
    }
}
