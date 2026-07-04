using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ButtonGlitchEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Tooltip("마우스를 올렸을 때 재생할 글리치 스프라이트 목록")]
    public List<Sprite> glitchSprites;

    [Tooltip("프레임 당 대기 시간 (초)")]
    public float secondsPerFrame = 0.1f;

    private Image imageComponent;
    private Sprite originalSprite;
    private Coroutine animationCoroutine;

    void Awake()
    {
        imageComponent = GetComponent<Image>();
        originalSprite = imageComponent.sprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 마우스가 들어오면 애니메이션 시작
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        animationCoroutine = StartCoroutine(AnimateSprites());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // 마우스가 나가면 애니메이션 중지 및 원본 복원
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
        imageComponent.sprite = originalSprite;
    }

    private IEnumerator AnimateSprites()
    {
        if (glitchSprites == null || glitchSprites.Count == 0)
        {
            yield break;
        }

        int currentIndex = 0;
        while (true)
        {
            imageComponent.sprite = glitchSprites[currentIndex];
            yield return new WaitForSeconds(secondsPerFrame);
            currentIndex = (currentIndex + 1) % glitchSprites.Count;
        }
    }
}
