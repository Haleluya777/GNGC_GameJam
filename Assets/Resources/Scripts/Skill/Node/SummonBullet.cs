using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;

[CreateNodeMenu("Skill/SummonBullet")]
public class SummonBullet : SkillNode
{
    //[Input] public Vector2 summonPos;
    public string objName;
    [Input] public Vector3 summonPos;

    public override void Evaluate(ISkillCaster caster)
    {
        var dmg = caster.GetAttackPower();
        var obj = LocalGameManager.instance.objectPoolManager.poolDic["Bullet"].GetGo(objName);

        //obj.transform.SetParent(caster.GetGameObject().transform);
        obj.transform.position = caster.GetGameObject().transform.position;

        obj.GetComponent<SkillObjBase>().ObjInit(caster.GetDirection(), caster.GetTag(), caster);
    }
}
