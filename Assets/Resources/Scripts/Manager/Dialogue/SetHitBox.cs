using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;

[CreateNodeMenu("Skill/SetHitBox")]
public class SetHitBox : SkillNode
{
    [Output(dynamicPortList = true)] public List<SkillNode> childs;

    [Header("판정 지속 시간")]
    public float duration;

    [Input] public Vector3 size;
    [Input] public Vector3 pos;

    public override void Evaluate(ISkillCaster caster)
    {
        size = GetInputValue<Vector3>("size", this.size);
        pos = GetInputValue<Vector3>("pos", this.pos);

        //데미지 설정.
        int totalDmg = caster.GetAttackPower();

        //히트박스 생성 및 위치, 크기, 각도, 태그 설정.
        GameObject hitBox = LocalGameManager.instance.objectPoolManager.poolDic["HitBox"].GetGo("HitBox");

        hitBox.transform.SetParent(caster.GetGameObject().transform.GetChild(2).transform.GetChild(0));

        HitBox hitBoxCom = hitBox.GetComponent<HitBox>();
        hitBox.tag = caster.GetGameObject().tag;

        hitBox.transform.localScale = size;
        hitBox.transform.localPosition = pos;

        hitBoxCom.Initialize(totalDmg, caster, duration);

        //자식 노드가 존재할 경우 실행.
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
}
