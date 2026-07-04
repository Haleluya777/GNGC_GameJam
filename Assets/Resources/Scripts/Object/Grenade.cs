using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Grenade : SkillObjBase, IDamageable
{
    [SerializeField] private Rigidbody rigid;
    private bool isReleased;

    private Vector3 startPoint, endPoint, gravity;
    private Vector3 launchVelocity;
    private float duration;

    public override void ObjMovement()
    {
        rigid.AddForce(launchVelocity, ForceMode.VelocityChange);
    }

    public override void ObjInit(string _tag, ISkillCaster _caster, Vector3 targetPos)
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;

        isReleased = false;
        duration = 1f;

        caster = _caster;
        startPoint = transform.position;
        endPoint = targetPos;
        gravity = Physics.gravity;

        Vector3 displacement = endPoint - startPoint;
        launchVelocity = (displacement / duration) - (.5f * gravity * duration);

        ObjMovement();
    }

    public override void Refelection(ISkillCaster newCaster, Vector3 targetPos, float duration)
    {
        //if (rigid.velocity == Vector3.zero) return;
        Debug.Log("반사중");
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;

        caster = newCaster;

        startPoint = transform.position;
        endPoint = targetPos;
        this.duration = duration;
        gravity = Physics.gravity;

        Vector3 displacement = endPoint - startPoint;
        launchVelocity = (displacement / duration) - (.5f * gravity * duration);

        ObjMovement();
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
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;

            DOVirtual.DelayedCall(1f, () => { if (!isReleased) Explosion(); });
        }
    }
}
