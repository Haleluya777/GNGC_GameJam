using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateNodeMenu("Skill/SummonGrenadeEnemy")]
public class SummonGrenadeEnemy : SkillNode
{
    public string objName;

    public override void Evaluate(ISkillCaster caster)
    {
        var dmg = caster.GetAttackPower();
        var obj = LocalGameManager.instance.objectPoolManager.poolDic["Bullet"].GetGo(objName);

        obj.transform.position = caster.GetShootObj().transform.position;
        Debug.Log($"알랄랄라 : {LocalGameManager.instance.unitManager.playerUnit.transform.position}");
        obj.GetComponent<SkillObjBase>().ObjInit(caster.GetTag(), caster, LocalGameManager.instance.unitManager.playerUnit.transform.position);
    }
}
