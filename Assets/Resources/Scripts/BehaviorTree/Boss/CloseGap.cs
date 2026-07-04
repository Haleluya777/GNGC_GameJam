using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateNodeMenu("BT/CloseGap")]
public class CloseGap : BTNode
{
    private ISkillCaster caster;
    public SkillModule closeDash;
    public SkillModule openDash;
    bool isInit = false;

    public void Initialize()
    {
        // closeDash = Instantiate(closeDash);
        // openDash = Instantiate(openDash);

        closeDash.InitSkill();
        openDash.InitSkill();

        isInit = true;
    }

    public override NodeState Evaluate(AIController controller)
    {
        if (!isInit) Initialize();
        caster = controller.ParentObj.GetComponentInChildren<ISkillCaster>();
        Attack attack = caster.GetCom<Attack>();

        if (blackboard.Get<float>("Distance") <= 5)
        {
            Debug.Log("거리 넓힘");
            openDash.UseSKill(caster);
        }

        else
        {
            Debug.Log("거리 좁힘");
            closeDash.UseSKill(caster);
        }

        return NodeState.Success;
    }
}
