using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet : SkillObjBase
{
    [SerializeField] private Rigidbody rigid;
    private Vector2 currentDir;
    private Tween delayCall;
    private float moveSpeed = 10f;
    public bool isReleased;

    void FixedUpdate()
    {
        ObjMovement();
    }

    public override void ObjMovement()
    {
        rigid.MovePosition(rigid.position + transform.forward * Time.deltaTime * 15f);
    }

    public override void ObjInit(Vector3 dir, string _tag, ISkillCaster _caster)
    {
        isReleased = false;
        this.gameObject.tag = _tag;
        this.caster = _caster;

        this.transform.rotation = Quaternion.LookRotation(dir.normalized);
        if (delayCall != null && delayCall.IsActive()) delayCall.Kill();
        delayCall = DOVirtual.DelayedCall(10f, () => { if (!isReleased) ReturnToPool(); });
    }

    public void ReturnToPool()
    {
        this.ReleaseObject();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable) && other.tag != this.gameObject.tag)
        {
            damageable.TakeDamage(1);
            isReleased = true;
            ReturnToPool();
        }
    }
}
