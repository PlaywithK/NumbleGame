using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameController _gameController;
    [SerializeField] private UIController _uiController;
    [SerializeField] private UIInput _uiInput;
    private List<int> currentGuess = new List<int>();
    private int cursorIndex = 0;

    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _retryButton;
    [SerializeField] private GameObject _resultPanel;
    [SerializeField] private TMP_Text _winText;
    [SerializeField] private TMP_Text _loseText;
    [SerializeField] private Image _resultPanelImage;
    [SerializeField] private Color _winColor = Color.green;
    [SerializeField] private Color _loseColor = Color.red;

    private void OnEnable()
    {
        _uiInput.OnDigitPressed.AddListener(AddDigit);
        _uiInput.OnDeletePressed.AddListener(DeleteLastDigit);
        InitializeButtons();
    }

    private void OnDisable()
    {
        _uiInput.OnDigitPressed.RemoveListener(AddDigit);
        _uiInput.OnDeletePressed.RemoveListener(DeleteLastDigit);
    }

    public void InitializeButtons()
    {
        _mainMenuButton.onClick.AddListener(BackToMainMenu);
        _retryButton.onClick.AddListener(RetryGameMode);
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

    public void ShowResult(bool isWin)
    {
        if (_resultPanel != null)
        {
            _resultPanel.SetActive(true);
        }

        if (_resultPanelImage != null)
        {
            _resultPanelImage.color = isWin ? _winColor : _loseColor;
        }

        if (_winText != null)
        {
            _winText.gameObject.SetActive(isWin);
            //_winText.transform.localScale = Vector3.one * 0.7f;
        }

        if (_loseText != null)
        {
            _loseText.gameObject.SetActive(!isWin);
            //_loseText.transform.localScale = Vector3.one * 0.7f;
        }
    }

    public void HideResultPanel()
    {
        if (_resultPanel != null)
        {
            _resultPanel.SetActive(false);
        }
    }

    public void StartNewRound(int digitCount)
    {
        currentGuess = new List<int>();
        cursorIndex = 0;
        _uiController.InitGuessFields(digitCount);
        _uiController.ClearPastAttempts();
        _uiController.UpdateFields(currentGuess, cursorIndex);
    }
    private void Update()
    {
        var k = Keyboard.current;
        if (k == null) return;

        if (k.digit0Key.wasPressedThisFrame || k.numpad0Key.wasPressedThisFrame) AddDigit(0);
        if (k.digit1Key.wasPressedThisFrame || k.numpad1Key.wasPressedThisFrame) AddDigit(1);
        if (k.digit2Key.wasPressedThisFrame || k.numpad2Key.wasPressedThisFrame) AddDigit(2);
        if (k.digit3Key.wasPressedThisFrame || k.numpad3Key.wasPressedThisFrame) AddDigit(3);
        if (k.digit4Key.wasPressedThisFrame || k.numpad4Key.wasPressedThisFrame) AddDigit(4);
        if (k.digit5Key.wasPressedThisFrame || k.numpad5Key.wasPressedThisFrame) AddDigit(5);
        if (k.digit6Key.wasPressedThisFrame || k.numpad6Key.wasPressedThisFrame) AddDigit(6);
        if (k.digit7Key.wasPressedThisFrame || k.numpad7Key.wasPressedThisFrame) AddDigit(7);
        if (k.digit8Key.wasPressedThisFrame || k.numpad8Key.wasPressedThisFrame) AddDigit(8);
        if (k.digit9Key.wasPressedThisFrame || k.numpad9Key.wasPressedThisFrame) AddDigit(9);

        if (k.backspaceKey.wasPressedThisFrame || k.deleteKey.wasPressedThisFrame) DeleteLastDigit();
        if (k.enterKey.wasPressedThisFrame || k.numpadEnterKey.wasPressedThisFrame) SubmitGuess();
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
}

