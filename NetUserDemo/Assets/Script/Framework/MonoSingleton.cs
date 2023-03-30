using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] public bool DontDes = false;
    private static object _locker = new object();

    protected static T instance = null;
    public static T Instance
    {
        get
        {
            return instance;
        }
    }

    protected virtual void Awake()
    {
        lock (_locker)
        {
            if (instance != null)
            {
                Debug.LogErrorFormat("MonoSingleton Cannot Derive Multiple Object,Only One,Error Type is = {0}", typeof(T));
                return;
            }
            instance = this as T;
            if (DontDes)
                DontDestroyOnLoad(this);
        }
    }

    protected virtual void OnDestroy()
    {
        lock (_locker)
        {
            instance = null;
        }
    }
}
