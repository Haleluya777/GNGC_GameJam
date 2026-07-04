using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateNodeMenu("Skill/BossMove")]
public class BossMove : SkillNode
{
    Unit targetUnit;
    public float dashDuration;
    public float dashDistance = 5f;
    public bool reverse;
    public override void Evaluate(ISkillCaster caster)
    {
        Debug.Log("할렐루야");
        targetUnit = LocalGameManager.instance.unitManager.playerUnit;

        var rigid = caster.GetCom<Rigidbody>();
        var col = caster.GetCom<BoxCollider>();

        Vector2 dir = (targetUnit.transform.position - rigid.position).normalized;
        Vector3 moveVec = new Vector3(dir.x, 0, dir.y);

        // reverse 값에 따라 최종 목표 위치를 계산합니다.
        Vector3 targetPos = reverse
            ? rigid.position - moveVec * dashDistance // 반대 방향
            : rigid.position + moveVec * dashDistance; // 정방향

        Debug.Log($"목적지 : {targetPos}");
        var virtualPos = rigid.position;
        Sequence dashSequence = DOTween.Sequence();

        dashSequence.Append(DOTween.To(() => virtualPos, x => virtualPos = x, targetPos, dashDuration).SetEase(Ease.Linear));

        dashSequence.OnUpdate(() =>
        {
            if (Physics.CheckBox(virtualPos + col.center, col.size / 2, Quaternion.identity, 11))
            {
                dashSequence.Kill();
                return;
            }

            rigid.MovePosition(virtualPos);
        });

        dashSequence.SetUpdate(UpdateType.Fixed);
    }
}
