using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillCaster
{
    // int TotalDmg { get; set; }
    // bool Attacking { get; set; }

    void PlayAnimation(string animName);
    int GetAttackPower();
    Vector3 GetDirection();
    Vector3 GetMousePosition();
    Vector2 GetPosition();
    Vector2 GetShootingPos();
    GameObject GetGameObject();
    GameObject GetShootObj();
    string GetTag();
    T GetCom<T>();
}
