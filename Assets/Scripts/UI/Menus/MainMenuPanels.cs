using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuPanels : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _classicModeButton;
    [SerializeField] private Button _classicModeButton_3;
    [SerializeField] private Button _classicModeButton_4;
    [SerializeField] private Button _classicModeButton_5;
    [SerializeField] private Button _classicModeButton_6;
    [SerializeField] private Button _classicModeButton_Custom;
    [SerializeField] private Button _endlessModeButton;
    [SerializeField] private Button _speedrunModeButton;
    [SerializeField] private Button _backToMainMenu;
    [SerializeField] private Button _backToModeSelect;

    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject modeSelectPanel;
    public GameObject classicPanel;

    private MenuManager _menuManager;

    public void Initialize(MenuManager menuManager)
    {
        _menuManager = menuManager;

        _startButton.onClick.AddListener(() => _menuManager.ShowPanel(_menuManager.currentPanels.modeSelectPanel));
        _classicModeButton.onClick.AddListener(() => _menuManager.ShowPanel(_menuManager.currentPanels.classicPanel));
        _backToMainMenu.onClick.AddListener(() => _menuManager.ShowPanel(_menuManager.currentPanels.mainMenuPanel));
        _backToModeSelect.onClick.AddListener(() => _menuManager.ShowPanel(_menuManager.currentPanels.modeSelectPanel));

        _endlessModeButton.onClick.AddListener(() => _menuManager.OnModeSelectPressed(0));
        _speedrunModeButton.onClick.AddListener(() => _menuManager.OnModeSelectPressed(1));

        _classicModeButton_3.onClick.AddListener(() => _menuManager.OnModeSelectPressed(2));
        _classicModeButton_4.onClick.AddListener(() => _menuManager.OnModeSelectPressed(3));
        _classicModeButton_5.onClick.AddListener(() => _menuManager.OnModeSelectPressed(4));
        _classicModeButton_6.onClick.AddListener(() => _menuManager.OnModeSelectPressed(5));
    }

    public void SelectPanelDefault(GameObject panel)
    {
        if (EventSystem.current == null || panel == null) return;

        Button targetButton = null;

        if (panel == mainMenuPanel)
        {
            targetButton = _startButton;
        }
        else if (panel == modeSelectPanel)
        {
            targetButton = _classicModeButton;
        }
        else if (panel == classicPanel)
        {
            targetButton = _classicModeButton_3;
        }

        if (targetButton != null)
        {
            EventSystem.current.SetSelectedGameObject(targetButton.gameObject);
        }
    }
}
