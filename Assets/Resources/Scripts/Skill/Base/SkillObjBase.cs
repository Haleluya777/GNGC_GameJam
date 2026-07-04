using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillObjBase : PoolAble
{
    public ISkillCaster caster;

    public abstract void ObjMovement();
    public virtual void ObjInit(Vector3 dir, string _tag, ISkillCaster _caster)
    {

    }
    public virtual void ObjInit(string _tag, ISkillCaster _caster, Vector3 targetPos)
    {

    }

    public virtual void Refelection(ISkillCaster newCaster, Vector3 targetPos, float duration)
    {

    }

    public virtual void Refelection(Vector3 dir, ISkillCaster newCaster)
    {

    }

    // public virtual void ObjInit(Vector2 dir, int _dmg, int _stunDmg, string _tag, ISkillCaster _caster, bool _reinforced, SkillBase parentSkill)
    // {
    //     ObjInit(dir, _dmg, _stunDmg, _tag, _caster, _reinforced);
    // }
}
