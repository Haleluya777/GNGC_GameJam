using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "SkillModule")]
public class SkillModule : ScriptableObject
{
    public IBlackBoard blackBoard;

    [SerializeField] private SkillNodeGraph nodeGraph;
    [SerializeField] private float coolDown;
    [SerializeField] private float attackTime;
    private float remainingCoolDown;
    private Unit unit;

    public bool OnCoolDown => remainingCoolDown > 0;
    public float RemainingCoolDown => remainingCoolDown;
    public float CoolDown => coolDown;

    void Awake()
    {

    }

    public void InitSkill()
    {
        blackBoard = new BlackBoard();

        nodeGraph = Instantiate(nodeGraph);
    }

    public bool UseSKill(ISkillCaster caster)
    {
        unit = caster.GetCom<Unit>();
        if (OnCoolDown)
        {
            Debug.Log($"남은 쿨타임 : {remainingCoolDown}");
            Debug.Log("쿨타임 중.");
            return false;
        }

        nodeGraph.rootNode.Evaluate(caster);
        //remainingCoolDown = 0f;
        remainingCoolDown = coolDown;
        unit.state = Unit.UnitState.Attacking;

        DOVirtual.DelayedCall(attackTime, () => unit.state = Unit.UnitState.Idle);

        return true;
    }

    public void UpdateCoolDown(float deltaTime)
    {
        if (!OnCoolDown) return;
        remainingCoolDown -= deltaTime;
    }
}
