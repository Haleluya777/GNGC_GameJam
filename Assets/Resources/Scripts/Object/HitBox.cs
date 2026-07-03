using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : PoolAble
{
    private ISkillCaster caster;
    private int totalDmg;
    private float limitTime;

    public void Initialize(int damage, ISkillCaster _caster, float _limitTime)
    {
        caster = _caster;
        totalDmg = damage;
        limitTime = _limitTime;
        Invoke("ReleaseHitBox", 1f);
    }

    public void ReleaseHitBox()
    {
        this.ReleaseObject();
    }

    void OnTriggerEnter(Collider other)
    {
        if (caster == null || other.gameObject.tag == caster.GetGameObject().tag) return;
    }
}
