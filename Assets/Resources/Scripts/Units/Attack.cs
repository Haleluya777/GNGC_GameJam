using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour, ISkillCaster
{
    public SkillNodeGraph shootSkill;
    public SkillNodeGraph dashSkill;
    [SerializeField] private GameObject parentObj;

    void Update()
    {

    }

    public void ProccessCoolDown()
    {

    }

    public void PerformAttack(InputAction.CallbackContext context)
    {
        shootSkill.rootNode.Evaluate(this);
    }

    public void Dash()
    {
        dashSkill.rootNode.Evaluate(this);
    }

    public void PlayAnimation(string animName)
    {

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
