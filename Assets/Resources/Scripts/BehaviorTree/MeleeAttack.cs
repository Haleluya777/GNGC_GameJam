using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateNodeMenu("BT/MeleeAttack")]
public class MeleeAttack : BTNode
{
    private ISkillCaster caster;
    public override NodeState Evaluate(AIController controller)
    {
        if (!LocalGameManager.instance.learnKnife) return NodeState.Success;

        caster = controller.ParentObj.GetComponentInChildren<ISkillCaster>();
        caster.GetCom<Attack>().parryingSkill.UseSKill(caster);
        return NodeState.Success;
    }
}
