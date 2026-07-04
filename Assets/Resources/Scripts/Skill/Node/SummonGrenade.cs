using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateNodeMenu("Skill/SummonGrenade")]
public class SummonGrenade : SkillNode
{
    private Unit unit;
    public string objName;

    public override void Evaluate(ISkillCaster caster)
    {
        unit = caster.GetCom<Unit>();
        var obj = LocalGameManager.instance.objectPoolManager.poolDic["Bullet"].GetGo(objName);

        obj.transform.position = caster.GetShootObj().transform.position;
        obj.GetComponent<SkillObjBase>().ObjInit(caster.GetTag(), caster, caster.GetMousePosition());
    }
}
