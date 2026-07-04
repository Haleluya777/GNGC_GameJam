using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public struct UnitData
{
    public PublicEnums.UnitType unitType;
    public int maxHp;
    public int curHp;
    public int grenadeCount;
}

public class Unit : MonoBehaviour, IDamageable
{
    public enum UnitState { Moving, Idle, Attacking }

    public UnitState state;
    public UnitData unitData;
    public Dictionary<string, StatusEffectBase> activeEffect = new Dictionary<string, StatusEffectBase>(); //유닛에 진행중인 상태 이상들. 버프/디버프 등
    public Dictionary<string, Coroutine> activeEffectCoroutines = new Dictionary<string, Coroutine>(); //상태이상 지속을 돕는 코루틴.

    [SerializeField] LayerMask groundLayer;
    [SerializeField] Camera cam;
    [SerializeField] private Animator anim;
    public Vector2 mouseScreenPos;
    public Vector3 dir;
    public Vector3 mouseTargetPos;

    public Movement movement;

    public bool isAttacking;
    [SerializeField] private float time;

    public bool useKnife;
    public bool useDash;

    void Awake()
    {
        foreach (IDataInitializable child in GetComponentsInChildren<IDataInitializable>())
        {
            child.DataInitialize();
        }

        useKnife = true;
        useDash = true;
    }

    public void TakeDamage(int dmg)
    {
        unitData.curHp -= dmg;
        if (unitData.curHp <= 0) Dead();
    }

    public void Dead()
    {
        this.gameObject.SetActive(false);
    }

    void Update()
    {
        if (unitData.unitType == PublicEnums.UnitType.Player)
        {
            Ray ray = cam.ScreenPointToRay(mouseScreenPos);
            if (!isAttacking) transform.localScale = dir.x >= 0 ? new Vector3(2, 2, 2) : new Vector3(-2, 2, 2);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer))
            {
                mouseTargetPos = hit.point;
                dir = mouseTargetPos - this.gameObject.transform.position;
                dir.y = 0;
            }
        }

        if (unitData.grenadeCount < 2) time += Time.deltaTime;
        if (time >= 5f)
        {
            unitData.grenadeCount++;
            time = 0;
        }

        SetStateAnim();
    }

    public void SetStateAnim()
    {
        switch (state)
        {
            case UnitState.Idle:
                anim.CrossFade("Player Animation", 0f);
                break;

            case UnitState.Moving:
                anim.CrossFade("Player_Walking", 0f);
                break;
        }
    }

    public void MousePosition(InputAction.CallbackContext context)
    {
        mouseScreenPos = context.ReadValue<Vector2>();
    }

    public void AddEffectProcess(StatusEffectBase effect)
    {
        if (!this.gameObject.activeSelf) return;
        ApplyEffect(effect);
    }

    private void ApplyEffect(StatusEffectBase effect) //상태 이상 적용.
    {
        //적용하려는 상태이상이 이미 존재하고 있는 경우. 적용되어 있는 상태이상 제거 후 다시 재적용.
        if (activeEffect.TryGetValue(effect.effectName, out StatusEffectBase exisitngEffect))
        {
            if (activeEffectCoroutines.TryGetValue(effect.effectName, out Coroutine runningCoroutine))
            {
                //코루틴을 이용한 상태이상의 타이머 제거.
                LocalGameManager.instance.coroutineRunner.StopCoroutine(runningCoroutine);
            }
            //적용 되어 있는 상태이상 또한 제거.
            exisitngEffect.RemoveEffect(true);
        }

        activeEffect[effect.effectName] = effect;
        effect.ApplyEffect();

        if (effect.duration > 0)
        {
            Coroutine newCoroutine = LocalGameManager.instance.coroutineRunner.StartRunnerCoroutine(RemoveEffectAfterDuration(effect));
            activeEffectCoroutines[effect.effectName] = newCoroutine;
        }
    }

    public void RemoveEffect(string effectName)
    {
        if (activeEffect.TryGetValue(effectName, out StatusEffectBase effect))
        {
            if (activeEffectCoroutines.TryGetValue(effectName, out Coroutine runningCoroutine))
            {
                LocalGameManager.instance.coroutineRunner.StopCoroutine(runningCoroutine);
                activeEffectCoroutines.Remove(effectName);
            }

            effect.RemoveEffect();
            activeEffect.Remove(effectName);
        }
    }

    public bool FindEffect(string effectName) //해당 이름의 상태 이상이 존재하는지 체크.
    {
        return activeEffect.ContainsKey(effectName);
    }

    IEnumerator RemoveEffectAfterDuration(StatusEffectBase effect) //상태 이상 삭제.
    {
        yield return new WaitForSeconds(effect.duration);
        effect.RemoveEffect();
        activeEffect.Remove(effect.effectName);
        activeEffectCoroutines.Remove(effect.effectName);
        //GameManager.instance.uIManager.combatUI.RemoveEffectUI(effect.effectName);
        //GameManager.instance.uIManager.combatUI.UpdateEffectUI();
    }
}
