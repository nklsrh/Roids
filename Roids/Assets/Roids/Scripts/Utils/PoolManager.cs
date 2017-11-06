using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PoolManager
{
    protected List<BaseObject> objectList;
    int lastUsedObject = -1;

    int poolSize = 20;
    float timeBetweenCleanups = 5.0f;
    float currentTimeBetweenCleanup = 5.0f;

    public int Count
    {
        get;
        private set;
    }

    public PoolManager() { }
    public PoolManager(int poolSize)
    {
        Setup(poolSize);
    }

    public virtual void Setup()
    {
        objectList = new List<BaseObject>();
    }

    public void Setup(int newSize)
    {
        poolSize = newSize;

        Setup();
    }

    public virtual BaseObject AddObject(BaseObject template)
    {
        BaseObject p;
        if (objectList.Count < poolSize)
        {
            p = BaseObject.Instantiate(template);
            objectList.Add(p);
            p.gameObject.name = "[POOLED] " + p.name + " : " + objectList.Count; 
        }
        else if (lastUsedObject >= poolSize - 1)
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
            if (objectList[i].gameObject.activeInHierarchy)
            {
                objectList[i].Logic();
            }
        }

        RefreshCount();
    }

    private void RefreshCount()
    {
        if (currentTimeBetweenCleanup > 0)
        {
            currentTimeBetweenCleanup -= Time.deltaTime;
        }
        else
        {
            Count = 0;
            for (int i = 0; i < objectList.Count; i++)
            {
                if (objectList[i].isActiveAndEnabled)
                {
                    Count++;
                }
            }

            currentTimeBetweenCleanup = timeBetweenCleanups;
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
