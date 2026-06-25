using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;

public class TerminalButtons : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
    [Header("Button Target")]
    public RectTransform target;

    [Header("Settings")]
    public float hoverScale = 1.05f;
    public float pressedScale = 0.9f;
    public float pressDuration = 0.07f;
    public float releaseDuration = 0.15f;

    private Vector3 originalScale;
    private bool isPressed = false;

    void Start()
    {
        if (target == null)
            target = GetComponent<RectTransform>();

        originalScale = target.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        target.DOKill();
        target.DOScale(originalScale * pressedScale, pressDuration).SetEase(Ease.OutQuad);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        target.DOKill();
        target.DOScale(originalScale, releaseDuration).SetEase(Ease.OutBack);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isPressed)
        {
            target.DOKill();
            target.DOScale(originalScale, releaseDuration).SetEase(Ease.OutBack);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isPressed)
        {
            target.DOKill();
            target.DOScale(originalScale * hoverScale, 0.15f).SetEase(Ease.OutQuad);
        }
    }
}
