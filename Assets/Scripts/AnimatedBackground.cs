using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.InputSystem;

public class AnimatedBackground : MonoBehaviour
{
    [Header("Gradient Pulse")]
    [SerializeField] private RawImage _rawImage;

    public Color colorTop = new Color(0.05f, 0.02f, 0.0f, 1f); //Black (Orange Tint)
    public Color colorBottom = new Color(1f, 0.55f, 0.0f, 1f); //Orange

    [Range(0.1f, 2f)] public float pulseSpeed = 0.65f;
    [Range(0f, 0.3f)] public float pulseIntensity = 0.25f;

    private Texture2D _gradientTexture;
    private float _time;

    [Header("Floating Numbers")]
    public Color numberColor = new Color(1f, 1f, 1f, 0f); //White
    [SerializeField] private int maxNumbers = 30;
    [SerializeField] private float spawnInterval = 0.25f;
    [SerializeField] private float minFontSize = 55f;
    [SerializeField] private float maxFontSize = 125f;
    [SerializeField] private float minAlpha = 0.06f;
    [SerializeField] private float maxAlpha = 0.22f;
    [SerializeField] private float minSpeed = 10f;
    [SerializeField] private float maxSpeed = 40f;
    [SerializeField] private float minLifetime = 5f;
    [SerializeField] private float maxLifetime = 25f;

    private RectTransform canvasRect;
    private readonly List<FloatData> _active = new();
    private readonly Queue<TextMeshProUGUI> _pool = new();
    private Transform _numberContainer;
    private float _spawnTimer;

    [Header("Mouse Interaction")]
    [SerializeField] private float mouseInfluenceRadius = 200f;
    [SerializeField] private float mouseForce = 80f;
    [SerializeField] private bool mouseRepels = true;

    struct FloatData
    {
        public TextMeshProUGUI tmp;
        public Vector2 velocity;
        public float lifetime;
        public float age;
        public float targetAlpha;
        public float fadeSpeed;
        public float fadeInDuration;
        public float currentAlpha;
        public float alphaDir;
    }

    void Start()
    {
        canvasRect = GetComponentInParent<Canvas>()?.GetComponent<RectTransform>();

        //Gradient Setup
        _gradientTexture = new Texture2D(1, 256, TextureFormat.RGBA32, false)
        {
            wrapMode = TextureWrapMode.Clamp,
            filterMode = FilterMode.Bilinear
        };
        ApplyGradient(0f);

        //Number Setup
        var go = new GameObject("_NumberContainer");
        go.transform.SetParent(transform.parent, false);
        _numberContainer = go.transform;

        for (int i = 0; i < maxNumbers; i++)
            _pool.Enqueue(CreateText());

        // Sofort alle Zahlen initial spawnen
        for (int i = 0; i < maxNumbers; i++)
            SpawnNumber();
    }

    void Update()
    {
        UpdateGradient();
        UpdateNumbers();
    }

    //Gradient
    void UpdateGradient()
    {
        _time += Time.deltaTime * pulseSpeed;
        float pulse = Mathf.Sin(_time) * pulseIntensity;
        ApplyGradient(pulse);
    }


    void ApplyGradient(float pulse)
    {
        Color[] pixels = new Color[256];
        for (int i = 0; i < 256; i++)
        {
            float t = i / 255f;
            float curved = Mathf.Pow(t, 1.5f + pulse);
            pixels[i] = Color.Lerp(colorBottom, colorTop, curved);
        }
        _gradientTexture.SetPixels(pixels);
        _gradientTexture.Apply();
        _rawImage.texture = _gradientTexture;
    }

    void OnDestroy()
    {
        if (_gradientTexture != null)
        {
            Destroy(_gradientTexture);
        }
    }

    //Numbers
    void UpdateNumbers()
    {
        _spawnTimer += Time.deltaTime;
        if (_spawnTimer >= spawnInterval && _active.Count < maxNumbers)
        {
            _spawnTimer = 0f;
            SpawnNumber();
        }

        //Mausposition
        Vector2 mouseLocalPos = Vector2.zero;
        if (Mouse.current != null)
        {
            Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect, mouseScreenPos, null, out mouseLocalPos
            );
        }

        for (int i = _active.Count - 1; i >= 0; i--)
        {
            var d = _active[i];
            d.age += Time.deltaTime;

            //Abstoßung zwischen Zahlen
            Vector2 separation = Vector2.zero;
            foreach (var other in _active)
            {
                if (other.tmp == d.tmp) continue;
                float dist = Vector2.Distance(d.tmp.rectTransform.anchoredPosition, other.tmp.rectTransform.anchoredPosition);
                float minDist = (d.tmp.fontSize + other.tmp.fontSize) * 0.4f;
                if (dist < minDist && dist > 0.01f)
                {
                    Vector2 pushDir = (d.tmp.rectTransform.anchoredPosition - other.tmp.rectTransform.anchoredPosition).normalized;
                    separation += pushDir * (minDist - dist);
                }
            }

            //Mouse-Interaction
            Vector2 toNumber = d.tmp.rectTransform.anchoredPosition - mouseLocalPos;
            float mouseDist = toNumber.magnitude;
            Vector2 mouseEffect = Vector2.zero;
            if (mouseDist < mouseInfluenceRadius && mouseDist > 0.01f)
            {
                float strength = 1f - (mouseDist / mouseInfluenceRadius);
                Vector2 dir = toNumber.normalized;
                if (!mouseRepels) dir = -dir;
                mouseEffect = dir * strength * mouseForce;
            }

            d.tmp.rectTransform.anchoredPosition += (d.velocity + separation * 0.5f + mouseEffect) * Time.deltaTime;

            //Scale-in
            float scaleProgress = Mathf.Clamp01(d.age / d.fadeInDuration);
            float scale = Mathf.SmoothStep(0f, 1f, scaleProgress);
            d.tmp.rectTransform.localScale = Vector3.one * scale;

            // Alpha pulsieren
            d.currentAlpha += d.alphaDir * d.fadeSpeed * Time.deltaTime;
            if (d.currentAlpha >= d.targetAlpha) { d.currentAlpha = d.targetAlpha; d.alphaDir = -1f; }
            else if (d.currentAlpha <= minAlpha) { d.currentAlpha = minAlpha; d.alphaDir = 1f; }

            //Fade-out
            float alpha = d.currentAlpha;
            float remaining = d.lifetime - d.age;
            if (remaining < 1.5f) alpha *= remaining / 1.5f;
            var c = d.tmp.color;
            c.a = alpha;
            d.tmp.color = c;

            if (d.age >= d.lifetime)
            {
                ReturnToPool(d.tmp);
                _active.RemoveAt(i);
                continue;
            }
            _active[i] = d;
        }
    }
    void SpawnNumber()
    {
        if (_pool.Count == 0) return;

        var tmp = _pool.Dequeue();
        tmp.gameObject.SetActive(true);

        tmp.text = Random.Range(0, 10).ToString();
        tmp.fontSize = Random.Range(minFontSize, maxFontSize);

        float halfW = canvasRect.rect.width * 0.5f;
        float halfH = canvasRect.rect.height * 0.5f;

        // Grid-basiertes Spawning für gleichmäßige Verteilung
        int cols = 6;
        int rows = 4;
        int cellIndex = Random.Range(0, cols * rows);
        int col = cellIndex % cols;
        int row = cellIndex / cols;

        float cellW = canvasRect.rect.width / cols;
        float cellH = canvasRect.rect.height / rows;

        float x = -halfW + cellW * col + Random.Range(cellW * 0.1f, cellW * 0.9f);
        float y = -halfH + cellH * row + Random.Range(cellH * 0.1f, cellH * 0.9f);

        tmp.rectTransform.anchoredPosition = new Vector2(x, y);

        var startColor = numberColor;
        startColor.a = 0f;
        tmp.color = startColor;

        _active.Add(new FloatData
        {
            tmp = tmp,
            velocity = new Vector2(
                Random.Range(-maxSpeed, maxSpeed),
                Random.Range(-maxSpeed, maxSpeed)
            ),
            lifetime = Random.Range(minLifetime, maxLifetime),
            age = 0f,
            targetAlpha = Random.Range(minAlpha, maxAlpha),
            currentAlpha = 0f,
            fadeSpeed = Random.Range(0.008f, 0.025f),
            fadeInDuration = Random.Range(1.0f, 2.0f),
            alphaDir = 1f
        });
    }

    TextMeshProUGUI CreateText()
    {
        var go = new GameObject("FloatNum", typeof(RectTransform), typeof(TextMeshProUGUI));
        go.transform.SetParent(_numberContainer, false);
        var tmp = go.GetComponent<TextMeshProUGUI>();
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = new Color(numberColor.r, numberColor.g, numberColor.b, 0f);
        tmp.raycastTarget = false;
        go.SetActive(false);
        return tmp;
    }

    void ReturnToPool(TextMeshProUGUI tmp)
    {
        tmp.gameObject.SetActive(false);
        _pool.Enqueue(tmp);
    }
}