using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected virtual bool ShouldOverride => false;
    protected virtual bool DestroyOnLoad => true;
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if ((Object) Singleton<T>.Instance != (Object) null)
        {
            if (this.ShouldOverride)
            {
                Object.Destroy((Object) Singleton<T>.Instance.gameObject);
            }
            else
            {
                Object.Destroy((Object) this.gameObject);
                return;
            }
        }
        Singleton<T>.Instance = this as T;
        if (this.DestroyOnLoad)
            return;
        Object.DontDestroyOnLoad((Object) this.gameObject);
    }

}
