using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();

                if (instance == null)
                {
                    GameObject go = new GameObject();
                    go.name = typeof(T).ToString();
                    instance = go.AddComponent<T>();
                }
            }

            return instance;
        }
    }
}

public class PersistentSingleton<T> : MonoBehaviour where T : Component
{
    private static T instance;

    public static T Instance
    {
        get
        {
            instance = FindObjectOfType<T>();

            if (instance == null)
            {
                GameObject go = new GameObject();
                go.name = typeof(T).ToString();
                instance = go.AddComponent<T>();
                DontDestroyOnLoad(go);
            }

            return instance;
        }
    }
}
