using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueFuncManager : MonoBehaviour
{
    public void ChangeBGM(string bgm)
    {

    }

    public void Learning(string detail)
    {
        switch (detail)
        {
            case "Knife":
                LocalGameManager.instance.KnifeLearning();
                break;

            case "Dash":
                LocalGameManager.instance.DashLearning();
                break;
        }
    }
}
