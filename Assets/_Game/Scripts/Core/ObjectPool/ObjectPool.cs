using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour, IObjectPool<T> where T : MonoBehaviour, IPoolable
{
    [SerializeField]
    private T prefab;

    private List<T> allInstances = new List<T>();
    private Stack<T> reusableInstances = new Stack<T>();

    public T GetPrefabInstance()
    {
        T instance;

        if (reusableInstances.Count > 0)
        {
            instance = reusableInstances.Pop();

            instance.transform.SetParent(null);

            instance.transform.localPosition = Vector3.zero;
            instance.transform.localEulerAngles = Vector3.zero;
            instance.transform.localScale = Vector3.one;

            instance.gameObject.SetActive(true);
        }
        else
        {
            instance = Instantiate(prefab);

            allInstances.Add(instance);
        }

        instance.Origin = this;
        instance.PrepareToUse();

        return instance;
    }

    public void ReturnToPool(T instance)
    {
        instance.gameObject.SetActive(false);

        instance.transform.SetParent(transform);

        instance.transform.localPosition = Vector3.zero;
        instance.transform.localEulerAngles = Vector3.zero;
        instance.transform.localScale = Vector3.one;

        reusableInstances.Push(instance);
    }

    public void ReturnToPool(object instance)
    {
        if (instance is T)
        {
            ReturnToPool(instance as T);
        }
    }

    public List<T> GetAllInstances()
    {
        return allInstances;
    }

    public void DestroyAllInstances()
    {
        for (int i = allInstances.Count - 1; i >= 0; i--)
        {
            if (allInstances[i] != null)
            {
                allInstances[i].ReturnToPool();

                Destroy(allInstances[i].gameObject);
            }

            allInstances.RemoveAt(i);
        }
    }
}
