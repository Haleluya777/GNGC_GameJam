using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProccessManager : MonoBehaviour, IDataInitializable
{
    public int proccess = 0;
    public int monsterCount;
    public List<TextAsset> dialogueScripts;
    public TextAsset gameOverScript;
    public TextAsset finalScript;

    public void DataInitialize()
    {
        GameProccess(proccess);
    }

    void Update()
    {

    }

    public void GameProccess(int proccess)
    {
        LocalGameManager.instance.unitManager.playerUnit.unitData.curHp = LocalGameManager.instance.unitManager.playerUnit.unitData.maxHp;
        switch (proccess)
        {
            case 0:
                LocalGameManager.instance.dialoguerunner.DialogueFile = dialogueScripts[0];
                LocalGameManager.instance.dialoguerunner.StartDialogue();
                break;

            case 1:
                LocalGameManager.instance.dialoguerunner.DialogueFile = dialogueScripts[1];
                LocalGameManager.instance.dialoguerunner.StartDialogue();
                break;

            case 2:
                LocalGameManager.instance.dialoguerunner.DialogueFile = dialogueScripts[2];
                LocalGameManager.instance.dialoguerunner.StartDialogue();
                break;

            case 3:
                LocalGameManager.instance.dialoguerunner.DialogueFile = dialogueScripts[3];
                LocalGameManager.instance.dialoguerunner.StartDialogue();
                LocalGameManager.instance.dialoguerunner.DialogueBoxGlitch();
                break;

            case 4:
                LocalGameManager.instance.dialoguerunner.DialogueFile = dialogueScripts[4];
                LocalGameManager.instance.dialoguerunner.StartDialogue();
                break;

            case 5:
                Debug.Log("보스 등장.");
                SoundManager.instance.PlayBgm("보스전 bgm");
                LocalGameManager.instance.unitManager.SummonEnemy(5);
                monsterCount = 1;
                break;

            case 6:
                LocalGameManager.instance.dialoguerunner.DialogueFile = finalScript;
                LocalGameManager.instance.dialoguerunner.StartDialogue();
                LocalGameManager.instance.dialoguerunner.DialogueBoxGlitch();
                break;
        }
    }
}
