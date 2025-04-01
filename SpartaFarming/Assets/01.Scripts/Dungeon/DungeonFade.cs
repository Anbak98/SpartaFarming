using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DungeonFade : MonoBehaviour
{
    [SerializeField]
    private Image fadeImage;
    [SerializeField]
    private float duration = 1f;
    [SerializeField]
    private Ease easeType = Ease.InOutQuad;

    private void Awake()
    {
        // 이벤트 추가 
        DungeonManager.Instance.AddEvent(FadeInOut);
    }

    private void FadeInOut() 
    {
        Color color = fadeImage.color;
        float alphaValue = color.a == 0 ? 1f : 0;

        fadeImage.DOFade(alphaValue, duration)
                .SetDelay(0.5f)
                .SetEase(easeType)
                .OnComplete(() => {
                    Debug.Log("페이드/아웃 완료!");
                });
    }
}
