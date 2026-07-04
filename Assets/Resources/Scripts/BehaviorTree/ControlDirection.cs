using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateNodeMenu("BT/ControlDirection")]
public class ControlDirection : BTNode
{
    [Header("블랙보드 키")]
    public string directionKey = "Direction";
    private Transform unitTransform;

    public override NodeState Evaluate(AIController controller)
    {
        if (blackboard is null) return NodeState.Failure;
        blackboard.Set<Vector3>("UnitPos", controller.ParentObj.transform.position);

        Vector3 unitPos = blackboard.Get<Vector3>("UnitPos");
        Vector3 playerPos = LocalGameManager.instance.unitManager.playerUnit.transform.position;

        if (playerPos.x < unitPos.x) controller.ParentObj.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        else
        {
            Debug.Log("d미낭러");
            controller.ParentObj.transform.localScale = new Vector3(-1.5f, 1.5f, 1.5f);
        }

        return NodeState.Success;
    }
}
