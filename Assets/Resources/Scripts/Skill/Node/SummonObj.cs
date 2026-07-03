using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;

[CreateNodeMenu("Skill/SummonObject")]
public class SummonObj : SkillNode
{
    //[Input] public Vector2 summonPos;
    public string objName;

    public float weight;
    [Input] public Vector3 summonPos;

    public override void Evaluate(ISkillCaster caster)
    {
        var dmg = (int)(weight * caster.GetAttackPower());
        var obj = LocalGameManager.instance.objectPoolManager.poolDic["Bullet"].GetGo(objName);

        //obj.transform.SetParent(caster.GetGameObject().transform);
        obj.transform.position = caster.GetGameObject().transform.position;

        obj.GetComponent<SkillObjBase>().ObjInit(caster.GetDirection(), dmg, 0, caster.GetTag(), caster);
    }
}
