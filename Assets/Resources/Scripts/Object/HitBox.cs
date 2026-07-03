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
        if (other.gameObject.layer == 10) //히트박스가 총알에 맞을 때.
        {
            if (other.gameObject.tag != this.gameObject.tag) //근데 내 총알이 아닐 때.
            {
                //무지개반사. 
                other.gameObject.tag = this.gameObject.tag;
                var casterTransform = other.gameObject.GetComponent<SkillObjBase>().caster.GetCom<Transform>();

                if (casterTransform == null) return;

                Vector3 dir = casterTransform.position - caster.GetGameObject().transform.position;
                dir.y = 0f;
                other.gameObject.transform.rotation = Quaternion.LookRotation(dir.normalized);
            }
        }
        else //총알을 제외한 나머지
        {
            if (this.gameObject.tag != other.tag)
            {
                Debug.Log("강타");
                other.TryGetComponent<IDamageable>(out var damageable);
                damageable.TakeDamage(2);
            }
        }
    }
}
