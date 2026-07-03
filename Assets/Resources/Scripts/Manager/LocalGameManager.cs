using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalGameManager : MonoBehaviour
{
    public static LocalGameManager instance;

    public CoroutineRunner coroutineRunner;
    public ObjectPoolManager objectPoolManager;

    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        foreach (IDataInitializable child in GetComponentsInChildren<IDataInitializable>())
        {
            child.DataInitialize();
        }
    }
}
