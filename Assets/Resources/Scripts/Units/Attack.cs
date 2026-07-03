using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour, ISkillCaster, IDataInitializable
{
    public SkillModule shootSkill;
    public SkillModule dashSkill;
    [SerializeField] private GameObject parentObj;
    private Animator anim;

    void LateUpdate()
    {
        shootSkill.UpdateCoolDown(Time.deltaTime);
        dashSkill.UpdateCoolDown(Time.deltaTime);
    }

    public void DataInitialize()
    {
        anim = parentObj.GetComponent<Animator>();

        shootSkill = Instantiate(shootSkill);
        dashSkill = Instantiate(dashSkill);

        shootSkill.InitSkill();
        dashSkill.InitSkill();
    }

    public void ProccessCoolDown()
    {

    }

    public void PerformAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            shootSkill.UseSKill(this);
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            dashSkill.UseSKill(this);
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
        return "";
    }

    public T GetCom<T>() => parentObj.GetComponentInChildren<T>();
}
