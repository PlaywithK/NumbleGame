using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GameController : MonoBehaviour
{
    [Header("Achievements")]
    public AchievementUnlocker achievementUnlocker;

    [Header("UI Controller")]
    public UIController uiController;


    [Header("Backend")]
    private ModeConfig mode;
    private List<int> currentCode;
    private int currentAttempts;
    private float remainingTime;
    private bool gameEnded = false;

    //Endless
    public int endlessWinStreak = 0;
    public int endlessDigitCount = 3;


    [Header("UI")]
    public TMP_Text attemptsText;
    public TMP_Text timerText;

    [Header("Game Over Panels")]
    public GameObject winPanel;
    public TMP_Text winText;

    public GameObject losePanel;
    public TMP_Text loseText;

    void Start()
    {
        mode = GameManager.Instance.currentMode;

        if (mode.type == ModeType.Endless)
        {
            endlessWinStreak = 0;
            endlessDigitCount = 3;
        }

#if UNITY_ANDROID || UNITY_EDITOR
        winText.transform.localScale = Vector3.one * 0.7f;
        loseText.transform.localScale = Vector3.one * 0.7f;
#endif

        StartGame();
    }

    void StartGame()
    {
        currentAttempts = 0;
        int digitCount = mode.digitCount;

        //Endless-spezifische Logik
        if (mode.type == ModeType.Endless)
        {
            digitCount = endlessDigitCount;
            GameManager.Instance.GenerateSecretCode(endlessDigitCount);
        }
        else
        {
            GameManager.Instance.GenerateSecretCode();
        }

        //Timer bei Speedrun aktivieren
        if (mode.type == ModeType.Speedrun)
        {
            timerText.gameObject.SetActive(true);
            remainingTime = mode.timeLimit;
            StartCoroutine(SpeedrunCountdown());
        }
        else
        {
            timerText.gameObject.SetActive(false);
        }

        //UI laden
        if (uiController != null)
        {
            uiController.ClearPastAttempts();
            uiController.InitGuessFields(digitCount);
        }

        losePanel.SetActive(false);
        winPanel.SetActive(false);
        gameEnded = false;

        UpdateUI();
    }

    public FeedbackChecker.FeedbackResult[] OnGuessSubmitted(List<int> input)
    {
        var currentCode = GameManager.Instance.GetCurrentAnswer;

        currentAttempts++;
        var feedback = FeedbackChecker.GetFeedback(input, currentCode);

        UpdateUI();


        if (AreListsEqual(input, currentCode))
        {
            Win();
        }
        else if (currentAttempts >= mode.maxAttempts)
        {
            Lose();
        }

        return feedback;
    }

    private bool AreListsEqual(List<int> a, List<int> b)
    {
        if (a.Count != b.Count) return false;
        for (int i = 0; i < a.Count; i++)
        {
            if (a[i] != b[i]) return false;
        }
        return true;
    }

    void Win()
    {
        if (gameEnded) return;

        if (mode.type == ModeType.Endless)
        {
            endlessWinStreak++;

            //Alle zwei Erfolge Ziffern erhöhen (max 6)
            if (endlessWinStreak % 2 == 0 && endlessDigitCount < 6)
            {
                endlessDigitCount++;
            }
            StartCoroutine(RestartEndlessRound());
            return;
        }

        //Normale Win-Logik
        gameEnded = true;
        winPanel.SetActive(true);

        //Achievements checken
        int codeLength = currentCode.Count;
        bool lastAttempt = (currentAttempts == mode.maxAttempts);
        bool firstAttempt = (currentAttempts == 1);

        achievementUnlocker.OnCodeSolved(codeLength, currentAttempts, lastAttempt, firstAttempt);
    }


    void Lose()
    {
        if (gameEnded) return;
        gameEnded = true;

        losePanel.SetActive(true);
    }

    void UpdateUI()
    {
        attemptsText.text = $"Versuch: {currentAttempts}/{mode.maxAttempts}";
    }

    IEnumerator SpeedrunCountdown()
    {
        while (remainingTime > 0)
        {
            if (gameEnded) yield break;

            remainingTime -= Time.deltaTime;
            timerText.text = $"Zeit: {remainingTime:F1}s";
            yield return null;
        }

        Lose();
    }

    IEnumerator RestartEndlessRound()
    {
        gameEnded = true;
        yield return new WaitForSeconds(1f);
        StartGame();
    }
}