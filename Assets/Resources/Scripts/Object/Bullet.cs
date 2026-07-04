using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet : SkillObjBase
{
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private GameObject reflectionVfx; // 반사 VFX 프리팹
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

    public override void Refelection(Vector3 dir, ISkillCaster newCaster)
    {
        // VFX 생성
        if (reflectionVfx != null)
        {
            Instantiate(reflectionVfx, transform.position, Quaternion.identity);
        }

        // 기존 소멸 로직 중지
        if (delayCall != null && delayCall.IsActive()) delayCall.Kill();

        // 새로운 캐스터와 태그, 방향으로 재설정
        ObjInit(dir, newCaster.GetTag(), newCaster);
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