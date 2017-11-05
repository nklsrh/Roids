using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PoolManager
{
    protected List<BaseObject> objectList;
    int lastUsedObject = -1;

    const int MAX_OBJECTS_ALLOWED = 20;

    public int Count
    {
        get;
        private set;
    }

    public virtual void Setup()
    {
        objectList = new List<BaseObject>();
    }

    public virtual BaseObject AddObject(BaseObject template)
    {
        BaseObject p;
        if (objectList.Count < MAX_OBJECTS_ALLOWED)
        {
            p = BaseObject.Instantiate(template);
            objectList.Add(p);
            p.gameObject.name = "[POOLED] " + p.name + " : " + objectList.Count; 
        }
        else if (lastUsedObject >= MAX_OBJECTS_ALLOWED - 1)
        {
            lastUsedObject = -1;
        }
        p = objectList[++lastUsedObject];
        Count++;
        return p;
    }

    public virtual void Logic()
    {
        for (int i = 0; i < objectList.Count; i++)
        {
            objectList[i].Logic();
        }
    }

    public virtual void DisableObject(BaseObject obj)
    {
        objectList.Remove(obj);
        objectList.Insert(lastUsedObject, obj);
        Count--;
    }

    //public virtual void ClearAll()
    //{
    //    for (int i = 0; i < objectList.Count; i++)
    //    {
    //        BaseObject.Destroy(objectList[i]);
    //    }
    //    objectList.Clear();
    //}
}
