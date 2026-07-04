using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class Attack : MonoBehaviour, ISkillCaster, IDataInitializable
{
    public SkillModule shootSkill;
    public SkillModule dashSkill;
    public SkillModule parryingSkill;
    public SkillModule grenadeSkill;
    [SerializeField] private GameObject parentObj;
    private Animator anim;
    private Unit unit;

    void LateUpdate()
    {
        ProccessCoolDown();
    }

    public void DataInitialize()
    {
        anim = parentObj.GetComponent<Animator>();
        unit = parentObj.GetComponent<Unit>();

        shootSkill = Instantiate(shootSkill);
        dashSkill = Instantiate(dashSkill);
        parryingSkill = Instantiate(parryingSkill);
        grenadeSkill = Instantiate(grenadeSkill);

        shootSkill.InitSkill();
        dashSkill.InitSkill();
        parryingSkill.InitSkill();
        grenadeSkill.InitSkill();
    }

    public void ProccessCoolDown()
    {
        shootSkill.UpdateCoolDown(Time.deltaTime);
        dashSkill.UpdateCoolDown(Time.deltaTime);
        parryingSkill.UpdateCoolDown(Time.deltaTime);
        grenadeSkill.UpdateCoolDown(Time.deltaTime);
    }

    public void PerformAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (shootSkill == null)
            {
                Debug.Log("이 스킬은 배우지 않음");
                return;
            }
            shootSkill.UseSKill(this);
        }
    }

    public void Melee(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && unit.useKnife)
        {
            if (parryingSkill == null)
            {
                Debug.Log("스킬 없다.");
                return;
            }

            unit.isAttacking = true;
            DOVirtual.DelayedCall(.5f, () => unit.isAttacking = false);
            parryingSkill.UseSKill(this);
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && unit.useDash)
        {
            if (dashSkill == null)
            {
                Debug.Log("스킬 없다.");
                return;
            }
            dashSkill.UseSKill(this);
        }
    }

    public void Grenade(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (grenadeSkill == null)
            {
                Debug.Log("스킬 없다.");
                return;
            }
            grenadeSkill.UseSKill(this);
        }
    }

    public void PlayAnimation(string animName)
    {
        anim.CrossFade(animName, 0f);
    }

    public int GetAttackPower()
    {
        return 1;
    }

    public Vector3 GetDirection()
    {
        return parentObj.GetComponent<Unit>().dir;
    }

    public Vector3 GetMousePosition()
    {
        return parentObj.GetComponent<Unit>().mouseTargetPos;
    }

    public GameObject GetGameObject()
    {
        return parentObj;
    }

    public Vector2 GetPosition()
    {
        return Vector2.zero;
    }

    public Vector2 GetShootingPos()
    {
        return Vector2.zero;
    }

    public string GetTag()
    {
        return parentObj.tag;
    }

    public T GetCom<T>() => parentObj.GetComponentInChildren<T>();
}
