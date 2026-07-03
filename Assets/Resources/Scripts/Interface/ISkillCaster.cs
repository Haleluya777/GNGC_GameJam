using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISkillCaster
{
    // int TotalDmg { get; set; }
    // bool Attacking { get; set; }

    void PlayAnimation(string animName);
    int GetAttackPower();
    Vector2 GetDirection();
    Vector2 GetPosition();
    Vector2 GetShootingPos();
    string GetTag();
    T GetCom<T>();
}
