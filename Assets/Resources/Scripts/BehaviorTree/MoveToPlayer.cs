using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateNodeMenu("BT/MoveToPlayer")]
public class MoveToPlayer : BTNode
{
    private Unit targetUnit;
    private Rigidbody rigid;

    public override NodeState Evaluate(AIController controller)
    {
        targetUnit = LocalGameManager.instance.unitManager.playerUnit;
        rigid = controller.ParentObj.GetComponent<Rigidbody>();

        var targetPos = targetUnit.transform.position;

        var dir = (targetPos - rigid.position).normalized;
        var movement = dir * 5f * Time.deltaTime;
        var nextPos = rigid.position + movement;

        rigid.MovePosition(nextPos);

        return NodeState.Running;
    }
}
