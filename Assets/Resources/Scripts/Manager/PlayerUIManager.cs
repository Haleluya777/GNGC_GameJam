using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerUIManager : MonoBehaviour, IDataInitializable
{
    [SerializeField] private Slider hpBar;
    [SerializeField] private Slider dodgeSlider;
    [SerializeField] private Slider bombSlider;
    [SerializeField] private Slider parryingSlider;
    [SerializeField] private GameObject fadePanel;

    [SerializeField] Unit unit;
    [SerializeField] Attack attack;

    public void DataInitialize()
    {

    }

    // 새 플레이어 유닛으로 UI 참조를 초기화하는 메소드
    public void Initialize(Unit newPlayer)
    {
        if (newPlayer == null)
        {
            Debug.LogError("PlayerUIManager.Initialize: newPlayer is null!");
            return;
        }
        Debug.Log("PlayerUIManager.Initialize: Initializing with new player: " + newPlayer.name);

        unit = newPlayer;
        // 자식 오브젝트에 있는 Attack 컴포넌트까지 모두 검색하도록 변경합니다.
        attack = newPlayer.GetComponentInChildren<Attack>();

        // 임시로 체력바를 채우던 코드는 이제 필요 없으므로 삭제합니다.
        // 이제부터는 Update문이 정상 동작하여 체력바를 올바르게 표시합니다.

        if (attack == null)
        {
            Debug.LogError("PlayerUIManager.Initialize: Attack component not found on new player OR its children!");
        }
        else
        {
            Debug.Log("PlayerUIManager.Initialize: Successfully found Attack component in children.");
        }
    }

    void Update()
    {
        // unit이나 attack이 null일 경우를 대비 (오브젝트 파괴 시 등)
        if (unit == null || attack == null) return;

        DodgeCoolDown();
        BombCoolDown();
        ParryingCoolDown();
        hpBar.value = (float)(unit.unitData.curHp / 5f);
    }

    public void FadeInOut()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendCallback(() => fadePanel.SetActive(true));
        sequence.Append(fadePanel.GetComponent<Image>().DOColor(new Color32(0, 0, 0, 255), 0f));
        sequence.Append(fadePanel.GetComponent<Image>().DOColor(new Color32(0, 0, 0, 255), 1f));
        sequence.AppendCallback(() => fadePanel.SetActive(false));
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