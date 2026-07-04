using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour, IDataInitializable
{
    [SerializeField] private Slider hpBar;
    [SerializeField] private Slider dodgeSlider;
    [SerializeField] private Slider bombSlider;
    [SerializeField] private Slider parryingSlider;

    [SerializeField] Unit unit;
    [SerializeField] Attack attack;

    public void DataInitialize()
    {

    }

    void Update()
    {
        DodgeCoolDown();
        BombCoolDown();
        ParryingCoolDown();
        hpBar.value = (float)(unit.unitData.curHp / 5f);
    }

    public void DodgeCoolDown()
    {
        float value = attack.dashSkill.RemainingCoolDown / attack.dashSkill.CoolDown;
        dodgeSlider.value = value;
    }

    public void BombCoolDown()
    {
        float value = attack.grenadeSkill.RemainingCoolDown / attack.grenadeSkill.CoolDown;
        bombSlider.value = value;
    }

    public void ParryingCoolDown()
    {
        float value = attack.parryingSkill.RemainingCoolDown / attack.parryingSkill.CoolDown;
        parryingSlider.value = value;
    }
}
