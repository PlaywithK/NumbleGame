using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class LogoBounce : MonoBehaviour, IPointerClickHandler
{
    [Header("Bounce Settings")]
    [SerializeField] private float _speed = 250f;
    [SerializeField]
    private Color[] _bounceColors = new Color[]
    {
        Color.red, Color.yellow, Color.green, Color.cyan, Color.blue, Color.magenta
    };
    [SerializeField] private bool _randomColorOrder = false;
    public bool isBouncing = false;

    private RectTransform _boundsContainer;
    private TextMeshProUGUI _tmp;
    private RectTransform _rect;
    private Vector2 _velocity;
    private Vector2 _originalPosition;
    private int _colorIndex = 0;

    private void Awake()
    {
        _tmp = GetComponent<TextMeshProUGUI>();
        _rect = GetComponent<RectTransform>();

        Canvas canvas = GetComponentInParent<Canvas>();
        _boundsContainer = canvas.GetComponent<RectTransform>();

        _originalPosition = _rect.anchoredPosition;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isBouncing)
        {
            StartBounce();
        }
    }

    private void StartBounce()
    {
        isBouncing = true;

        //Zufällige Startrichtung
        float dirX = Random.value < 0.5f ? -1f : 1f;
        float dirY = Random.value < 0.5f ? -1f : 1f;
        _velocity = new Vector2(dirX, dirY).normalized * _speed;

        ApplyNextColor();
    }

    public void StopBounce()
    {
        isBouncing = false;
        _rect.anchoredPosition = _originalPosition;
        _tmp.color = new Color(1f, 0.55f, 0.0f, 1f);
    }

    private void Update()
    {
        if (!isBouncing || _boundsContainer == null) return;

        Vector2 pos = _rect.anchoredPosition;
        pos += _velocity * Time.deltaTime;

        Vector2 size = _rect.rect.size;
        Vector2 halfSize = Vector2.Scale(size, _rect.pivot);
        Vector2 halfSizeInv = size - halfSize;


        Rect boundsRect = _boundsContainer.rect;

        float minX = boundsRect.xMin + halfSize.x;
        float maxX = boundsRect.xMax - halfSizeInv.x;
        float minY = boundsRect.yMin + halfSize.y;
        float maxY = boundsRect.yMax - halfSizeInv.y;

        bool bounced = false;

        if (pos.x <= minX)
        {
            pos.x = minX;
            _velocity.x = Mathf.Abs(_velocity.x);
            bounced = true;
        }
        else if (pos.x >= maxX)
        {
            pos.x = maxX;
            _velocity.x = -Mathf.Abs(_velocity.x);
            bounced = true;
        }

        if (pos.y <= minY)
        {
            pos.y = minY;
            _velocity.y = Mathf.Abs(_velocity.y);
            bounced = true;
        }
        else if (pos.y >= maxY)
        {
            pos.y = maxY;
            _velocity.y = -Mathf.Abs(_velocity.y);
            bounced = true;
        }

        _rect.anchoredPosition = pos;

        if (bounced)
        {
            ApplyNextColor();
        }
    }

    private void ApplyNextColor()
    {
        if (_bounceColors == null || _bounceColors.Length == 0) return;

        Color next;
        if (_randomColorOrder)
        {
            next = _bounceColors[Random.Range(0, _bounceColors.Length)];
        }
        else
        {
            next = _bounceColors[_colorIndex % _bounceColors.Length];
            _colorIndex++;
        }

        _tmp.color = next;
    }
}
