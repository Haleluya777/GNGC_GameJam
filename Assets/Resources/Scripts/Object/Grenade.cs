using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Grenade : SkillObjBase, IDamageable
{
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private float explosionRadius = 3f; // 폭발 반경
    [SerializeField] private GameObject explosionVfx; // 폭발 VFX 프리팹
    [SerializeField] private GameObject reflectionVfx; // 반사 VFX 프리팹
    [SerializeField] private float emissionMultiplier = 1.0f; // 파티클 양 조절 변수

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
        // VFX 생성
        if (reflectionVfx != null)
        {
            Instantiate(reflectionVfx, transform.position, Quaternion.identity);
        }

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
        Explosion();
    }

    public void Explosion()
    {
        // 이미 폭발했거나 풀에 반환된 상태라면 중복 실행 방지
        if (isReleased) return;
        isReleased = true;

        Debug.Log("뿜!");
        SoundManager.instance.PlaySfx("수류탄");

        // VFX 생성 및 조절
        if (explosionVfx != null)
        {
            GameObject vfxInstance = Instantiate(explosionVfx, transform.position, Quaternion.identity);
            
            // 1. 수류탄의 스케일을 VFX에 적용
            vfxInstance.transform.localScale = transform.localScale;

            // 2. 파티클 양 조절
            var particleSystems = vfxInstance.GetComponentsInChildren<ParticleSystem>();
            foreach (var ps in particleSystems)
            {
                var emission = ps.emission;
                emission.rateOverTimeMultiplier *= emissionMultiplier;

                int burstCount = emission.burstCount;
                if (burstCount > 0)
                {
                    var bursts = new ParticleSystem.Burst[burstCount];
                    emission.GetBursts(bursts);
                    for (int i = 0; i < burstCount; i++)
                    {
                        // 구조체를 직접 수정하는 대신, 복사 후 재할당합니다.
                        var curve = bursts[i].count;
                        curve.constant *= emissionMultiplier;
                        curve.constantMin *= emissionMultiplier;
                        curve.constantMax *= emissionMultiplier;
                        bursts[i].count = curve;
                    }
                    emission.SetBursts(bursts);
                }
            }
        }

        // 지정된 반경 내의 모든 콜라이더를 찾습니다.
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (var col in colliders)
        {
            // IDamageable 인터페이스를 가진 컴포넌트를 찾습니다.
            if (col.TryGetComponent<IDamageable>(out var damageable))
            {
                // 수류탄 자기 자신에게는 데미지를 주지 않습니다.
                if ((MonoBehaviour)damageable != this)
                {
                    damageable.TakeDamage(3);
                }
            }
        }
        
        this.ReleaseObject();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 6)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;

            // 1초 뒤에 폭발 (이미 폭발했다면 실행되지 않음)
            DOVirtual.DelayedCall(1f, () => Explosion());
        }
    }
}
