using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    [SerializeField] private Transform _guessFieldContainer;
    [SerializeField] private Transform _attemptsContainer;
    [SerializeField] private GameObject _guessFieldPrefab;
    [SerializeField] private GameObject _pastAttemptPrefab;
    public int FieldCount => _currentFields.Count;
    private List<GuessField> _currentFields = new List<GuessField>();

    public void InitGuessFields(int digitCount)
    {
        foreach (Transform child in _guessFieldContainer)
            Destroy(child.gameObject);
        _currentFields.Clear();

        for (int i = 0; i < digitCount; i++)
        {
            var go = Instantiate(_guessFieldPrefab, _guessFieldContainer);
            _currentFields.Add(go.GetComponent<GuessField>());
        }
    }

    public void UpdateFields(List<int> guess, int cursorIndex)
    {
        for (int i = 0; i < _currentFields.Count; i++)
        {
            if (i < cursorIndex && i < guess.Count)
            {
                _currentFields[i].SetDigit(guess[i].ToString()[0]);
            }
            else if (i == cursorIndex)
            {
                _currentFields[i].SetCursor(true);
            }
            else
            {
                _currentFields[i].SetEmpty();
            }
        }
    }

    public void ShowPastAttempt(List<int> guess, FeedbackChecker.FeedbackResult[] feedback)
    {
        var pastAttempt = Instantiate(_pastAttemptPrefab, _attemptsContainer);
        TMP_Text attemptsText = pastAttempt.GetComponentInChildren<TMP_Text>();
        string feedbackStr = "";

        for (int i = 0; i < feedback.Length; i++)
        {
            string colorTag = feedback[i] == FeedbackChecker.FeedbackResult.Correct ? "<color=green>" :
                              feedback[i] == FeedbackChecker.FeedbackResult.WrongPosition ? "<color=yellow>" : "<color=white>";
            feedbackStr += $"{colorTag}{guess[i]}</color> ";
        }
        attemptsText.text = feedbackStr;
    }

    public void ClearPastAttempts()
    {
        foreach (Transform child in _attemptsContainer)
            Destroy(child.gameObject);
    }
}
