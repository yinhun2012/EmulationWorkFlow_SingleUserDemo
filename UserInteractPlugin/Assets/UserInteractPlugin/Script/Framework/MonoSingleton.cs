using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UserInteractPlugin
{
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
#if UNITY_EDITOR
                    Debug.LogErrorFormat("MonoSingleton Cannot Derive Multiple Object,Only One,Error Type is = {0}", typeof(T));
#endif
                    return;
                }
                instance = this as T;
                if (DontDes)
                    DontDestroyOnLoad(this);
#if UNITY_EDITOR
                //Debug.LogFormat("MonoSingleton set instance object name = {0} gettype = {1} getid = {2}", instance.name, instance.GetType(), instance.GetInstanceID());
#endif
            }
        }

        protected virtual void OnDestroy()
        {
            lock (_locker)
            {
#if UNITY_EDITOR
                //Debug.LogFormat("MonoSingleton null object name = {0} gettype = {1} getid = {2}", instance.name, instance.GetType(), instance.GetInstanceID());
#endif
                instance = null;
            }
        }
    }
}