using UnityEngine;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameController _gameController;
    [SerializeField] private UIController _uiController;
    [SerializeField] private UIInput _uiInput;
    private List<int> currentGuess = new List<int>();
    private int cursorIndex = 0;


    private void OnEnable()
    {
        _uiInput.OnDigitPressed.AddListener(AddDigit);
        _uiInput.OnDeletePressed.AddListener(DeleteLastDigit);
    }

    private void OnDisable()
    {
        _uiInput.OnDigitPressed.RemoveListener(AddDigit);
        _uiInput.OnDeletePressed.RemoveListener(DeleteLastDigit);
    }

    public void StartNewRound(int digitCount)
    {
        currentGuess = new List<int>();
        cursorIndex = 0;
        _uiController.InitGuessFields(digitCount);
        _uiController.ClearPastAttempts();
        _uiController.UpdateFields(currentGuess, cursorIndex);
    }
    public void AddDigit(int digit)
    {
        if (cursorIndex >= _uiController.FieldCount) return;

        currentGuess.Insert(cursorIndex, digit);
        cursorIndex++;
        _uiController.UpdateFields(currentGuess, cursorIndex);
    }

    public void DeleteLastDigit()
    {
        if (cursorIndex > 0 && currentGuess.Count > 0)
        {
            cursorIndex--;
            currentGuess.RemoveAt(cursorIndex);
            _uiController.UpdateFields(currentGuess, cursorIndex);
        }
    }

    public void SubmitGuess()
    {
        if (currentGuess.Count != _uiController.FieldCount) return;

        var feedback = _gameController.OnGuessSubmitted(currentGuess);
        _uiController.ShowPastAttempt(currentGuess, feedback);

        currentGuess = new List<int>();
        cursorIndex = 0;
        _uiController.UpdateFields(currentGuess, cursorIndex);
    }

    public void RetryGameMode()
    {
        GameManager.Instance.LoadMode(GameManager.Instance.CurrentModeIndex);
        SceneLoader.Instance.LoadScene("GameScene");
    }

    public void BackToMainMenu()
    {
        SceneLoader.Instance.LoadScene("MenuScene");
    }
}

