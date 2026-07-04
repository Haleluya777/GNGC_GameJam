using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateNodeMenu("Skill/SummonGrenade")]
public class SummonGrenade : SkillNode
{
    public string objName;

    public override void Evaluate(ISkillCaster caster)
    {
        var dmg = caster.GetAttackPower();
        var obj = LocalGameManager.instance.objectPoolManager.poolDic["Bullet"].GetGo(objName);

        obj.transform.position = caster.GetGameObject().transform.position;
        obj.GetComponent<SkillObjBase>().ObjInit(caster.GetTag(), caster, caster.GetMousePosition());
    }
}
