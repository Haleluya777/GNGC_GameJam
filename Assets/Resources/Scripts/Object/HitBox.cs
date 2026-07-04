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
        if (other.gameObject.layer == 10 || other.gameObject.layer == 9) //히트박스가 총알에 맞을 때.
        {
            Debug.Log($"--- HitBox 감지 (프레임: {Time.frameCount}) ---");
            Debug.Log($"충돌한 오브젝트: {other.name}, 태그: {other.tag}");
            if (other.gameObject.tag != this.gameObject.tag) //근데 내 총알이 아닐 때.
            {
                //무지개반사. 
                //other.gameObject.tag = this.gameObject.tag;

                var skillObj = other.gameObject.GetComponent<SkillObjBase>();
                var casterTransform = skillObj.caster.GetCom<Transform>();
                if (casterTransform == null) return;

                Vector3 dir = casterTransform.position - caster.GetGameObject().transform.position;
                dir.y = 0f;

                if (other.gameObject.layer == 10) //불렛
                {
                    skillObj.ObjInit(dir, this.gameObject.tag, caster);
                }
                else if (other.gameObject.layer == 9) //그라나도
                {
                    //if (other.GetComponent<Rigidbody>().velocity == Vector3.zero) return; //멈춰 있는 오브젝트는 무지개 반사 안통함.
                    skillObj.Refelection(caster, casterTransform.position, 1f);
                }
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
