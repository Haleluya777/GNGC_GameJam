using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateNodeMenu("Skill/UnitMove")]
public class UnitMove : SkillNode
{
    [Output(dynamicPortList = true)] public List<SkillNode> childs;


    public float dashDistance = 5f;
    public float dashDuration = .2f;
    public Ease dashEase = Ease.Linear;

    public LayerMask wall;

    public override void Evaluate(ISkillCaster caster)
    {
        var movement = caster.GetCom<Movement>();
        var rigid = caster.GetCom<Rigidbody>();
        var col = caster.GetCom<BoxCollider>();

        Vector2 moveDir = movement.dir;
        if (moveDir == Vector2.zero) return;

        Vector3 dashDirection = new Vector3(moveDir.x, 0, moveDir.y).normalized;
        Vector3 startPos = rigid.position;
        Vector3 targetPosition = startPos + dashDirection * dashDistance;

        Vector3 virtualPos = startPos;
        Sequence dashSequence = DOTween.Sequence();

        dashSequence.Append(DOTween.To(() => virtualPos, x => virtualPos = x, targetPosition, dashDuration).SetEase(dashEase));

        dashSequence.OnUpdate(() =>
        {
            if (Physics.CheckBox(virtualPos + col.center, col.size / 2, Quaternion.identity, wall))
            {
                dashSequence.Kill();
                ExecuteNode(caster);
                return;
            }

            rigid.MovePosition(virtualPos);
        });

        dashSequence.SetUpdate(UpdateType.Fixed);
        dashSequence.OnComplete(() =>
        {
            ExecuteNode(caster);
        });
    }

    private void ExecuteNode(ISkillCaster caster)
    {
        try
        {
            foreach (var port in DynamicOutputs)
            {
                if (port.fieldName.StartsWith("childs "))
                {
                    if (port.IsConnected)
                    {
                        var child = port.Connection.node as SkillNode;
                        child.Evaluate(caster);
                    }
                }
            }
        }
        finally
        {

        }
    }
}
