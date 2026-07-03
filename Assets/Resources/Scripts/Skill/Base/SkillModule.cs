using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class SkillModule : ScriptableObject
{
    public IBlackBoard blackBoard;

    [SerializeField] private SkillNodeGraph nodeGraph;
    [SerializeField] private float coolDown;
    private float remainingCoolDown;

    public bool OnCoolDown => remainingCoolDown > 0;
    public float RemainingCoolDown => remainingCoolDown;

    void Awake()
    {
        blackBoard = new BlackBoard();
    }

    public void InitSkill()
    {
        blackBoard.Set("Condition", true);

        nodeGraph = Instantiate(nodeGraph);
    }

    public bool UseSKill(ISkillCaster caster)
    {
        Unit unit = caster.GetCom<Unit>();
        if (OnCoolDown || !blackBoard.Get<bool>("Condition"))
        {
            Debug.Log("쿨타임 중.");
            return false;
        }

        nodeGraph.rootNode.Evaluate(caster);
        remainingCoolDown = coolDown;
        return true;
    }

    public void UpdateCoolDown(float deltaTime)
    {
        if (!OnCoolDown) return;
        remainingCoolDown -= deltaTime;
    }
}
