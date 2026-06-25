using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    [SerializeField] private Transform _guessFieldContainer;
    [SerializeField] private Transform _attemptsContainer;
    [SerializeField] private GameObject _guessFieldPrefab;
    [SerializeField] private GameObject _pastAttemptPrefab;
    public int FieldCount => currentFields.Count;
    private List<GuessField> currentFields = new List<GuessField>();

    public void InitGuessFields(int digitCount)
    {
        foreach (Transform child in _guessFieldContainer)
            Destroy(child.gameObject);
        currentFields.Clear();

        for (int i = 0; i < digitCount; i++)
        {
            var go = Instantiate(_guessFieldPrefab, _guessFieldContainer);
            currentFields.Add(go.GetComponent<GuessField>());
        }
    }

    public void UpdateFields(List<int> guess, int cursorIndex)
    {
        for (int i = 0; i < currentFields.Count; i++)
        {
            if (i < cursorIndex && i < guess.Count)
                currentFields[i].SetDigit(guess[i].ToString()[0]);
            else if (i == cursorIndex)
                currentFields[i].SetCursor(true);
            else
                currentFields[i].SetEmpty();
        }
    }

    public void ShowPastAttempt(List<int> guess, FeedbackChecker.FeedbackResult[] feedback)
    {
        var pastAttempt = Instantiate(_pastAttemptPrefab, _attemptsContainer);
        TMP_Text text = pastAttempt.GetComponentInChildren<TMP_Text>();
        string feedbackStr = "";

        for (int i = 0; i < feedback.Length; i++)
        {
            string colorTag = feedback[i] == FeedbackChecker.FeedbackResult.Correct ? "<color=green>" :
                              feedback[i] == FeedbackChecker.FeedbackResult.WrongPosition ? "<color=yellow>" :
                              "<color=white>";
            feedbackStr += $"{colorTag}{guess[i]}</color> ";
        }

        text.text = feedbackStr;
    }

    public void ClearPastAttempts()
    {
        foreach (Transform child in _attemptsContainer)
            Destroy(child.gameObject);
    }
}
