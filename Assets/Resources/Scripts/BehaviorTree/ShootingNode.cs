using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateNodeMenu("BT/ShootingNode")]
public class ShootingNode : BTNode
{
    [SerializeField] private SkillNodeGraph shootingModule;

    public override NodeState Evaluate(AIController controller)
    {
        Debug.Log("빵야빵야");
        shootingModule.rootNode.Evaluate(controller.ParentObj.GetComponentInChildren<ISkillCaster>());
        return NodeState.Success;
    }
}
