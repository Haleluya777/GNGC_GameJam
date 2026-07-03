using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateNodeMenu("BT/Aiming")]
public class Aiming : BTNode
{
    private Unit target;

    public override NodeState Evaluate(AIController controller)
    {
        target = LocalGameManager.instance.unitManager.playerUnit;

        Vector3 dir = target.gameObject.GetComponent<Rigidbody>().position - controller.ParentObj.GetComponent<Rigidbody>().position;
        dir.y = 0f;

        controller.ParentObj.GetComponent<Unit>().dir = dir.normalized;

        return NodeState.Failure;
    }
}
