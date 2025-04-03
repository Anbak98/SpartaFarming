using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Component
{
    protected static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                T component = FindFirstObjectByType<T>();

                if (component == null)
                {
                    GameObject gameObject;
                    gameObject = new GameObject(typeof(T).Name);
                    _instance = gameObject.AddComponent<T>();
                }
                else
                {
                    _instance = component;
                }
            }
            
            return _instance;
        }
    }
}
