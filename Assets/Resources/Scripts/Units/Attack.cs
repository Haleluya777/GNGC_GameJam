using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour, ISkillCaster
{
    public SkillNodeGraph testSkill;
    [SerializeField] private GameObject parentObj;

    public void PerformAttack(InputAction.CallbackContext context)
    {
        testSkill.rootNode.Evaluate(this);

        //var bullet = LocalGameManager.instance.objectPoolManager.poolDic["Bullet"].GetGo("Bullet");
        //bullet.transform.position = Vector3.zero;
    }

    public void Dash()
    {
        Debug.Log("짜란");
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
