using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : Component
{
    protected static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    GameObject go = new GameObject(typeof(T).ToString() + " (Singleton)");
                    _instance = go.AddComponent<T>();
                    if (!Application.isBatchMode)
                    {
                        if (Application.isPlaying)
                            DontDestroyOnLoad(go);
                    }
                }
            }
            return _instance;
        }
    }

    public static bool IsCreatedInstance()
    {
        return (_instance != null);
    }
}