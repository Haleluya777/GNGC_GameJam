using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LocalGameManager : MonoBehaviour
{
    public static LocalGameManager instance;

    public CoroutineRunner coroutineRunner;
    public ObjectPoolManager objectPoolManager;
    public UnitManager unitManager;
    public DialogueFuncManager dialogueFuncManager;
    public DialogueRunner dialoguerunner;
    public TimeLineManager timeLineManager;
    public PlayerUIManager playerUiManager;
    public GameProccessManager gameProccessManager;

    [SerializeField] private PlayerInput playerInput;

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

    public void DisableAllInput()
    {
        playerInput.actions.Disable();
    }

    public void EnableAllInput()
    {
        playerInput.actions.Enable();
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
