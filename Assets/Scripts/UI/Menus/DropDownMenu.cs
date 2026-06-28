using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DropDownMenu : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject _dropdownButtons;
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private GameObject _achievementPanel;
    [SerializeField] private GameObject _quitConfirmPanel;

    [Header("Buttons")]
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _achievementsButton;
    [SerializeField] private Button _quitButton;

    [SerializeField] private Button _confirmQuitButton;
    [SerializeField] private Button _cancelQuitButton;

    private void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard != null && keyboard.escapeKey.wasPressedThisFrame)
        {
            ToggleMenu();
            return;
        }

        var gamepad = Gamepad.current;
        if (gamepad != null && gamepad.selectButton.wasPressedThisFrame)
        {
            ToggleMenu();
            EventSystem.current.SetSelectedGameObject(_settingsButton.gameObject);
        }
    }

    void Start()
    {
        InitializeButtons();
    }

    public void InitializeButtons()
    {
        _settingsButton.onClick.AddListener(() => OpenOptions());
        _achievementsButton.onClick.AddListener(() => OpenAchievements());
        _quitButton.onClick.AddListener(() => OpenQuitMenu());

        _confirmQuitButton.onClick.AddListener(() => ConfirmQuit());
        _cancelQuitButton.onClick.AddListener(() => CancelQuit());
    }

    public void ToggleMenu()
    {
        bool currentlyActive = _dropdownButtons.activeSelf;
        _dropdownButtons.SetActive(!currentlyActive);
        Log.Message("Dropdown is now: " + (!currentlyActive));
    }

    public void OpenOptions()
    {
        if (_optionsPanel) _optionsPanel.SetActive(true);
        _dropdownButtons.SetActive(false);
    }

    public void OpenAchievements()
    {
        if (_achievementPanel) _achievementPanel.SetActive(true);
        _dropdownButtons.SetActive(false);
    }

    public void OpenQuitMenu()
    {
        if (_quitConfirmPanel) _quitConfirmPanel.SetActive(true);
        _dropdownButtons.SetActive(false);
        EventSystem.current.SetSelectedGameObject(_confirmQuitButton.gameObject);
    }

    public void ConfirmQuit()
    {
        Application.Quit();
    }

    public void CancelQuit()
    {
        if (_quitConfirmPanel) _quitConfirmPanel.SetActive(false);
    }
}
