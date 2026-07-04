using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateNodeMenu("BT/ThrowGrenade")]
public class ThrowGrenade : BTNode
{
    private ISkillCaster caster;

    public override NodeState Evaluate(AIController controller)
    {
        caster = controller.ParentObj.GetComponentInChildren<ISkillCaster>();
        caster.GetCom<Attack>().grenadeSkill.UseSKill(caster);
        return NodeState.Success;
    }
}
