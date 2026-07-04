using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalGameManager : MonoBehaviour
{
    public static LocalGameManager instance;

    public CoroutineRunner coroutineRunner;
    public ObjectPoolManager objectPoolManager;
    public UnitManager unitManager;
    public DialogueFuncManager dialogueFuncManager;

    public bool learnKnife;
    public bool learnDash;

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

        learnKnife = false;
        learnDash = false;
    }

    public void KnifeLearning()
    {
        unitManager.playerUnit.useKnife = true;
        learnKnife = true;
    }

    public void DashLearning()
    {
        unitManager.playerUnit.useDash = true;
        learnDash = true;
    }
}
