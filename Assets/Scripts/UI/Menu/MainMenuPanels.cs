using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanels : MonoBehaviour
{
    
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _classicModeButton;
    [SerializeField] private Button _endlessModeButton;
    [SerializeField] private Button _speedrunModeButton;
    [SerializeField] private Button _backFromModeSelect;
    [SerializeField] private Button _backFromClassic;

    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject modeSelectPanel;
    public GameObject classicPanel;

    public DropDownMenu dropDownMenu;

    private MenuManager _menuManager;

    public void Initialize(MenuManager menuManager)
    {
        _menuManager = menuManager;

        _startButton.onClick.AddListener(_menuManager.ShowModeSelect);
        _classicModeButton.onClick.AddListener(_menuManager.ShowClassicSelect);
        _backFromModeSelect.onClick.AddListener(_menuManager.ShowMainMenu);
        _backFromClassic.onClick.AddListener(_menuManager.ShowModeSelect);

        _endlessModeButton.onClick.AddListener(() => _menuManager.OnModeSelectPressed(0));
        _speedrunModeButton.onClick.AddListener(() => _menuManager.OnModeSelectPressed(1));
    }
}
