using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateNodeMenu("Skill/TripleShoot")]
public class TripleShooting : SkillNode
{
    private WaitForSeconds wait = new WaitForSeconds(.25f);

    public override void Evaluate(ISkillCaster caster)
    {
        LocalGameManager.instance.coroutineRunner.StartCoroutine(Triple(caster));
    }

    private IEnumerator Triple(ISkillCaster caster)
    {
        for (int i = 0; i < 3; i++)
        {
            var obj = LocalGameManager.instance.objectPoolManager.poolDic["Bullet"].GetGo("Bullet");
            obj.transform.position = caster.GetShootObj().transform.position;
            obj.GetComponent<SkillObjBase>().ObjInit(caster.GetDirection(), caster.GetTag(), caster);
            yield return wait;
        }
    }
}
