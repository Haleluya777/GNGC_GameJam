using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProccessManager : MonoBehaviour, IDataInitializable
{
    public int proccess = 0;
    public int monsterCount;
    public List<TextAsset> dialogueScripts;

    public void DataInitialize()
    {
        GameProccess(proccess);
    }

    void Update()
    {
        if (monsterCount == 0)
        {
            proccess++;
            GameProccess(proccess);
        }
    }

    public void GameProccess(int proccess)
    {
        switch (proccess)
        {
            case 0:
                LocalGameManager.instance.dialoguerunner.DialogueFile = dialogueScripts[0];
                LocalGameManager.instance.dialoguerunner.StartDialogue();
                monsterCount = 1;
                break;

            case 1:
                LocalGameManager.instance.dialoguerunner.DialogueFile = dialogueScripts[1];
                LocalGameManager.instance.dialoguerunner.StartDialogue();
                monsterCount = 0;
                break;

            case 2:
                LocalGameManager.instance.dialoguerunner.DialogueFile = dialogueScripts[2];
                LocalGameManager.instance.dialoguerunner.StartDialogue();
                monsterCount = 1;
                break;

            case 3:
                LocalGameManager.instance.dialoguerunner.DialogueFile = dialogueScripts[3];
                LocalGameManager.instance.dialoguerunner.StartDialogue();
                monsterCount = 0;
                break;
        }
    }
}
