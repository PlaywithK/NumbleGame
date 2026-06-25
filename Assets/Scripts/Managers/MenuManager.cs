using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _mobileCanvas;
    [SerializeField] private GameObject _pcCanvas;

    private DeviceManager _deviceManager;
    private GameObject _currentCanvas;
    private MainMenuPanels _currentPanels;

    private void Awake()
    {
        if (_deviceManager == null)
        {
            _deviceManager = FindAnyObjectByType<DeviceManager>();
        }
    }

    private void Start()
    {
        if (_deviceManager != null)
        {
            InitializeCanvas(_deviceManager.IsMobile());
        }
        else
        {
            Log.Warning("DeviceManager not found! Canvas could not be initialized. Defaulting to PC Canvas.");
            InitializeCanvas(false);
        }
    }

    public void InitializeCanvas(bool isMobile)
    {
        if (isMobile)
        {
            _currentCanvas = Instantiate(_mobileCanvas);
        }
        else
        {
            _currentCanvas = Instantiate(_pcCanvas);
        }
        InitializePanels();
    }

    private void InitializePanels()
    {
        _currentPanels = _currentCanvas.GetComponent<MainMenuPanels>();
        if (_currentPanels != null)
        {
            _currentPanels.Initialize(this);
        }
        else
        {
            Log.Error("MainMenuPanels is missing!");
        }
        ShowMainMenu();
    }

    private void HideAllPanels()
    {
        _currentPanels.mainMenuPanel.SetActive(false);
        _currentPanels.settingsPanel.SetActive(false);
        _currentPanels.modeSelectPanel.SetActive(false);
        _currentPanels.classicPanel.SetActive(false);
    }

    public void ShowMainMenu()
    {
        HideAllPanels();
        _currentPanels.mainMenuPanel.SetActive(true);
    }

    public void ShowSettings()
    {
        HideAllPanels();
        _currentPanels.settingsPanel.SetActive(true);
    }

    public void ShowModeSelect()
    {
        HideAllPanels();
        _currentPanels.modeSelectPanel.SetActive(true);
    }

    public void ShowClassicSelect()
    {
        HideAllPanels();
        _currentPanels.classicPanel.SetActive(true);
    }


    public void OnModeSelectPressed(int modeIndex)
    {
        GameModeSelection.SelectedIndex = modeIndex;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadMode(modeIndex);
        }

        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.LoadScene("GameScene");
        }
        else
        {
            Log.Error("SceneLoader is missing!");
        }
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }
}
