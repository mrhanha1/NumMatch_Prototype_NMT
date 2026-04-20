using DG.Tweening;
using UnityEngine;

public abstract class BasePanel : MonoBehaviour
{
    protected CanvasGroup canvasGroup;
    private const float FadeDuration = 0.25f;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
        canvasGroup.DOFade(1f, FadeDuration);
    }

    public virtual void Hide()
    {
        //canvasGroup.DOFade(0f, FadeDuration)
        //    .OnComplete(() => gameObject.SetActive(false));
        gameObject.SetActive(false);
    }
}