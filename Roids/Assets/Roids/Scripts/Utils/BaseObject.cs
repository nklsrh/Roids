using UnityEngine;

public abstract class BaseObject : MonoBehaviour
{
    public abstract void Setup();
    public virtual void Logic()
    {
        // if object finds itself in some crazy corner of the universe, bring it back to play area
        if (transform.position.sqrMagnitude > 5000)
        {
            transform.position = transform.position.normalized * 50;
        }
    }
}
