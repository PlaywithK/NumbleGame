using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonComponent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler, ISubmitHandler
{
    [Header("Scale")]
    [SerializeField] private Vector3 _hoverScale = Vector3.one * 1.1f;
    [SerializeField] private float _hoverDuration = 0.18f;

    [Header("Rotation")]
    [SerializeField] private bool _useRotation = true;
    [SerializeField] private float _rotationAngle = 4f;
    [SerializeField] private float _rotationDuration = 0.12f;

    [Header("Pulsing")]
    [SerializeField] private Vector3 _pulseScale = Vector3.one * 1.05f;
    [SerializeField] private float _pulseDuration = 0.5f;

    [Header("Click")]
    [SerializeField] private float _pressedScale = 0.95f;
    [SerializeField] private float _pressDuration = 0.08f;
    [SerializeField] private float _releaseDuration = 0.12f;
    [SerializeField] private string _clickSfxName = "ButtonClick";

    private Vector3 _originalScale;
    private Quaternion _originalRotation;
    private Tween _rotationTween;
    private Tween _pulseTween;
    private bool _isHovered;
    private bool _isPressed;
    private Vector3 _pressedScaleVector;

    private void Awake()
    {
        _originalScale = transform.localScale;
        _originalRotation = transform.localRotation;
        _hoverScale = new Vector3(
            _originalScale.x * _hoverScale.x,
            _originalScale.y * _hoverScale.y,
            _originalScale.z * _hoverScale.z
        );
        _pulseScale = new Vector3(
            _originalScale.x * _pulseScale.x,
            _originalScale.y * _pulseScale.y,
            _originalScale.z * _pulseScale.z
        );
        _pressedScaleVector = new Vector3(
            _originalScale.x * _pressedScale,
            _originalScale.y * _pressedScale,
            _originalScale.z * _pressedScale
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HandleHoverEnter();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HandleHoverExit();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        HandlePressStart();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        HandlePressEnd();
    }

    public void OnSelect(BaseEventData eventData)
    {
        HandleHoverEnter();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        HandleHoverExit();
    }

    public void OnSubmit(BaseEventData eventData)
    {
        HandlePressStart();
        DOVirtual.DelayedCall(_pressDuration, HandlePressEnd);
    }

    private void HandleHoverEnter()
    {
        if (_isHovered) return;

        PlaySfx("ButtonHover");

        _isHovered = true;
        StopHoverTweens();

        transform.DOKill();
        transform.DOScale(_hoverScale, _hoverDuration).SetEase(Ease.OutBack);
        StartRotation();
    }

    private void HandleHoverExit()
    {
        _isHovered = false;
        _isPressed = false;
        StopHoverTweens();

        transform.DOKill();
        transform.DOScale(_originalScale, _hoverDuration).SetEase(Ease.OutQuad);
        transform.DOLocalRotateQuaternion(_originalRotation, _hoverDuration).SetEase(Ease.OutQuad);
    }

    private void HandlePressStart()
    {
        if (_isPressed) return;

        _isPressed = true;
        transform.DOKill();
        transform.DOScale(_pressedScaleVector, _pressDuration).SetEase(Ease.OutQuad);

        PlaySfx(_clickSfxName);
    }

    private void HandlePressEnd()
    {
        if (!_isPressed) return;

        _isPressed = false;
        transform.DOKill();
        transform.DOScale(_isHovered ? _hoverScale : _originalScale, _releaseDuration).SetEase(Ease.OutBack);
    }

    private void PlaySfx(string soundName)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(soundName);
        }
    }

    private void StartRotation()
    {
        if (!_useRotation)
        {
            StartPulse();
            return;
        }

        _rotationTween?.Kill();
        _rotationTween = DOTween.Sequence()
            .Append(transform.DOLocalRotate(new Vector3(0, 0, _rotationAngle), _rotationDuration)
                .SetEase(Ease.OutQuad)
                .SetRelative(true))
            .Append(transform.DOLocalRotateQuaternion(_originalRotation, _rotationDuration)
                .SetEase(Ease.InOutQuad))
            .OnComplete(() =>
            {
                if (!_isHovered) return;
                StartPulse();
            });
    }

    private void StartPulse()
    {
        _pulseTween?.Kill();
        _pulseTween = transform.DOScale(_pulseScale, _pulseDuration).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    private void StopHoverTweens()
    {
        _pulseTween?.Kill();
        _rotationTween?.Kill();
    }

    private void OnDisable()
    {
        StopHoverTweens();
        transform.DOKill();
        transform.localScale = _originalScale;
        transform.localRotation = _originalRotation;
    }
}
