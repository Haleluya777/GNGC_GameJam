using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SoundManager))] // SoundManager를 필수 컴포넌트로 지정
public class LocalGameManager : MonoBehaviour
{
    public static LocalGameManager instance;

    public Action killAll;

    public CoroutineRunner coroutineRunner;
    public ObjectPoolManager objectPoolManager;
    public UnitManager unitManager;
    public DialogueFuncManager dialogueFuncManager;
    public DialogueRunner dialoguerunner;
    public TimeLineManager timeLineManager;
    public PlayerUIManager playerUiManager;
    public GameProccessManager gameProccessManager;
    public SoundManager soundManager; // 사운드 매니저 참조

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

        soundManager = GetComponent<SoundManager>(); // 사운드 매니저 컴포넌트 가져오기
        killAll += unitManager.DestroyAllEnemies; // 이벤트에 적 파괴 메소드 등록

        foreach (IDataInitializable child in GetComponentsInChildren<IDataInitializable>())
        {
            child.DataInitialize();
        }

        learnKnife = false;
        learnDash = false;
    }

    void Start()
    {
        // 게임 시작 시 기본 BGM 재생
        soundManager.PlayBgm("기본 bgm");
    }

    public void KillAllEnemy()
    {
        killAll?.Invoke();
    }

    public void DisableAllInput()
    {
        playerInput.actions.Disable();
        if (unitManager.playerUnit != null) // 플레이어가 파괴되었을 경우를 대비
        {
            unitManager.playerUnit.movement.dir = Vector2.zero;
        }
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

    public void RestartGame()
    {
        Debug.Log("--- Starting RestartGame ---");
        // 0. 현재 실행중인 대화가 있다면 완전히 중지
        dialoguerunner.StopDialogueBoxGlitch(); // 글리치 효과 중지 및 원상복구
        dialoguerunner.StopAndReset();
        Debug.Log("Dialogue stopped and reset.");

        // 1. 기존 플레이어 및 적 파괴
        if (unitManager.playerUnit != null)
        {
            Destroy(unitManager.playerUnit.gameObject);
            Debug.Log("Old player destroyed.");
        }
        KillAllEnemy();
        Debug.Log("KillAllEnemy called.");

        // 2. 게임 상태 초기화
        learnKnife = false;
        learnDash = false;
        gameProccessManager.proccess = 0;
        Debug.Log("Game state reset.");

        // 3. 새 플레이어 생성 및 참조 갱신
        var playerObj = Instantiate(unitManager.playerPrefab);
        playerObj.transform.position = unitManager.summonPos[5].position;
        Debug.Log("New player instantiated.");

        unitManager.playerUnit = playerObj.GetComponent<Unit>();
        playerInput = playerObj.GetComponent<PlayerInput>(); // PlayerInput 참조 갱신
        if (playerInput == null)
        {
            Debug.LogError("RestartGame: PlayerInput component not found on new player prefab!");
        }
        else
        {
            Debug.Log("RestartGame: New PlayerInput assigned.");
        }


        // 4. UI 매니저에 새 플레이어 정보 전달
        playerUiManager.Initialize(unitManager.playerUnit);

        // 5. BGM 및 게임 첫 프로세스 시작
        soundManager.PlayBgm("기본 bgm");
        gameProccessManager.GameProccess(0);
        Debug.Log("--- RestartGame Finished ---");
    }
}
