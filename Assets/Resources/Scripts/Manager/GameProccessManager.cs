using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProccessManager : MonoBehaviour
{
    public int proccess = 0;
    public int monsterCount;
    public List<TextAsset> dialogueScripts;

    void Awake()
    {
        GameProccess(0);
    }

    public void GameProccess(int proccess)
    {
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
                break;
        }
    }
}
