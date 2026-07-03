using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Grenade : SkillObjBase, IDamageable
{
    [SerializeField] private Rigidbody rigid;
    private bool isReleased;

    public override void ObjMovement()
    {

    }

    public override void ObjInit(Vector3 dir, int _dmg, int _stunDmg, string _tag, ISkillCaster _caster)
    {
        isReleased = false;
        this.transform.rotation = Quaternion.LookRotation(dir.normalized);

        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;

        rigid.AddForce((transform.forward * 6f) + (Vector3.up * 4f), ForceMode.Impulse);
    }

    public void TakeDamage(int dmg)
    {
        Debug.Log("데미지 받음");
        Dead();
    }

    public void Dead()
    {
        isReleased = true;
        Explosion();
    }

    public void Explosion()
    {
        Debug.Log("뿜!");
        this.ReleaseObject();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 6)
        {
            DOVirtual.DelayedCall(1f, () => { if (!isReleased) Explosion(); });
        }
    }
}
