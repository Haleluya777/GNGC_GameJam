using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateNodeMenu("Skill/PlayerDataNode")]
public class PlayerDataNode : SkillNode
{
    private Unit unit;

    public override void Evaluate(ISkillCaster caster)
    {
        unit = caster.GetCom<Unit>();

        
    }
}
