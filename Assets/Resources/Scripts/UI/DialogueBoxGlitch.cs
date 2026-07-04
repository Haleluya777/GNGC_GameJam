using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DialogueBoxGlitch : MonoBehaviour
{
    [Tooltip("글리치 애니메이션에 사용할 스프라이트 목록")]
    public List<Sprite> glitchSprites;

    [Tooltip("프레임 당 대기 시간 (초)")]
    public float secondsPerFrame = 1.0f;

    private Image imageComponent;
    private Coroutine animationCoroutine;
    private Sprite originalSprite; // 원본 스프라이트를 저장할 변수

    void Awake()
    {
        imageComponent = GetComponent<Image>();
        originalSprite = imageComponent.sprite; // 시작할 때 원본 스프라이트 저장
    }

    /// <summary>
    /// 글리치 애니메이션을 시작합니다.
    /// </summary>
    public void StartGlitch()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
        }
        animationCoroutine = StartCoroutine(AnimateSprites());
    }

    /// <summary>
    /// 글리치 애니메이션을 중지하고 원래 스프라이트로 되돌립니다.
    /// </summary>
    public void StopGlitch()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }

        // 원본 스프라이트로 복원
        if (imageComponent != null && originalSprite != null)
        {
            imageComponent.sprite = originalSprite;
        }
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