using UnityEngine;
using System.Collections;

public class ProjectilePlayer : Projectile
{
    // stub - in the future we can make these do cool things
    public override void SetupProjectile(System.Action<Projectile> onDeath)
    {
        base.SetupProjectile(onDeath);

        lifetime = 3.0f;
    }
}
