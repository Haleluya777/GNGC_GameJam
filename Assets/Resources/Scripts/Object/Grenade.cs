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
        rigid.AddForce(transform.forward * 3f, ForceMode.Impulse);
        DOVirtual.DelayedCall(1f, () => { if (!isReleased) Explosion(); });
    }

    public void TakeDamage(int dmg)
    {
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
}
