using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : SkillObjBase
{
    [SerializeField] private Rigidbody rigid;
    private Vector2 currentDir;
    private float moveSpeed = 10f;

    void FixedUpdate()
    {
        ObjMovement();
    }

    public override void ObjMovement()
    {
        rigid.MovePosition(rigid.position + transform.forward * Time.deltaTime * 10f);
    }

    public override void ObjInit(Vector3 dir, int _dmg, int _stunDmg, string _tag, ISkillCaster _caster)
    {
        this.transform.rotation = Quaternion.LookRotation(dir.normalized);
        Invoke("ReturnToPool", 3f);
    }

    public void ReturnToPool()
    {
        this.ReleaseObject();
    }

    void OnTriggerEnter(Collider other)
    {
        //this.ReleaseObject();
    }
}
