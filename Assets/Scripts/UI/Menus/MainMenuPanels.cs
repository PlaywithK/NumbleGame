using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MainMenuPanels : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject modeSelectPanel;
    public GameObject classicPanel;

    public GameObject achievementsPanel;
    public GameObject settingsPanel;


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

    private MenuManager _menuManager;
    private GameObject _currentPanel;
    private bool _selectionAllowed = true;
    private Dictionary<GameObject, Button> _defaultSelectButtons;

    void Awake()
    {
        _defaultSelectButtons = new Dictionary<GameObject, Button>
    {
        { mainMenuPanel, _startButton },
        { modeSelectPanel, _classicModeButton },
        { classicPanel, _classicModeButton_3 },

        { settingsPanel, _startButton },
        { achievementsPanel, _startButton }
    };
    }

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

    void OnEnable()
    {
        InputManager.OnSelectionModeChanged += HandleSelectionModeChanged;
    }

    void OnDisable()
    {
        InputManager.OnSelectionModeChanged -= HandleSelectionModeChanged;
    }

    private void HandleSelectionModeChanged(bool allowed)
    {
        _selectionAllowed = allowed;

        if (!allowed)
        {
            // Maus bewegt -> Selection sofort entfernen
            EventSystem.current?.SetSelectedGameObject(null);
        }
        else
        {
            // Wieder Tastatur/Gamepad -> aktuelles Panel erneut selecten
            if (_currentPanel != null)
                SelectPanelDefault(_currentPanel);
        }
    }

    public void SelectPanelDefault(GameObject panel)
    {
        _currentPanel = panel;

        if (EventSystem.current == null || panel == null || !_selectionAllowed) return;

        if (_defaultSelectButtons.TryGetValue(panel, out var targetButton) && targetButton != null)
        {
            EventSystem.current.SetSelectedGameObject(targetButton.gameObject);
        }
    }
}
