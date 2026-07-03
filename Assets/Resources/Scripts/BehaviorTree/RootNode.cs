using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateNodeMenu("BT/Root")]
public class RootNode : BTNode
{
    [Output] public BTNode selectorNode;

    public override NodeState Evaluate(AIController controller)
    {
        Debug.Log("아듀");
        return selectorNode.Evaluate(controller);
    }
}
