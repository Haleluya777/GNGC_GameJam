using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateNodeMenu("BT/ShootingNode")]
public class ShootingNode : BTNode
{
    //[SerializeField] private SkillNodeGraph shootingModule;
    public SkillModule shootingSkill;
    private ISkillCaster caster;

    public override NodeState Evaluate(AIController controller)
    {
        caster = controller.ParentObj.GetComponentInChildren<ISkillCaster>();
        //shootingModule.rootNode.Evaluate(controller.ParentObj.GetComponentInChildren<ISkillCaster>());
        caster.GetCom<Attack>().shootSkill.UseSKill(caster);
        return NodeState.Success;
    }
}
