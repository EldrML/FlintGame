using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{

    public static T Instance
    {
        get
        {
            return _instance;
        }
    }

    private static T _instance;

    protected virtual void Awake()
    {
        if (_instance != null) throw new System.Exception("There is already an assigned Singleton of type " + typeof(T).ToString() + "!");
        _instance = GetSingletonInstance;
        if (_instance != null) InstanceFound?.Invoke(_instance);
        Debug.Log(typeof(T) + " Instance Set -> " + _instance, this);
    }

    protected virtual void OnDestroy()
    {
        _instance = null;
        Debug.Log(typeof(T) + " Instance Destroyed -> " + _instance, this);
    }

    protected abstract T GetSingletonInstance { get; }

    public static event Action<T> InstanceFound;
}
