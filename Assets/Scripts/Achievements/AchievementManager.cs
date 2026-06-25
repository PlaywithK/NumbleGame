using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;

    [Header("Achievements")]
    public List<Achievement> achievements;

    [Header("UI")]
    public GameObject popupPrefab;
    private GameObject popupInstance;
    private CanvasGroup popupCanvasGroup;
    public Sprite defaultIcon;
    private Image popupIcon;
    private TextMeshProUGUI popupTitle;
    private TextMeshProUGUI popupDescription;

    [Header("Einstellungen")]
    public float popupDuration = 3f;
    private bool isShowingPopup = false;
    private Queue<Achievement> popupQueue = new Queue<Achievement>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Canvas canvas = FindAnyObjectByType<Canvas>();
        if (canvas == null) return;

        if (popupInstance != null)
        {
            Destroy(popupInstance);
        }

        popupInstance = Instantiate(popupPrefab);
        popupInstance.transform.SetParent(canvas.transform, false);

        popupCanvasGroup = popupInstance.GetComponent<CanvasGroup>();
        popupIcon = popupInstance.transform.Find("Panel/Icon").GetComponent<Image>();
        popupTitle = popupInstance.transform.Find("Panel/Title").GetComponent<TextMeshProUGUI>();
        popupDescription = popupInstance.transform.Find("Panel/Description").GetComponent<TextMeshProUGUI>();

        popupCanvasGroup.alpha = 0;
        popupCanvasGroup.interactable = false;
        popupCanvasGroup.blocksRaycasts = false;
    }

    public void Unlock(string achievementId)
    {
        Achievement ach = achievements.Find(a => a.id == achievementId);
        if (ach == null || ach.unlocked) return;

        ach.unlocked = true;
        Log.Message($"Achievement unlocked: {ach.title}");

        popupQueue.Enqueue(ach);
        if (!isShowingPopup)
        {
            StartCoroutine(ShowPopupQueue());
        }
    }

    private IEnumerator ShowPopupQueue()
    {
        isShowingPopup = true;

        while (popupQueue.Count > 0)
        {
            Achievement ach = popupQueue.Dequeue();

            if (popupCanvasGroup == null)
            {
                Log.Warning("PopupCanvasGroup missing.");
                isShowingPopup = false;
                yield break;
            }

            popupIcon.sprite = ach.icon != null ? ach.icon : defaultIcon;
            popupTitle.text = ach.title;
            popupDescription.text = ach.description;

            float t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * 4f;
                popupCanvasGroup.alpha = Mathf.Lerp(0, 1, t);
                yield return null;
            }

            yield return new WaitForSeconds(popupDuration);

            t = 0f;
            while (t < 1f)
            {
                t += Time.deltaTime * 4f;
                popupCanvasGroup.alpha = Mathf.Lerp(1, 0, t);
                yield return null;
            }
        }

        isShowingPopup = false;
    }
}
