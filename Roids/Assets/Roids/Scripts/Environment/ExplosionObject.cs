using UnityEngine;
using System.Collections;

public class ExplosionObject : BaseObject
{
    [SerializeField]
    ParticleSystem[] particleSystems;

    public override void Setup()
    {
        gameObject.SetActive(true);
        for (int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].Play();
        }
    }

    public override void Logic()
    {
    }
}
